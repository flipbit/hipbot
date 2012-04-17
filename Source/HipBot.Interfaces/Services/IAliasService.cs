using System.Collections.Generic;

namespace HipBot.Services
{
    /// <summary>
    /// Interface for the Alias Service
    /// </summary>
    public interface IAliasService
    {
        /// <summary>
        /// Sets an alias to the given value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        void SetAlias(string name, string value);

        /// <summary>
        /// Determines whether the specified name is alias.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if the specified name is alias; otherwise, <c>false</c>.
        /// </returns>
        bool IsAlias(string name);

        /// <summary>
        /// Gets the alias with the given name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        string GetAlias(string name);

        /// <summary>
        /// Removes the alias with the given name.
        /// </summary>
        /// <param name="name">The name.</param>
        void RemoveAlias(string name);

        /// <summary>
        /// Lists the defined aliases.
        /// </summary>
        /// <returns></returns>
        IList<string> ListAliases();
    }
}
