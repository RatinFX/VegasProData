using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace VegasProData
{
    /// <summary>
    /// Saved Config JSON data
    /// </summary>
    public class Config
    {
        public bool DarkMode { get; set; } = true;
        public List<AppTheme> Themes { get; set; }
        public List<Favorite> Favorites { get; set; }

        public Config() { }
        public Config(bool init = false)
        {
            if (!init) return;

            Favorites = new List<Favorite>
            {
                new Favorite(FavType.VideoFX),
                new Favorite(FavType.AudioFX),
                new Favorite(FavType.Generators),
                new Favorite(FavType.Transitions),
            };

            CreateBaseThemes();
        }

        public void Add(string uniqueID, FavType type)
        {
            var fav = Find(type, x => !x.UniqueIDs.Contains(uniqueID));
            if (fav == null) return;
            fav.UniqueIDs.Add(uniqueID);
        }

        public void Remove(string uniqueID, FavType type)
        {
            var fav = Find(type, x => x.UniqueIDs.Contains(uniqueID));
            if (fav == null) return;
            fav.UniqueIDs.Remove(uniqueID);
        }

        private Favorite Find(FavType type, Func<Favorite, bool> predicate)
        {
            return Favorites.Where(x => x.Type == type).FirstOrDefault(predicate);
        }

        private void CreateBaseThemes()
        {
            if (Themes == null) Themes = new List<AppTheme>();

            if (!Themes.Any(x => x.Name.ToLower().Contains("dark")))
            {
                Themes.Add(new AppTheme(
                    "Dark",
                    Color.FromArgb(45, 45, 45),
                    Color.FromArgb(70, 70, 70),
                    Color.FromArgb(45, 45, 45),
                    Color.FromArgb(0, 0, 0))
                );
            }

            if (!Themes.Any(x => x.Name.ToLower().Contains("light")))
            {
                Themes.Add(new AppTheme(
                    "Light",
                    Color.FromArgb(245, 245, 245),
                    Color.FromArgb(255, 255, 255),
                    Color.FromArgb(245, 245, 245),
                    Color.FromArgb(0, 0, 0))
                );
            }
        }
    }
}
