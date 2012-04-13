using Moq;
using NUnit.Framework;

namespace HipBot.Commands.System
{
    [TestFixture]
    public class LoginTest : AutoMockingTest
    {
        private Login command;

        [SetUp]
        public void SetUp()
        {
            command = Create<Login>();
        }

        [Test]
        public void TestLogin()
        {
            var options = new Login.Options();

            command.Execute(options);
        }
    }
}
