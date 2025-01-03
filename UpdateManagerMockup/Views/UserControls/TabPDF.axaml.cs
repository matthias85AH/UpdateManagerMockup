using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using AvaloniaWebView;
using UpdateManagerMockup.Services;

namespace UpdateManagerMockup.Views.UserControls;

public partial class TabPDF : UserControl
{
    public TabPDF()
    {
        InitializeComponent();

        WebView wv = this.Get<WebView>("PDFWebView");
        wv.Loaded += Wv_Loaded;

        this.Loaded += TabPDF_Loaded;

        //if (App.PlatformDependendUtils != null)
        //{
            
        //    string configureWebClientResult = App.PlatformDependendUtils.ConfigureWebView(wv);
        //    this.Get<TextBlock>("Text1").Text = configureWebClientResult;
        //}

        //this.Get<Button>("Button1").Click += ButtonCounter_Click;
        //this.Get<Button>("Button2").Click += Button_PDF_internal_fromUri_Click;
        //this.Get<Button>("Button3").Click += Button_PDF_external_fromUri_Click;
        //this.Get<Button>("Button4").Click += Button_PDF_reader_external_Click;
        //this.Get<Button>("Button5").Click += Button_PDF_reader_internal_Click;
        //this.Get<Button>("Button6").Click += SaveAndLoadHTML_Clicked;
    }

