using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HipBot.Domain;
using HipBot.Interfaces.Services;
using Sugar.Configuration;
using Sugar.IO;

namespace HipBot.Services
{
    /// <summary>
    /// Service to manage this bots nicknames.
    /// </summary>
    public class NicknameService : INicknameService
    {
        private Config cfg;

        #region Dependencies
        
        /// <summary>
        /// Gets or sets the file service.
        /// </summary>
        /// <value>
        /// The file service.
        /// </value>
        public IFileService FileService { get; set; }

        #endregion

        private Config GetConfiguration()
        {
            if (cfg == null)
            {
                // Get file location
                var filename = GetConfigurationFilename();

                // Load the configuration data
                cfg = Config.FromFile(filename, FileService);
            }

            return cfg;
        }

        private string GetConfigurationFilename()
        {
            // Configuration file in user directory
            var directory = FileService.GetUserDataDirectory();

            return Path.Combine(directory, "hipbot.config");
        }

        /// <summary>
        /// Adds the specified nickname to this bot.
        /// </summary>
        /// <param name="nickname">The nickname.</param>
        public void Add(string nickname)
        {
            // Get Configuration
            var config = GetConfiguration();

            config.SetValue("Nicknames", nickname, string.Empty);

            // Save to disk
            config.Write(GetConfigurationFilename());
        }

        /// <summary>
        /// Removes the specified nickname from this bot.
        /// </summary>
        /// <param name="nickname">The nickname.</param>
        public void Remove(string nickname)
        {
            // Get Configuration
            var config = GetConfiguration();

            config.Delete("Nicknames", nickname);

            // Save to disk
            config.Write(GetConfigurationFilename());
        }

        /// <summary>
        /// Lists the nicknames this bot will respond to.
        /// </summary>
        /// <returns></returns>
        public IList<string> List()
        {
            // Get Configuration
            var config = GetConfiguration();

            return config.GetSection("Nicknames").Select(l => l.Key).ToList();
        }

        /// <summary>
        /// Determines whether the specified message is addressed to me.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <c>true</c> if the specified message is addressed to me; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAddressedToMe(Message message)
        {
            if (string.IsNullOrWhiteSpace(message.Body))
            {
                return false;
            }

            foreach (var nick in List())
            {
                if (message.Body.IndexOf(nick + " ", 0, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return true;
                }
            };

            return false;
        }
    }
}
