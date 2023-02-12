using VegasProData.Base;

namespace VegasProData.Favorites
{
    public class FavoriteExtendedPlugInNode : ExtendedPlugInNode
    {
        public static FavoriteExtendedPlugInNode New(ExtendedPlugInNode p) => new FavoriteExtendedPlugInNode(p);
        public FavoriteExtendedPlugInNode(ExtendedPlugInNode p) : base(p) { }
        public FavoriteExtendedPlugInNode(FavoriteType type)
        {
            var name = type switch
            {
                FavoriteType.VideoFX => "VIDEO FX",
                FavoriteType.AudioFX => "AUDIO FX",
                FavoriteType.Generators => "GENERATORS",
                FavoriteType.Transitions => "TRANSITIONS ",
                _ => ""
            };

            Name = "- - - - " + name + " - - - -";
        }

        public FavoriteType GetFavType()
        {
            if (IsVideoFX) return FavoriteType.VideoFX;
            if (IsAudioFX) return FavoriteType.AudioFX;
            if (IsGenerator) return FavoriteType.Generators;
            return FavoriteType.Transitions;
        }
    }
}
