using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace IPCViewer.Forms.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel ()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://github.com/AndreaMarinosa/IPCViewer")));
        }

        public ICommand OpenWebCommand { get; }
    }
}