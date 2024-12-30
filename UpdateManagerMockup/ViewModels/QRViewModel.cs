using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Plugin.NFC;

namespace UpdateManagerMockup.ViewModels;

public partial class QRViewModel : ViewModelBase
{
    public RelayCommand StartNFCCommand { get; }

    private string _debugOutput = "";
    public string DebugOutput
    {
        get
        {
            return _debugOutput;
        }
    }

    public QRViewModel()
    {
        StartNFCCommand = new RelayCommand(OnNFCStart);
    }

    private void DbgOutput(string txt)
    {
        _debugOutput += txt + Environment.NewLine;
        Debug.WriteLine(txt);
        OnPropertyChanged(nameof(DebugOutput));
    }

    private void OnNFCStart()
    {
        DbgOutput("NFC Test");

        if (CrossNFC.IsSupported)
        {
            DbgOutput("NFC supported");

            if (CrossNFC.Current.IsAvailable)
            {
                DbgOutput("NFC available");

                if (CrossNFC.Current.IsEnabled)
                {
                    DbgOutput("NFC enabled");

                    DbgOutput("Wiring Events");
                    CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
                    CrossNFC.Current.OnTagDiscovered += Current_OnTagDiscovered;
                    CrossNFC.Current.OnNfcStatusChanged += Current_OnNfcStatusChanged;
                    CrossNFC.Current.OnTagConnected += Current_OnTagConnected;

                    DbgOutput("Start listening");
                    CrossNFC.Current.StartListening();
                }
                else
                {
                    DbgOutput("NFC not enabled");
                }
            }
            else
            {
                DbgOutput("NFC not available");
            }
        }
        else
        {
            DbgOutput("NFC not supported");
        }
    }

    private void Current_OnTagConnected(object? sender, EventArgs e)
    {
        DbgOutput("NFC Tag connected");
    }

    private void Current_OnNfcStatusChanged(bool isEnabled)
    {
        DbgOutput($"NFC Status Changed to {isEnabled}");
    }

    private void Current_OnTagDiscovered(ITagInfo tagInfo, bool format)
    {
        DbgOutput("NFC Tag discoverd");
        DbgOutput($"SerialNumber: {tagInfo.SerialNumber}");
        DbgOutput($"Capacity: {tagInfo.Capacity}");
        DbgOutput($"IsWritable: {tagInfo.IsWritable}");
        DbgOutput($"Records length: {tagInfo.Records.Length}");
        DbgOutput($"IsSupported: {tagInfo.IsSupported}");
    }

    private void Current_OnMessageReceived(ITagInfo tagInfo)
    {
        DbgOutput("NFC message received");
        DbgOutput($"SerialNumber: {tagInfo.SerialNumber}");
        DbgOutput($"Capacity: {tagInfo.Capacity}");
        DbgOutput($"IsWritable: {tagInfo.IsWritable}");
        DbgOutput($"IsSupported: {tagInfo.IsSupported}");
        DbgOutput($"Records length: {tagInfo.Records.Length}");
        if (tagInfo.Records.Any())
        {
            for (int i = 0; i< tagInfo.Records.Length; i++)
            {
                DbgOutput($"Record [{i}] Message: {tagInfo.Records[i].Message}");
            }
        }
    }
}
