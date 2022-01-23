using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptPortal.Vegas;

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
}

