using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace UpdateManagerMockup.Views.UserControls;

public partial class TabInterface : UserControl
{
    public static readonly RoutedEvent<RoutedEventArgs> NextTabRequestedEvent =
        RoutedEvent.Register<TabInterface, RoutedEventArgs>("NextTabRequestedEvent", RoutingStrategies.Bubble);

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

        RaiseEvent(new RoutedEventArgs(NextTabRequestedEvent));
    }
}