using Newtonsoft.Json;
using ScriptPortal.Vegas;
using System.IO;
using System.Linq;

namespace VegasProData.Base
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
            if (preset != null || preset != "")
                effect.Preset = preset;
        }

        /// <summary>
        /// Set an effect preset by index
        /// </summary>
        public static void SetPreset(Effect effect, int index)
        {
            effect.Preset = effect.Presets[index].Name;
        }

        /// <summary>
        /// Create and Read Config file
        /// </summary>
        public static T CreateReadConfig<T>(T config, string filePath)
            where T : class
        {
            // Create config file if it doesn't exist already
            if (!File.Exists(filePath))
                SaveConfig(config, filePath);

            var file = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(file);
        }

        /// <summary>
        /// Write the whole file
        /// TODO: only remove / add new lines instead
        /// </summary>
        public static void SaveConfig<T>(T config, string filePath)
            where T : class
        {
            // Convert to .json
            var serialized = JsonConvert.SerializeObject(config);

            // Create folder if it doesn't exist already
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // Write file
            File.WriteAllText(filePath, serialized);
        }
    }
}
