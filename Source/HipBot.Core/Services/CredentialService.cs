using HipBot.Domain;

namespace HipBot.Services
{
    /// <summary>
    /// Service to allow the application to store and retrieve security credentials.
    /// </summary>
    public class CredentialService : ICredentialService
    {
        #region Dependencies
        
        /// <summary>
        /// Gets or sets the config service.
        /// </summary>
        /// <value>
        /// The config service.
        /// </value>
        public IConfigService ConfigService { get; set; }

        #endregion

        /// <summary>
        /// Gets the default credentials.
        /// </summary>
        /// <returns></returns>
        public Credentials GetCredentials()
        {
            // Get Configuration
            var config = ConfigService.GetConfig();

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
            var config = ConfigService.GetConfig();

            // Set values
            config.SetValue("Credentials", "Name", credentials.Name);
            config.SetValue("Credentials", "JabberId", credentials.JabberId);
            config.SetValue("Credentials", "Password", credentials.Password);
            config.SetValue("Credentials", "ApiToken", credentials.ApiToken);

            // Save to disk
            ConfigService.SetConfig(config);
        }

        /// <summary>
        /// Determines if the credentials for the HipChat service have been set.
        /// </summary>
        /// <returns></returns>
        public bool CredentialsSet()
        {
            // Get Configuration
            var config = ConfigService.GetConfig();

            return !string.IsNullOrWhiteSpace(config.GetValue("Credentials", "Name", string.Empty));
        }
    }
}
