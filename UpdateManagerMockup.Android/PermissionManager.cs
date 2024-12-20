using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.PM;

namespace UpdateManagerMockup.Android
{
    public class PermissionManager : IPermissionManager
    {
        private Context? _appContext;

        public PermissionManager(Context? applicationContext) 
        {
            _appContext = applicationContext;
        }

        public string PermissionStatus(string permission)
        {
            if (_appContext != null)
            {
                try
                {
                    return _appContext.CheckSelfPermission(permission).ToString();
                }
                catch (Exception)
                {
                    return "exception";
                }
                
            }
            else 
            { 
                return "no app context";
            }
        }
    }
}
