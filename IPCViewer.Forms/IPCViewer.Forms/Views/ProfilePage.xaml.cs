using IPCViewer.Common.Helpers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IPCViewer.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage ()
        {
            InitializeComponent();

            if ( Settings.IsDarkMode)
            {
                Resources["ContentPageStyle"] = Resources["DarkBcMode"];
                Resources["EntryStyle"] = Resources["DarkEntryMode"];
                Resources["PickerStyle"] = Resources["DarkPickerMode"];
                Resources["LabelStyle"] = Resources["DarkLabelMode"];
            }
        }

    }
}