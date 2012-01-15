using Moq;
using NUnit.Framework;

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
        public void TestStoreCredentials()
        {
            
        }
    }
}
