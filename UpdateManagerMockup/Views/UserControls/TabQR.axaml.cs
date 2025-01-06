using System;
using System.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Maui.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace UpdateManagerMockup.Views.UserControls;

public partial class TabQR : UserControl
{
    private ICameraProvider cameraProvider;

    public TabQR()
    {
        InitializeComponent();
        CreateButton();
        CreateCameraView();
    }

    private void CreateButton()
    {
        // Create the Button
        Avalonia.Controls.Button newButton = new Avalonia.Controls.Button
        {
            Content = "Start",
            Width = 200,
            Height = 50
        };

        // Subscribe to Button click event
        newButton.Click += NewButton_Click;

        // Find the parent container (e.g., StackPanel, Grid, etc.)
        var parentContainer = this.FindControl<StackPanel>("mainStackPanel");

        // Add the button to the parent container
        parentContainer?.Children.Add(newButton);
    }

    private void CreateCameraView()
    {
        var grid = new Avalonia.Controls.Grid();

        var canvas = new Canvas();
        var rectangle = new Rectangle
        {
            Width = 30,
            Height = 30,
            Fill = Brushes.Red
        };

        Canvas.SetLeft(rectangle, 10);
        Canvas.SetTop(rectangle, 10);

        canvas.Children.Add(rectangle);

        var img = new Avalonia.Controls.Image();
        img.Source = new Bitmap(AssetLoader.Open(new Uri("avares://UpdateManagerMockup/Assets/LdLogo_w.png")));

        var controlHost = new MauiControlHost();

        var camView = new CameraView();

        controlHost.Content = camView;

        grid.Children.Add(controlHost);
        //grid.Children.Add(img);

        controlHost.ZIndex = 1;
        img.ZIndex = 2;

        // Find the parent container (e.g., StackPanel, Grid, etc.)
        var parentContainer = this.FindControl<StackPanel>("mainStackPanel");

        // Add the button to the parent container
        parentContainer?.Children.Add(grid);
        controlHost.Width = 400;
        controlHost.Height = 300;
    }

    private void NewButton_Click(object sender, RoutedEventArgs e)
    {
        // Handle the button click event
        var button = sender as Avalonia.Controls.Button;
        button.Content = "Clicked!";
    }
}

