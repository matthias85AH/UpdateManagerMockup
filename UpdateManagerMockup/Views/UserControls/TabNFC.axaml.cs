using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace UpdateManagerMockup.Views.UserControls;

public partial class TabNFC : UserControl
{
    public TabNFC()
    {
        InitializeComponent();
    }

    private void ConnectCommand(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
}