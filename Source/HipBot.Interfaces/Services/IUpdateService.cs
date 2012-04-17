namespace HipBot.Services
{
    /// <summary>
    /// Interface for the Automatic Update service
    /// </summary>
    public interface IUpdateService
    {
        /// <summary>
        /// Gets the version directory from URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        string GetVersionDirectoryFromUrl(string url);

        /// <summary>
        /// Determines whether if a new version of the Bot is available.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if a new version is available; otherwise, <c>false</c>.
        /// </returns>
        bool IsNewVersionAvailable();

        /// <summary>
        /// Downloads the latest version of the bot from the Update server.
        /// </summary>
        void DownloadUpdate();

        /// <summary>
        /// Runs the latest version of the bot.
        /// </summary>
        /// <param name="waitForExit">if set to <c>true</c> wait for the child process to exit.</param>
        /// <param name="allowThisInstance">if set to <c>true</c> allow this instance to be run again.</param>
        void RunLatestVersion(bool waitForExit, bool allowThisInstance);
    }
}
