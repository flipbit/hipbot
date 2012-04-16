using System.Collections.Generic;
using System.Linq;
using HipBot.Interfaces.Services;

namespace HipBot.Services
{
    /// <summary>
    /// Service to manipulate command aliases.
    /// </summary>
    public class AliasService : IAliasService
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
        /// Sets an alias to the given value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void SetAlias(string name, string value)
        {
            var config = ConfigService.GetConfig();

            // Check for single quotes, replace with double
            if (value.Contains("'"))
            {
                value = value.Replace("'", @"""");
            }

            config.SetValue("Aliases", name, value);

            ConfigService.SetConfig(config);
        }

        /// <summary>
        /// Determines whether the specified name is alias.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if the specified name is alias; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAlias(string name)
        {
            var config = ConfigService.GetConfig();

            return !string.IsNullOrEmpty(config.GetValue("Aliases", name, string.Empty));
        }

        /// <summary>
        /// Gets the alias with the given name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string GetAlias(string name)
        {
            var config = ConfigService.GetConfig();

            return config.GetValue("Aliases", name, string.Empty);
        }

        /// <summary>
        /// Removes the alias with the given name.
        /// </summary>
        /// <param name="name">The name.</param>
        public void RemoveAlias(string name)
        {
            var config = ConfigService.GetConfig();

            config.Delete("Aliases", name);
        }

        /// <summary>
        /// Lists the defined aliases.
        /// </summary>
        /// <returns></returns>
        public IList<string> ListAliases()
        {
            var config = ConfigService.GetConfig();

            var lines = config.GetSection("Aliases");

            return lines.Select(line => line.Key + "=" + line.Value).ToList();
        }
    }
}
