using Avalonia.Interactivity;

namespace UpdateManagerMockup.Events;

public class RequestUpdateEventArgs : RoutedEventArgs
{
    public string DeviceToUpdate { get; }

    public RequestUpdateEventArgs(RoutedEvent routedEvent, string deviceToUpdate)
        : base(routedEvent)
    {
        DeviceToUpdate = deviceToUpdate;
    }
}