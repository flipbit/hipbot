using System;
using HipBot.Domain;
using HipBot.Events;

namespace HipBot.Interfaces.Services
{
    /// <summary>
    /// Interface for accessing the HipChat network.
    /// </summary>
    public interface IHipChatService
    {
        /// <summary>
        /// Logs in with the specified credentials.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        void Login(Credentials credentials);

        /// <summary>
        /// Gets a value indicating whether this instance is logged in.
        /// </summary>
        /// <value>
        ///   <c>true</c> if logged in; otherwise, <c>false</c>.
        /// </value>
        bool LoggedIn { get; }

        /// <summary>
        /// Joins the specified room.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <returns></returns>
        bool Join(Room room);

        /// <summary>
        /// Says the message in the specified room.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <param name="message">The message.</param>
        /// <param name="html">if set to <c>true</c> HTML formatted message</param>
        void Say(Room room, string message, bool html = false);

        event EventHandler<LoginEventArgs> OnLogin;

        event EventHandler<MessageEventArgs> OnMessage;
    }
}
