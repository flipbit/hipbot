using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using HipBot.Core;
using HipBot.Interfaces.Handlers;
using HipBot.Services;
using Sugar.Command;

namespace HipBot
{
    public class Program
    {
        [ImportMany(typeof(IHandler))]
        public IList<IHandler> Handlers = new List<IHandler>();

        /// <summary>
        /// Gets the directory of the current process.
        /// </summary>
        /// <returns></returns>
        private static string GetDirectory()
        {
            var location = Assembly.GetExecutingAssembly().Location;

            return Path.GetDirectoryName(location);
        }

        public void DoImport()
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();

            //Adds all the parts found in all assemblies in 
            //the same directory as the executing program
            catalog.Catalogs.Add(new DirectoryCatalog(GetDirectory()));

            //Create the CompositionContainer with the parts in the catalog
            var container = new CompositionContainer(catalog);

            //Fill the imports of this object
            container.ComposeParts(this);
        }

        /// <summary>
        /// Starts the HipBot application
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public void Initialize(string[] args)
        {
            // Load MEF
            DoImport();

            // Register types
            Stencil.Defaults.Assemblies.Add(typeof(Program).Assembly);
            Stencil.Defaults.Assemblies.Add(typeof(Stencil).Assembly);
            Stencil.Defaults.Assemblies.Add(typeof(ICommand).Assembly);

            // Register handlers
            foreach (var handler in Handlers)
            {
                Stencil.Defaults.Types.Add(handler.GetType());
            }

            // Run latest version
            var service = Stencil.Instance.Resolve<UpdateService>();
            service.RunLatestVersion(true, false);

            // Get Console
            var console = Stencil.Instance.Resolve<HipBotConsole>();

            // Go!
            console.Run(args);
        }

        /// <summary>
        /// Main entry point for the HipBot program logic.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            new Program().Initialize(args);
        }
    }
}
