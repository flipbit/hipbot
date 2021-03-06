﻿using HipBot.Domain;
using HipBot.Services;
using Sugar.Command;

namespace HipBot.Handlers.Rooms
{
    /// <summary>
    /// Adds a nickname to this bot
    /// </summary>
    public class JoinRoom : Handler<JoinRoom.Options>
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
            [Parameter("join", Required = true)]
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
            RoomService.Join(options.Room);

            HipChatService.Say(room, "Joined room '{0}'.", options.Room);
        }
    }
}
