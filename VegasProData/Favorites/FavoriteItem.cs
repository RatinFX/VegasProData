using System.Collections.Generic;

namespace VegasProData.Favorites
{
    /// <summary>
    /// Favorite configuration
    /// </summary>
    public class FavoriteItem
    {
        public HashSet<string> UniqueIDs { get; set; } = new HashSet<string>();
        public FavoriteType Type { get; set; } = 0;

        public FavoriteItem() { }
        public FavoriteItem(FavoriteType type)
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
