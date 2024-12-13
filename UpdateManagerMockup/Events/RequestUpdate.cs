using Avalonia.Interactivity;
using DeviceManagerMockup;

namespace UpdateManagerMockup.Events;

public class RequestUpdateEventArgs : RoutedEventArgs
{
    public Device DeviceToUpdate { get; }

    public RequestUpdateEventArgs(RoutedEvent routedEvent, Device deviceToUpdate)
        : base(routedEvent)
    {
        DeviceToUpdate = deviceToUpdate;
    }
}