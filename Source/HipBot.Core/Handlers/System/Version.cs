using System.IO;
using HipBot.Domain;
using HipBot.Interfaces.Services;
using Sugar.Command;

namespace HipBot.Handlers.System
{
    /// <summary>
    /// Says hello to the sender
    /// </summary>
    public class Version : Handler<Version.Options>
    {
        [Flag("version")]
        public class Options {}

        /// <summary>
        /// Gets or sets the hip chat service.
        /// </summary>
        /// <value>
        /// The hip chat service.
        /// </value>
        public IHipChatService HipChatService { get; set; }

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="room">The room.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Room room, Options options)
        {
            var assembly = typeof (Version).Assembly.GetName().Version;
            var location = typeof (Version).Assembly.Location;
            var built = File.GetLastWriteTime(location);

            HipChatService.Say(room, "Version: {0}", assembly);
            HipChatService.Say(room, "Built: {0:dd MMM yyyy} at {0:HH:mm}", built);
        }
    }
}
