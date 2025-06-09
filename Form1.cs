using NAudio.Wave;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using Vosk;

namespace AudioTranscriptionHelper
{
    public partial class Form1 : Form
    {
        string _currentSelectedFilePath = string.Empty;
        string _currentTranscription = string.Empty;
        WaveOutEvent _waveOutDevice;
        AudioFileReader _audioFileReader;

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // Ensure cleanup of audio resources on form close.
            DisposeWave();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select the folder containing audio files";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    txtWorkfolder.Text = selectedPath;

                    LoadAudioFiles(selectedPath);
                }
            }
        }

        private void LoadAudioFiles(string workFolder)
        {
            lstFileList.Items.Clear();
            btnPlay.Enabled = false;
            btnTranscribe.Enabled = false;
            btnRename.Enabled = false;

            try
            {
                var allowedExtensions = new[] { "*.mp3", "*.wav" };

                var audioFiles = allowedExtensions
                    .SelectMany(ext => Directory.EnumerateFiles(workFolder, ext)).ToList();

                if (audioFiles.Any())
                {
                    foreach (var filePath in audioFiles)
                    {
                        lstFileList.Items.Add(Path.GetFileName(filePath));
                    }
                }
                else
                {
                    lstFileList.Items.Add("No audio files found in the selected folder.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading audio files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ConvertMp3ToWav(string mp3FilePath)
        {
            var outFormat = new WaveFormat(16000, 16, 1); // 16KHz, 16-bit, Mono

            // Create temporary path for output WAV
            string tempWavPath = Path.ChangeExtension(Path.GetTempFileName(), ".wav");

            // Use NAudio to Read and Convert

            using (var reader = new Mp3FileReader(mp3FilePath))
            {
                // Resample audio to target using conversion stream
                using (var conversionStream = new WaveFormatConversionStream(outFormat, reader))
                {
                    WaveFileWriter.CreateWaveFile(tempWavPath, conversionStream);
                }
            }

            return tempWavPath;
        }

        private async void btnTranscribe_Click(object sender, EventArgs e)
        {
            //FIX: Ensure file handle is released before starting transcription.
            DisposeWave(); // Ensure any previous playback is stopped before starting transcription
            if (string.IsNullOrEmpty(_currentSelectedFilePath))
            {
                MessageBox.Show("Please select a file to transcribe.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string fileToProcess = _currentSelectedFilePath;
            bool isTempFile = false;

            // --- UI SETUP ---
            // Disable buttons to prevent multiple clicks while processing
            btnTranscribe.Enabled = false;
            btnPlay.Enabled = false;
            btnRename.Enabled = false;
            txtTranscriptionOutput.Text = "Preparing file...";
            Application.DoEvents(); // Allow UI to update

            try
            {
                // --- ASYNC EXECUTION ---
                // Task.Run pushes the enclosed code to a background thread

                // Let's first check if the file requires conversion from MP3 to WAV
                if (Path.GetExtension(fileToProcess).ToLower() == ".mp3")
                {
                    // Convert MP3 to WAV
                    txtTranscriptionOutput.Text = "Converting MP3 to WAV...";
                    Application.DoEvents(); // Allow UI to update
                    try
                    {
                        fileToProcess = await Task.Run(() => ConvertMp3ToWav(fileToProcess));
                        isTempFile = true; // Mark as temporary file
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error converting MP3 to WAV: {ex.Message}", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtTranscriptionOutput.Text = "[Conversion failed]";
                        return;
                    }
                }

                string transcribedText = await Task.Run(() =>
                {
                    // This is the heavy work that now runs in the background
                    int sampleRate = GetWaveFileSampleRate(fileToProcess);

                    Vosk.Vosk.SetLogLevel(-1); // Turn off verbose logging
                    var model = new Model("model");
                    var recognizer = new VoskRecognizer(model, sampleRate);

                    // These settings are already optimized for speed
                    recognizer.SetMaxAlternatives(0);
                    recognizer.SetWords(false);

                    using (Stream source = File.OpenRead(fileToProcess))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            recognizer.AcceptWaveform(buffer, bytesRead);
                        }
                    }

                    string finalResultJson = recognizer.FinalResult();
                    JsonDocument doc = JsonDocument.Parse(finalResultJson);
                    return doc.RootElement.GetProperty("text").GetString();
                });

                _currentTranscription = transcribedText;

                if (string.IsNullOrWhiteSpace(_currentTranscription))
                {
                    txtTranscriptionOutput.Text = "[No speech detected.]";
                }
                else
                {
                    txtTranscriptionOutput.Text = _currentTranscription;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during transcription: {ex.Message}", "Transcription Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTranscriptionOutput.Text = "[Transcription failed]";
            }
            finally
            {
                // Re-enable buttons after processing regardless of success or failure
                btnTranscribe.Enabled = true;
                btnPlay.Enabled = true;
                btnRename.Enabled = true;

                // CRITICAL: Delete temporary file if it was created
                if (isTempFile && File.Exists(fileToProcess))
                {
                    try
                    {
                        File.Delete(fileToProcess);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Non-Critical Error deleting temporary file: {ex.Message}\nConsider deleting the file manually.", "Cleanup Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

        }

        private int GetWaveFileSampleRate(string filePath)
        {
            using (var reader = new WaveFileReader(filePath))
            {
                return reader.WaveFormat.SampleRate;
            }
        }

        private string SanitizeFileName(string input)
        {
            // Remove invalid characters from file names.
            string invalidChars = new string(Path.GetInvalidFileNameChars());
            foreach (char c in invalidChars)
            {
                input = input.Replace(c.ToString(), "");
            }
            return input.Trim();
        }

        private void lstFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFileList.SelectedItems.Count == 0)
            {
                btnPlay.Enabled = false;
                btnTranscribe.Enabled = false;
                btnRename.Enabled = false;
                return;
            }

            btnPlay.Enabled = true;
            btnTranscribe.Enabled = true;
            btnRename.Enabled = true;

            // Store full path of selected file.
            string workingDir = txtWorkfolder.Text;
            string selectedFile = lstFileList.SelectedItems[0].Text;
            _currentSelectedFilePath = Path.Combine(workingDir, selectedFile);

        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentSelectedFilePath)) return;

            // Stop previous Playback if any
            DisposeWave();

            try
            {
                _waveOutDevice = new WaveOutEvent();
                _audioFileReader = new AudioFileReader(_currentSelectedFilePath);
                _waveOutDevice.Init(_audioFileReader);
                _waveOutDevice.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing audio: {ex.Message}", "Playback Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DisposeWave();
            }
        }
        // Cleanup Playback resources when the form is closed or playback is stopped.
        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            DisposeWave();
        }

        private void DisposeWave()
        {
            // Clean up audio playback resources.
            _waveOutDevice?.Dispose();
            _waveOutDevice = null;
            _audioFileReader?.Dispose();
            _audioFileReader = null;
        }

        private void btnRename_Click(object sender, EventArgs e)
        {

            // FIX: Release file handle before trying to rename.
            DisposeWave();
            if (string.IsNullOrEmpty(_currentTranscription) || string.IsNullOrEmpty(_currentSelectedFilePath))
            {
                MessageBox.Show("No transcription available to rename the file.", "Rename Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string sanitizedName = SanitizeFileName(txtTranscriptionOutput.Text);
                if (string.IsNullOrEmpty(sanitizedName))
                {
                    MessageBox.Show("Transcription is empty or invalid for renaming.", "Rename Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string directory = Path.GetDirectoryName(_currentSelectedFilePath);
                string extension = Path.GetExtension(_currentSelectedFilePath);
                string newFilePath = Path.Combine(directory, sanitizedName + extension);

                if (File.Exists(newFilePath))
                {
                    MessageBox.Show($"A file with the name '{Path.GetFileName(newFilePath)}' already exists, please rename or delete the existing file.", "Rename Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Rename the file
                File.Move(_currentSelectedFilePath, newFilePath);

                // Update ListView to reflect the new file name
                ListViewItem selectedItem = lstFileList.SelectedItems[0];
                selectedItem.Text = Path.GetFileName(newFilePath);

                // Update stored file path
                _currentSelectedFilePath = newFilePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error renaming file: {ex.Message}", "Rename Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Audio Transcription Helper\nVersion 1.0\n\nThis application helps you transcribe audio files and rename them based on the transcription.\n\nDeveloped by Haru Nakamura // NAKAMURA SYSTEMS E.I.R.L.", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
