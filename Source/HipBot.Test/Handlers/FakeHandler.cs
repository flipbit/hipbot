using HipBot.Domain;

namespace HipBot.Handlers
{
    internal class FakeHandler : IHandler
    {
        private readonly string respondsTo;

        public FakeHandler(string respondsTo)
        {
            this.respondsTo = respondsTo;
        }

        public bool Fired { get; private set; }

        public bool CanHandle(Message message)
        {
            return message.Body == respondsTo;
        }

        public void Receive(Message message, Room room)
        {
            Fired = true;
        }
    }
}
