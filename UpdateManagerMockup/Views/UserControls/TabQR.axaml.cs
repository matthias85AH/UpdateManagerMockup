using System;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Maui.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui;

namespace UpdateManagerMockup.Views.UserControls;

public partial class TabQR : UserControl
{
    private CameraView camView;

    public TabQR()
    {
        InitializeComponent();
        CreateDynamicControls();
    }

    private void CreateDynamicControls()
    {
        var mainGrid = this.FindControl<Grid>("mainGrid");

        btnTakePicture.Click += TakePicture_Click;

        // Camera Preview
        if (!Design.IsDesignMode)
        {
            var controlHost = new MauiControlHost();
            camView = new CameraView();
            camView.MediaCaptured += CamView_MediaCaptured;

            controlHost.Content = camView;
            //controlHost.Width = 400;
            //controlHost.Height = 300;

            Grid.SetColumn(controlHost, 1);
            Grid.SetRow(controlHost, 0);
            mainGrid?.Children.Add(controlHost);
        }
        else
        {
            var imgPlaceholder = new Avalonia.Controls.Image();
            imgPlaceholder.Source = new Bitmap(AssetLoader.Open(new Uri("avares://UpdateManagerMockup/Assets/qr_code_scan_placeholder.jpg")));
            Grid.SetColumn(imgPlaceholder, 1);
            Grid.SetRow(imgPlaceholder, 0);
            mainGrid?.Children.Add(imgPlaceholder);
        }
    }

    private void CamView_MediaCaptured(object? sender, MediaCapturedEventArgs e)
    {
        Dispatcher.UIThread.Post(() => 
        {
            imgPicture.Source = new Bitmap(e.Media);
        });
    }

    private void TakePicture_Click(object sender, RoutedEventArgs e)
    {
        camView.CaptureImage(CancellationToken.None);
    }
}

