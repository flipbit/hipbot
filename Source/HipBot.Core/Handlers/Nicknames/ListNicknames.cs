using HipBot.Domain;
using HipBot.Services;
using Sugar.Command;

namespace HipBot.Handlers.Nicknames
{
    /// <summary>
    /// Adds a nickname to this bot
    /// </summary>
    public class ListNickname : Handler<ListNickname.Options>
    {
        [Flag("nick", "list")]
        public class Options { }

        #region Dependencies

        /// <summary>
        /// Gets or sets the nickname service.
        /// </summary>
        /// <value>
        /// The nickname service.
        /// </value>
        public INicknameService NicknameService { get; set; }

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
            HipChatService.Say(room, "I answer to the following names:");

            foreach (var name in NicknameService.List())
            {
                HipChatService.Say(room, name);
            }
        }
    }
}
