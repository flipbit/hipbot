using System.IO;
using HipBot.Domain;
using Moq;
using NUnit.Framework;
using Sugar.Net;

namespace HipBot.Services
{
    [TestFixture]
    public class UserServiceTest : AutoMockingTest
    {
        private UserService service;

        [SetUp]
        public void SetUp()
        {
            service = Create<UserService>();
        }

        [Test]
        public void TestListUsers()
        {
            var response = new FakeHttpResponse(File.ReadAllText("../../Examples/UserList.xml"));
            var credentials = new Credentials { ApiToken = "ABC123" };

            Mock<ICredentialService>()
                .Setup(call => call.GetCredentials())
                .Returns(credentials);

            Mock<IHttpService>()
                .Setup(call => call.Get("https://api.hipchat.com/v1/users/list?format=xml&auth_token=ABC123"))
                .Returns(response);

            var users = service.List();

            Assert.AreEqual(3, users.Count);
            Assert.AreEqual(1, users[0].Id);
            Assert.AreEqual("Chris Rivers", users[0].Name);
            Assert.AreEqual("chris@hipchat.com", users[0].Email);
            Assert.AreEqual("Developer", users[0].Title);
            Assert.AreEqual("https://www.hipchat.com/chris.png", users[0].PhotoUrl);
            Assert.AreEqual("away", users[0].Status);
            Assert.AreEqual("gym, bbl", users[0].StatusMessage);
            Assert.AreEqual(true, users[0].IsGroupAdmin);
        }


        [Test]
        public void TestGetUser()
        {
            var response = new FakeHttpResponse(File.ReadAllText("../../Examples/UserShow.xml"));
            var credentials = new Credentials { ApiToken = "ABC123" };

            Mock<ICredentialService>()
                .Setup(call => call.GetCredentials())
                .Returns(credentials);

            Mock<IHttpService>()
                .Setup(call => call.Get("https://api.hipchat.com/v1/users/show?user_id=5&format=xml&auth_token=ABC123"))
                .Returns(response);

            var user = service.GetUser(5);

            Assert.AreEqual(5, user.Id);
            Assert.AreEqual("Garret Heaton", user.Name);
            Assert.AreEqual("garret@hipchat.com", user.Email);
            Assert.AreEqual("Co-founder", user.Title);
            Assert.AreEqual("https://www.hipchat.com/img/silhouette_125.png", user.PhotoUrl);
            Assert.AreEqual("available", user.Status);
            Assert.AreEqual("Come see what I'm working on!", user.StatusMessage);
            Assert.AreEqual(true, user.IsGroupAdmin);
        }

        [Test]
        public void TestGetJabberIdForUser()
        {
            var user = new User { Id = 1234 };
            var response = new FakeHttpResponse(File.ReadAllText("../../Examples/RoomList.xml"));
            var credentials = new Credentials { ApiToken = "ABC123" };

            Mock<ICredentialService>()
                .Setup(call => call.GetCredentials())
                .Returns(credentials);

            Mock<IHttpService>()
                .Setup(call => call.Get("https://api.hipchat.com/v1/rooms/list?format=xml&auth_token=ABC123"))
                .Returns(response);

            var jabberId = service.GetJabberIdForUser(user);

            Assert.AreEqual("7_1234", jabberId);
        }
    }
}
