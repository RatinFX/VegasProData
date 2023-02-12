using System.Collections.Generic;
using System.Linq;
using VegasProData.Base;

namespace VegasProData.Theme
{
    public class ThemeConfig : BaseConfig
    {
        public Theme CurrentTheme { get; set; } = Theme.Dark;
        public List<Theme> Themes { get; set; }

        public ThemeConfig() { }
        public ThemeConfig(bool init) : base("VegasProData-Theme")
        {
            if (!init) return;

            var config = Methods.CreateReadConfig(this);
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
            SaveConfig(this);
        }
    }
}
