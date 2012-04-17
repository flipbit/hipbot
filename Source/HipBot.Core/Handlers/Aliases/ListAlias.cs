using HipBot.Domain;
using HipBot.Services;
using Sugar.Command;

namespace HipBot.Handlers.Aliases
{
    /// <summary>
    /// Adds a nickname to this bot
    /// </summary>
    public class ListAlias : Handler<ListAlias.Options>
    {
        [Flag("alias", "list")]
        public class Options {}

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
            var aliases = AliasService.ListAliases();

            foreach (var alias in aliases)
            {
                HipChatService.Say(room, alias);
            }

            HipChatService.Say(room, "{0} aliases.", aliases.Count);
        }
    }
}