    private void TabPDF_Loaded(object? sender, RoutedEventArgs e)
    {
        if (App.PlatformDependendUtils == null)
        {
            // Desktop
            var manualPDFPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "manual.pdf");
            var PDFstream = AssetLoader.Open(new System.Uri("avares://UpdateManagerMockup/Assets/example_manual_ld.pdf"));

            using (FileStream fileStream = new FileStream(manualPDFPath, FileMode.Create, FileAccess.Write))
            {
                // Copy the contents of the input stream to the file stream
                PDFstream.CopyTo(fileStream);
            }

            this.Get<WebView>("PDFWebView").Url = new System.Uri(manualPDFPath);
        }
    }

    private void Wv_Loaded(object? sender, RoutedEventArgs e)
    {
        // Android
        if (App.PlatformDependendUtils != null)
        {
            WebView wv = this.Get<WebView>("PDFWebView");
            string configureWebClientResult = App.PlatformDependendUtils.ConfigureWebView(wv);
            //this.Get<TextBlock>("Text1").Text = configureWebClientResult;
            UnzipAndShowPDF();
        }
    }

    private async void Button_PDF_external_fromUri_Click(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        if (topLevel != null)
        {
            await topLevel.Launcher.LaunchUriAsync(new System.Uri("https://adamhall.s3.amazonaws.com/media/MARKEN/LDSYSTEMS/LDSAT122G2WMB/LDSAT122G2WMB_LD_Systems_Bedienungsanleitung_EN_DE.pdf"));
        }
    }

    private void Button_PDF_reader_external_Click(object? sender, RoutedEventArgs e)
    {
       // throw new System.NotImplementedException();
    }

    private void Button_PDF_internal_fromUri_Click(object? sender, RoutedEventArgs e)
    {
        this.Get<WebView>("PDFWebView").Url = new System.Uri("https://adamhall.s3.amazonaws.com/media/MARKEN/LDSYSTEMS/LDSAT122G2WMB/LDSAT122G2WMB_LD_Systems_Bedienungsanleitung_EN_DE.pdf");
    }

    private void ButtonCounter_Click(object? sender, RoutedEventArgs e)
    {
        var backingFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "count.txt");

        if (backingFilePath == null)
        {
            this.Get<TextBlock>("Text1").Text = "Backing File Path null";
            return;
        }

        if (!File.Exists(backingFilePath))
        {
            this.Get<TextBlock>("Text1").Text = $"Path {backingFilePath} not existing. Trying to create it";

            try
            {
                File.WriteAllText(backingFilePath, "0");
            }
            catch (System.Exception ex)
            {
                this.Get<TextBlock>("Text1").Text = $"Error writing to {backingFilePath}. {ex.Message}";
            }
        }

        // Try to read file and increase counter
        try
        {
            var count = int.Parse(File.ReadAllText(backingFilePath));
            count++;
            this.Get<TextBlock>("Text1").Text = $"Counter: {count}";
            File.WriteAllText(backingFilePath, $"{count}");
        }
        catch (System.Exception ex)
        {
            this.Get<TextBlock>("Text1").Text = $"Error increasing counter to {backingFilePath}. {ex.Message}";
        }

        this.Get<WebView>("PDFWebView").Url = new System.Uri("https://www.google.de");
    }

    private void Button_PDF_reader_internal_Click(object? sender, RoutedEventArgs e)
    {
        //

        var manualPDFPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "manual.pdf");

        var PDFstream = AssetLoader.Open(new System.Uri("avares://UpdateManagerMockup/Assets/example_manual_ld.pdf"));

        if (App.PlatformDependendUtils != null)
        {
            string savePath = null;

            using (MemoryStream ms = new MemoryStream())
            {
                PDFstream.CopyTo(ms);
                savePath = App.PlatformDependendUtils.SaveFileToDownload("example_manual.pdf", ms.ToArray());
            }

            this.Get<WebView>("PDFWebView").Url = new System.Uri(savePath);
        }
        else
        {
            using (FileStream fileStream = new FileStream(manualPDFPath, FileMode.Create, FileAccess.Write))
            {
                // Copy the contents of the input stream to the file stream
                PDFstream.CopyTo(fileStream);
            }

            this.Get<WebView>("PDFWebView").Url = new System.Uri(manualPDFPath);
        }
    }

    private void UnzipAndShowPDF()
    {
        var zipStream = AssetLoader.Open(new System.Uri("avares://UpdateManagerMockup/Assets/manual_pdf_viewer.zip"));

        using (MemoryStream ms = new MemoryStream())
        {
            zipStream.CopyTo(ms);

            using (var archive = new ZipArchive(ms, ZipArchiveMode.Read))
            {
                foreach (var entry in archive.Entries)
                {
                    // Ensure the correct folder structure is created
                    string destinationPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "pdf_viewer", entry.FullName);

                    // If the entry is a directory, create the directory
                    if (entry.FullName.EndsWith("/"))
                    {
                        Directory.CreateDirectory(destinationPath);
                    }
                    else
                    {
                        // Create necessary directories for the file
                        string fileDirectory = Path.GetDirectoryName(destinationPath);
                        if (!Directory.Exists(fileDirectory))
                        {
                            Directory.CreateDirectory(fileDirectory);
                        }

                        // Extract the file
                        using (var entryStream = entry.Open())
                        using (var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
                        {
                            entryStream.CopyTo(fileStream);
                        }
                    }
                }
            }

        }

        // Copy PDF as manual.pdf
        var pdfPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "pdf_viewer", "manual.pdf");
        var pdfStream = AssetLoader.Open(new System.Uri("avares://UpdateManagerMockup/Assets/example_manual_ld.pdf"));

        using (FileStream fileStream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write))
        {
            pdfStream.CopyTo(fileStream);
        }

        var htmlPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "pdf_viewer", "viewer.html");
        this.Get<WebView>("PDFWebView").Url = new System.Uri(htmlPath);
    }

    private void SaveAndLoadHTML_Clicked(object? sender, RoutedEventArgs e)
    {
        //var htmlPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "helloworld_3.html");
        //var htmlStream = AssetLoader.Open(new System.Uri("avares://UpdateManagerMockup/Assets/helloworld_3.html"));

        //using (FileStream fileStream = new FileStream(htmlPath, FileMode.Create, FileAccess.Write))
        //{
        //    // Copy the contents of the input stream to the file stream
        //    htmlStream.CopyTo(fileStream);
        //}

        //var cssPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "styles.css");
        //var cssStream = AssetLoader.Open(new System.Uri("avares://UpdateManagerMockup/Assets/styles.css"));

        //using (FileStream fileStream = new FileStream(cssPath, FileMode.Create, FileAccess.Write))
        //{
        //    // Copy the contents of the input stream to the file stream
        //    cssStream.CopyTo(fileStream);
        //}

        //var scriptPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "script.js");
        //var scriptStream = AssetLoader.Open(new System.Uri("avares://UpdateManagerMockup/Assets/script.js"));

        //using (FileStream fileStream = new FileStream(scriptPath, FileMode.Create, FileAccess.Write))
        //{
        //    // Copy the contents of the input stream to the file stream
        //    scriptStream.CopyTo(fileStream);
        //}

        //var htmlPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "pdfjs2.html");
        //var htmlStream = AssetLoader.Open(new System.Uri("avares://UpdateManagerMockup/Assets/pdfjs2.html"));

        //using (FileStream fileStream = new FileStream(htmlPath, FileMode.Create, FileAccess.Write))
        //{
        //    htmlStream.CopyTo(fileStream);
        //}

        //var pdfPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "example_manual_ld.pdf");
        //var pdfStream = AssetLoader.Open(new System.Uri("avares://UpdateManagerMockup/Assets/example_manual_ld.pdf"));

        //using (FileStream fileStream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write))
        //{
        //    pdfStream.CopyTo(fileStream);
        //}

        //var cssPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "pdf.mjs");
        //var cssStream = AssetLoader.Open(new System.Uri("avares://UpdateManagerMockup/Assets/pdf.mjs"));

        //using (FileStream fileStream = new FileStream(cssPath, FileMode.Create, FileAccess.Write))
        //{
        //    cssStream.CopyTo(fileStream);
        //}

        //var scriptPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "pdf.worker.mjs");
        //var scriptStream = AssetLoader.Open(new System.Uri("avares://UpdateManagerMockup/Assets/pdf.worker.mjs"));

        //using (FileStream fileStream = new FileStream(scriptPath, FileMode.Create, FileAccess.Write))
        //{
        //    scriptStream.CopyTo(fileStream);
        //}

        //this.Get<WebView>("PDFWebView").Url = new System.Uri(htmlPath);

        
    }
}