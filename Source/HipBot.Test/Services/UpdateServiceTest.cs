using System;
using Moq;
using NUnit.Framework;
using Sugar.Configuration;
using Sugar.IO;
using Sugar.Net;

namespace HipBot.Services
{
    [TestFixture]
    public class UpdateServiceTest : AutoMockingTest
    {
        private UpdateService service;

        [SetUp]
        public void SetUp()
        {
            service = Create<UpdateService>();
        }

        [Test]
        public void TestGetUpdateDirectoryFromUrl()
        {
            const string url = "http://localhost/HipBot.12345678.zip";

            Mock<IFileService>()
                .Setup(call => call.GetUserDataDirectory())
                .Returns("c:\\users\\chris\\appdata\\roaming");

            var result = service.GetVersionDirectoryFromUrl(url);

            Assert.AreEqual("c:\\users\\chris\\appdata\\roaming\\hipbot\\12345678", result);
        }

        [Test]
        public void TestGetUpdateDirectoryFromUrlWithInvalidUrl()
        {
            const string url = "not valid!";

            Assert.Throws<ApplicationException>(() => service.GetVersionDirectoryFromUrl(url));
        }

        [Test]
        public void TestIsNewVersionAvailableWhenTrue()
        {
            var config = new Config();

            config.SetValue("Update", "Current", "http://localhost/HipBot.12345678.zip");
            config.SetValue("Update", "Url", "http://localhost/version.txt");

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            Mock<IHttpService>()
                .Setup(call => call.Get("http://localhost/version.txt"))
                .Returns(new FakeHttpResponse("http://localhost/HipBot.23456789.zip"));

            Assert.IsTrue(service.IsNewVersionAvailable());
        }

        [Test]
        public void TestIsNewVersionAvailableWhenFalse()
        {
            var config = new Config();

            config.SetValue("Update", "Current", "http://localhost/HipBot.12345678.zip");
            config.SetValue("Update", "Url", "http://localhost/version.txt");

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            Mock<IHttpService>()
                .Setup(call => call.Get("http://localhost/version.txt"))
                .Returns(new FakeHttpResponse("http://localhost/HipBot.12345678.zip"));

            Assert.IsFalse(service.IsNewVersionAvailable());
        }
    }
}
