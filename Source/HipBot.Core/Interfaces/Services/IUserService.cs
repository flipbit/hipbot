using System.Collections.Generic;
using HipBot.Domain;

namespace HipBot.Interfaces.Services
{
    /// <summary>
    /// Interface for the User service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Lists all the users on the network.
        /// </summary>
        /// <returns></returns>
        IList<User> List();

        /// <summary>
        /// Gets the user with the given id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        User GetUser(int userId);

        /// <summary>
        /// Gets the jabber id for user with the given id.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        string GetJabberIdForUser(User user);
    }
}
