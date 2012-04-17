using System.IO;
using Sugar.Configuration;
using Sugar.IO;

namespace HipBot.Services
{
    /// <summary>
    /// Wrapper for accessing the configuration
    /// </summary>
    public class ConfigService : IConfigService
    {
        #region Dependencies
        
        /// <summary>
        /// Gets or sets the file service.
        /// </summary>
        /// <value>
        /// The file service.
        /// </value>
        public IFileService FileService { get; set; }

        #endregion

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        public Config GetConfig()
        {
            // Get file location
            var filename = GetConfigurationFilename();

            // Load the configuration data
            return Config.FromFile(filename, FileService);
        }

        /// <summary>
        /// Sets the configuration.
        /// </summary>
        /// <param name="config">The configuration to set.</param>
        public void SetConfig(Config config)
        {
            config.Write(GetConfigurationFilename());
        }

        private string GetConfigurationFilename()
        {
            // Configuration file in user directory
            var directory = FileService.GetUserDataDirectory();
            directory = Path.Combine(directory, "HipBot");

            // Ensure directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return Path.Combine(directory, "HipBot.config");
        }
    }
}
