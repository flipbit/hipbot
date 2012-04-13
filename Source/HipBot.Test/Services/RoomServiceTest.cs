using System;
using System.IO;
using HipBot.Domain;
using HipBot.Interfaces.Services;
using Moq;
using NUnit.Framework;
using Sugar.Net;

namespace HipBot.Services
{
    [TestFixture]
    public class RoomServiceTest : AutoMockingTest
    {
        private RoomService service;

        [SetUp]
        public void SetUp()
        {
            service = Create<RoomService>();
        }

        [Test]
        public void TestListUsers()
        {
            var response = new FakeHttpResponse(File.ReadAllText("../../Examples/RoomList.xml"));
            var credentials = new Credentials { ApiToken = "ABC123" };

            Mock<ICredentialService>()
                .Setup(call => call.GetCredentials())
                .Returns(credentials);

            Mock<IHttpService>()
                .Setup(call => call.Get("https://api.hipchat.com/v1/rooms/list?format=xml&auth_token=ABC123"))
                .Returns(response);

            var rooms = service.List();

            Assert.AreEqual(2, rooms.Count);
            Assert.AreEqual(7, rooms[0].Id);
            Assert.AreEqual("Development", rooms[0].Name);
            Assert.AreEqual("Make sure to document your API functions well!", rooms[0].Topic);
            Assert.AreEqual(new DateTime(1970, 1, 1), rooms[0].LastActive);
            Assert.AreEqual(new DateTime(2010, 3, 19, 14, 51, 51), rooms[0].Created);
            Assert.AreEqual(1, rooms[0].OwnerUserId);
            Assert.AreEqual(false, rooms[0].IsArchived);
            Assert.AreEqual(false, rooms[0].IsPrivate);
            Assert.AreEqual("7_development", rooms[0].JabberId);
        }
    }
}
