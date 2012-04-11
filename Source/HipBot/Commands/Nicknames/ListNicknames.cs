using HipBot.Core;
using HipBot.Interfaces.Services;
using Sugar.Command;

namespace HipBot.Commands.Nicknames
{
    /// <summary>
    /// Sets the bot nick name.
    /// </summary>
    public class ListNickNames : BoundCommand<ListNickNames.Options>
    {
        [Flag("nick", "list")]
        public class Options
        {
        }

        /// <summary>
        /// Gets or sets the nickname service.
        /// </summary>
        /// <value>
        /// The nickname service.
        /// </value>
        public INicknameService NicknameService { get; set; }

        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void Execute(Options options)
        {
            var nicks = NicknameService.List();

            Out.WriteLine("Nicknames associated with this bot:");

            foreach (var nick in nicks)
            {
                Out.WriteLine(nick);
            }
        }
    }
}
