using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using FFImageLoading.Work;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IPCViewer.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditCameraPage : ContentPage
    {
        public EditCameraPage()
        {
            InitializeComponent();

            var cachedImage = new CachedImage()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                DownsampleToViewSize = true,
                LoadingPlaceholder = "loading.png",
                ErrorPlaceholder = "error.png",
                Source = "http://loremflickr.com/600/600/nature?filename=simple.jpg"
            };
        }
    }
}