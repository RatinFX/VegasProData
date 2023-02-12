using System.Drawing;

namespace VegasProData.Theme
{
    public class Theme
    {
        public string Name { get; set; }
        public Color PanelBG { get; set; }
        public Color BoxBG { get; set; }
        public Color Highlight { get; set; }
        public Color Text { get; set; }
        public Theme() { }
        public Theme(string name, Color panelBG, Color boxBG, Color highlight, Color text)
        {
            Name = name;
            PanelBG = panelBG;
            BoxBG = boxBG;
            Highlight = highlight;
            Text = text;
        }

        public static Theme Dark { get; } = new Theme("Dark",
            Color.FromArgb(45, 45, 45),
            Color.FromArgb(70, 70, 70),
            Color.FromArgb(45, 45, 45),
            Color.White
        );
        public static Theme Light { get; } = new Theme("Light",
            Color.WhiteSmoke,
            Color.White,
            Color.WhiteSmoke,
            Color.Black
        );
    }
}
