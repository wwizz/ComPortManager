using System.Windows.Forms;

namespace ComPortManager
{
    public class TrayMenu : ContextMenu
    {
        public void Update()
        {
            MenuItems.Clear();
            AddMenuItemAvailableComPorts();
            AddMenuItemSeperator();
            AddMenuItemStartup();
            AddMenuItemExit();
        }

        private void AddMenuItemSeperator()
        {
            MenuItems.Add("-");
        }

        private void AddMenuItemAvailableComPorts()
        {
            MenuItems.Add("Available COM ports:");
            foreach (var port in ComPortMonitor.CurrentPortList)
            {
                MenuItems.Add(new MenuItem { Text = port });
            }
        }

        private void AddMenuItemStartup()
        {
            var item = new MenuItem
            {
                Text = "Run " + Application.ProductName + " on startup",
                Checked = SystemManagement.IsEnabledStartup(Application.ProductName)
            };

            item.Click += (sender, args) => {
                if (item.Checked)
                {
                    SystemManagement.DisableStartup(Application.ProductName);
                }
                else
                {
                    SystemManagement.EnableStartup(Application.ProductName);
                }
                item.Checked = SystemManagement.IsEnabledStartup(Application.ProductName);

            };
            MenuItems.Add(item);
        }

        private void AddMenuItemExit()
        {
            MenuItems.Add("Exit", (sender, args) => { Application.Exit();});
        }

        public ComPortMonitor ComPortMonitor { get; set; }
    }
}