using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeviceManagerMockup;
using UpdateManagerMockup.Views.UserControls;

namespace UpdateManagerMockup.ViewModels;

public partial class DeviceViewModel : ViewModelBase
{
    private int _scanProgressPercent = -1;

    IProgress<int> _scanProgress;
    Action<Device> _newDeviceObserver;

    public RelayCommand StartScanCommand { get; }

    public ObservableCollection<Device> Devices { get; }

    public DeviceViewModel()
    {
        _scanProgress = new Progress<int>(ScanProgressChanged);
        _newDeviceObserver = new Action<Device>(OnDeviceFound);

        StartScanCommand = new RelayCommand(OnScan);

        Devices = new ObservableCollection<Device>();
    }

    public string ScanButtonText
    {
        get
        {
            if (_scanProgressPercent == -1)
            {
                return "Start Scan";
            }
            else
            {
                return $"Scanning... ({_scanProgressPercent}%)";
            }
        }
    }

    private void OnScan()
    {
        // Your business logic here, for example:
        Debug.WriteLine("Scan start!");

        Devices.Clear();
        _scanProgressPercent = -1;

        OnPropertyChanged(nameof(ScanButtonText));
        
        Task.Run(() =>
        {
            DeviceManager.ScanAsync(AppState.SelectedInterfaceType, _scanProgress, null, _newDeviceObserver);
        });
    }

    private void ScanProgressChanged(int progress)
    {
        _scanProgressPercent = progress;

        OnPropertyChanged(nameof(ScanButtonText));
    }

    private void OnDeviceFound(Device device)
    {
        Debug.WriteLine($"Device found {device.Address}");

        Dispatcher.UIThread.Post(() => 
        {
            Devices.Add(device);
            OnPropertyChanged(nameof(Devices));
        });
    }
}
