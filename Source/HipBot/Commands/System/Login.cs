using HipBot.Core;
using HipBot.Services;
using Sugar.Command;

namespace HipBot.Commands.System
{
    /// <summary>
    /// Logs into the HipChat network
    /// </summary>
    public class Login : BoundCommand<Login.Options>
    {
        [Flag("login")]
        public class Options {}

        #region Dependencies
        
        /// <summary>
        /// Gets or sets the hip chat service.
        /// </summary>
        /// <value>
        /// The hip chat service.
        /// </value>
        public IHipChatService HipChatService { get; set; }

        /// <summary>
        /// Gets or sets the credential service.
        /// </summary>
        /// <value>
        /// The credential service.
        /// </value>
        public ICredentialService CredentialService { get; set; }

        #endregion

        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void Execute(Options options)
        {
            var credentials = CredentialService.GetCredentials();

            Out.WriteLine("Logging in as {0}...", credentials.JabberId);

            HipChatService.Login(credentials);
        }
    }
}
