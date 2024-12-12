using System.Collections.ObjectModel;
using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using UpdateManagerMockup.Models;
using Avalonia.Interactivity;
using System.Diagnostics;
using UpdateManagerMockup.Events;

namespace UpdateManagerMockup.Views.UserControls;

public partial class TabDevice : UserControl
{
    public static readonly RoutedEvent<RoutedEventArgs> PrevTabRequestedEvent =
        RoutedEvent.Register<TabInterface, RoutedEventArgs>("PrevTabRequestedEvent", RoutingStrategies.Bubble);

    public static readonly RoutedEvent<RequestUpdateEventArgs> RequestedDeviceUpdateEvent =
        RoutedEvent.Register<TabDevice, RequestUpdateEventArgs>("RequestedDeviceUpdateEvent", RoutingStrategies.Bubble);

    public TabDevice()
    {
        InitializeComponent();

        btnUpdate.IsEnabled = false;

        dgDevices.SelectionChanged += DgDevices_SelectionChanged;
    }

    private void DgDevices_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (dgDevices.SelectedIndex >= 0)
        {
            btnUpdate.IsEnabled = true;
        }
    }

    public void PrevButtonClicked(object source, RoutedEventArgs args)
    {
        Debug.WriteLine("PrevButtonClicked");

        RaiseEvent(new RoutedEventArgs(PrevTabRequestedEvent));
    }

    public void UpdateButtonClicked(object source, RoutedEventArgs args)
    {
        Debug.WriteLine("UpdateButtonClicked");

        RaiseEvent(new RequestUpdateEventArgs(RequestedDeviceUpdateEvent, dgDevices.SelectedIndex.ToString()));
    }
}