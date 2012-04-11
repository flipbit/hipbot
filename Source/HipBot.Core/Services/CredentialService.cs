using System.IO;
using HipBot.Domain;
using HipBot.Interfaces.Services;
using Sugar.Configuration;
using Sugar.IO;

namespace HipBot.Services
{
    /// <summary>
    /// Service to allow the application to store and retrieve security credentials.
    /// </summary>
    public class CredentialService : ICredentialService
    {
        /// <summary>
        /// Gets or sets the file service.
        /// </summary>
        /// <value>
        /// The file service.
        /// </value>
        public IFileService FileService { get; set; }

        /// <summary>
        /// Gets the default credentials.
        /// </summary>
        /// <returns></returns>
        public Credentials GetCredentials()
        {
            // Get Configuration
            var config = GetConfiguration();

            var credentials = new Credentials
            {
                Name = config.GetValue("Credentials", "Name", string.Empty),
                JabberId = config.GetValue("Credentials", "JabberID", string.Empty),
                Password = config.GetValue("Credentials", "Password", string.Empty),
                ApiToken = config.GetValue("Credentials", "ApiToken", string.Empty)
            };

            return credentials;
        }

        /// <summary>
        /// Sets the credentials.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        public void SetCredentials(Credentials credentials)
        {
            // Get Configuration
            var config = GetConfiguration();

            // Set values
            config.SetValue("Credentials", "Name", credentials.Name);
            config.SetValue("Credentials", "JabberId", credentials.JabberId);
            config.SetValue("Credentials", "Password", credentials.Password);
            config.SetValue("Credentials", "ApiToken", credentials.ApiToken);

            // Save to disk
            config.Write(GetConfigurationFilename());
        }

        private Config GetConfiguration()
        {
            // Get file location
            var filename = GetConfigurationFilename();

            // Load the configuration data
            return Config.FromFile(filename, FileService);
        }

        private string GetConfigurationFilename()
        {
            // Configuration file in user directory
            var directory = FileService.GetUserDataDirectory();

            return Path.Combine(directory, "hipbot.config");
        }
    }
}
