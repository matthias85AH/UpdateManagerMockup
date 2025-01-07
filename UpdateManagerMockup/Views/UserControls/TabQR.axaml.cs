using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Maui.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using ZXing;


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
        Bitmap bitmapImg = new Bitmap(e.Media);

        Dispatcher.UIThread.Post(() => 
        {
            imgPicture.Source = bitmapImg;
        });

        e.Media.Seek(0, SeekOrigin.Begin);

        //byte[] imgJpegData;
        //using (MemoryStream ms = new MemoryStream())
        //{
        //    e.Media.CopyTo(ms);  // Copies all bytes from the stream to memoryStream
        //    imgJpegData = ms.ToArray();  // Returns the byte array from the memory stream
        //}

        //byte[] imgRawData;
        //using (MemoryStream ms = new MemoryStream())
        //{
        //    bitmapImg.Save(ms);
        //    imgRawData = ms.ToArray();  // Returns the byte array from the memory stream
        //}

        //Dispatcher.UIThread.Post(() =>
        //{
        //    txtDetectedContent.Text = "";
        //    txtDetectedContent.Text += $"ImageData Size: {bitmapImg.Size.Width} x {bitmapImg.Size.Height}{Environment.NewLine}";
        //    txtDetectedContent.Text += $"ImageData Format: {bitmapImg.Format}{Environment.NewLine}";
        //    txtDetectedContent.Text += $"ImageData Length: {imgJpegData.Length}{Environment.NewLine}";
        //    txtDetectedContent.Text += $"First Bytes: {string.Join(" ", imgJpegData.Take(10))}{Environment.NewLine}";
        //    txtDetectedContent.Text += $"Raw Data Length: {imgRawData.Length}{Environment.NewLine}";
        //    txtDetectedContent.Text += $"First Bytes: {string.Join(" ", imgRawData.Take(10))}{Environment.NewLine}";
        //});

        var imageSharpImg = SixLabors.ImageSharp.Image.Load<Rgb24>(e.Media);

        var reader = new ZXing.ImageSharp.BarcodeReader<Rgb24>();

        reader.Options.PossibleFormats = new List<BarcodeFormat>() { BarcodeFormat.QR_CODE, BarcodeFormat.DATA_MATRIX, BarcodeFormat.AZTEC };

        Result? decodeResult = null;

        try
        {
            decodeResult = reader.Decode<Rgb24>(imageSharpImg);
        }
        catch (Exception ex)
        {
            throw;
        }

        if (decodeResult != null)
        {
            Dispatcher.UIThread.Post(() =>
            {
                txtDetectedContent.Text = "";
                txtDetectedContent.Text += $"Decode Result{Environment.NewLine}";
                txtDetectedContent.Text += $"Barcode Format: {decodeResult.BarcodeFormat}{Environment.NewLine}";
                txtDetectedContent.Text += $"Text: {decodeResult.Text}{Environment.NewLine}";
            });
        }
        else
        {
            Dispatcher.UIThread.Post(() =>
            {
                txtDetectedContent.Text = "";
                txtDetectedContent.Text += $"No Barcode found {DateTime.Now.Second}";
            });
        }
    }

    private void TakePicture_Click(object sender, RoutedEventArgs e)
    {
        camView.CaptureImage(CancellationToken.None);
    }
}

