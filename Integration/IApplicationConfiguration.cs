using System;
using System.Collections;

namespace Integration
{
    /// <summary>
    /// Simple application config interface.
    /// </summary>
    public interface IApplicationConfiguration : IEnumerable
    {
        /// <summary>
        /// Gets / Sets / Updates a given configuration value.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>the current value (after the process)</returns>
        string this[string key] { get; set; }

        /// <summary>
        /// Checks if a given key exists and is not null or empty.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// Gets a given configuration value.
        /// This does NOT check if the key exists, is null or empty.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>value</returns>
        string Get(string key);

        /// <summary>
        /// Removes a given key and it's value permanently from the configuration.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>if the operation succeded, the key and the current value (before the process)</returns>
        Tuple<bool, string, string> Remove(string key);

        /// <summary>
        /// Sets / Updates a given configuration value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>the key and the current value (before the process)</returns>
        Tuple<string, string> SetOrAdd(string key, string value);
    }
}