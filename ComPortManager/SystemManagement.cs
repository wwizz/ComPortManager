using System.Collections.Generic;
using System.Linq;
using System.Management;

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
    }
}