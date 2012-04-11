using System.Collections.Generic;
using HipBot.Domain;
using HipBot.Interfaces.Handlers;
using HipBot.Interfaces.Services;

namespace HipBot.Services
{
    /// <summary>
    /// Service for handling incoming messages
    /// </summary>
    public class HandlerService : IHandlerService
    {
        #region Dependencies
        
        /// <summary>
        /// Gets or sets the handlers.
        /// </summary>
        /// <value>
        /// The handlers.
        /// </value>
        public IList<IHandler> Handlers { get; set; }

        /// <summary>
        /// Gets or sets the nickname service.
        /// </summary>
        /// <value>
        /// The nickname service.
        /// </value>
        public INicknameService NicknameService { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerService"/> class.
        /// </summary>
        public HandlerService()
        {
            Handlers = new List<IHandler>();
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="room"></param>
        public void Handle(Message message, Room room)
        {
            if (!CanHandle(room, message))
            {
                return;
            }

            foreach (var handler in Handlers)
            {
                if (!handler.CanHandle(message)) continue;

                handler.Receive(message, room);

                break;
            }
        }

        private bool CanHandle(Room room, Message message)
        {
            var canHandle = true;

            // Check message is not one-on-one message
            if (!room.IsChat)
            {
                // Check message is addressed to this bot
                if (!NicknameService.IsAddressedToMe(message))
                {
                    canHandle = false;
                }

                // Strip Bot name from message
                var index = message.Body.IndexOf(" ");
                message.Body = message.Body.Substring(index + 1);
            }

            return canHandle;
        }
    }
}
