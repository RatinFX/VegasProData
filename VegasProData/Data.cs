using ScriptPortal.Vegas;
using System.Collections.Generic;
using System.Linq;

namespace VegasProData
{
    /// <summary>
    /// More accessible VEGAS Data
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="vegas">OUR VEGAS</param>
        public void Initialize(Vegas vegas)
        {
            Vegas = vegas;
        }

        /***
         *  GENERAL
         */

        public static string ConfigFilePath = "";

        public static Config Config { get; set; } = new Config(init: true);

        /***
		 *  GENERAL VEGAS
		 */

        /// <summary>
        /// Vegas Vegas -> VEGAS
        /// </summary>
        public static Vegas Vegas { get; set; }

        /// <summary>
        /// Current cursor position on the timeline
        /// </summary>
        public static Timecode CursorPosition => Vegas.Transport.CursorPosition;

        /// <summary>
        /// List of currently Selected Medias
        /// </summary>
        public static IEnumerable<TrackEvent> SelectedMedias => Tracks.SelectMany(x => x.Events.Where(y => y.Selected));

        /// <summary>
        /// Available Video Effects
        /// </summary>
        public static IEnumerable<ExtendedPlugInNode> VideoFX => Vegas.VideoFX.Where(x => !x.IsContainer)
            .Select(x => new ExtendedPlugInNode(x) { IsVideoFX = true })
            .OrderBy(x => x.Name);

        /// <summary>
        /// Available Audio Effects
        /// </summary>
        public static IEnumerable<ExtendedPlugInNode> AudioFX => Vegas.AudioFX.Where(x => !x.IsContainer)
            .Select(x => new ExtendedPlugInNode(x) { IsAudioFX = true })
            .OrderBy(x => x.Name);

        /// <summary>
        /// Available Transitions
        /// </summary>
        public static IEnumerable<ExtendedPlugInNode> Transitions => Vegas.Transitions.Where(x => !x.IsContainer)
            .Select(x => new ExtendedPlugInNode(x) { IsTransition = true })
            .OrderBy(x => x.Name);

        /// <summary>
        /// Available Generators
        /// </summary>
        public static IEnumerable<ExtendedPlugInNode> Generators => Vegas.Generators.Where(x => !x.IsContainer)
            .Select(x => new ExtendedPlugInNode(x) { IsGenerator = true })
            .OrderBy(x => x.Name);

        /// <summary>
        /// Search in the given List
        /// </summary>
        private static IEnumerable<ExtendedPlugInNode> SearchIn(
            IEnumerable<ExtendedPlugInNode> list, FavType type = 0, string input = "", bool onlyFav = false)
        {
            return new List<ExtendedPlugInNode> { new ExtendedPlugInNode(type) }.Concat(
                list
                .Where(x => x.Contains(input))
                .Where(x => !onlyFav ||
                             Config.Favorites.Any(y => y.UniqueIDs.Contains(x.UniqueID) && y.Type == type)));
        }

        /// <summary>
        /// Get the list of available PlugInNodes
        /// </summary>
        public static IEnumerable<ExtendedPlugInNode> GetSearchResult(string input, bool onlyFav = false)
        {
            return SearchIn(VideoFX, FavType.VideoFX, input, onlyFav)
            .Concat(SearchIn(AudioFX, FavType.AudioFX, input, onlyFav))
            .Concat(SearchIn(Generators, FavType.Generators, input, onlyFav))
            .Concat(SearchIn(Transitions, FavType.Transitions, input, onlyFav))
            ;
        }

        /***
		 *  TRACKS
		 */

        /// <summary>
        /// Vegas Tracks
        /// </summary>
        public static Tracks Tracks => Vegas.Project.Tracks;

        /***
		 *  > VIDEO TRACKS
		 */

        /// <summary>
        /// Video Tracks
        /// </summary>
        public static IEnumerable<Track> VideoTracks => Vegas.Project.Tracks.Where(x => x.MediaType == MediaType.Video);

        /// <summary>
        /// Selected Video Tracks
        /// </summary>
        public static IEnumerable<Track> SelectedVideoTracks => VideoTracks.Where(x => x.Selected);

        /// <summary>
        /// First Selected Video Track
        /// </summary>
        public static Track FirstSelectedVideoTrack => SelectedVideoTracks.FirstOrDefault();

        /***
		 *  > AUDIO TRACKS
		 */

        /// <summary>
        /// Audio Tracks
        /// </summary>
        public static IEnumerable<Track> AudioTracks => Vegas.Project.Tracks.Where(x => x.MediaType == MediaType.Audio);

        /// <summary>
        /// Selected Audio Tracks
        /// </summary>
        public static IEnumerable<Track> SelectedAudioTracks => AudioTracks.Where(x => x.Selected);

        /// <summary>
        /// First Selected Audio Tracks
        /// </summary>
        public static Track FirstSelectedAudioTrack => SelectedAudioTracks.FirstOrDefault();
    }
}
