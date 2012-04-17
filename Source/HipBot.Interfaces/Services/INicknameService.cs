using System.Collections.Generic;
using HipBot.Domain;

namespace HipBot.Services
{
    /// <summary>
    /// Service to manage this bot's nicknames
    /// </summary>
    public interface INicknameService
    {
        /// <summary>
        /// Adds the specified nickname to this bot.
        /// </summary>
        /// <param name="nickname">The nickname.</param>
        void Add(string nickname);

        /// <summary>
        /// Removes the specified nickname from this bot.
        /// </summary>
        /// <param name="nickname">The nickname.</param>
        void Remove(string nickname);

        /// <summary>
        /// Lists the nicknames this bot will respond to.
        /// </summary>
        /// <returns></returns>
        IList<string> List();

        /// <summary>
        /// Determines whether the specified message is addressed to me.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <c>true</c> if the specified message is addressed to me; otherwise, <c>false</c>.
        /// </returns>
        bool IsAddressedToMe(Message message);
    }
}
