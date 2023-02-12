using System.Drawing;
using System.Windows.Forms;

namespace VegasProData.Theme
{
    public class CustomProfessionalColors : ProfessionalColorTable
    {
        public Color Highlight { get; set; }
        public CustomProfessionalColors() { }
        public CustomProfessionalColors(Theme theme)
        {
            Highlight = theme.Highlight;
        }
        public override Color MenuItemBorder => Color.Transparent;
        public override Color MenuItemSelected => Highlight;
        public override Color MenuItemPressedGradientBegin => Highlight;
        public override Color MenuItemPressedGradientEnd => Highlight;
        public override Color MenuItemSelectedGradientBegin => Highlight;
        public override Color MenuItemSelectedGradientEnd => Highlight;
    }
}
