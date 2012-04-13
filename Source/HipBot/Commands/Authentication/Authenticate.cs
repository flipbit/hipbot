using HipBot.Core;
using HipBot.Interfaces.Services;
using Sugar.Command;

namespace HipBot.Commands.Authentication
{
    /// <summary>
    /// Sets the bots authentication credentials.
    /// </summary>
    public class Authenticate : BoundCommand<Authenticate.Options>
    {
        public class Options
        {
            /// <summary>
            /// Gets or sets the API token to use.
            /// </summary>
            /// <value>
            /// The token.
            /// </value>
            [Parameter("auth", Required = true)]
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
            var credentials = CredentialService.GetCredentials();

            credentials.ApiToken = options.ApiToken;

            CredentialService.SetCredentials(credentials);

            Out.WriteLine("Set API Token.");
        }
    }
}
