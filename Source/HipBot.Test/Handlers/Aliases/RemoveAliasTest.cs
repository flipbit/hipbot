using HipBot.Domain;
using HipBot.Interfaces.Services;
using Moq;
using NUnit.Framework;

namespace HipBot.Handlers.Aliases
{
    [TestFixture]
    public class RemoveAliasTest : AutoMockingTest
    {
        private RemoveAlias handler;

        [SetUp]
        public void SetUp()
        {
            handler = Create<RemoveAlias>();
        }

        [Test]
        public void TestAddAlias()
        {
            var options = new RemoveAlias.Options();
            options.Name = "test";

            handler.Receive(new Message(), new Room(), options);

            Mock<IAliasService>()
                .Verify(call => call.RemoveAlias("test"));
        }
    }
}
