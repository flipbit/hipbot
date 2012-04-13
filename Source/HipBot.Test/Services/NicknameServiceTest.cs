using HipBot.Interfaces.Services;
using Moq;
using NUnit.Framework;
using Sugar.Configuration;

namespace HipBot.Services
{
    [TestFixture]
    public class NicknameServiceTest : AutoMockingTest
    {
        private NicknameService service;

        [SetUp]
        public void SetUp()
        {
            service = Create<NicknameService>();
        }

        [Test]
        public void TestAddNickname()
        {
            var config = new Config();

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            service.Add("bob");

            var section = config.GetSection("Nicknames");

            Assert.AreEqual(1, section.Count);
            Assert.AreEqual("bob", section[0].Key);
        }

        [Test]
        public void TestRemoveNickname()
        {
            var config = new Config();
            config.SetValue("Nicknames", "bob", string.Empty);
            config.SetValue("Nicknames", "alice", string.Empty);

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            service.Remove("bob");

            var section = config.GetSection("Nicknames");

            Assert.AreEqual(1, section.Count);
            Assert.AreEqual("alice", section[0].Key);
        }

        [Test]
        public void TestListNicknames()
        {
            var config = new Config();
            config.SetValue("Nicknames", "bob", string.Empty);
            config.SetValue("Nicknames", "alice", string.Empty);

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            var results = service.List();

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("bob", results[0]);
            Assert.AreEqual("alice", results[1]);
        }
    }
}
