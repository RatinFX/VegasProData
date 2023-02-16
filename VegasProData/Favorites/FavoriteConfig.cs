using System;
using System.Collections.Generic;
using System.Linq;
using VegasProData.Base;

namespace VegasProData.Favorites
{
    public class FavoriteConfig
    {
        public List<FavoriteItem> Favorites { get; set; } = new List<FavoriteItem>();

        public FavoriteConfig() { }
        public FavoriteConfig(bool init)
        {
            if (!init)
                return;

            var config = BaseConfig.LoadConfig(this, "VegasProData-Favorites");
            if (config != null && config.Favorites != null)
            {
                Favorites = config.Favorites;
            }

            SetDefaultValues();
            Save();
        }

        void SetDefaultValues()
        {
            if (Favorites == null || Favorites.Count == 0)
            {
                Favorites = new List<FavoriteItem>
                {
                    new FavoriteItem(FavoriteType.VideoFX),
                    new FavoriteItem(FavoriteType.AudioFX),
                    new FavoriteItem(FavoriteType.Generators),
                    new FavoriteItem(FavoriteType.Transitions),
                };
            }

            // No VideoFX list
            if (!Favorites.Any(x => x.Type == FavoriteType.VideoFX))
            {
                Favorites.Add(new FavoriteItem(FavoriteType.VideoFX));
            }

            // No AudioFX list
            if (!Favorites.Any(x => x.Type == FavoriteType.AudioFX))
            {
                Favorites.Add(new FavoriteItem(FavoriteType.AudioFX));
            }

            // No Generators list
            if (!Favorites.Any(x => x.Type == FavoriteType.Generators))
            {
                Favorites.Add(new FavoriteItem(FavoriteType.Generators));
            }

            // No Transitions list
            if (!Favorites.Any(x => x.Type == FavoriteType.Transitions))
            {
                Favorites.Add(new FavoriteItem(FavoriteType.Transitions));
            }
        }

        public void Save()
        {
            BaseConfig.SaveConfig(this, "VegasProData-Favorites");
        }

        /// <summary>
        /// Add FX to Favorites list and Save the list to a file
        /// </summary>
        public void Add(FavoriteExtendedPlugInNode plugin)
        {
            if (plugin is null)
                return;

            Add(plugin.UniqueID, plugin.GetFavType());
        }

        /// <summary>
        /// Add FX to Favorites list and Save the list to a file
        /// </summary>
        public void Add(string uniqueID, FavoriteType type)
        {
            var fav = Find(type, x => !x.UniqueIDs.Contains(uniqueID));
            if (fav == null)
                return;

            fav.UniqueIDs.Add(uniqueID);

            Save();
        }

        /// <summary>
        /// Remove FX from the Favorites list and Save the list to a file
        /// </summary>
        public void Remove(FavoriteExtendedPlugInNode plugin)
        {
            if (plugin is null)
                return;

            Remove(plugin.UniqueID, plugin.GetFavType());
        }

        /// <summary>
        /// Remove FX from the Favorites list and Save the list to a file
        /// </summary>
        public void Remove(string uniqueID, FavoriteType type)
        {
            var fav = Find(type, x => x.UniqueIDs.Contains(uniqueID));
            if (fav == null)
                return;

            fav.UniqueIDs.Remove(uniqueID);

            Save();
        }

        /// <summary>
        /// Find the first Item by Type and Predicate
        /// </summary>
        public FavoriteItem Find(FavoriteType type, Func<FavoriteItem, bool> predicate)
        {
            return Favorites.Where(x => x.Type == type).FirstOrDefault(predicate);
        }
    }
}
