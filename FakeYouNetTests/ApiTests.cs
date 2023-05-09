using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeYouNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime;

namespace FakeYouNet.Tests
{
    [TestClass()]
    public class ApiTests
    {
        [TestMethod()]
        public async Task GetListOfVoiceCategories()
        {
            var categories = await Api.GetListOfVoiceCategories();
            Assert.IsTrue(categories.Count() > 0);

        }

        [TestMethod()]
        public async Task GetListOfVoices()
        {
            var models = await Api.GetListOfVoices();
            Assert.IsTrue(models.Count() > 0);
        }

        [TestMethod()]
        public async Task GetTTSAudioClip()
        {
            var uri = "/tts_inference_output/0/5/0/vocodes_050fa9b6-7033-462f-9650-a95e3e8fb679.wav";
            var data = await Api.GetTTSAudioClip(uri);

            Assert.IsNotNull(data);
            Assert.IsTrue(data.Count() > 0);
        }

        [TestMethod()]
        public async Task DownloadTTSAudioClip()
        {
            var filename = "test.wav";
            var uri = "/tts_inference_output/0/5/0/vocodes_050fa9b6-7033-462f-9650-a95e3e8fb679.wav";
            await Api.DownloadTTSAudioClip(uri, filename);

            Assert.IsTrue(File.Exists(filename));
        }

        [TestMethod()]
        public async Task MakeTTSRequest()
        {
            var model_token = "TM:d7p6m07zr4x9";
            var job = await Api.MakeTTSRequest(model_token, "hello, how are you?");

            Assert.IsNotNull(job);
            Assert.IsTrue(string.IsNullOrWhiteSpace(job.inference_job_token) == false);
        }

        [TestMethod()]
        public async Task GetInterferenceJobStatus()
        {
            var jobToken = "JTINF:bmqtakc4fgdq7g80hhmc46knh1";
            var jobStatus = await Api.GetInterferenceJobStatus(jobToken);

            Assert.IsNotNull(jobStatus);            
        }

        [TestMethod()]
        public async Task MakeTTSRequestAndWait()
        {            
            var model_token = "TM:d7p6m07zr4x9";
            var response = await Api.MakeTTSRequestAndWait(model_token, "testing 1, 2, 3, testing", 60);

            Assert.IsNotNull(response);
        }

        [TestMethod()]
        public async Task MakeTTSRequestAndDownload()
        {
            var model_token = "TM:d7p6m07zr4x9";
            var response = await Api.MakeTTSRequestAndWait(model_token, "testing 1, 2, 3, testing", 60);
            await Api.DownloadTTSAudioClip(response, "test.wav");
        }
    }
}