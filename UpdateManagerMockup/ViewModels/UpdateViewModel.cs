using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeviceManagerMockup;
using UpdateManagerMockup.Views.UserControls;

namespace UpdateManagerMockup.ViewModels;

public partial class UpdateViewModel : ViewModelBase
{
    private int _updateProgressPercent = -1;

    IProgress<int> _updateProgress;

    public RelayCommand StartUpdateCommand { get; }
    //public RelayCommand SelectFirmwareCommand { get; }

    public UpdateViewModel()
    {
        _updateProgress = new Progress<int>(UpdateProgressChanged);

        StartUpdateCommand = new RelayCommand(OnUpdate);
        //SelectFirmwareCommand = new RelayCommand(OnSelectFirmware);
    }

    public int UpdateProgressPercent
    {
        get
        {
            return _updateProgressPercent;
        }
    }

    public string HeaderText
    {
        get
        {
            if (AppState.SelectedDevice != null)
            {
                return $"Update for {AppState.SelectedDevice.Address}";
            }
            else
            {
                return $"Please select a device to update";
            }
        }
    }

    private void OnUpdate()
    {
        // Your business logic here, for example:
        Debug.WriteLine("Update start!");

        _updateProgressPercent = -1;

        OnPropertyChanged(nameof(UpdateProgressPercent));

        Task.Run(() =>
        {
            AppState.SelectedDevice?.Update(_updateProgress, null);
        });
    }

    [RelayCommand]
    private async Task SelectFirmware(CancellationToken token)
    {
        Debug.WriteLine("Select Firmware called");
        //ErrorMessages?.Clear();
        try
        {
            var file = await DoOpenFilePickerAsync();
            if (file is null) return;

            // Limit the text file to 1MB so that the demo won't lag.
            if ((await file.GetBasicPropertiesAsync()).Size <= 1024 * 1024 * 1)
            {
                await using var readStream = await file.OpenReadAsync();
                using var reader = new StreamReader(readStream);
                Debug.WriteLine($"File Read with size {reader.BaseStream.Length}");
                //FileText = await reader.ReadToEndAsync(token);
            }
            else
            {
                throw new Exception("File exceeded 1MB limit.");
            }
        }
        catch (Exception e)
        {
            //ErrorMessages?.Add(e.Message);
        }
    }

    private async Task<IStorageFile?> DoOpenFilePickerAsync()
    {
        // For learning purposes, we opted to directly get the reference
        // for StorageProvider APIs here inside the ViewModel. 

        // For your real-world apps, you should follow the MVVM principles
        // by making service classes and locating them with DI/IoC.

        // See IoCFileOps project for an example of how to accomplish this.
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");

        var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open Text File",
            AllowMultiple = false
        });

        return files?.Count >= 1 ? files[0] : null;
    }

    //private void OnSelectFirmware()
    //{
    //    Debug.WriteLine("Select Firmware");

    //}

    private void UpdateProgressChanged(int progress)
    {
        _updateProgressPercent = progress;

        OnPropertyChanged(nameof(UpdateProgressPercent));
    }
}
