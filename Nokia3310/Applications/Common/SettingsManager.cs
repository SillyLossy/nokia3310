using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using Newtonsoft.Json;

namespace Nokia3310.Applications.Common
{
    public static class SettingsManager
    {
        private const string SettingsFile = "nokiasettings.json";
        private static readonly Dictionary<string, object> settings;

        static SettingsManager()
        {
            settings = ReadSettings();
        }

        private static Dictionary<string, object> ReadSettings()
        {
            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(SettingsFile));
            }
            catch (Exception)
            {
                return new Dictionary<string, object>();
            }
        }

        public static void Write<T>(T value)
        {
            settings[typeof(T).Name] = value;
            PersistSettings();
        }

        public static T Read<T>() where T : class, new()
        {
            if (settings.ContainsKey(typeof(T).Name))
            {
                return settings[typeof(T).Name] as T;
            }
            var value = new T();
            settings[typeof(T).Name] = value;
            return value;
        }

        private static void PersistSettings()
        {
            File.WriteAllText(SettingsFile, JsonConvert.SerializeObject(settings));
        }
    }
}
