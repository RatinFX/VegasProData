using ScriptPortal.Vegas;

namespace VegasProData
{
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
        public ExtendedPlugInNode(FavType type)
        {
            var name = "";
            switch (type)
            {
                case FavType.VideoFX: name = "VIDEO FX"; break;
                case FavType.AudioFX: name = "AUDIO FX"; break;
                case FavType.Generators: name = "GENERATORS"; break;
                case FavType.Transitions: name = "TRANSITIONS "; break;
                default: break;
            }

            Name = "- - - - " + name + " - - - -";
        }
        public ExtendedPlugInNode(PlugInNode plugin)
        {
            UniqueID = plugin.UniqueID ?? "";
            Name = plugin.Name;
            Plugin = plugin;
            OFXLabel = plugin.OFXPlugIn?.Label ?? "";
            OFXGrouping = plugin.OFXPlugIn?.Grouping ?? "";
        }

        public FavType GetFavType()
        {
            if (IsVideoFX) return FavType.VideoFX;
            if (IsAudioFX) return FavType.AudioFX;
            if (IsGenerator) return FavType.Generators;
            return FavType.Transitions;
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
}
