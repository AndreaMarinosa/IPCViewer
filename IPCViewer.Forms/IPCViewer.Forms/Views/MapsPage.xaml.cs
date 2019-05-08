using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace IPCViewer.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapsPage : ContentPage
    {
        public MapsPage()
        {
            InitializeComponent();
            //AddMarkers();
        }

        private void AddMarkers()
        {
            Position loc1 = new Position(17.430486, 78.341331);
            Position loc2 = new Position(17.427579, 78.342017);

            Pin marker1 = new Pin()
            {
                Address = "Gachibowli",
                IsVisible = true,
                Label = "Microsoft Hyderabad",
                //Icon = BitmapDescriptorFactory.FromBundle("ipCameraPin"),
                Position = loc1,
                Type = PinType.Place

            };
            Pin marker2 = new Pin()
            {
                Address = "Gachibowli",
                IsVisible = true,
                Label = "Wipro Hyderabad",
                //Icon = BitmapDescriptorFactory.FromBundle("ipCameraPin"),
                Position = loc2,
                Type = PinType.Place

            };

            //mapView.Pins.Add(marker1);
            //mapView.Pins.Add(marker2);
            //mapView.MoveToRegion(MapSpan.FromCenterAndRadius(marker1.Position, Distance.FromMeters(1000)));
        }
    }
}