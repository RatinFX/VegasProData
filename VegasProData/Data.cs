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

		/**
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
		public static PlugInNode VideoFx => Vegas.VideoFX;

		/// <summary>
		/// Available Audio Effects
		/// </summary>
		public static PlugInNode AudioFX => Vegas.AudioFX;

		/// <summary>
		/// Available Generators
		/// </summary>
		public static PlugInNode Generators => Vegas.Generators;

		/// <summary>
		/// Available Transitions
		/// </summary>
		public static PlugInNode Transitions => Vegas.Transitions;

		/**
		 *  TRACKS
		 */
		/// <summary>
		/// Vegas Tracks
		/// </summary>
		public static Tracks Tracks => Vegas.Project.Tracks;

		/**
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

		/**
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
	/// Extended PlugInNode
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
		public ExtendedPlugInNode(PlugInNode plugin, bool isVideoFX, bool isAudioFX, bool isTransition, bool isGenerator)
		{
			Plugin = plugin;
			IsVideoFX = isVideoFX;
			IsAudioFX = isAudioFX;
			IsTransition = isTransition;
			IsGenerator = isGenerator;
		}
		public ExtendedPlugInNode(string name, PlugInNode plugin, bool isVideoFX, bool isAudioFX, bool isTransition, bool isGenerator)
			: this(plugin, isVideoFX, isAudioFX, isTransition, isGenerator)
		{
			Name = name;
		}

		/// <summary>
		/// VEGAS VideoFX
		/// </summary>
		public IEnumerable<ExtendedPlugInNode> VideoFX => Separate(Data.Vegas.VideoFX, isVideoFX: true);

		/// <summary>
		/// VEGAS AudioFX
		/// </summary>
		public IEnumerable<ExtendedPlugInNode> AudioFX => Separate(Data.Vegas.AudioFX, isAudioFX: true);

		/// <summary>
		/// VEGAS Transitions
		/// </summary>
		public IEnumerable<ExtendedPlugInNode> Transitions => Separate(Data.Vegas.Transitions, isTransition: true);

		/// <summary>
		/// VEGAS Generators
		/// </summary>
		public IEnumerable<ExtendedPlugInNode> Generators => Separate(Data.Vegas.Generators, isGenerator: true);

		/// <summary>
		/// Separate the different PlugInNode lists by types
		/// </summary>
		/// <returns>PlugInNode list</returns>
		public IEnumerable<ExtendedPlugInNode> Separate(PlugInNode list, bool isVideoFX = false, bool isAudioFX = false, bool isTransition = false, bool isGenerator = false)
		{
			return from plugin in list.Where(x => !x.IsContainer)
				   select new ExtendedPlugInNode(plugin.Name, plugin, isVideoFX, isAudioFX, isTransition, isGenerator);
		}

		/// <summary>
		/// Search in the given List
		/// </summary>
		/// <returns>Filtered list</returns>
		public IEnumerable<ExtendedPlugInNode> SearchIn(IEnumerable<ExtendedPlugInNode> list, string filter = "")
		{
			return list.Where(x => x.Plugin.Name.ToLower().Contains(filter.ToLower()));
		}
	}
}
