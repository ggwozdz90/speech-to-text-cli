# Speech-to-Text CLI

This CLI application allows you to request speech-to-text transcription in SRT subtitle format from an API. It leverages the Speech-to-Text API Client library to communicate with the Speech-to-Text API.

## Features

- **Transcription**: Generate SRT subtitles from audio files.
- **Translation**: Generate translated SRT subtitles from audio files.
- **Local Processing**: All processing is done locally on the user's machine, ensuring that no files are sent over the internet.

## Commands

The CLI provides the following commands:

- `generate-srt` or `gs`: Generate SRT subtitles from an audio file.
  - **Options**:
    - `--file` or `-f`: The audio file to transcribe. (Required)
    - `--source-language` or `-sl`: The source language of the audio in the pattern xx_XX. (Can be configured in `appSettings.json`)
- `generate-translated-srt` or `gts`: Generate translated SRT subtitles from an audio file.
  - **Options**:
    - `--file` or `-f`: The audio file to transcribe. (Required)
    - `--source-language` or `-sl`: The source language of the audio in the pattern xx_XX. (Can be configured in `appSettings.json`)
    - `--target-language` or `-tl`: The target language for the translation in the pattern xx_XX. (Can be configured in `appSettings.json`)

## Available Distributions

- The CLI application leverages the [Speech-to-Text API Client](https://github.com/ggwozdz90/speech-to-text-api-client) library to handle API requests and responses.
- It communicates with the [Speech-to-Text API](https://github.com/ggwozdz90/speech-to-text-api), which performs transcription using OpenAI's Whisper model and translation using Seamless or mBART models.
- The application is built as a self-contained executable in .NET 9 and does not require the .NET runtime to be installed. You can download the executable from the [releases page](https://github.com/ggwozdz90/speech-to-text-cli/releases).
- **Important**: The CLI requires the Speech-to-Text API to be running locally. Without the API, the CLI cannot function.

## Usage

### Generate SRT Subtitles

```shell
SpeechToTextCli.exe generate-srt --file "path/to/audio/file.wav" --source-language "en_US"
```

### Generate Translated SRT Subtitles

```shell
SpeechToTextCli.exe generate-translated-srt --file "path/to/audio/file.wav" --source-language "en_US" --target-language "es_ES"
```

## Configuration

The CLI application uses the .NET configuration system. You can configure the base address of the Speech-to-Text API, as well as the default source and target languages, in your `appSettings.json` file:

```json
{
  "SpeechToText": {
    "BaseAddress": "http://localhost:8000",
    "SourceLanguage": "en_US",
    "TargetLanguage": "pl_PL"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Trace",
      "System": "Warning",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    },
    "Console": {
      "FormatterName": "simple",
      "TimestampFormat": "yyyy-MM-dd HH:mm:ss fff ",
      "SingleLine": true,
      "IncludeScopes": true,
      "UseUtcTimestamp": true,
      "ColorBehavior": "Enabled"
    }
  }
}
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Table of Contents

- [Speech-to-Text CLI](#speech-to-text-cli)
  - [Features](#features)
  - [Commands](#commands)
  - [Available Distributions](#available-distributions)
  - [Usage](#usage)
    - [Generate SRT Subtitles](#generate-srt-subtitles)
    - [Generate Translated SRT Subtitles](#generate-translated-srt-subtitles)
  - [Configuration](#configuration)
  - [License](#license)
  - [Table of Contents](#table-of-contents)
