
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Plugin.Permissions;

namespace IPCViewer.Forms.Droid
{
    [Activity(Label = "IPCViewer.Forms", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate (Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            //Para que cuando el proyecto arranque, inicialice las librerías de hacer fotos
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Syncfusion.XForms.Android.PopupLayout.SfPopupLayoutRenderer.Init();

            //var platformConfig = new PlatformConfig
            //{
            //    BitmapDescriptorFactory = new AccessNativeBitmapConfig()
            //};

            // Initialize plugin
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

            //Initialize maps
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState);
            Xamarin.FormsGoogleMapsBindings.Init();
            LoadApplication(new App());
        }

        public override void OnBackPressed ()
        {
            if ( Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed) )
            {

            }
            else
            {

            }
        }

        public override void OnRequestPermissionsResult (
            int requestCode,
            string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(
                requestCode,
                permissions,
                grantResults);
        }

    }
}