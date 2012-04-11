using System.ComponentModel.Composition;
using System.Diagnostics;
using HipBot.Core;
using HipBot.Domain;
using HipBot.Interfaces.Handlers;
using HipBot.Interfaces.Services;
using Sugar.Command;

namespace HipBot.Handlers
{
    /// <summary>
    /// Displays the current CPU usage
    /// </summary>
    [Export(typeof(IHandler))]
    public class Exec : Handler<Exec.Options>
    {
        [Flag("exec")]
        public class Options
        {
            [Parameter("-file")]
            public string FileName { get; set; }

            [Parameter("-args")]
            public string Arguments { get; set; }
        }

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
            var process = new Process
            {              
                StartInfo =
                {
                    UseShellExecute = false, 
                    RedirectStandardOutput = true,
                    FileName = options.FileName,
                    Arguments = options.Arguments
                }
            };

            process.OutputDataReceived += (sender, args) => HipChatService.Say(room, args.Data);

            Out.WriteLine("Starting Process");

            process.Start();
            process.BeginOutputReadLine();

            process.WaitForExit();

            Out.WriteLine("Finished Process");
        }
    }
}
