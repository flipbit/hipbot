using System.ComponentModel.Composition;
using System.Diagnostics;
using HipBot.Domain;
using HipBot.Services;
using Sugar.Command;

namespace HipBot.Handlers.System
{
    /// <summary>
    /// Displays the current CPU usage
    /// </summary>
    [Export(typeof(IHandler))]
    public class Cpu : Handler<Cpu.Options>
    {
        [Flag("cpu")]
        public class Options {}

        /// <summary>
        /// Gets or sets the hip chat service.
        /// </summary>
        /// <value>
        /// The hip chat service.
        /// </value>
        public IHipChatService HipChatService { get; set; }

 
        public override void Receive(Message message, Room room, Options options)
        {
            var counter = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };

            HipChatService.Say(room, "CPU Usage: " + counter.NextValue() + "%");
        }
    }
}
