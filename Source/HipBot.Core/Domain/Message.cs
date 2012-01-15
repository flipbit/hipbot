using System;

namespace HipBot.Domain
{
    public class Message
    {
        /// <summary>
        /// Gets or sets the name of the user who this message is from.
        /// </summary>
        /// <value>
        /// </value>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the room.
        /// </summary>
        /// <value>
        /// The room.
        /// </value>
        public string Room { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public string To { get; set; }

        /// <summary>
        /// Gets or sets the received.
        /// </summary>
        /// <value>
        /// The received.
        /// </value>
        public DateTime Received { get; set; }
    }
}
