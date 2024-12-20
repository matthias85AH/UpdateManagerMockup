using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateManagerMockup.Desktop
{
    public class PermissionManager : IPermissionManager
    {
        public string PermissionStatus(string permission)
        {
            return "granted";
        }
    }
}

