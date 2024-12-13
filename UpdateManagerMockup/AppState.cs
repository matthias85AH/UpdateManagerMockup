using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceManagerMockup;

namespace UpdateManagerMockup
{
    public static class AppState
    {
        public static DeviceInterfaceType? SelectedInterfaceType {  get; set; }
        public static Device? SelectedDevice {  get; set; }

    }
}
