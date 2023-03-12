using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace VegasProData.Theme
{
    public class ThemeController
    {
        public static void ChangeThemeTo(
            Theme scheme,
            Form form = null,
            UserControl userControl = null,
            ArrangedElementCollection elementCollection = null,
            Control.ControlCollection controlCollection = null,
            MenuStrip menuStrip = null
        )
        {
            if (menuStrip != null)
                menuStrip.Renderer = SetMenuStripRenderer(scheme);

            if (form != null)
                SetColors(scheme, form);

            if (userControl != null)
                SetColors(scheme, userControl);

            if (elementCollection != null)
                SetCollectionColors(scheme, elementCollection);

            if (controlCollection != null)
                SetCollectionColors(scheme, controlCollection);
        }

        public static ToolStripProfessionalRenderer SetMenuStripRenderer(Theme theme)
        {
            return new ToolStripProfessionalRenderer(new CustomProfessionalColors(theme));
        }

        public static void SetColors(Theme scheme, dynamic item, bool forceBoxOnBack = false)
        {
            item.ForeColor = scheme.Text;
            item.BackColor =
                forceBoxOnBack ? scheme.BoxBG
                : item is CheckBox || item is GroupBox || item is UserControl || item is Control ? scheme.PanelBG
                : scheme.BoxBG;
        }

        public static void SetCollectionColors(Theme theme, dynamic collection)
        {
            foreach (var item in collection)
            {
                if (item is MenuStrip)
                {
                    var i = item as MenuStrip;
                    SetColors(theme, i, forceBoxOnBack: true);
                    if (i.Items.Count == 0)
                        continue;

                    SetCollectionColors(theme, i.Items);
                    continue;
                }

                if (item is ToolStripMenuItem)
                {
                    var i = item as ToolStripMenuItem;
                    SetColors(theme, i);
                    if (i.DropDownItems.Count == 0)
                        continue;

                    SetCollectionColors(theme, i.DropDownItems);
                    continue;
                }

                var c = item as Control;
                SetColors(theme, c);
                if (c.Controls.Count == 0)
                    continue;

                SetCollectionColors(theme, c.Controls);
            }
        }
    }
}
