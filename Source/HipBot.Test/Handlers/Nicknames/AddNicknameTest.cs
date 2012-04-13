using HipBot.Domain;
using HipBot.Interfaces.Services;
using Moq;
using NUnit.Framework;

namespace HipBot.Handlers.Nicknames
{
    [TestFixture]
    public class AddNicknameTest : AutoMockingTest
    {
        private AddNickname handler;

        [SetUp]
        public void SetUp()
        {
            handler = Create<AddNickname>();
        }

        [Test]
        public void TestAddNickname()
        {
            var message = new Message();
            var room = new Room();

            message.Body = "nick add bob";

            handler.Receive(message, room);

            Mock<INicknameService>()
                .Verify(call => call.Add("bob"));
        }
    }
}
