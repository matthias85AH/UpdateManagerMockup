using Avalonia.Interactivity;
using DeviceManagerMockup;

namespace UpdateManagerMockup.Events;

public class RequestDeviceSelectionEventArgs : RoutedEventArgs
{
    public DeviceInterfaceType SelectedInterface { get; }

    public RequestDeviceSelectionEventArgs(RoutedEvent routedEvent, DeviceInterfaceType selectedInterface)
        : base(routedEvent)
    {
        SelectedInterface = selectedInterface;
    }
}