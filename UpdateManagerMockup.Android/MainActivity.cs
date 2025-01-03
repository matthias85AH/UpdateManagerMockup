using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Avalonia;
using Avalonia.Android;
using Plugin.NFC;
using Avalonia.WebView.Android;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android;
using Android.Widget;

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
        //List<string> notGrantedPermissions = new List<string>();

        //foreach (string permission in neededPermissions)
        //{
        //    if (CTX.CheckSelfPermission(permission) == Permission.Denied)
        //        neededPermissions.Add(permission);
        //}

        //if (neededPermissions.Any())
        //{ RequestPermissions(neededPermissions.ToArray(), 2); }

        App.PermissionManager = new PermissionManager(this.ApplicationContext);
        App.PlatformDependendUtils = new PlatformDependendUtils();

        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .UseAndroidWebView();
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        CrossNFC.Init(this);

        base.OnCreate(savedInstanceState);

        if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
        {
            Toast.MakeText(this, "External Write Storage Permission needed", ToastLength.Long);
            ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, 1);
        }

        if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
        {
            Toast.MakeText(this, "External Read Storage Permission needed", ToastLength.Long);
            ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage }, 1);
        }

        
    }

    protected override void OnResume()
    {
        base.OnResume();

        CrossNFC.OnResume();
    }

    protected override void OnNewIntent(Intent? intent)
    {
        base.OnNewIntent(intent);

        CrossNFC.OnNewIntent(intent);
    }
}
