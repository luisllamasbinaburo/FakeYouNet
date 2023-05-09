using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeYouNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet.Frameworks;
using FakeYouNet.Responses;

namespace FakeYouNet.Tests
{
    [TestClass()]
    public class ClientTests
    {
        [TestMethod()]
        public async Task Init()
        {
            var client = new Client();
            await client.Init();
        }

        [TestMethod()]
        public async Task LoginTest()
        {
            var email = "usernameOrEmail";
            var password = "password";

            var client = new Client(email, password);
            await client.Login();            
        }

        [TestMethod()]
        public async Task SearchVoice()
        {
            var client = new Client();
            var voices = await client.SearchVoice("mario");

            Assert.IsNotNull(voices);
            Assert.IsTrue(voices.Count() >= 1);
        }

        [TestMethod()]
        public async Task GenerateTTS()
        {
            var client = new Client();
            var voice = await client.FindVoiceByTitle("mario");

            var bytes = await client.MakeTTS(voice, "testing 1, 2, 3, testing");
            Assert.IsNotNull(voice);
        }

        [TestMethod()]
        public async Task DownloadTTS()
        {
            var client = new Client();
            var voice = await client.FindVoiceByTitle("mario");

            await client.DownloadMakeTTS(voice, "testing 1, 2, 3, testing", "test.wav");
            Assert.IsNotNull(voice);
        }
    }
}