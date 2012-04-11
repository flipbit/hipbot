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
    public class Update : Handler<Update.Options>
    {
        [Flag("update")]
        public class Options {}

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

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="room">The room.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Room room, Options options)
        {
            if (!UpdateService.IsNewVersionAvailable())
            {
                HipChatService.Say(room, "No new version is available.");
            }
            else
            {
                HipChatService.Say(room, "Downloading new version of bot...");

                UpdateService.DownloadUpdate();

                HipChatService.Say(room, "New version downloaded.  Recycle me to upgrade.");                
            }
        }
    }
}
