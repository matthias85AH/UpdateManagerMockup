using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using UpdateManagerMockup.Services;

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

        await this.OpenFileDialogAsync("Hello Avalonia");
    }

    private void UpdateProgressChanged(int progress)
    {
        _updateProgressPercent = progress;

        OnPropertyChanged(nameof(UpdateProgressPercent));
    }
}
