using FakeYouNet.Classes;
using Newtonsoft.Json;

namespace FakeYouNet
{
    public class Client
    {
        public Client()
        {
        }

        public Client(string usernameOrEmail, string password)
        {
            UsernameOrEmail = usernameOrEmail;
            Password = password;
        }

        public VoiceCategoryModel[] Categories { get; private set; }
        public VoiceModel[] Voices { get; private set; }

        private string UsernameOrEmail { get; set; }
        private string Password { get; set; }
        private ClientSession Session { get; set; }
        public bool IsInitialized { get; private set; }


        public async Task Init()
        {
            await LoadCache();
            if (IsInitialized == false) await RefreshCache();

            if (UsernameOrEmail != null && Password != null) await Login();
            
            IsInitialized = true;
        }

        public async Task Login()
        {
            Session = new ClientSession();
            Session.AuthCookie = await Api.Login(UsernameOrEmail, Password);
        }

        public async Task RefreshCache()
        {
            await GetData();
            await SaveCache();
        }

        private async Task GetData()
        {
            Categories = await Api.GetListOfVoiceCategories();
            Voices = await Api.GetListOfVoices();
        }

        private async Task SaveCache()
        {
            var jsonCategories = JsonConvert.SerializeObject(Categories);
            await File.WriteAllTextAsync("categories.json", jsonCategories);

            var jsonVoices = JsonConvert.SerializeObject(Voices);
            await File.WriteAllTextAsync("voices.json", jsonVoices);
        }

        private async Task LoadCache()
        {
            if (File.Exists("categories.json"))
            {
                var json = File.ReadAllText("categories.json");
                Categories = JsonConvert.DeserializeObject<VoiceCategoryModel[]>(json);
            }

            if (File.Exists("voices.json"))
            {
                var json = File.ReadAllText("voices.json");
                Voices = JsonConvert.DeserializeObject<VoiceModel[]>(json);
            }
        }

        public async Task<VoiceModel[]> SearchVoice(string title)
        {
            if (IsInitialized == false) await Init();

            var voice = Voices.Where(x => x.title.Contains(title, StringComparison.InvariantCultureIgnoreCase)).ToArray();
            if (voice == null) await RefreshCache();

            return Voices.Where(x => x.title.Contains(title, StringComparison.InvariantCultureIgnoreCase)).ToArray();
        }

        public async Task<VoiceModel> FindVoiceByTitle(string title)
        {
            if (IsInitialized == false) await Init();

            var voice = Voices.FirstOrDefault(x => x.title.Contains(title, StringComparison.InvariantCultureIgnoreCase));
            if (voice == null) await RefreshCache();

            return Voices.FirstOrDefault(x => x.title.Contains(title, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<VoiceModel> FindVoiceByToken(string token)
        {
            if (IsInitialized == false) await Init();

            var voice = Voices.FirstOrDefault(x => x.model_token.Contains(token, StringComparison.InvariantCultureIgnoreCase));
            if (voice == null) await RefreshCache();

            return Voices.FirstOrDefault(x => x.title.Contains(token, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<byte[]> MakeTTS(VoiceModel voice, string text, int timeout = 60)
        {
            var jobstatus = await Api.MakeTTSRequestAndWait(voice.model_token, text, timeout);

            if (jobstatus != null)
            {
                return await Api.GetTTSAudioClip(jobstatus);
            }

            return null;
        }

        public async Task DownloadMakeTTS(VoiceModel voice, string text, string filepath, int timeout = 60)
        {
            var jobstatus = await Api.MakeTTSRequestAndWait(voice.model_token, text, timeout);

            if (jobstatus != null)
            {
                await Api.DownloadTTSAudioClip(jobstatus, filepath);
            }
        }
    }
}