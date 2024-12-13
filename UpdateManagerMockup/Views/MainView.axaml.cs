using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using UpdateManagerMockup.Events;
using UpdateManagerMockup.Views.UserControls;

namespace UpdateManagerMockup.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        tabInterface.AddHandler(TabInterface.RequestDeviceSelection, OnRequestDeviceSelection, handledEventsToo: true);
        tabDevice.AddHandler(TabDevice.PrevTabRequestedEvent, OnGoToPrevTab, handledEventsToo: true);
        tabDevice.AddHandler(TabDevice.RequestedDeviceUpdateEvent, OnDeviceUpdateRequested, handledEventsToo: true);
        tabUpdate.AddHandler(TabUpdate.PrevTabRequestedEvent, OnGoToPrevTab, handledEventsToo: true);
    }

    private void OnGoToNextTab(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Switch to next Tab in Main View");
        tabControlMain.SelectedIndex += 1;
    }

    private void OnRequestDeviceSelection(object? sender, RoutedEventArgs e)
    {
        RequestDeviceSelectionEventArgs reqDeviceSelectionArgs = (RequestDeviceSelectionEventArgs)e;
        Debug.WriteLine($"Switch to Device Selection Tab. {reqDeviceSelectionArgs.SelectedInterface} selected");
        AppState.SelectedInterfaceType = reqDeviceSelectionArgs.SelectedInterface;
        tabControlMain.SelectedIndex = 1;
    }

    private void OnGoToPrevTab(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Switch to prev Tab in Main View");
        tabControlMain.SelectedIndex -= 1;
    }

    private void OnDeviceUpdateRequested(object? sender, RoutedEventArgs e)
    {
        RequestUpdateEventArgs reqUpdArgs = (RequestUpdateEventArgs)e;

        Debug.WriteLine($"Update of Device {reqUpdArgs.DeviceToUpdate.Address}");
        AppState.SelectedDevice = reqUpdArgs.DeviceToUpdate;
        tabControlMain.SelectedIndex = 2;
    }
}