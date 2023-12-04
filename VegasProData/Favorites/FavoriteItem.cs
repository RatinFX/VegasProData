using System.Collections.Generic;
using VegasProData.Base;

namespace VegasProData.Favorites
{
    /// <summary>
    /// Favorite configuration
    /// </summary>
    public class FavoriteItem
    {
        public HashSet<string> UniqueIDs { get; set; } = new HashSet<string>();
        public PlugInNodeType Type { get; set; } = 0;

        public FavoriteItem() { }
        public FavoriteItem(PlugInNodeType type)
        {
            Type = type;
        }
        public FavoriteItem(FavoriteItem favorite)
        {
            UniqueIDs = favorite.UniqueIDs;
            Type = favorite.Type;
        }
    }
}
