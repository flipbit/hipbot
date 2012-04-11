using System.ComponentModel.Composition;
using HipBot.Domain;
using HipBot.Interfaces.Handlers;
using HipBot.Interfaces.Services;
using Sugar.Command;

namespace HipBot.Handlers
{
    /// <summary>
    /// Displays the current processes running
    /// </summary>
    [Export(typeof(IHandler))]
    public class Recycle : Handler<Recycle.Options>
    {
        [Flag("recycle")]
        public class Options {}
        
        #region Dependencies

        /// <summary>
        /// Gets or sets the hip chat service.
        /// </summary>
        /// <value>
        /// The hip chat service.
        /// </value>
        public IHipChatService HipChatService { get; set; }

        /// <summary>
        /// Gets or sets the update service.
        /// </summary>
        /// <value>
        /// The update service.
        /// </value>
        public IUpdateService UpdateService { get; set; }

        #endregion

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="room">The room.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Room room, Options options)
        {
            HipChatService.Say(room, "Recycling bot.");

            UpdateService.RunLatestVersion(false);
        }
    }
}
