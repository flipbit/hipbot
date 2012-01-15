using HipBot.Core;
using HipBot.Interfaces.Services;
using Sugar.Command;

namespace HipBot.Commands
{
    /// <summary>
    /// Lists all the rooms available.
    /// </summary>
    public class ListRoomsCommand : BoundCommand<ListRoomsCommand.Options>
    {
        [Flag("list", "rooms")]
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
            var rooms = RoomService.GetRooms();

            foreach (var room in rooms)
            {
                Out.WriteLine(room.Name);
            }
        }
    }
}
