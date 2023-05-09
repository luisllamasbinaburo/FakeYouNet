using RestSharp;
using Newtonsoft.Json;
using System.Net;
using FakeYouNet.Classes;
using Mapster;
using System.IO;
using System.Text;
using Polly;
using System.Xml;

namespace FakeYouNet
{
    public static class Api
    {
        public static async Task<Cookie> Login(string username_or_email, string password)
        {
            var options = new RestClientOptions(Constants.URL.api);
            var client = new RestClient(options);
            var request = new RestRequest(Constants.URL.login);
            request.AddJsonBody(JsonConvert.SerializeObject(new { username_or_email, password}));

            var response = await client.PostAsync(request);


            return response.IsSuccessStatusCode ? response.Cookies.FirstOrDefault() : null;
        }

        /// <summary>
        /// Gets Voice Categories
        /// </summary>
        public static async Task<VoiceCategoryModel[]> GetListOfVoiceCategories()
        {
            List<VoiceCategoryModel> result = new();

            var options = new RestClientOptions(Constants.URL.api);
            var client = new RestClient(options);
            var request = new RestRequest(Constants.URL.categories);
            var response = await client.GetAsync(request);

            var content = JsonConvert.DeserializeObject<Responses.VoiceCategories>(response.Content);
            return content.categories;
        }

        /// <summary>
        /// Gets list of available voices
        /// </summary>
        public static async Task<VoiceModel[]> GetListOfVoices()
        {
            var options = new RestClientOptions(Constants.URL.api);
            var client = new RestClient(options);
            var request = new RestRequest(Constants.URL.models);
            var response = await client.GetAsync(request);

            var content = JsonConvert.DeserializeObject<Responses.Voices>(response.Content);
            return content.models;
        }

        /// <summary>
        /// Request a new TTS job
        /// </summary>
        public static async Task<InterferenceJob> MakeTTSRequest(VoiceModel voice, string text, ClientSession session = null)
        {
            return await MakeTTSRequest(voice.model_token, text, session);
        }

        public static async Task<InterferenceJob> MakeTTSRequest(string modelToken, string text, ClientSession session = null)
        {
            var payload = new
            {
                tts_model_token = modelToken,
                uuid_idempotency_token = Guid.NewGuid().ToString(),
                inference_text = text,
            };
            var json = JsonConvert.SerializeObject(payload);

            var options = new RestClientOptions(Constants.URL.api);
            var client = new RestClient(options);
            var request = new RestRequest(Constants.URL.inference);
            request.AddJsonBody(json);
            if (session != null) request.AddParameter("Cookie", $"session={session.AuthCookie}");            
            
            var response = await client.PostAsync(request);
            var content = JsonConvert.DeserializeObject<Responses.InferenceJob>(response.Content);
            return content?.success == true ? content.Adapt<InterferenceJob>() : null;
        }

        public static async Task<InterferenceJobStatus> MakeTTSRequestAndWait(VoiceModel voice, string text, int timeout, ClientSession session = null)
        {
            return await MakeTTSRequestAndWait(voice.model_token, text, timeout, session);
        }

        public static async Task<InterferenceJobStatus> MakeTTSRequestAndWait(string modelToken, string text, int timeout, ClientSession session = null)
        {
            var job = await MakeTTSRequest(modelToken, text, session);

            var policy = Policy
                  .Handle<Exception>()
                  .WaitAndRetryAsync(timeout, retryAttempt => TimeSpan.FromSeconds(1));

            InterferenceJobStatus response = null;
            await policy.ExecuteAsync(async () =>
            {
                response = await GetInterferenceJobStatus(job);
                if(response.status != "complete_success") throw new Exception();
            });

            return response;
        }

        /// <summary>
        /// Requests current status on the job
        /// </summary>
        public static async Task<InterferenceJobStatus> GetInterferenceJobStatus(InterferenceJob job)
        {
            return await GetInterferenceJobStatus(job.inference_job_token);
        }

        public static async Task<InterferenceJobStatus> GetInterferenceJobStatus(string jobToken)
        {

            var options = new RestClientOptions(Constants.URL.api);
            var client = new RestClient(options);
            var request = new RestRequest(Constants.URL.job(jobToken));

            var response = await client.GetAsync(request);
            var content = JsonConvert.DeserializeObject<Responses.InterferenceJobStatus>(response.Content);

            return content?.success == true ? content.state : null;
        }

        /// <summary>
        /// Get the TTS audio clip as byte array
        /// </summary>
        /// 
        public static async Task<byte[]> GetTTSAudioClip(InterferenceJobStatus jobStatus)
        {
            return await GetTTSAudioClip(jobStatus.maybe_public_bucket_wav_audio_path);
        }

        public static async Task<byte[]> GetTTSAudioClip(string uri)
        {
            var client = new RestClient(Constants.URL.cdn);
            var request = new RestRequest(uri);

            return await client.DownloadDataAsync(request);           
        }

        /// <summary>
        /// Downloads the TTS audio clip to a file
        /// </summary>
        public static async Task DownloadTTSAudioClip(InterferenceJobStatus jobStatus, string localpath)
        {
            await DownloadTTSAudioClip(jobStatus.maybe_public_bucket_wav_audio_path, localpath);
        }

        public static async Task DownloadTTSAudioClip(string uri, string localpath)
        {
            var data = await GetTTSAudioClip(uri);
            File.WriteAllBytes(localpath, data);
        }

        public static string GetTTSAudioUrl(InterferenceJobStatus jobStatus) => $"{Constants.URL.cdn}{jobStatus.maybe_public_bucket_wav_audio_path}";

        public static string GetTTSAudioUrl(string uri) => $"{Constants.URL.cdn}{uri}";

    }
}