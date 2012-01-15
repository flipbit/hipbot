using System;

namespace HipBot.Domain
{
    /// <summary>
    /// Represents a Room on the HipChat network.
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Gets or sets the room id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the topic.
        /// </summary>
        /// <value>
        /// The topic.
        /// </value>
        public string Topic { get; set; }

        /// <summary>
        /// Gets or sets the last active.
        /// </summary>
        /// <value>
        /// The last active.
        /// </value>
        public DateTime LastActive { get; set; }

        /// <summary>
        /// Gets or sets the owner id.
        /// </summary>
        /// <value>
        /// The owner id.
        /// </value>
        public int OwnerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is archived.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is archived; otherwise, <c>false</c>.
        /// </value>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the jabber id.
        /// </summary>
        /// <value>
        /// The jabber id.
        /// </value>
        public string JabberId { get; set; }
    }
}
