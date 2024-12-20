using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeviceManagerMockup;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE;
using UpdateManagerMockup.Views.UserControls;

namespace UpdateManagerMockup.ViewModels;

public partial class BLEViewModel : ViewModelBase
{
    private IDevice device;

    private CancellationTokenSource cts = new();
    private bool deviceFound = false;

    public AsyncRelayCommand ConnectCommand { get; }

    private int _levelMeterValue = 80;
    public int LevelMeterValue
    {
        get
        {
            return _levelMeterValue;
        }
    }

    private string _debugOutput = "";
    public string DebugOutput
    {
        get
        {
            return _debugOutput;
        }
    }

    public BLEViewModel()
    {
        ConnectCommand = new AsyncRelayCommand(OnConnect);
    }

    private void DbgOutput(string txt)
    {
        _debugOutput += txt + Environment.NewLine;
        Debug.WriteLine(txt);
        OnPropertyChanged(nameof(DebugOutput));
    }

    private async Task OnConnect()
    {
        DbgOutput("Bluetooth Test");

        if (App.PermissionManager != null)
        {
            DbgOutput($"READ_EXTERNAL_STORAGE:  {App.PermissionManager.PermissionStatus("android.permission.READ_EXTERNAL_STORAGE")}");
            DbgOutput($"WRITE_EXTERNAL_STORAGE: {App.PermissionManager.PermissionStatus("android.permission.WRITE_EXTERNAL_STORAGE")}");
            DbgOutput($"ACCESS_COARSE_LOCATION: {App.PermissionManager.PermissionStatus("android.permission.ACCESS_COARSE_LOCATION")}");
            DbgOutput($"ACCESS_FINE_LOCATION: {App.PermissionManager.PermissionStatus("android.permission.ACCESS_FINE_LOCATION")}");
            DbgOutput($"BLUETOOTH: {App.PermissionManager.PermissionStatus("android.permission.BLUETOOTH")}");
            DbgOutput($"BLUETOOTH_ADMIN: {App.PermissionManager.PermissionStatus("android.permission.BLUETOOTH_ADMIN")}");
            DbgOutput($"BLUETOOTH_ADVERTISE: {App.PermissionManager.PermissionStatus("android.permission.BLUETOOTH_ADVERTISE")}");
            DbgOutput($"BLUETOOTH_CONNECT: {App.PermissionManager.PermissionStatus("android.permission.BLUETOOTH_CONNECT")}");
            DbgOutput($"BLUETOOTH_SCAN: {App.PermissionManager.PermissionStatus("android.permission.BLUETOOTH_SCAN")}");
        }

        var ble = CrossBluetoothLE.Current;
        var adapter = CrossBluetoothLE.Current.Adapter;

        adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
        adapter.ScanMode = ScanMode.Balanced;

        try
        {
            DbgOutput("Starting Scan");
            // Start scanning asynchronously and pass the cancellation token
            var scanTask = adapter.StartScanningForDevicesAsync(cancellationToken: cts.Token);

            // Wait for the device to be discovered or timeout after 3 seconds
            var delayTask = Task.Delay(3000, cts.Token);

            // Wait for either the scan to complete or the delay to expire
            var completedTask = await Task.WhenAny(scanTask, delayTask);

            // If the delay task completed first, stop scanning
            if (!deviceFound)
            {
                DbgOutput("Timeout reached, stopping scan.");
                cts.Cancel();  // Cancel the scanning task
            }
        }
        catch (Exception e)
        {
            DbgOutput($"Exception: {e}");
        }

        if (deviceFound)
        {
            try
            {
                DbgOutput("Retrieving Services...");
                var services = await device.GetServicesAsync();
                foreach (var service in services)
                {
                    DbgOutput($"Found Service: {service.Id}");
                    var characteristics = await service.GetCharacteristicsAsync();

                    foreach (var characteristic in characteristics)
                    {
                        DbgOutput($"  Found Characteristic: ID:{characteristic.Id}");
                        if (characteristic.Id.ToString().EndsWith("6a8"))
                        {
                            DbgOutput($"Found notification Characteristic");
                            characteristic.ValueUpdated += Characteristic_ValueUpdated;

                            await characteristic.StartUpdatesAsync();
                        }
                    }
                }
            }
            catch (Exception)
            {
                DbgOutput("Error while retrieving Services");
            }

            await Task.Delay(Timeout.Infinite);
        }

        DbgOutput("Test ended");

        //_levelMeterValue = 20;
        //await Dispatcher.UIThread.InvokeAsync(() =>
        //{
        //    OnPropertyChanged(nameof(LevelMeterValue));
        //});
    }

    private void Characteristic_ValueUpdated(object? sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs e)
    {
        var potiValue = BitConverter.ToUInt16(e.Characteristic.Value);
        DbgOutput($"New Value received: {potiValue}");
        _levelMeterValue = potiValue * 100 / 4096;
        DbgOutput($"New LevelMeter Value: {_levelMeterValue}");
        OnPropertyChanged(nameof(LevelMeterValue));
    }

    private void Adapter_DeviceDiscovered(object? sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
    {
        if (e.Device.Id.ToString().EndsWith("1097bdd2923a"))
        {
            DbgOutput($"Found Poti");
            deviceFound = true;
            cts.Cancel();
        }

        //Console.WriteLine($"Found Device ID:{e.Device.Id} Name:{e.Device.Name} Name:{e.Device}");
        device = e.Device;
    }
}
