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
    }
}
