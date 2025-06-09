# Audio Transcription Helper

This is a little program meant to help with the task of transcribing small voice files. It can playback files, do auto transcription using the Vosk library, and rename the files.

It requires a VOSK-compatible language model for the AutoTranscript feature to work.

Please download a model here: https://alphacephei.com/vosk/models

This software was tested and confirmed to work with the vosk-model-en-us-0.22-lgraph model.
Any other model should work just as well though.

Once the model is extracted, please rename the folder containing the model (**vosk-model-en-us-0.22-lgraph**) to just "model" and put it in the same folder as the .exe

## Things to take into account

VOSK only supports .wav file formats. Audio Transcription Helper will temporarily convert .mp3s (and only MP3 files) into temporary .wav for processing.
The VOSK format requires the .wav to be 16KHz, 16-bit, Mono.

When renaming, it will keep the original .mp3 format as to not lose any audio quality through conversion.
