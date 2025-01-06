using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaWebView;
using CommunityToolkit.Maui.Core;

namespace UpdateManagerMockup
{
    public interface IPlatformDependendUtils
    {
        ICameraProvider CameraProvider { get; }

        event EventHandler<string> OnMessage;

        string SaveFileToDownload(string fileName, byte[] fileData);

        string SaveFileToTemp(string fileName, byte[] fileData);

        string ConfigureWebView(WebView wv);

        void StartBLE();
    }
}
