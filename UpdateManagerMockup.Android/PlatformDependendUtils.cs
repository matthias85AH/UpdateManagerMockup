using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
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
        public event EventHandler<string>? OnMessage;

        private Context? _appContext;

        public PlatformDependendUtils(Context? appContext)
        {
            _appContext = appContext;
        }

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

        public void StartBLE()
        {
            OnMessage?.Invoke(this, "BLE Scan starting");

            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            if (adapter != null)
                OnMessage?.Invoke(this, "Bluetooth adapter found.");
            else
            {
                OnMessage?.Invoke(this, "No Bluetooth adapter found.");
                return;
            }

            if (adapter.IsEnabled)
                OnMessage?.Invoke(this, "Bluetooth adapter is enabled.");
            else
            {
                OnMessage?.Invoke(this, "Bluetooth adapter is not enabled.");
                return;
            }

            BLEScanCallback myCallback = new BLEScanCallback(_appContext);
            myCallback.OnMessage += (s, e) => OnMessage?.Invoke(this, e);
            myCallback.OnReqScanStop += (s, e) => adapter.BluetoothLeScanner.StopScan(myCallback);

            adapter.BluetoothLeScanner.StartScan(myCallback);
        }
    }

    public class BLEScanCallback : ScanCallback
    {
        public event EventHandler<string>? OnMessage;
        public event EventHandler? OnReqScanStop;

        private Context? _appContext;

        public BLEScanCallback(Context? appContext)
        {
            _appContext = appContext;
        }

        public override void OnScanResult([GeneratedEnum] ScanCallbackType callbackType, ScanResult? result)
        {
            base.OnScanResult(callbackType, result);

            OnMessage?.Invoke(this, $"Scan Result: {result.Device.Name}, {result.AdvertisingSid}, {result.Device.Address}");

            if (result.Device.Name == "BLE Poti")
            {
                OnReqScanStop?.Invoke(this, new EventArgs());

                var myGattCallback = new BLEGattCallback();
                myGattCallback.OnMessage += (s, e) => OnMessage?.Invoke(this, e);

                var gattServer = result.Device.ConnectGatt(_appContext, false, myGattCallback);
                

            }
        }
    }

    public class BLEGattCallback : BluetoothGattCallback
    {
        public event EventHandler<string>? OnMessage;

        public override void OnConnectionStateChange(BluetoothGatt? gatt, [GeneratedEnum] GattStatus status, [GeneratedEnum] ProfileState newState)
        {
            base.OnConnectionStateChange(gatt, status, newState);

            OnMessage?.Invoke(this, $"Connection State changed: {status}, {newState}");

            OnMessage?.Invoke(this, $"Discovering Services");
            gatt?.DiscoverServices();
        }

        public override void OnServicesDiscovered(BluetoothGatt? gatt, [GeneratedEnum] GattStatus status)
        {
            base.OnServicesDiscovered(gatt, status);

            OnMessage?.Invoke(this, $"Services discovered: {status}");

            foreach (var service in gatt.Services)
            {
                OnMessage?.Invoke(this, $"Service: {service.Type}, {service.Uuid}, {service.InstanceId}, {service.Characteristics.Count}");

                if (service.Uuid.ToString().EndsWith("1914b"))
                {
                    OnMessage?.Invoke(this, $"Discovered Poti Service ");

                    foreach (var characteristic in service.Characteristics)
                    {
                        OnMessage?.Invoke(this, $"Characteristic discovered: {characteristic.Uuid}");

                        if (characteristic.Uuid.ToString().EndsWith("b26a8"))
                        {
                            OnMessage?.Invoke(this, $"Poti Characteristic discovered");

                            OnMessage?.Invoke(this, $"Showing descriptors");
                            
                            foreach (var descriptor in characteristic.Descriptors)
                            {
                                OnMessage?.Invoke(this, $"Descriptor: {descriptor.Uuid}");

                                if (descriptor.Uuid.ToString().EndsWith("34fb"))
                                {
                                    OnMessage?.Invoke(this, $"Notification Descriptor found. Enabling Notification");
                                    descriptor.SetValue(BluetoothGattDescriptor.EnableNotificationValue.ToArray());
                                    OnMessage?.Invoke(this, $"Writing descriptor");
                                    gatt.WriteDescriptor(descriptor);
                                }
                            }

                            OnMessage?.Invoke(this, $"Setting Characteristic notification");
                            gatt.SetCharacteristicNotification(characteristic, true);
                        }
                    }
                }
            }
        }

        public override void OnCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, byte[] value)
        {
            //base.OnCharacteristicChanged(gatt, characteristic, value);

            var potiValue = BitConverter.ToUInt16(value);
            OnMessage?.Invoke(this, $"Poti:{potiValue}");
        }
    }
}
