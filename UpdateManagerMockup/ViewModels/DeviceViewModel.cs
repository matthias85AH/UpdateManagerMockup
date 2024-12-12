using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using UpdateManagerMockup.Models;

namespace UpdateManagerMockup.ViewModels;

public partial class DeviceViewModel : ViewModelBase
{
    public ObservableCollection<Device> Devices { get; }

    public DeviceViewModel() 
    { 
        List<Device> devices = new List<Device>();
        devices.Add(new Device() { Product = "MAUI", AdapterID= "C7-DE-8E-DD-6C-A8", InstalledFW="0.5.1", Status="Device needs update", Address= "C7-DE-8E-DD-6C-A8" });
        devices.Add(new Device() { Product = "MAILA", AdapterID= "6D-FB-26-C5-9E-3A", InstalledFW="1.4.0", Status="Device needs update", Address= "6D-FB-26-C5-9E-3A" });
        devices.Add(new Device() { Product = "MAUI", AdapterID= "67-3F-93-CC-FC-1B", InstalledFW="0.6.1", Status="Device is up to date", Address= "67-3F-93-CC-FC-1B" });
        devices.Add(new Device() { Product = "MAILA", AdapterID = "AA-EA-AB-52-CC-C0", InstalledFW="1.3.5", Status="Device needs update", Address= "AA-EA-AB-52-CC-C0" });

        Devices = new ObservableCollection<Device>(devices);

        OnPropertyChanged(nameof(Devices));
    }

    public void OutputText(string text)
    {
        Debug.WriteLine(text);
    }
}
