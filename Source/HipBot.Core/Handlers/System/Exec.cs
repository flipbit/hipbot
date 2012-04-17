using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Text;
using HipBot.Core;
using HipBot.Domain;
using HipBot.Services;
using Sugar;
using Sugar.Command;

namespace HipBot.Handlers.System
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

        private IList<string> lines = new List<string>();

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
            HipChatService.Say(room, "Starting process..");

            lines.Clear();

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

            process.OutputDataReceived += process_OutputDataReceived;

            Out.WriteLine("Starting Process");

            process.Start();
            process.BeginOutputReadLine();

            process.WaitForExit();

            Out.WriteLine("Finished Process");

            var result = lines.Join(Environment.NewLine);

            HipChatService.SayHtml(room, result);
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                lines.Add(e.Data);

                if (lines.Count > 30)
                {
                    lines.RemoveAt(0);
                }
            }
        }
    }
}
