using VegasProData.Base;

namespace VegasProData.Favorites
{
    public class FavoriteExtendedPlugInNode : ExtendedPlugInNode
    {
        public static FavoriteExtendedPlugInNode New(ExtendedPlugInNode p)
        {
            return new FavoriteExtendedPlugInNode(p);
        }
        public FavoriteExtendedPlugInNode(ExtendedPlugInNode p) : base(p) { }
        public FavoriteExtendedPlugInNode(PlugInNodeType type)
        {
            var name = type switch
            {
                PlugInNodeType.VideoFX => "VIDEO FX",
                PlugInNodeType.AudioFX => "AUDIO FX",
                PlugInNodeType.Generator => "GENERATORS",
                PlugInNodeType.Transition => "TRANSITIONS ",
                _ => ""
            };

            Name = "- - - - " + name + " - - - -";

            IsVideoFX = type is PlugInNodeType.VideoFX;
            IsAudioFX = type is PlugInNodeType.AudioFX;
            IsGenerator = type is PlugInNodeType.Generator;
            IsTransition = type is PlugInNodeType.Transition;
        }

        public PlugInNodeType GetFavType()
        {
            if (IsVideoFX) return PlugInNodeType.VideoFX;
            if (IsAudioFX) return PlugInNodeType.AudioFX;
            if (IsGenerator) return PlugInNodeType.Generator;
            return PlugInNodeType.Transition;
        }
    }
}
