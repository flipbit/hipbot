using HipBot.Interfaces.Services;
using Moq;
using NUnit.Framework;
using Sugar.Configuration;

namespace HipBot.Services
{
    [TestFixture]
    public class CredentialServiceTest : AutoMockingTest
    {
        private CredentialService service;

        [SetUp]
        public void SetUp()
        {
            service = Create<CredentialService>();
        }

        [Test]
        public void TestGetCredentials()
        {
            var config = new Config();
            config.SetValue("Credentials", "Name", "Bob");
            config.SetValue("Credentials", "JabberID", "123");
            config.SetValue("Credentials", "Password", "Password");
            config.SetValue("Credentials", "ApiToken", "123456");

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            var result = service.GetCredentials();

            Assert.AreEqual("123456", result.ApiToken);
            Assert.AreEqual("123", result.JabberId);
            Assert.AreEqual("Bob", result.Name);
            Assert.AreEqual("Password", result.Password);
        }

        [Test]
        public void TestStoreCredentials()
        {
            
        }
    }
}
