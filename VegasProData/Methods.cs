using Newtonsoft.Json;
using ScriptPortal.Vegas;
using System;
using System.IO;
using System.Linq;

namespace VegasProData
{
    /// <summary>
    /// Helper Methods
    /// </summary>
    public class Methods
    {
        /// <summary>
        /// Set an effect preset by name
        /// </summary>
        public static void SetPreset(Effect effect, string name)
        {
            var preset = effect.Presets.FirstOrDefault(x => x.Name == name)?.Name;
            if (preset != null || preset != "") effect.Preset = preset;
        }

        /// <summary>
        /// Set an effect preset by index
        /// </summary>
        public static void SetPreset(Effect effect, int index)
        {
            effect.Preset = effect.Presets[index].Name;
        }

        static void GetAndSetConfigFolder()
        {
            // TODO: test all possible Vegas Application Extensions folders
            //       instead of the hard coded Documents folder
            Data.ConfigFilePath =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                @"\Vegas Application Extensions\VegasProData.json";

            // Create Config file
            if (!File.Exists(Data.ConfigFilePath)) SaveConfig();
        }

        /// <summary>
        /// Read or Create Config file
        /// </summary>
        public static void ReadConfig()
        {
            GetAndSetConfigFolder();
            var file = File.ReadAllText(Data.ConfigFilePath);
            Data.Config = JsonConvert.DeserializeObject<Config>(file);
        }

        /// <summary>
        /// Add FX to Favorites list and Save the list to a file
        /// </summary>
        /// <param name="uniqueID">UniqueID of the FX</param>
        public static void AddToFavorites(string uniqueID, FavType type)
        {
            Data.Config.Add(uniqueID, type);
            SaveConfig();
        }

        /// <summary>
        /// Remove FX from the Favorites list and Save the list to a file
        /// </summary>
        /// <param name="uniqueID">UniqueID of the FX</param>
        public static void RemoveFromFavorites(string uniqueID, FavType type)
        {
            Data.Config.Remove(uniqueID, type);
            SaveConfig();
        }

        /// <summary>
        /// Write the whole file
        /// TODO: only remove / add new lines instead
        /// </summary>
        public static void SaveConfig()
        {
            var config = JsonConvert.SerializeObject(Data.Config);
            File.WriteAllText(Data.ConfigFilePath, config);
        }
    }
}
