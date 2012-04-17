using HipBot.Domain;
using Sugar.Command;

namespace HipBot.Handlers
{
    /// <summary>
    /// Base Handler for handling incoming messages to the Bot.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Handler<T> : IHandler where T : class, new()
    {
        private T options;

        /// <summary>
        /// Determines whether this instance can handle the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified message; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(Message message)
        {
            var parameters = new Parameters(message.Body);
            parameters.Switches.Clear();

            options = new ParameterBinder().Bind<T>(parameters);

            return options != null;
        }

        /// <summary>
        /// Occurs when a message is received from the given room.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="room"></param>
        public void Receive(Message message, Room room)
        {
            if (options == null)
            {
                CanHandle(message);
            }

            Receive(message, room, options);
        }

        /// <summary>
        /// Occurs when a message is received from the given room.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="room">The room.</param>
        /// <param name="options">The options.</param>
        public abstract void Receive(Message message, Room room, T options);
    }
}
