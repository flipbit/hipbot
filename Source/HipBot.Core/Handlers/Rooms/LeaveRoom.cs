using HipBot.Domain;
using HipBot.Interfaces.Services;
using Sugar.Command;

namespace HipBot.Handlers.Rooms
{
    /// <summary>
    /// Adds a nickname to this bot
    /// </summary>
    public class LeaveRoom : Handler<LeaveRoom.Options>
    {
        [Flag("room")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            [Parameter("leave", Required = true)]
            public string Room { get; set; }
        }

        #region Dependencies

        /// <summary>
        /// Gets or sets the room service.
        /// </summary>
        /// <value>
        /// The room service.
        /// </value>
        public IRoomService RoomService { get; set; }

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
            RoomService.Leave(options.Room);

            HipChatService.Say(room, "Leaving room '{0}'.", options.Room);
        }
    }
}
