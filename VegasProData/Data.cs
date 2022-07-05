using System.Collections.Generic;
using System.Linq;
using ScriptPortal.Vegas;
//using Sony.Vegas;

namespace VegasProData
{
	/// <summary>
	/// Make Vegas Data more accessible while scripting
	/// </summary>
	public class Data
	{
		/// <summary>
		/// Initialize
		/// </summary>
		/// <param name="vegas">it's OUR Vegas now :)</param>
		public void Initialize(Vegas vegas)
		{
			Vegas = vegas;
		}

		/***
		 *  GENERAL
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
			.Select(x => new ExtendedPlugInNode(x) { IsVideoFX = true });

		/// <summary>
		/// Available Audio Effects
		/// </summary>
		public static IEnumerable<ExtendedPlugInNode> AudioFX => Vegas.AudioFX.Where(x => !x.IsContainer)
			.Select(x => new ExtendedPlugInNode(x) { IsAudioFX = true });

		/// <summary>
		/// Available Generators
		/// </summary>
		public static IEnumerable<ExtendedPlugInNode> Transitions => Vegas.Transitions.Where(x => !x.IsContainer)
			.Select(x => new ExtendedPlugInNode(x) { IsTransition = true });

		/// <summary>
		/// Available Transitions
		/// </summary>
		public static IEnumerable<ExtendedPlugInNode> Generators => Vegas.Generators.Where(x => !x.IsContainer)
			.Select(x => new ExtendedPlugInNode(x) { IsGenerator = true });

		/// <summary>
		/// Search in the given List
		/// </summary>
		/// <returns>Filtered list</returns>
		public static IEnumerable<ExtendedPlugInNode> SearchIn(IEnumerable<ExtendedPlugInNode> list, string text = "") =>
			list.Where(x => x.Plugin.Name.ToLower().Contains(text.ToLower()));

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

	/// <summary>
	/// PlugInNode with more accessible info
	/// </summary>
	public class ExtendedPlugInNode
	{
		public string Name { get; set; }
		public PlugInNode Plugin { get; set; }
		public bool IsVideoFX { get; set; } = false;
		public bool IsAudioFX { get; set; } = false;
		public bool IsTransition { get; set; } = false;
		public bool IsGenerator { get; set; } = false;

		public ExtendedPlugInNode() { }
		public ExtendedPlugInNode(PlugInNode plugin)
		{
			Name = plugin.Name;
			Plugin = plugin;
		}
	}
}
