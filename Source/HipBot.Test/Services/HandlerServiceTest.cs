using HipBot.Domain;
using HipBot.Handlers;
using HipBot.Interfaces.Services;
using Moq;
using NUnit.Framework;

namespace HipBot.Services
{
    [TestFixture]
    public class HandlerServiceTest : AutoMockingTest
    {
        private HandlerService service;

        [SetUp]
        public void SetUp()
        {
            service = Create<HandlerService>();

            service.Handlers.Add(new FakeHandler("one"));
            service.Handlers.Add(new FakeHandler("two"));
            service.Handlers.Add(new FakeHandler("three"));
        }

        [Test]
        public void TestHandleMessage()
        {
            var room = new Room();
            var message = new Message { Body = "two" };

            Mock<INicknameService>()
                .Setup(call => call.IsAddressedToMe(message))
                .Returns(true);

            service.Handle(message, room);

            Assert.IsFalse(((FakeHandler) service.Handlers[0]).Fired);
            Assert.IsTrue(((FakeHandler) service.Handlers[1]).Fired);
            Assert.IsFalse(((FakeHandler) service.Handlers[2]).Fired);
        }

        [Test]
        public void TestHandleMessageWhenNotAddressedToBot()
        {
            var room = new Room();
            var message = new Message { Body = "two" };

            Mock<INicknameService>()
                .Setup(call => call.IsAddressedToMe(message))
                .Returns(false);

            service.Handle(message, room);

            Assert.IsFalse(((FakeHandler)service.Handlers[0]).Fired);
            Assert.IsFalse(((FakeHandler)service.Handlers[1]).Fired);
            Assert.IsFalse(((FakeHandler)service.Handlers[2]).Fired);
        }

        [Test]
        public void TestHandleMessageWhenAliased()
        {
            var room = new Room();
            var message = new Message { Body = "two" };

            Mock<INicknameService>()
                .Setup(call => call.IsAddressedToMe(message))
                .Returns(true);

            Mock<IAliasService>()
                .Setup(call => call.IsAlias("two"))
                .Returns(true);

            Mock<IAliasService>()
                .Setup(call => call.GetAlias("two"))
                .Returns("three");

            service.Handle(message, room);

            Assert.IsFalse(((FakeHandler)service.Handlers[0]).Fired);
            Assert.IsFalse(((FakeHandler)service.Handlers[1]).Fired);
            Assert.IsTrue(((FakeHandler)service.Handlers[2]).Fired);
        }

        [Test]
        public void TestHandleMessageWhenAliasedButBypassed()
        {
            var room = new Room();
            var message = new Message { Body = "!two" };

            Mock<INicknameService>()
                .Setup(call => call.IsAddressedToMe(message))
                .Returns(true);

            Mock<IAliasService>()
                .Setup(call => call.IsAlias("!two"))
                .Returns(false);

            service.Handle(message, room);

            Assert.IsFalse(((FakeHandler)service.Handlers[0]).Fired);
            Assert.IsTrue(((FakeHandler)service.Handlers[1]).Fired);
            Assert.IsFalse(((FakeHandler)service.Handlers[2]).Fired);
        }
    }
}
