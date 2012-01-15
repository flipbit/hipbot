using HipBot.Core;
using Sugar.Command;

namespace HipBot
{
    public class Program
    {
        /// <summary>
        /// Main entry point for the HipBot program logic.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            // Register types
            Stencil.Defaults.Assemblies.Add(typeof(Program).Assembly);
            Stencil.Defaults.Assemblies.Add(typeof(Stencil).Assembly);
            Stencil.Defaults.Assemblies.Add(typeof(ICommand).Assembly);

            // Get Console
            var console = Stencil.Instance.Resolve<HipBotConsole>();

            // Go!
            console.Run(args);
        }
    }
}
