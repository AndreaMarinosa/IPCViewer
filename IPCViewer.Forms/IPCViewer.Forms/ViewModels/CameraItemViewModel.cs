using System;

namespace IPCViewer.Forms.ViewModels
{
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;
    using Helpers;

    /**
     * Clase para meter un comando en el modelo 'Camera'
     * Se utiliza esta nueva clase para no meter codigo interno
     * al modelo de camara, de esta manera se conserva el modelo limpio
     */

    public class CameraItemViewModel : Camera
    {
        public string NameSort => string.IsNullOrWhiteSpace(City.Name) || City.Name.Length == 0 ? "?" : City.Name.ToUpper();

        public ImageSource Icon { get => this.ImageUrl.StartsWith("http") ? "ic_available" : "ic_error"; }

        public ICommand SelectCameraCommand => new RelayCommand(SelectCamera);

        public ICommand SelectCameraImageCommand => new RelayCommand(SelectCameraImage);

        private async void SelectCameraImage ()
        {
            MainViewModel.GetInstance().DisplayCamera = new DisplayViewModel(this);
            await App.Navigator.PushAsync(new DisplayCameraPage(), true);
        }

        async void SelectCamera ()
        {

            var source = await Application.Current.MainPage.DisplayActionSheet(
                this.Name, Languages.Cancel, null,
                Languages.EditCamera, Languages.ViewCamera, Languages.ViewMaps);

            if ( source.Equals(Languages.ViewCamera))
            {
                MainViewModel.GetInstance().DisplayCamera = new DisplayViewModel(this);
                await App.Navigator.PushAsync(new DisplayCameraPage(), true);
                return;
            }
            else if ( source.Equals(Languages.EditCamera))
            {
                MainViewModel.GetInstance().EditCamera = new EditCameraViewModel(this);
                await App.Navigator.PushAsync(new EditCameraPage());
                return;
            }
            else if ( source.Equals(Languages.ViewMaps))
            {
                MainViewModel.GetInstance().Maps = new MapViewModel(this);
                await App.Navigator.PushAsync(new MapsPage());
                return;
            }
            else
            {
                return;
            }
        }
    }
}