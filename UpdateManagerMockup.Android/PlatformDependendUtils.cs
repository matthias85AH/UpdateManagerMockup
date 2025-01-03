using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.Webkit;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Avalonia.WebView.Android.Core;
using AvaloniaWebView;
using Java.Nio.FileNio.Attributes;

namespace UpdateManagerMockup.Android
{
    public class PlatformDependendUtils : IPlatformDependendUtils
    {
        public string SaveFileToDownload(string fileName, byte[] fileData)
        {
            // Get the path to the Downloads folder
            var downloadsPath = global::Android.OS.Environment.GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryDownloads).AbsolutePath;

            // Create the file path
            var filePath = Path.Combine(downloadsPath, fileName);

            // Save the file
            if (!File.Exists(filePath))
            {
                File.WriteAllBytes(filePath, fileData);
            }

            return filePath;
        }

        public string ConfigureWebView(AvaloniaWebView.WebView wv)
        {
            if (wv.PlatformWebView is AndroidWebViewCore awvc)
            {
                awvc.WebView.Settings.JavaScriptEnabled = true;
                awvc.WebView.Settings.AllowFileAccess = true;
                awvc.WebView.Settings.AllowFileAccessFromFileURLs = true;
                awvc.WebView.Settings.AllowUniversalAccessFromFileURLs = true;
                return "Settings made";
            }
            else
            {
                if (wv.PlatformWebView != null)
                {
                    return $"PlatformWebView is {wv.PlatformWebView.GetType().Name}";
                }
                else
                {
                    return $"PlatformWebView is null";
                }
            }
            
        }

        public string SaveFileToTemp(string fileName, byte[] fileData)
        {
            // Get the path to the Downloads folder
            var downloadsPath = global::Android.OS.Environment.GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryDownloads).AbsolutePath;

            // Create the file path
            var filePath = Path.Combine(downloadsPath, fileName);

            // Save the file
            if (!File.Exists(filePath))
            {
                File.WriteAllBytes(filePath, fileData);
            }

            return filePath;
        }
    }
}
