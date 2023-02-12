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
        public static T CreateReadConfig<T>(T config)
            where T : BaseConfig
        {
            // Create config file if it doesn't exist already
            if (!File.Exists(config.ConfigFileName))
                SaveConfig(config);

            var file = File.ReadAllText(config.ConfigFileName);
            return JsonConvert.DeserializeObject<T>(file);
        }

        /// <summary>
        /// Write the whole file
        /// TODO: only remove / add new lines instead
        /// </summary>
        public static void SaveConfig<T>(T config)
            where T : BaseConfig
        {
            var serialized = JsonConvert.SerializeObject(config);
            File.WriteAllText(config.ConfigFileName, serialized);
        }
    }
}
