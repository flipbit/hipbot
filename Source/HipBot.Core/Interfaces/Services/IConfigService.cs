using Sugar.Configuration;

namespace HipBot.Interfaces.Services
{
    /// <summary>
    /// Service interface for the Configuration.
    /// </summary>
    public interface IConfigService
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        Config GetConfig();

        /// <summary>
        /// Sets the configuration.
        /// </summary>
        /// <param name="config">The configuration to set.</param>
        void SetConfig(Config config);
    }
}
