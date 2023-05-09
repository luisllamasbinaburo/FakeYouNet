# FakeYouNet

FakeYouNet is .NET library that allows you to interact with the FakeYouAPI easily.

## Examples

### Search voices
```c#
var client = new Client();
var voices = await client.SearchVoice("mario");
```

### Generate TTS
```c#
var client = new Client();
var voice = await client.FindVoiceByTitle("mario");

var bytes = await client.MakeTTS(voice, "testing 1, 2, 3, testing");
```

### Play TTS with NAudio
```c#
var client = new Client();
var voice = await client.FindVoiceByTitle("mario");

var bytes = await client.MakeTTS(voice, "testing 1, 2, 3, testing");

using MemoryStream memoryStream = new MemoryStream(audioData);
using WaveFileReader reader = new WaveFileReader(ms);
using WaveOutEvent outputDevice = new WaveOutEvent();
outputDevice.Init(reader);
outputDevice.Play();
```

### Download TTS as wav
```c#
var client = new Client();
var voice = await client.FindVoiceByTitle("mario");

await client.DownloadMakeTTS(voice, "testing 1, 2, 3, testing", "test.wav");
```

## TODO
- [x] Get categories and voices
- [x] Cache categories and voices
- [x] Get TTS bytes with timeout
- [x] Download TTS as wav withtimeout
- [x] Login free account
- [ ] Premium account
- [ ] LeaderBoard
- [ ] Edit client
