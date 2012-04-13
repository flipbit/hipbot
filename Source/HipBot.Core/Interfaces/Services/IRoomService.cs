using System.Collections.Generic;
using HipBot.Domain;

namespace HipBot.Interfaces.Services
{
    /// <summary>
    /// Service interface for HipChat rooms.
    /// </summary>
    public interface IRoomService
    {
        /// <summary>
        /// Gets all the rooms.
        /// </summary>
        /// <returns></returns>
        IList<Room> List();

        /// <summary>
        /// Joins the room with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        bool Join(string name);

        /// <summary>
        /// Leaves the room with the specified name.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <returns></returns>
        bool Leave(string name);

        /// <summary>
        /// Reconnects all rooms on login.
        /// </summary>
        void Reconnect();

    }
}
