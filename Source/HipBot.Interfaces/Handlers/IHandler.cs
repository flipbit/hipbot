using HipBot.Domain;

namespace HipBot.Handlers
{
    /// <summary>
    /// Interface to define the handling of incomming messages
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// Determines whether this instance can handle the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified message; otherwise, <c>false</c>.
        /// </returns>
        bool CanHandle(Message message);

        /// <summary>
        /// Occurs when a message is received from the given room.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="room"></param>
        void Receive(Message message, Room room);
    }
}
