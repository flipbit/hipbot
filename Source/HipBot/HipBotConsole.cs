using System;
using System.Linq;
using System.Threading;
using HipBot.Core;
using HipBot.Events;
using HipBot.Interfaces.Services;
using Sugar.Command;

namespace HipBot
{
    /// <summary>
    /// The HipBot console
    /// </summary>
    public class HipBotConsole : BaseCommandConsole
    {
        /// <summary>
        /// Gets or sets the hip chat service.
        /// </summary>
        /// <value>
        /// The hip chat service.
        /// </value>
        public IHipChatService HipChatService { get; set; }

        /// <summary>
        /// Gets or sets the room service.
        /// </summary>
        /// <value>
        /// The room service.
        /// </value>
        public IRoomService RoomService { get; set; }

        /// <summary>
        /// Main entry point for the console.
        /// </summary>
        protected override void Main()
        {
            HipChatService.OnLogin += HipChatService_OnLogin;
            HipChatService.OnMessage += HipChatService_OnMessage;

            base.Main();
        }

        /// <summary>
        /// Handles the OnMessage event of the HipChatService.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="HipBot.Events.MessageEventArgs"/> instance containing the event data.</param>
        void HipChatService_OnMessage(object sender, MessageEventArgs e)
        {
            Out.WriteLine("{0:HH:mm:ss} [{1}] {2}", e.Message.Received, e.Message.From, e.Message.Body);
        }

        /// <summary>
        /// Handles the OnLogin event of the HipChatService.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void HipChatService_OnLogin(object sender, EventArgs e)
        {
            Out.WriteLine("{0:HH:mm:ss} Logged in.", DateTime.Now);

            RoomService.Reconnect();
        }

        /// <summary>
        /// The interactive HipBot command line.
        /// </summary>
        public override void Default()
        {
            In.OnInput += In_OnInput;

            In.Listen();

            Console.WriteLine("HipBot.");
            Console.Write(">");

            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        void In_OnInput(OnInputArgs args)
        {
            Console.WriteLine(string.Empty);

            SetArguments(args.Input);

            if (!Execute())
            {
                Console.WriteLine("Unknown command: {0}", Arguments.FirstOrDefault());
            }

            Console.Write(">");
        }      
    }
}
