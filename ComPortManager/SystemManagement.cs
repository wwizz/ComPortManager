using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ComPortManager
{
    public class SystemManagement
    {
        public static List<string> GetFriendlyComPortNames()
        {
            List<string> ports;
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
            {
                ports = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString()).ToList();
            }
            return ports;
        }

        public static bool IsEnabledStartup(string name)
        {
            return GetRunKey().GetValue(name) != null;
        }


        public static void EnableStartup(string name)
        {
            GetRunKey().SetValue(name, Application.ExecutablePath);
        }

        public static void DisableStartup(string name)
        {
            GetRunKey().DeleteValue(name, false);

        }
        private static RegistryKey GetRunKey()
        {
            return Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        }

    }
}