using IPCViewer.Common.Models;
using IPCViewer.Forms.ViewModels;
using IPCViewer.Forms.Views;
using System;
using System.IO;
using Xamarin.Forms;

namespace IPCViewer.Forms.Helpers
{
    public static class Extensions
    {
        public static byte[] ToByteArray (this Stream stream)
        {
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            for ( int totalBytesCopied = 0; totalBytesCopied < stream.Length; )
                totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
            return buffer;
        }

        public async static void SelectCamera (Camera Camera, int type)
        {
            var source = "";

            if (type == 0 )
            {
                source = await Application.Current.MainPage.DisplayActionSheet(
                    Camera.Name, "Cancel", null,
                    "Edit camera", "View Camera", "View Maps");
            } 
            else if (type == 1 )
            {
                source = await Application.Current.MainPage.DisplayActionSheet(
                Camera.Name, "Cancel", null,
                "Edit camera", "View Camera");
            }

            switch ( source )
            {
                case "Cancel":
                    {
                        return;
                    }
                case "Edit camera":
                    {
                        MainViewModel.GetInstance().EditCamera = new EditCameraViewModel(Camera);
                        await App.Navigator.PushAsync(new EditCameraPage());
                        break;
                    }
                case "View Camera":
                    {
                        MainViewModel.GetInstance().DisplayCamera = new DisplayViewModel(Camera);
                        await App.Navigator.PushAsync(new DisplayCameraPage(), true);
                        break;
                    }
                case "View Maps":
                    {
                        MainViewModel.GetInstance().Maps = new MapViewModel(Camera);
                        await App.Navigator.PushAsync(new MapsPage());
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
        }
    }
}
