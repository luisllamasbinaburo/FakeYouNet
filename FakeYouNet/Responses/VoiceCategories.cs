using FakeYouNet.Classes;

namespace FakeYouNet.Responses
{
    public class VoiceCategories
    {
        public bool success { get; set; }
        public VoiceCategoryModel[] categories { get; set; }
    }
}