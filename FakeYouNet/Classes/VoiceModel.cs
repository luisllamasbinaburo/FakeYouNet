namespace FakeYouNet.Classes
{
    public class VoiceModel
    {
        public string model_token { get; set; }
        public string tts_model_type { get; set; }
        public string creator_user_token { get; set; }
        public string creator_username { get; set; }
        public string creator_display_name { get; set; }
        public string creator_gravatar_hash { get; set; }
        public string title { get; set; }
        public string ietf_language_tag { get; set; }
        public string ietf_primary_language_subtag { get; set; }
        public bool? is_front_page_featured { get; set; }
        public bool? is_twitch_featured { get; set; }
        public string maybe_suggested_unique_bot_command { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

    }
}
