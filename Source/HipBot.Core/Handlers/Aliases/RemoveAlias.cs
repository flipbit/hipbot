using HipBot.Domain;
using HipBot.Services;
using Sugar.Command;

namespace HipBot.Handlers.Aliases
{
    /// <summary>
    /// Adds a nickname to this bot
    /// </summary>
    public class RemoveAlias : Handler<RemoveAlias.Options>
    {
        [Flag("alias", "remove")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            [Parameter("name", Required = true)]
            public string Name { get; set; }
        }

        #region Dependencies
        
        /// <summary>
        /// Gets or sets the alias service.
        /// </summary>
        /// <value>
        /// The nickname service.
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
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="room">The room.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Room room, Options options)
        {
            AliasService.RemoveAlias(options.Name);

            HipChatService.Say(room, "Removed alias '{0}'.", options.Name);
        }
    }
}
