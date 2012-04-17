using HipBot.Core;
using HipBot.Services;
using Sugar;
using Sugar.Command;

namespace HipBot.Commands.Rooms
{
    /// <summary>
    /// Lists all the rooms available.
    /// </summary>
    public class ListRooms : BoundCommand<ListRooms.Options>
    {
        [Flag("room", "list")]
        public class Options {}

        /// <summary>
        /// Gets or sets the room service.
        /// </summary>
        /// <value>
        /// The room service.
        /// </value>
        public IRoomService RoomService { get; set; }

        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void Execute(Options options)
        {
            var rooms = RoomService.List();

            var table = new TextTable("Name", "Jabber ID");
            table.AddSeperator();

            foreach (var room in rooms)
            {
                table.AddRow(room.Name, room.JabberId);
            }

            Out.WriteLine(table.ToString());
        }
    }
}
