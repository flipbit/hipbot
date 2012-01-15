namespace HipBot.Domain
{
    /// <summary>
    /// Represents a set of credentials used to connect to the 
    /// HipChat service.
    /// </summary>
    public class Credentials
    {
        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Jabber ID.
        /// </summary>
        /// <value>
        /// The Jabber ID.
        /// </value>
        public string JabberId { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the API token.
        /// </summary>
        /// <value>
        /// The API token.
        /// </value>
        public string ApiToken { get; set; }
    }
}
