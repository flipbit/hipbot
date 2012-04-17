using System.Collections.Generic;
using HipBot.Domain;
using HipBot.Handlers;

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

        /// <summary>
        /// Gets or sets the alias service.
        /// </summary>
        /// <value>
        /// The alias service.
        /// </value>
        public IAliasService AliasService { get; set; }

        /// <summary>
        /// Gets or sets the hip chat service.
        /// </summary>
        /// <value>
        /// The hip chat service.
        /// </value>
        public IHipChatService HipChatService { get; set; }

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
            // Check bot can handle this message
            if (!CanHandle(message, room))
            {
                return;
            }

            // Check for aliases
            if (AliasService.IsAlias(message.Body))
            {
                message.Body = AliasService.GetAlias(message.Body);
            }

            // Remove Alias bypass
            if (message.Body.StartsWith("!"))
            {
                message.Body = message.Body.Substring(1);
            }

            var handled = false;

            // Check each handler
            foreach (var handler in Handlers)
            {
                if (!handler.CanHandle(message)) continue;

                handler.Receive(message, room);

                handled = true;

                break;
            }

            if (!handled)
            {
                HipChatService.Say(room, "I didn't understand: {0}", message.Body);
            }
        }

        /// <summary>
        /// Determines whether this instance can handle the specified message in the room.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="room">The room.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified room; otherwise, <c>false</c>.
        /// </returns>
        private bool CanHandle(Message message, Room room)
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
