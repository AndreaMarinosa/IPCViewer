using IPCViewer.Common.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IPCViewer.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CamerasPage : ContentPage
    {
        public CamerasPage ()
        {
            InitializeComponent();

            if ( Settings.IsDarkMode )
            {
                Resources["ContentPageStyle"] = Resources["DarkBcMode"];
                Resources["EntryStyle"] = Resources["DarkEntryMode"];
                Resources["PickerStyle"] = Resources["DarkPickerMode"];
                Resources["LabelStyle"] = Resources["DarkLabelMode"];
                Resources["LabelSecondStyle"] = Resources["DarkLabelSecondMode"];
                Resources["SearchStyle"] = Resources["DarkSearchrMode"];
            }
            else
            {
                Resources["LabelSecondStyle"] = Resources["LightMode"];
            }
        }
    }
}