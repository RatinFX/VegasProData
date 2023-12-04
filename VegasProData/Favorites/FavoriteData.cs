using System.Collections.Generic;
using System.Linq;
using VegasProData.Base;

namespace VegasProData.Favorites
{
    public class FavoriteData : Data
    {
        /// <summary>
        /// Search in a specific Plugin list
        /// </summary>
        /// <param name="config">Config file with Favorite list</param>
        /// <param name="list">FX list</param>
        /// <param name="type">Optional - List filter type</param>
        /// <param name="searchText">Optional - filter by SearchText</param>
        /// <param name="onlyFav">Optional - only show Favorited FX</param>
        public static IEnumerable<FavoriteExtendedPlugInNode> SearchIn(
            FavoriteConfig config,
            IEnumerable<ExtendedPlugInNode> list,
            PlugInNodeType type = 0,
            string searchText = "",
            bool onlyFav = false
        )
        {
            return new List<FavoriteExtendedPlugInNode> { new FavoriteExtendedPlugInNode(type) }.Concat(
                list.Select(FavoriteExtendedPlugInNode.New)
                .Where(x => x.Contains(searchText))
                .Where(x => !onlyFav ||
                     config.Favorites.Any(y =>
                     y.UniqueIDs.Contains(x.UniqueID) && y.Type == type)
                )
            );
        }

        /// <summary>
        /// Search in all Plugin lists
        /// </summary>
        /// <param name="config">Config file with Favorite list</param>
        /// <param name="searchText">Filter by SearchText</param>
        /// <param name="onlyFav">Optional - only show Favorited FX</param>
        public static IEnumerable<FavoriteExtendedPlugInNode> GetSearchResult(
            FavoriteConfig config, 
            string searchText, 
            bool onlyFav = false
        )
        {
            return SearchIn(config, VideoFX, PlugInNodeType.VideoFX, searchText, onlyFav)
                .Concat(SearchIn(config, AudioFX, PlugInNodeType.AudioFX, searchText, onlyFav))
                .Concat(SearchIn(config, Generators, PlugInNodeType.Generator, searchText, onlyFav))
                .Concat(SearchIn(config, Transitions, PlugInNodeType.Transition, searchText, onlyFav));
        }
    }
}
