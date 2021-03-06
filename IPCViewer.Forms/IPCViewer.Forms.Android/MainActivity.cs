﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Xamarin.Forms.GoogleMaps.Android;

namespace IPCViewer.Forms.Droid
{
    [Activity(Label = "IPCViewer.Forms", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            
            var platformConfig = new PlatformConfig
            {
                BitmapDescriptorFactory = new AccessNativeBitmapConfig()
            };

            // Initialize plugin
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

            //Initialize maps
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState, platformConfig);
            Xamarin.FormsGoogleMapsBindings.Init();

            LoadApplication(new App());
            //LoadApplication(UXDivers.Gorilla.Droid.Player.CreateApplication(
            //    this,
            //    new UXDivers.Gorilla.Config("Good Gorilla")
            //      // Google Maps
            //      // FFImageLoading.Transformations
            //      .RegisterAssemblyFromType<FFImageLoading.Transformations.BlurredTransformation>()
            //      // FFImageLoading.Forms
            //      .RegisterAssemblyFromType<FFImageLoading.Forms.CachedImage>()
            //    ));
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