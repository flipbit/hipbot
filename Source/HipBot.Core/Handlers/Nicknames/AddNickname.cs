using HipBot.Domain;
using HipBot.Services;
using Sugar.Command;

namespace HipBot.Handlers.Nicknames
{
    /// <summary>
    /// Adds a nickname to this bot
    /// </summary>
    public class AddNickname : Handler<AddNickname.Options>
    {
        [Flag("nick")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            [Parameter("add", Required = true)]
            public string Nickname { get; set; }
        }

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
            NicknameService.Add(options.Nickname);

            HipChatService.Say(room, "Added nickname '{0}'.", options.Nickname);
        }
    }
}
