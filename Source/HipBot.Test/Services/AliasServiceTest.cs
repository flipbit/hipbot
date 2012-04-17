using Moq;
using NUnit.Framework;
using Sugar.Configuration;

namespace HipBot.Services
{
    [TestFixture]
    public class AliasServiceTest : AutoMockingTest
    {
        private AliasService service;

        [SetUp]
        public void SetUp()
        {
            service = Create<AliasService>();
        }

        [Test]
        public void TestSetAlias()
        {
            var config = new Config();

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            service.SetAlias("test", "alias");
            
            Assert.AreEqual("alias", config.GetValue("Aliases", "test", string.Empty));
        }

        [Test]
        public void TestSetAliasOverwritesExistingValue()
        {
            var config = new Config();
            config.SetValue("Aliases", "test", "alias one");

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            service.SetAlias("test", "alias two");

            Assert.AreEqual("alias two", config.GetValue("Aliases", "test", string.Empty));
        }


        [Test]
        public void TestSetAliasWithSingleQuotes()
        {
            var config = new Config();

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            service.SetAlias("test", "alias 'three four'");

            Assert.AreEqual(@"alias ""three four""", config.GetValue("Aliases", "test", string.Empty));
        }

        [Test]
        public void TestAsAliasWhenTrue()
        {
            var config = new Config();
            config.SetValue("Aliases", "test", "alias one");

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            Assert.IsTrue(service.IsAlias("test"));
        }

        [Test]
        public void TestAsAliasWhenFalse()
        {
            var config = new Config();

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            Assert.IsFalse(service.IsAlias("test"));
        }

        [Test]
        public void TestGetAlias()
        {
            var config = new Config();
            config.SetValue("Aliases", "test", "alias one");

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            Assert.AreEqual("alias one", service.GetAlias("test"));
        }

        [Test]
        public void TestRemoveAlias()
        {
            var config = new Config();
            config.SetValue("Aliases", "test", "alias one");

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            service.RemoveAlias("test");

            Assert.AreEqual(string.Empty, config.GetValue("Aliases", "test", string.Empty));
        }

        [Test]
        public void TestListAliases()
        {
            var config = new Config();
            config.SetValue("Aliases", "test one", "alias one");
            config.SetValue("Aliases", "test two", "alias two");

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            var lines = service.ListAliases();

            Assert.AreEqual(2, lines.Count);
            Assert.AreEqual("test one=alias one", lines[0]);
            Assert.AreEqual("test two=alias two", lines[1]);
        }
    }
}
