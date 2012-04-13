using HipBot.Domain;
using HipBot.Interfaces.Services;
using Moq;
using NUnit.Framework;

namespace HipBot.Handlers.Nicknames
{
    [TestFixture]
    public class RemoveNicknameTest : AutoMockingTest
    {
        private RemoveNickname handler;

        [SetUp]
        public void SetUp()
        {
            handler = Create<RemoveNickname>();
        }

        [Test]
        public void TestAddNickname()
        {
            var message = new Message();
            var room = new Room();

            message.Body = "nick remove tracy";

            handler.Receive(message, room);

            Mock<INicknameService>()
                .Verify(call => call.Remove("tracy"));
        }
    }
}
