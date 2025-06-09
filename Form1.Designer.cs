namespace AudioTranscriptionHelper
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtWorkfolder = new TextBox();
            btnSelect = new Button();
            lstFileList = new ListView();
            label1 = new Label();
            btnTranscribe = new Button();
            txtTranscriptionOutput = new TextBox();
            label2 = new Label();
            btnPlay = new Button();
            btnRename = new Button();
            label3 = new Label();
            btnExit = new Button();
            btnAbout = new Button();
            label4 = new Label();
            SuspendLayout();
            // 
            // txtWorkfolder
            // 
            txtWorkfolder.Enabled = false;
            txtWorkfolder.Location = new Point(12, 12);
            txtWorkfolder.Name = "txtWorkfolder";
            txtWorkfolder.PlaceholderText = "Select working folder containing the audio files     ---->";
            txtWorkfolder.Size = new Size(341, 23);
            txtWorkfolder.TabIndex = 0;
            // 
            // btnSelect
            // 
            btnSelect.Location = new Point(369, 12);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(124, 23);
            btnSelect.TabIndex = 1;
            btnSelect.Text = "Select Folder";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            // 
            // lstFileList
            // 
            lstFileList.BackColor = SystemColors.ScrollBar;
            lstFileList.Location = new Point(13, 57);
            lstFileList.Name = "lstFileList";
            lstFileList.ShowItemToolTips = true;
            lstFileList.Size = new Size(341, 230);
            lstFileList.Sorting = SortOrder.Ascending;
            lstFileList.TabIndex = 2;
            lstFileList.UseCompatibleStateImageBehavior = false;
            lstFileList.View = View.List;
            lstFileList.SelectedIndexChanged += lstFileList_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 38);
            label1.Name = "label1";
            label1.Size = new Size(46, 15);
            label1.TabIndex = 3;
            label1.Text = "File List";
            // 
            // btnTranscribe
            // 
            btnTranscribe.Location = new Point(369, 93);
            btnTranscribe.Name = "btnTranscribe";
            btnTranscribe.Size = new Size(124, 30);
            btnTranscribe.TabIndex = 4;
            btnTranscribe.Text = "AutoTranscribe";
            btnTranscribe.UseVisualStyleBackColor = true;
            btnTranscribe.Click += btnTranscribe_Click;
            // 
            // txtTranscriptionOutput
            // 
            txtTranscriptionOutput.BackColor = SystemColors.ScrollBar;
            txtTranscriptionOutput.Location = new Point(12, 308);
            txtTranscriptionOutput.Multiline = true;
            txtTranscriptionOutput.Name = "txtTranscriptionOutput";
            txtTranscriptionOutput.ScrollBars = ScrollBars.Vertical;
            txtTranscriptionOutput.Size = new Size(481, 86);
            txtTranscriptionOutput.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 290);
            label2.Name = "label2";
            label2.Size = new Size(315, 15);
            label2.TabIndex = 6;
            label2.Text = "Transcription text (verify accuracy when using Automode):";
            // 
            // btnPlay
            // 
            btnPlay.Location = new Point(369, 57);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(124, 30);
            btnPlay.TabIndex = 7;
            btnPlay.Text = "Play Audio File";
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += btnPlay_Click;
            // 
            // btnRename
            // 
            btnRename.Location = new Point(369, 129);
            btnRename.Name = "btnRename";
            btnRename.Size = new Size(124, 30);
            btnRename.TabIndex = 8;
            btnRename.Text = "Rename File";
            btnRename.UseVisualStyleBackColor = true;
            btnRename.Click += btnRename_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(13, 397);
            label3.Name = "label3";
            label3.Size = new Size(439, 15);
            label3.TabIndex = 9;
            label3.Text = "The transcription text will also be used for the filename when presing Rename File.";
            // 
            // btnExit
            // 
            btnExit.Location = new Point(369, 257);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(124, 30);
            btnExit.TabIndex = 10;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // btnAbout
            // 
            btnAbout.Location = new Point(369, 221);
            btnAbout.Name = "btnAbout";
            btnAbout.Size = new Size(124, 30);
            btnAbout.TabIndex = 11;
            btnAbout.Text = "About this Software";
            btnAbout.UseVisualStyleBackColor = true;
            btnAbout.Click += btnAbout_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(13, 412);
            label4.Name = "label4";
            label4.Size = new Size(378, 15);
            label4.TabIndex = 12;
            label4.Text = "Please avoid the following characters for renaming: <  >  :  \"  /   \\  |  ?  *";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(508, 435);
            Controls.Add(label4);
            Controls.Add(btnAbout);
            Controls.Add(btnExit);
            Controls.Add(label3);
            Controls.Add(btnRename);
            Controls.Add(btnPlay);
            Controls.Add(label2);
            Controls.Add(txtTranscriptionOutput);
            Controls.Add(btnTranscribe);
            Controls.Add(label1);
            Controls.Add(lstFileList);
            Controls.Add(btnSelect);
            Controls.Add(txtWorkfolder);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Form1";
            Text = "Audio Transcription Helper";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtWorkfolder;
        private Button btnSelect;
        private ListView lstFileList;
        private Label label1;
        private Button btnTranscribe;
        private TextBox txtTranscriptionOutput;
        private Label label2;
        private Button btnPlay;
        private Button btnRename;
        private Label label3;
        private Button btnExit;
        private Button btnAbout;
        private Label label4;
    }
}
