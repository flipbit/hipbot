using System;
using HipBot.Core;
using HipBot.Domain;
using HipBot.Interfaces.Services;
using Sugar.Command;

namespace HipBot.Commands
{
    /// <summary>
    /// Sets the bots authentication credentials.
    /// </summary>
    public class SetCredentials : BoundCommand<SetCredentials.Options>
    {
        [Flag("auth")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the jabber id.
            /// </summary>
            /// <value>
            /// The jabber id.
            /// </value>
            [Parameter("-id", Required = true)]
            public string JabberId { get; set; }

            /// <summary>
            /// Gets or sets the password.
            /// </summary>
            /// <value>
            /// The password.
            /// </value>
            [Parameter("-pass", Required = true)]
            public string Password { get; set; }

            /// <summary>
            /// Gets or sets the API token.
            /// </summary>
            /// <value>
            /// The API token.
            /// </value>
            [Parameter("-token", Required = false, Default = "n/a")]
            public string ApiToken { get; set; }
        }

        /// <summary>
        /// Gets or sets the credential service.
        /// </summary>
        /// <value>
        /// The credential service.
        /// </value>
        public ICredentialService CredentialService { get; set; }

        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void Execute(Options options)
        {
            var credentials = new Credentials
            {
                JabberId = options.JabberId,
                Password = options.Password,
                ApiToken = options.ApiToken
            };

            CredentialService.SetCredentials(credentials);

            Out.WriteLine("Saved credentials");
        }
    }
}
