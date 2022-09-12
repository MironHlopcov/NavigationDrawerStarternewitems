using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MyFinToControl.Configs.ManagerCore
{
    public sealed class MccConfigurationManager
    {
        /// <summary>
        /// holds a reference to the single created instance, if any.
        /// </summary>
        private static readonly Lazy<MccConfigurationManager> lazy = new Lazy<MccConfigurationManager>(() => new MccConfigurationManager());

        /// <summary>
        /// Getting reference to the single created instance, creating one if necessary.
        /// </summary>
        public static MccConfigurationManager ConfigManager { get; } = lazy.Value;

        public Dictionary<int, string> MccConfigurationFromJson { get; set; }
        private MccConfigurationManager()
        {
            MccConfigurationFromJson = this.Read();
        }
        /// <summary>
        /// Read the configuration files and return Configuration Object
        /// </summary>
        private Dictionary<int, string> Read()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = "MyFinToControl.Configs.ConfigMcc.json";
            string jsonFile = "";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                jsonFile = reader.ReadToEnd(); //Make string equal to full file
            }

            var configs = JsonConvert.DeserializeObject<Dictionary<int, string>>(jsonFile);
            //var mccCodes = JsonConvert.DeserializeObject<Dictionary<int, string>>(jsonFile);
            return configs;
        }
    }
}