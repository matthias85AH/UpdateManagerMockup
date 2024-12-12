using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace UpdateManagerMockup.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public void OutputText(string text)
    {
        Debug.WriteLine(text);
    }
}
