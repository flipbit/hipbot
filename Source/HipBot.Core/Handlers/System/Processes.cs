using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HipBot.Domain;
using HipBot.Services;
using Sugar.Command;

namespace HipBot.Handlers.System
{
    /// <summary>
    /// Displays the current processes running
    /// </summary>
    [Export(typeof(IHandler))]
    public class Processes : Handler<Processes.Options>
    {
        [Flag("ps")]
        public class Options {}

        private class ProcessInfo
        {
            public DateTime StartTime { get; set; }

            public DateTime ActiveTime { get; set; }

            public string Name { get; set; }
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
            var sb = new StringBuilder();
            var now = DateTime.Now;
            var infos = new List<ProcessInfo>();

            var processes = Process.GetProcesses();

            foreach (var process in processes)
            {
                var info = new ProcessInfo();

                try
                {
                    info.Name = process.ProcessName;
                    info.ActiveTime = new DateTime(process.TotalProcessorTime.Ticks);
                    info.StartTime = new DateTime((now - process.StartTime).Ticks);
                }
                catch
                {
                    info.StartTime = new DateTime(1, 1, 2, 0, 0, 0);
                }

                infos.Add(info);
            }

            foreach (var info in infos.OrderByDescending(i => i.StartTime))
            {
                sb.AppendFormat("{0:HH:mm:ss} {1:HH:mm:ss} {2}", info.StartTime, info.ActiveTime, info.Name);

                sb.AppendLine(string.Empty);
            }

            HipChatService.SayHtml(room, sb.ToString(), true);
        }
    }
}
