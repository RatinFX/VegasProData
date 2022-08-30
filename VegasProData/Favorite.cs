using System.Collections.Generic;

namespace VegasProData
{
    /// <summary>
    /// Favorite configuration
    /// </summary>
    public class Favorite
    {
        public List<string> UniqueIDs { get; set; } = new List<string>();
        public FavType Type { get; set; } = 0;
        public Favorite() { }
        public Favorite(FavType type) { Type = type; }
        public Favorite(Favorite favorite) { UniqueIDs = favorite.UniqueIDs; Type = favorite.Type; }
    }
}
