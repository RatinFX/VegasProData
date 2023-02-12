using Newtonsoft.Json;
using System;

namespace VegasProData.Base
{
    /// <summary>
    /// Saved Config JSON data
    /// </summary>
    public class BaseConfig
    {
        [JsonIgnore]
        public readonly string ConfigFileName = "";

        protected BaseConfig(){}
        protected BaseConfig(string fileName)
        {
            ConfigFileName =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                $@"\Vegas Application Extensions\{fileName}.json";
        }

        protected static T LoadConfig<T>(T config)
            where T: BaseConfig
        {
            return Methods.CreateReadConfig(config);
        }

        protected static void SaveConfig<T>(T config)
            where T : BaseConfig
        {
            Methods.SaveConfig(config);
        }
    }
}
