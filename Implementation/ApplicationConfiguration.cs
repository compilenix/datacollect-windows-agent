using System;
using System.Collections;
using System.Configuration;
using Integration;

namespace Implementation
{
    /// <summary>
    /// Simple application config interface.
    /// </summary>
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        /// <summary>
        /// Gets / Sets / Updates a given configuration value.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>the current value (after the process)</returns>
        public string this[string key]
        {
            get { return Get(key); }
            set { SetOrAdd(key, value); }
        }

        /// <summary>
        /// Gets a given configuration value.
        /// This does NOT check if the key exists, is null or empty.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>value</returns>
        public string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// Sets / Updates a given configuration value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>the key and the current value (before the process)</returns>
        public Tuple<string, string> SetOrAdd(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }

            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

            return new Tuple<string, string>(key, value);
        }

        /// <summary>
        /// Checks if a given key exists and is not null or empty.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(Get(key))) return true;
            }
            catch
            {
                // ignored
            }

            return false;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        /// <filterpriority>2</filterpriority>
        public IEnumerator GetEnumerator()
        {
            yield return ConfigurationManager.AppSettings;
        }

        /// <summary>
        /// Removes a given key and it's value permanently from the configuration.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>if the operation succeded, the key and the current value (before the process)</returns>
        public Tuple<bool, string, string> Remove(string key)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            var value = string.Empty;
            var removed = false;

            try
            {
                value = Get(key);
                settings.Remove(key);
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                removed = true;
            }
            catch
            {
                // ignored
            }

            return new Tuple<bool, string, string>(removed, key, value);
        }
    }
}