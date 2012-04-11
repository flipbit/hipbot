using HipBot.Interfaces.Services;
using Sugar.Command;

namespace HipBot.Commands.AutoUpdate
{
    /// <summary>
    /// Sets the URL for the automatic updates
    /// </summary>
    public class SetUrl : BoundCommand<SetUrl.Options>
    {
        [Flag("update")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the URL to check to check for the automatic updates.
            /// </summary>
            /// <value>
            /// The URL.
            /// </value>
            [Parameter("url", Required = true)]
            public string Url { get; set; }
        }

        /// <summary>
        /// Gets or sets the config service.
        /// </summary>
        /// <value>
        /// The config service.
        /// </value>
        public IConfigService ConfigService { get; set; }

        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void Execute(Options options)
        {
            var config = ConfigService.GetConfig();

            config.SetValue("Update", "Url", options.Url);

            ConfigService.SetConfig(config);
        }
    }
}
