using HipBot.Services;
using Sugar.Command;

namespace HipBot.Commands.Nicknames
{
    /// <summary>
    /// Sets the bot nick name.
    /// </summary>
    public class AddNickName : BoundCommand<AddNickName.Options>
    {
        [Flag("nick")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the jabber id.
            /// </summary>
            /// <value>
            /// The jabber id.
            /// </value>
            [Parameter("add", Required = true)]
            public string Name { get; set; }
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
            NicknameService.Add(options.Name);
        }
    }
}
