using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaWebView;

namespace UpdateManagerMockup
{
    public interface IPlatformDependendUtils
    {
        event EventHandler<string> OnMessage;

        string SaveFileToDownload(string fileName, byte[] fileData);

        string SaveFileToTemp(string fileName, byte[] fileData);

        string ConfigureWebView(WebView wv);

        void StartBLE();
    }
}
