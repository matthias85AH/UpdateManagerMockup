using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace UpdateManagerMockup.Views.UserControls;

public partial class TabUpdate : UserControl
{
    public static readonly RoutedEvent<RoutedEventArgs> PrevTabRequestedEvent =
        RoutedEvent.Register<TabUpdate, RoutedEventArgs>("PrevTabRequestedEvent", RoutingStrategies.Bubble);

    public TabUpdate()
    {
        InitializeComponent();
    }

    public void PrevButtonClicked(object source, RoutedEventArgs args)
    {
        Debug.WriteLine("PrevButtonClicked");

        RaiseEvent(new RoutedEventArgs(PrevTabRequestedEvent));
    }
}