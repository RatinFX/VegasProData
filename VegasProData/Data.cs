using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Threading;
using Newtonsoft.Json;
using ScriptPortal.Vegas;

namespace VegasProData
{
    /// <summary>
    /// Favorite types
    /// </summary>
    public enum FavType
    {
        VideoFX,
        AudioFX,
        Generators,
        Transitions,
    }

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

    /// <summary>
    /// Saved Config JSON data
    /// </summary>
    public class Config
    {
        public bool DarkMode { get; set; } = true;
        public List<Favorite> Favorites { get; set; }

        public Config() { }
        public Config(bool init = false)
        {
            if (!init) return;

            Favorites = new List<Favorite> {
                new Favorite(FavType.VideoFX),
                new Favorite(FavType.AudioFX),
                new Favorite(FavType.Generators),
                //new Favorite(FavType.Transitions),
            };
        }

        public void Add(string uniqueID, FavType type)
        {
            var fav = Find(type, x => !x.UniqueIDs.Contains(uniqueID));
            if (fav == null) return;
            fav.UniqueIDs.Add(uniqueID);
        }

        public void Remove(string uniqueID, FavType type)
        {
            var fav = Find(type, x => x.UniqueIDs.Contains(uniqueID));
            if (fav == null) return;
            fav.UniqueIDs.Remove(uniqueID);
        }

        private Favorite Find(FavType type, Func<Favorite, bool> predicate)
        {
            return Favorites.Where(x => x.Type == type).FirstOrDefault(predicate);
        }

    }

    /// <summary>
    /// PlugInNode with more accessible info
    /// </summary>
    public class ExtendedPlugInNode
    {
        public string UniqueID { get; set; } = "";
        public string Name { get; set; } = "";
        public PlugInNode Plugin { get; set; }
        public string OFXLabel { get; set; } = "";
        public string OFXGrouping { get; set; } = "";
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
            UniqueID = plugin.UniqueID ?? "";
            Name = plugin.Name;
            Plugin = plugin;
            OFXLabel = plugin.OFXPlugIn?.Label ?? "";
            OFXGrouping = plugin.OFXPlugIn?.Grouping ?? "";
        }

        public bool Contains(string input) =>
            Search(UniqueID, input) ||
            Search(Name, input) ||
            Search(OFXLabel, input) ||
            Search(OFXGrouping, input) ||
            (Search(nameof(IsVideoFX), input) && IsVideoFX) ||
            (Search(nameof(IsAudioFX), input) && IsAudioFX) ||
            (Search(nameof(IsGenerator), input) && IsGenerator) ||
            (Search(nameof(IsTransition), input) && IsTransition)
            ;

        static bool Search(string text, string input) => text.ToLower().Contains(input.ToLower());
    }

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

        public static string ConfigFilePath = @".\VegasProData.json";

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
        public static IEnumerable<ExtendedPlugInNode> SearchIn(IEnumerable<ExtendedPlugInNode> list, string input = "") =>
            list.Where(x => x.Contains(input));

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
    /// Helper Methods
    /// </summary>
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

        /// <summary>
        /// Read or Create Config file
        /// </summary>
        public static void ReadConfig()
        {
            if (!File.Exists(Data.ConfigFilePath)) SaveConfig();
            var file = File.ReadAllText(Data.ConfigFilePath);
            Data.Config = JsonConvert.DeserializeObject<Config>(file);
        }

        /// <summary>
        /// Add FX to Favorites list and Save the list to a file
        /// </summary>
        /// <param name="uniqueID">UniqueID of the FX</param>
        public static void AddToFavorites(string uniqueID, FavType type)
        {
            Data.Config.Add(uniqueID, type);
            SaveConfig();
        }

        /// <summary>
        /// Remove FX from the Favorites list and Save the list to a file
        /// </summary>
        /// <param name="uniqueID">UniqueID of the FX</param>
        public static void RemoveFromFavorites(string uniqueID, FavType type)
        {
            Data.Config.Remove(uniqueID, type);
            SaveConfig();
        }

        /// <summary>
        /// Write the whole file
        /// TODO: only remove / add new lines instead
        /// </summary>
        public static void SaveConfig()
        {
            var config = JsonConvert.SerializeObject(Data.Config);
            File.WriteAllText(Data.ConfigFilePath, config);
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
    /// Original source:
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
