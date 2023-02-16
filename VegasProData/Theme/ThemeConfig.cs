using System.Collections.Generic;
using System.Linq;
using VegasProData.Base;

namespace VegasProData.Theme
{
    public class ThemeConfig
    {
        public Theme CurrentTheme { get; set; } = Theme.Dark;
        public List<Theme> Themes { get; set; }

        public ThemeConfig() { }
        public ThemeConfig(bool init)
        {
            if (!init) return;

            var config = BaseConfig.LoadConfig(this, "VegasProData-Theme");
            CurrentTheme = config.CurrentTheme;
            Themes = config.Themes;

            SetDefaultValues();
            Save();
        }

        void SetDefaultValues()
        {
            if (CurrentTheme == null)
            {
                CurrentTheme = Theme.Dark;
            }

            if (Themes == null)
            {
                Themes = new List<Theme>();
            }

            if (!Themes.Any(x => x.Name.ToLower().Contains("dark")))
            {
                Themes.Add(Theme.Dark);
            }

            if (!Themes.Any(x => x.Name.ToLower().Contains("light")))
            {
                Themes.Add(Theme.Light);
            }
        }

        public void Save()
        {
            BaseConfig.SaveConfig(this, "VegasProData-Theme");
        }
    }
}
