using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using HipBot.Core;
using HipBot.Domain;
using HipBot.Handlers;
using HipBot.Services;
using Sugar.Command;

namespace HipBot
{
    public class Program
    {
        [ImportMany(typeof(IHandler), AllowRecomposition = true)]
        public IList<IHandler> Handlers = new List<IHandler>();

        private FileSystemWatcher watcher;
        private DirectoryCatalog directoryCatalog;
        private HandlerService handlerService;

        /// <summary>
        /// Gets the directory of the current process.
        /// </summary>
        /// <returns></returns>
        private static string GetDirectory()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            location = Path.GetDirectoryName(location);
            location = Path.Combine(location, "Plugins");

            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }

            return location;
        }

        public void DoImport()
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();

            directoryCatalog = new DirectoryCatalog(GetDirectory());
            directoryCatalog.Changing += directoryCatalog_Changing;
            directoryCatalog.Changed += directoryCatalog_Changed;

            //Adds all the parts found in all assemblies in 
            //the same directory as the executing program
            catalog.Catalogs.Add(directoryCatalog);

            //Create the CompositionContainer with the parts in the catalog
            var container = new CompositionContainer(catalog);

            try
            {
                //Fill the imports of this object
                container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                Out.WriteLine("Unable to load plugins: {0}", ex.Message);
            }

        }

        void directoryCatalog_Changing(object sender, ComposablePartCatalogChangeEventArgs e)
        {
            try
            {
                // Remove handlers
                foreach (var handler in Handlers)
                {
                    for (var i = handlerService.Handlers.Count - 1; i > 0; i--)
                    {
                        var h = handlerService.Handlers[i];

                        if (h.GetType() == handler.GetType())
                        {
                            handlerService.Handlers.RemoveAt(i);
                        }

                    }

                    Stencil.Instance.RemoveType(handler.GetType());
                }
            }
            catch (Exception ex)
            {              
                Out.WriteLine(ex.Message);
            }
        }

        void directoryCatalog_Changed(object sender, ComposablePartCatalogChangeEventArgs e)
        {
            // Register handlers
            foreach (var handler in Handlers)
            {
                Stencil.Instance.AddType(handler.GetType());

                var instance = Stencil.Instance.Resolve(handler.GetType()) as IHandler;

                handlerService.Handlers.Add(instance);
            }
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
            Stencil.Defaults.Assemblies.Add(typeof(Room).Assembly);
            Stencil.Defaults.Assemblies.Add(typeof(Program).Assembly);
            Stencil.Defaults.Assemblies.Add(typeof(Stencil).Assembly);
            Stencil.Defaults.Assemblies.Add(typeof(ICommand).Assembly);

            // Register handlers
            foreach (var handler in Handlers)
            {
                Stencil.Defaults.Types.Add(handler.GetType());
            }

            Stencil.Instance.Initilize();

            // Start watching plugins
            InitializePluginWatcher();

            // Initilize handler service
            handlerService = Stencil.Instance.Resolve<IHandlerService>() as HandlerService;

            // Run latest version
            var service = Stencil.Instance.Resolve<UpdateService>();
            service.RunLatestVersion(true, false);

            // Get Console
            var console = Stencil.Instance.Resolve<HipBotConsole>();

            // Go!
            console.Run(args);
        }

        private void InitializePluginWatcher()
        {
            GetDirectory();

            watcher = new FileSystemWatcher();

            watcher.Path = GetDirectory();
            //watcher.Filter = "*.dll";

            watcher.Changed += watcher_Changed;
            watcher.Created += watcher_Created;
            watcher.Deleted += watcher_Deleted;
            watcher.Renamed += watcher_Renamed;

            watcher.EnableRaisingEvents = true;
        }

        void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            Out.WriteLine("Plugins Renamed");

            directoryCatalog.Refresh();
        }

        void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Out.WriteLine("Plugins Deleted");

            directoryCatalog.Refresh();
        }

        void watcher_Created(object sender, FileSystemEventArgs e)
        {
            Out.WriteLine("Plugins Created");

            directoryCatalog.Refresh();
        }

        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Out.WriteLine("Plugins Changed");

            directoryCatalog.Refresh();
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
