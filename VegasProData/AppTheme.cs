using System.Drawing;

namespace VegasProData
{
    public class AppTheme
    {
        public string Name { get; set; }
        public Color PanelBG { get; set; }
        public Color BoxBG { get; set; }
        public Color Highlight { get; set; }
        public Color Text { get; set; }
        public AppTheme() { }
        public AppTheme(string name, Color panelBG, Color boxBG, Color highlight, Color text)
        {
            Name = name;
            PanelBG = panelBG;
            BoxBG = boxBG;
            Highlight = highlight;
            Text = text;
        }
    }
}
