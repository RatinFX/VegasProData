#if VP14
using ScriptPortal.Vegas;
#elif VP13
using Sony.Vegas;
#endif
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using VegasProData.Base;

namespace VegasProData.Templates
{
    /// <summary>
    /// Example <see cref="UserControl"/> with a <see cref="Vegas"/> variable
    /// </summary>
    class ExampleUserControl : UserControl
    {
        Vegas VEGAS { get; set; }
        public ExampleUserControl(Vegas vegas)
        {
            // Set `Vegas` in `Data` to use
            // `VegasProData.Base` related values/methods
            Data.Vegas = vegas;

            // OR it can be used in
            // a local `Vegas` variable
            VEGAS = vegas;
        }
    }

    /// <summary>
    /// Change the following variables:
    /// <list type="number">
    ///     <item>
    ///         <term>"_I_NAME"</term>
    ///         <description>An internal value Vegas looks forto identify your Custom Command</description>
    ///     </item>
    ///     <item>
    ///         <term>"_DISPLAY_NAME"</term>
    ///         <description>The Displayed name of the Custom Command inside Vegas</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ExampleUserControl"/></term>
    ///         <description>Your <see cref="UserControl"/> class</description>
    ///     </item>
    /// </list>
    /// </summary>
    class ExampleDockableControl : DockableControl
    {
        private ExampleUserControl _mainControl;
        public ExampleDockableControl() : base("_I_NAME")
        {
            DisplayName = "_DISPLAY_NAME";
        }

        public override DockWindowStyle DefaultDockWindowStyle
        {
            get => DockWindowStyle.Floating;
        }

        public override Size DefaultFloatingSize
        {
            get => new Size(300, 300);
        }

        protected override void OnLoad(EventArgs e)
        {
            _mainControl = new ExampleUserControl(myVegas)
            {
                Dock = DockStyle.Fill,
            };

            Controls.Add(_mainControl);
        }

        protected override void OnClosed(EventArgs args)
        {
            base.OnClosed(args);
        }
    }

    /// <summary>
    /// Example Handler, runs when the user clicks clicks the <see cref="CustomCommand"/>
    /// under one of the <see cref="CommandCategory"/> menus
    /// </summary>
    class ExampleUserControlCustomCommandModule : ICustomCommandModule
    {
        /// <summary>
        /// Vegas application instance
        /// </summary>
        Vegas myVegas = null;

        /// <summary>
        /// Initialized CustomCommand named "_DISPLAY_NAME" under the "View" category
        /// </summary>
        readonly CustomCommand _cc = new CustomCommand(CommandCategory.View, "_DISPLAY_NAME");

        /// <summary>
        /// Initialize the module which hosts a set of custom commands
        /// <list type="bullet">
        ///     <item>Most likely runs during Vegas' loading screen</item>
        /// </list>
        /// </summary>
        public void InitializeModule(Vegas vegas)
        {
            myVegas = vegas;
            _cc.MenuItemName = "_DISPLAY_NAME";
        }

        /// <summary>
        /// Get the collection of <see cref="CustomCommand"/> objects hosted by this module
        /// </summary>
        public ICollection GetCustomCommands()
        {
            _cc.MenuPopup += HandleMenuPopup;
            _cc.Invoked += HandleInvoked;
            var ccs = new CustomCommand[] { _cc };
            return ccs;
        }

        /// <summary>
        /// Occurs just before the <see cref="CustomCommand"/>'s menu item appears
        /// </summary>
        private void HandleMenuPopup(object sender, EventArgs e)
        {
            // Check the CheckBox if the CC is already displayed
            _cc.Checked = myVegas.FindDockView("_I_NAME");
        }

        /// <summary>
        /// Occurs when the <see cref="CustomCommand"/> is invoked (clicked)
        /// </summary>
        private void HandleInvoked(object sender, EventArgs e)
        {
            // Is it already displayed?
            if (myVegas.ActivateDockView("_I_NAME"))
                return;

            // Create the new window
            var dock = new ExampleDockableControl
            {
                // Pass CC
                AutoLoadCommand = _cc,
                // Keeps it open all the time and reload on Vegas reload
                PersistDockWindowState = true
            };

            // Load the window
            myVegas.LoadDockView(dock);
        }
    }
}
