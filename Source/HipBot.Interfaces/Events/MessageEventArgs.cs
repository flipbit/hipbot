using System;
using HipBot.Domain;

namespace HipBot.Events
{
    /// <summary>
    /// Event arguments for when a message is received.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public Message Message { get; set; }

        /// <summary>
        /// Gets or sets the room this message was recieved from.
        /// </summary>
        /// <value>
        /// The room.
        /// </value>
        public Room Room { get; set; }
    }
}
