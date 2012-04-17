using HipBot.Core;
using HipBot.Services;
using Sugar;
using Sugar.Command;

namespace HipBot.Commands.Users
{
    /// <summary>
    /// Lists all the rooms available.
    /// </summary>
    public class ListUsers : BoundCommand<ListUsers.Options>
    {
        [Flag("user", "list")]
        public class Options {}

        /// <summary>
        /// Gets or sets the room service.
        /// </summary>
        /// <value>
        /// The room service.
        /// </value>
        public IUserService UserService { get; set; }

        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void Execute(Options options)
        {
            var users = UserService.List();

            var table = new TextTable("Name", "Email", "ID");
            table.AddSeperator();

            foreach (var user in users)
            {
                table.AddRow(user.Name, user.Email, user.Id);
            }

            Out.WriteLine(table.ToString());
        }
    }
}
