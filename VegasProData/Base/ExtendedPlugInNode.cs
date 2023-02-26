﻿using ScriptPortal.Vegas;

namespace VegasProData.Base
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
        public ExtendedPlugInNode(PlugInNode plugin)
        {
            UniqueID = plugin.UniqueID ?? "";
            Name = plugin.Name;
            Plugin = plugin;
            OFXLabel = plugin.OFXPlugIn?.Label ?? "";
            OFXGrouping = plugin.OFXPlugIn?.Grouping ?? "";
        }
        public ExtendedPlugInNode(ExtendedPlugInNode p)
        {
            UniqueID = p.UniqueID;
            Name = p.Name;
            Plugin = p.Plugin;
            OFXLabel = p.OFXLabel;
            OFXGrouping = p.OFXGrouping;
            IsVideoFX = p.IsVideoFX;
            IsAudioFX = p.IsAudioFX;
            IsTransition = p.IsTransition;
            IsGenerator = p.IsGenerator;
        }

        public bool Contains(string input)
        {
            return
                Search(UniqueID, input) ||
                Search(Name, input) ||
                Search(OFXLabel, input) ||
                Search(OFXGrouping, input) ||
                Search(nameof(IsVideoFX), input) && IsVideoFX ||
                Search(nameof(IsAudioFX), input) && IsAudioFX ||
                Search(nameof(IsGenerator), input) && IsGenerator ||
                Search(nameof(IsTransition), input) && IsTransition
                ;
        }

        static bool Search(string text, string input)
        {
            return text.ToLower().Contains(input.ToLower());
        }
    }
}
