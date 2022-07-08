using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using ScriptPortal.Vegas;
//using Sony.Vegas;

namespace VegasProData
{
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
		public ExtendedPlugInNode(string name)
		{
			Name = name;
		}
		public ExtendedPlugInNode(PlugInNode plugin)
		{
			Name = plugin.Name;
			Plugin = plugin;
		}
	}

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

	public class Methods
	{

		/// <summary>
		/// Set an effect preset by name
		/// </summary>
		public static void SetPreset(Effect effect, string name)
		{
			var preset = effect.Presets.FirstOrDefault(x => x.Name == name)?.Name;
			if (preset != null || preset != "") effect.Preset = preset;
		}

		/// <summary>
		/// Set an effect preset by index
		/// </summary>
		public static void SetPreset(Effect effect, int index)
		{
			effect.Preset = effect.Presets[index].Name;
		}
	}

	/// <summary>
	/// Provides Debounce() and Throttle() methods.
	/// Use these methods to ensure that events aren't handled too frequently.
	/// 
	/// Throttle() ensures that events are throttled by the interval specified.
	/// Only the last event in the interval sequence of events fires.
	/// 
	/// Debounce() fires an event only after the specified interval has passed
	/// in which no other pending event has fired. Only the last event in the
	/// sequence is fired.
	/// 
	/// https://weblog.west-wind.com/posts/2017/Jul/02/Debouncing-and-Throttling-Dispatcher-Events
	/// </summary>
	public class DebounceDispatcher
	{
		private static DispatcherTimer timer;
		private static DateTime timerStarted { get; set; } = DateTime.UtcNow.AddYears(-1);
		private static DateTime curTime;

		/// <summary>
		/// This method throttles events by allowing only 1 event to fire for the given
		/// timeout period. Only the last event fired is handled - all others are ignored.
		/// Throttle will fire events every timeout ms even if additional events are pending.
		/// 
		/// Use Throttle where you need to ensure that events fire at given intervals.
		/// </summary>
		/// <param name="interval">Timeout in Milliseconds</param>
		/// <param name="action">Action<object> to fire when debounced event fires</object></param>
		/// <param name="param">optional parameter</param>
		/// <param name="priority">optional priorty for the dispatcher</param>
		/// <param name="disp">optional dispatcher. If not passed or null CurrentDispatcher is used.</param>
		public static void Throttle(Action<object> action, int interval = 250,
			object param = null,
			DispatcherPriority priority = DispatcherPriority.ApplicationIdle,
			Dispatcher disp = null)
		{

			Debounce(action, interval, param, priority, disp, throttle: true);

			timerStarted = curTime;
		}

		/// <summary>
		/// Debounce an event by resetting the event timeout every time the event is 
		/// fired. The behavior is that the Action passed is fired only after events
		/// stop firing for the given timeout period.
		/// 
		/// Use Debounce when you want events to fire only after events stop firing
		/// after the given interval timeout period.
		/// 
		/// Wrap the logic you would normally use in your event code into
		/// the  Action you pass to this method to debounce the event.
		/// Example: https://gist.github.com/RickStrahl/0519b678f3294e27891f4d4f0608519a
		/// </summary>
		/// <param name="interval">Timeout in Milliseconds</param>
		/// <param name="action">Action<object> to fire when debounced event fires</object></param>
		/// <param name="param">optional parameter</param>
		/// <param name="priority">optional priorty for the dispatcher</param>
		/// <param name="disp">optional dispatcher. If not passed or null CurrentDispatcher is used.</param>
		public static void Debounce(Action<object> action, int interval = 250,
			object param = null,
			DispatcherPriority priority = DispatcherPriority.ApplicationIdle,
			Dispatcher disp = null,
			bool throttle = false)
		{
			// kill pending timer and pending ticks
			timer?.Stop();
			timer = null;

			if (disp == null)
				disp = Dispatcher.CurrentDispatcher;

			if (throttle)
			{
				curTime = DateTime.UtcNow;
				// if timeout is not up yet - adjust timeout to fire 
				// with potentially new Action parameters           
				if (curTime.Subtract(timerStarted).TotalMilliseconds < interval)
					interval -= (int)curTime.Subtract(timerStarted).TotalMilliseconds;
			}

			// timer is recreated for each event and effectively
			// resets the timeout. Action only fires after timeout has fully
			// elapsed without other events firing in between
			timer = new DispatcherTimer(TimeSpan.FromMilliseconds(interval), priority, (s, e) =>
			{
				if (timer == null)
					return;

				timer?.Stop();
				timer = null;
				action.Invoke(param);
			}, disp);

			timer.Start();
		}
	}
}
