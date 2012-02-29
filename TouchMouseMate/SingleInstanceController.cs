using System;
using System.Windows.Forms;
using Microsoft.Research.TouchMouseSensor;
using Microsoft.VisualBasic.ApplicationServices;

namespace Lextm.TouchMouseMate
{
    public sealed class SingleInstanceManager : WindowsFormsApplicationBase
    {
        [STAThread]
        public static void Main(string[] args)
        {
            new SingleInstanceManager().Run(args);
        }

        public SingleInstanceManager()
        {
            IsSingleInstance = true;
        }

        private NotifyIcon _icon;
        private TouchMouseCallback _callback;

        protected override bool OnStartup(StartupEventArgs e)
        {
            _callback = NativeMethods.TouchMouseCallbackFunction;
            // Set up callback with TouchMouseSensor DLL.
            TouchMouseSensorInterop.RegisterTouchMouseCallback(_callback);

            var strip = new ContextMenuStrip();
            var help = new ToolStripMenuItem("Help",
                                             Properties.Resources.help_browser,
                                             (sender, args) =>
                                             Help.ShowHelp(null, @"http://touchmousemate.codeplex.com"));
            var touchOverClick = new ToolStripMenuItem("Touch-over-click",
                                                         Properties.Resources.user_desktop,
                                                         (sender, args) =>
                                                             {
                                                                 var menu = (ToolStripMenuItem)sender;
                                                                 NativeMethods.Section.TouchOverClick = menu.Checked;
                                                                 NativeMethods.Config.Save();
                                                             }) { Checked = NativeMethods.Section.TouchOverClick, CheckOnClick = true }; 
            var middleClick = new ToolStripMenuItem("Middle-click",
                                                          Properties.Resources.start_here,
                                                          (sender, args) =>
                                                              {
                                                                  var menu = (ToolStripMenuItem)sender;
                                                                  NativeMethods.Section.MiddleClick = menu.Checked;
                                                                  NativeMethods.Config.Save();
                                                              }) {Checked = NativeMethods.Section.MiddleClick, CheckOnClick = true};
            var leftHand = new ToolStripMenuItem("Left-hand",
                                                 Properties.Resources.input_mouse,
                                                 (sender, args) =>
                                                     {
                                                         var menu = (ToolStripMenuItem)sender;
                                                         NativeMethods.Section.LeftHandMode = menu.Checked;
                                                         NativeMethods.Config.Save();
                                                     }) {Checked = NativeMethods.Section.LeftHandMode, CheckOnClick = true};
            var exit = new ToolStripMenuItem("Exit",
                                             Properties.Resources.system_log_out,
                                             (sender, args) =>
                                                 {
                                                     // Cancel callback with TouchMouseSensor DLL.
                                                     TouchMouseSensorInterop.
                                                         UnregisterTouchMouseCallback();
                                                     _icon.Visible = false;
                                                     Application.Exit();
                                                 });
            strip.Items.Add(middleClick);
            strip.Items.Add(touchOverClick);
            strip.Items.Add(leftHand);
            strip.Items.Add(help);
            strip.Items.Add(new ToolStripMenuItem("-"));
            strip.Items.Add(exit);
            //Published on November 23rd 2008 by Aston.
            //Released under the Free Art (copyleft) license.
            // Icon for Windows XP, Vista and 7.
            _icon = new NotifyIcon
                        {
                            Icon = Properties.Resources.mouse,
                            ContextMenuStrip = strip,
                            Visible = true
                        };
            _icon.ShowBalloonTip(5000, "Info", "Touch Mouse Mate has started", ToolTipIcon.Info);
            _icon.Text = string.Format("Touch Mouse Mate {0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            Application.Run();
            return false;
        }

        protected override void OnStartupNextInstance(
            StartupNextInstanceEventArgs eventArgs)
        {
            base.OnStartupNextInstance(eventArgs);
            _icon.ShowBalloonTip(5000, "Warning", "Touch Mouse Mate is already running", ToolTipIcon.Warning);
        }
    }
}
