using System;
using System.IO;

namespace VegasProData.Base
{
    /// <summary>
    /// Saved Config JSON data
    /// </summary>
    public static class BaseConfig
    {
        public static string DEFAULT_PATH =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
            $@"\Vegas Application Extensions\";

        public static string GetFolder(string folderName)
        {
            var path = DEFAULT_PATH + folderName;
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            return path;
        }

        public static string GetCofigPath(string fileName)
        {
            return DEFAULT_PATH + fileName + ".json";
        }

        public static T LoadConfig<T>(T config, string fileName)
            where T : class
        {
            return Methods.CreateReadConfig(config, GetCofigPath(fileName));
        }

        public static void SaveConfig<T>(T config, string fileName)
            where T : class
        {
            Methods.SaveConfig(config, GetCofigPath(fileName));
        }
    }
}
