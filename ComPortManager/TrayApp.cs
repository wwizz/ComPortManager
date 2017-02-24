using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComPortManager
{
    public class TrayApp : Form
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TrayApp());
        }

        private const string AppTitle = "ComPortManager";
        private readonly NotifyIcon _trayIcon;
        private readonly ContextMenu _trayMenu;
        private readonly ComPortMonitor _comPortMonitor;

        public TrayApp()
        {
            _trayMenu = new ContextMenu();
            _trayIcon = new NotifyIcon
            {
                Text = AppTitle,
                Icon = new Icon(SystemIcons.Shield, 40, 40),
                ContextMenu = _trayMenu,
                Visible = true,
                BalloonTipTitle = AppTitle
            };

            var timer = new Timer {Interval = 5000};
            timer.Tick += (sender, args) => { _comPortMonitor.Update(); };
            timer.Start();

            _comPortMonitor = new ComPortMonitor();
            _comPortMonitor.Change += (sender, s) => { OnComPortMonitorChange(s); };
            UpdateTrayMenu();
        }

        private void OnComPortMonitorChange(string e)
        {
            ShowBalloon(e);
            UpdateTrayMenu();
        }

        private void AddMenuItemExit()
        {
            _trayMenu.MenuItems.Add("Exit", OnExit);
        }


        private void ShowBalloon(string message)
        {
            _trayIcon.BalloonTipText = message;
            _trayIcon.ShowBalloonTip(5000);
        }


        private void UpdateTrayMenu()
        {
            _trayMenu.MenuItems.Clear();
            foreach (var port in _comPortMonitor.CurrentPortList)
            {
                _trayMenu.MenuItems.Add(port);
            }
            _trayMenu.MenuItems.Add("---");
            AddMenuItemExit();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                _trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}