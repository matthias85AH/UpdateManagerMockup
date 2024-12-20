using System.Collections.Generic;
using System.Linq;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;

namespace UpdateManagerMockup.Android;

[Activity(
    Label = "UpdateManagerMockup.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    //Context? CTX;

    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        //CTX = this.ApplicationContext;

        List<string> neededPermissions = new List<string>(
            [
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.Bluetooth,
            Manifest.Permission.BluetoothAdmin,
            Manifest.Permission.BluetoothAdvertise,
            Manifest.Permission.BluetoothConnect,
            Manifest.Permission.BluetoothScan
            ]);

        //List<string> notGrantedPermissions = new List<string>();

        //foreach (string permission in neededPermissions)
        //{
        //    if (CTX.CheckSelfPermission(permission) == Permission.Denied)
        //        neededPermissions.Add(permission);
        //}

        //if (neededPermissions.Any())
        //{ RequestPermissions(neededPermissions.ToArray(), 2); }

        App.PermissionManager = new PermissionManager(this.ApplicationContext);

        return base.CustomizeAppBuilder(builder)
            .WithInterFont();
    }
}
