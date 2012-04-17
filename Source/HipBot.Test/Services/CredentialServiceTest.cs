using HipBot.Domain;
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
            var config = new Config();

            var credentials = new Credentials
            {
                Name = "Bob",
                JabberId = "123",
                Password = "Password",
                ApiToken = "123456"
            };

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            service.SetCredentials(credentials);

            Assert.AreEqual("123456", config.GetValue("Credentials", "ApiToken", string.Empty));
            Assert.AreEqual("123", config.GetValue("Credentials", "JabberID", string.Empty));
            Assert.AreEqual("Bob", config.GetValue("Credentials", "Name", string.Empty));
            Assert.AreEqual("Password", config.GetValue("Credentials", "Password", string.Empty));
        }

        [Test]
        public void TestGetCredentialsWhenSet()
        {
            var config = new Config();
            config.SetValue("Credentials", "Name", "Bob");

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            var result = service.CredentialsSet();

            Assert.AreEqual(true, result);
        }

        [Test]
        public void TestGetCredentialsWhenNotSet()
        {
            var config = new Config();

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            var result = service.CredentialsSet();

            Assert.AreEqual(false, result);
        }
    }
}
