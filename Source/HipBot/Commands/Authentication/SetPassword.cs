using HipBot.Core;
using HipBot.Services;
using Sugar.Command;

namespace HipBot.Commands.Authentication
{
    /// <summary>
    /// Sets the bots authentication credentials.
    /// </summary>
    public class SetPassword : BoundCommand<SetPassword.Options>
    {
        [Flag("set")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the jabber id.
            /// </summary>
            /// <value>
            /// The jabber id.
            /// </value>
            [Parameter("password", Required = true)]
            public string Password { get; set; }
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

            credentials.Password = options.Password;

            CredentialService.SetCredentials(credentials);

            Out.WriteLine("Password set.");
        }
    }
}
