using FakeYouNet.Classes;

namespace FakeYouNet.Responses
{
    public class Voices
    {
        public bool success { get; set; }
        public VoiceModel[] models { get; set; }
    }
}