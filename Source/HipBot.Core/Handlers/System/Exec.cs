using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
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
        public class Options
        {
            /// <summary>
            /// Gets or sets the name of the file.
            /// </summary>
            /// <value>
            /// The name of the file.
            /// </value>
            [Parameter("exec", Required = true)]
            public string FileName { get; set; }

            /// <summary>
            /// Gets or sets the arguments.
            /// </summary>
            /// <value>
            /// The arguments.
            /// </value>
            [Parameter("args")]
            public string Arguments { get; set; }

            /// <summary>
            /// Gets or sets the status.
            /// </summary>
            /// <value>
            /// The status.
            /// </value>
            [Parameter("status")]
            public string Status { get; set; }
        }

        private readonly IList<string> lines = new List<string>();
        private bool executing;

        #region Dependencies
        
        /// <summary>
        /// Gets or sets the hip chat service.
        /// </summary>
        /// <value>
        /// The hip chat service.
        /// </value>
        public IHipChatService HipChatService { get; set; }

        #endregion

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="room">The room.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Room room, Options options)
        {
            if (executing)
            {
                HipChatService.Say(room, "Can't you see, I'm busy!");

                return;                
            }

            var start = DateTime.Now;

            HipChatService.Say(room, "Starting: {0}", options.FileName);
            HipChatService.SetStatus(Status.Busy, options.Status);

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

            try
            {
                executing = true;

                Out.WriteLine("Starting Process");

                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();

                Out.WriteLine("Finished Process");

                var time = DateTime.Now - start;
                HipChatService.Say(room, "Finished process in {0}:{1:00} minutes.", time.Minutes, time.Seconds);
            }
            catch (Exception ex)
            {
                Out.WriteLine("Error: " + ex.Message);
                HipChatService.Say(room, "Error: " + ex.Message);
            }
            finally
            {
                executing = false;
                HipChatService.SetStatus(Status.Available, string.Empty);                
            }

            var result = lines.Join(Environment.NewLine);
            HipChatService.SayHtml(room, result);
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                var chars = Enumerable
                    .Range(0, char.MaxValue + 1)
                    .Select(i => (char)i)
                    .Where(c => !char.IsControl(c))
                    .ToArray();

                var toKeep = new string(chars);

                var line = e.Data.Keep(toKeep).TrimTo(80, string.Empty);

                lines.Add(line);

                if (lines.Count > 30)
                {
                    lines.RemoveAt(0);
                }
            }
        }
    }
}
