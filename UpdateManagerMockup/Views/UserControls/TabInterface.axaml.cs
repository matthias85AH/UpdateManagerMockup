using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DeviceManagerMockup;
using UpdateManagerMockup.Events;

namespace UpdateManagerMockup.Views.UserControls;

public partial class TabInterface : UserControl
{
    public static readonly RoutedEvent<RequestDeviceSelectionEventArgs> RequestDeviceSelection =
        RoutedEvent.Register<TabDevice, RequestDeviceSelectionEventArgs>("RequestDeviceSelection", RoutingStrategies.Bubble);

    public TabInterface()
    {
        InitializeComponent();

        btnNext.IsEnabled = false;

        lbInterfaceSelect.SelectionChanged += LbInterfaceSelect_SelectionChanged;
    }

    private void LbInterfaceSelect_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if(lbInterfaceSelect.SelectedIndex >= 0)
        {
            btnNext.IsEnabled = true;
        }
    }

    public void NextButtonClicked(object source, RoutedEventArgs args)
    {
        Debug.WriteLine("NextButtonClicked");

        RaiseEvent(new RequestDeviceSelectionEventArgs(RequestDeviceSelection, (DeviceInterfaceType)(lbInterfaceSelect.SelectedIndex)));
    }
}