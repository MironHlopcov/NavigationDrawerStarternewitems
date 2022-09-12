using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MyFinToControl.Configs.ManagerCore
{
    public sealed class ConfigurationManager
    {
        /// <summary>
        /// holds a reference to the single created instance, if any.
        /// </summary>
        private static readonly Lazy<ConfigurationManager> lazy = new Lazy<ConfigurationManager>(() => new ConfigurationManager());
        
        public static event EventHandler ConfigurationManagerChanged;
        /// <summary>
        /// Getting reference to the single created instance, creating one if necessary.
        /// </summary>
        public static ConfigurationManager ConfigManager { get; } = lazy.Value;
        private string localFileName = System.IO.Path.
            Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ConfigBank.json");
        public AppConfiguration BankConfigurationFromJson { get; set; }
        private ConfigurationManager()
        {
            BankConfigurationFromJson = this.Read();
        }
        /// <summary>
        /// Read the configuration files and return Configuration Object
        /// </summary>

        //private AppConfiguration Read()
        //{
        //    var assembly = Assembly.GetExecutingAssembly();
        //    string resourceName = "MyFinToControl.Configs.ConfigBank.json";
        //    string jsonFile = "";

        //    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        //    using (StreamReader reader = new StreamReader(stream))
        //    {
        //        jsonFile = reader.ReadToEnd(); //Make string equal to full file
        //    }

        //    var configs = JsonConvert.DeserializeObject<AppConfiguration>(jsonFile);
        //    //var mccCodes = JsonConvert.DeserializeObject<Dictionary<int, string>>(jsonFile);
        //    return configs;
        //}

        private AppConfiguration Read()
        {
            AppConfiguration configs;
           
            if (!File.Exists(localFileName))
            {
                var assembly = Assembly.GetExecutingAssembly();
                string resourceName = "MyFinToControl.Configs.ConfigBank.json";
                string jsonFile = "";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    jsonFile = reader.ReadToEnd(); //Make string equal to full file
                }
                configs = JsonConvert.DeserializeObject<AppConfiguration>(jsonFile);
                Write(configs);
            }
            else
            {
                string jsonFile = "";
                using (StreamReader reader = new StreamReader(localFileName))
                {
                    jsonFile = reader.ReadToEnd(); //Make string equal to full file
                }
                configs = JsonConvert.DeserializeObject<AppConfiguration>(jsonFile);
            }

            return configs;
        }
        private void Write(AppConfiguration appConfiguration)
        {
            var serializer = new JsonSerializer();

            using (var sw = new StreamWriter(localFileName))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, appConfiguration);
            }
        }
        public void Save()
        {
            Write(BankConfigurationFromJson);
            ConfigurationManagerChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}