using HipBot.Domain;

namespace HipBot.Services
{
    /// <summary>
    /// Interface for handling incoming messages.
    /// </summary>
    public interface IHandlerService
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="room"></param>
        void Handle(Message message, Room room);
    }
}
