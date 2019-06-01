using IPCViewer.Forms.Helpers;
using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace IPCViewer.Forms.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel ()
        {
            Title = Languages.About;

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://github.com/AndreaMarinosa")));
        }

        public ICommand OpenWebCommand { get; }
    }
}