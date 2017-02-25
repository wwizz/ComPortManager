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

        private NotifyIcon _trayIcon;
        private TrayMenu _trayMenu;
        private ComPortMonitor _comPortMonitor;

        public TrayApp()
        {
            InitializeComponent();

            CreateComPortMonitor();
            CreateTrayMenu();
            CreateTrayIcon();
            CreateTimer();

            _trayMenu.Update();
        }

        private void CreateTrayMenu()
        {
            _trayMenu = new TrayMenu {ComPortMonitor = _comPortMonitor};
        }

        private void CreateTrayIcon()
        {
            _trayIcon = new NotifyIcon
            {
                Text = Application.ProductName,
                Icon = Icon,
                ContextMenu = _trayMenu,
                Visible = true,
                BalloonTipTitle = Application.ProductName
            };
        }

        private void CreateComPortMonitor()
        {
            _comPortMonitor = new ComPortMonitor();
            _comPortMonitor.Change += (sender, s) => { OnComPortMonitorChange(s); };
        }

        private void CreateTimer()
        {
            var timer = new Timer {Interval = 5000};
            timer.Tick += (sender, args) => { _comPortMonitor.Update(); };
            timer.Start();
        }

        private void OnComPortMonitorChange(string e)
        {
            ShowBalloon(e);
            _trayMenu.Update();
        }


        private void ShowBalloon(string message)
        {
            _trayIcon.BalloonTipText = message;
            _trayIcon.ShowBalloonTip(5000);
        }
       
        protected override void OnLoad(EventArgs e)
        {
            Visible = false; 
            ShowInTaskbar = false;
            base.OnLoad(e);
        }

       
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
               _trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrayApp));
            this.SuspendLayout();
            // 
            // TrayApp
            // 
            this.ClientSize = new System.Drawing.Size(274, 229);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TrayApp";
            this.ResumeLayout(false);

        }
    }
}