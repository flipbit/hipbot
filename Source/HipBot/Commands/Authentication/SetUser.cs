using HipBot.Core;
using HipBot.Interfaces.Services;
using Sugar.Command;

namespace HipBot.Commands.Authentication
{
    /// <summary>
    /// Sets the bots authentication credentials.
    /// </summary>
    public class SetUser : BoundCommand<SetUser.Options>
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
            [Parameter("user", Required = true)]
            public int UserId { get; set; }
        }

        /// <summary>
        /// Gets or sets the credential service.
        /// </summary>
        /// <value>
        /// The credential service.
        /// </value>
        public ICredentialService CredentialService { get; set; }

        /// <summary>
        /// Gets or sets the user service.
        /// </summary>
        /// <value>
        /// The user service.
        /// </value>
        public IUserService UserService { get; set; }

        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void Execute(Options options)
        {
            var credentials = CredentialService.GetCredentials();
            var user = UserService.GetUser(options.UserId);

            if (user == null)
            {
                Out.WriteLine("Couldn't find user with id {0}.", options.UserId);

                return;
            }

            credentials.Name = user.Name;
            credentials.JabberId = UserService.GetJabberIdForUser(user);

            CredentialService.SetCredentials(credentials);

            Out.WriteLine("Bot will use {0} ({1}).", credentials.Name, credentials.JabberId);
        }
    }
}
