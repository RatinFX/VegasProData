#if VP14 || DEBUG
using ScriptPortal.Vegas;
#elif VP13
using Sony.Vegas;
#endif
using System.Collections.Generic;
using System.Linq;

namespace VegasProData.Base
{
    /// <summary>
    /// More accessible VEGAS Data
    /// </summary>
    public class Data
    {
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
        /// Current cursor position on the timeline - could be different if the preview is playing
        /// </summary>
        public static Timecode PlayCursorPosition => Vegas.Transport.PlayCursorPosition;

        /// <summary>
        /// List of currently Selected Medias
        /// </summary>
        public static IEnumerable<TrackEvent> SelectedMedias => Tracks.SelectMany(x => x.Events.Where(y => y.Selected));
        
        /// <summary>
        /// The First selected Video Event
        /// </summary>
        public static VideoEvent FirstSelectedVideoEvent => SelectedMedias?.FirstOrDefault(x => x.IsVideo()) as VideoEvent;

        /// <summary>
        /// The First selected Audio Event
        /// </summary>
        public static AudioEvent FirstSelectedAudioEvent => SelectedMedias?.FirstOrDefault(x => x.IsAudio()) as AudioEvent;

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
