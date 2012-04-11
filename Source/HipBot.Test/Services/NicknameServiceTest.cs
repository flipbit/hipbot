using Moq;
using NUnit.Framework;

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
    }
}
