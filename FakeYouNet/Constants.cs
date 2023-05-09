using System;
using System.Text.RegularExpressions;

namespace FakeYouNet
{
    public static class Constants
    {     
        public static class URL
        {
            public const string api = "https://api.fakeyou.com";
            public const string webPage = "https://fakeyou.com";
            public const string gravatar = "https://www.gravatar.com/avatar/";
            public const string cdn = "https://storage.googleapis.com/vocodes-public";
            public const string models = "/tts/list";
            public const string categories = "/category/list/tts";
            public const string login = "/login";
            public const string session = "/session";
            public const string queue = "/tts/queue_length";
            public const string inference = "/tts/inference";

            public static string job(string token) => $"/tts/job/{token}"; 
            public static string model(string token) => $"/tts/model/{token}"; 
            public static string category(string token) => $"/category/view/{token}"; 
            public static string assignments(string token) => $"/category/assignments/tts/{token}"; 
            public static string result(string token) => $"/tts/result/{token}"; 
            public static string profile(string user) => $"/user/{user}/profile"; 
            public static string editProfile(string user) => $"/user/{user}/edit_profile"; 
            public static string ttsResults(string user) => $"/user/{user}/tts_results"; 
            public static string ttsModels(string user) => $"/user/{user}/tts_models"; 
        }

        public static class Error
        {
            public const string webUnavailable = "The FakeYouAPI is not available";
            public const string alreadyLogged = "You have already logged in";
            public const string notModel = "The voice option is missing";
            public const string notText = "The text option is missing";
            public const string invalidToken = "Invalid token";
            public const string noAudioPath = "The poll has not been finished";

            public static string failureResult(string token) => "The \"{token}\" request is a completed failure";
            public static string optionNotFound(string option) => "Required option \"{option}\" re is missing";
            public static string invalidType(object value, string type) => $"\"{value}\" option must be a{(Regex.IsMatch(type, "^[aieou].*", RegexOptions.IgnoreCase) ? "n" : "")} {type}"; 
            public static string invalidOption(string option) => "The provided option \"{option}\" is invalid or not matches the verification";
            public static string modelNotFound(string data) => "\"{data}\" model not found";
            public static string notCredentials(string option) => "The {option} credential is missing";
        }
    }
}