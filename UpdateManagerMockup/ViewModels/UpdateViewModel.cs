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

public partial class UpdateViewModel : ViewModelBase
{
    private int _updateProgressPercent = -1;

    IProgress<int> _updateProgress;

    public RelayCommand StartUpdateCommand { get; }

    public UpdateViewModel()
    {
        _updateProgress = new Progress<int>(UpdateProgressChanged);

        StartUpdateCommand = new RelayCommand(OnUpdate);
    }

    public int UpdateProgressPercent
    {
        get
        {
            return _updateProgressPercent;
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

    private void UpdateProgressChanged(int progress)
    {
        _updateProgressPercent = progress;

        OnPropertyChanged(nameof(UpdateProgressPercent));
    }
}
