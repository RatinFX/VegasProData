using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using VegasProData.Base;

namespace VegasProData.Favorites
{
    public class FavoriteConfig
    {
        public List<FavoriteItem> Favorites { get; set; } = new List<FavoriteItem>();
        [JsonIgnore] private string FileName { get; }

        public FavoriteConfig() { }
        public FavoriteConfig(bool init, string fileName = null)
        {
            if (!init)
                return;

            FileName = fileName ?? "VegasProData-Favorites";
            var config = BaseConfig.LoadConfig(this, FileName);
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
                    new FavoriteItem(PlugInNodeType.VideoFX),
                    new FavoriteItem(PlugInNodeType.AudioFX),
                    new FavoriteItem(PlugInNodeType.Generator),
                    new FavoriteItem(PlugInNodeType.Transition),
                };
            }

            // No VideoFX list
            if (Favorites.All(x => x.Type != PlugInNodeType.VideoFX))
            {
                Favorites.Add(new FavoriteItem(PlugInNodeType.VideoFX));
            }

            // No AudioFX list
            if (Favorites.All(x => x.Type != PlugInNodeType.AudioFX))
            {
                Favorites.Add(new FavoriteItem(PlugInNodeType.AudioFX));
            }

            // No Generators list
            if (Favorites.All(x => x.Type != PlugInNodeType.Generator))
            {
                Favorites.Add(new FavoriteItem(PlugInNodeType.Generator));
            }

            // No Transitions list
            if (Favorites.All(x => x.Type != PlugInNodeType.Transition))
            {
                Favorites.Add(new FavoriteItem(PlugInNodeType.Transition));
            }
        }

        public void Save()
        {
            BaseConfig.SaveConfig(this, FileName);
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
        public void Add(string uniqueID, PlugInNodeType type)
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
        public void Remove(string uniqueID, PlugInNodeType type)
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
        public FavoriteItem Find(PlugInNodeType type, Func<FavoriteItem, bool> predicate)
        {
            return Favorites.Where(x => x.Type == type).FirstOrDefault(predicate);
        }
    }
}
