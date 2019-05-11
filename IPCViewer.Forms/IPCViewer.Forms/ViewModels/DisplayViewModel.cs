using GalaSoft.MvvmLight.Command;
using IPCViewer.Common.Models;
using IPCViewer.Common.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IPCViewer.Forms.ViewModels
{
    public class DisplayViewModel
    {
        private readonly ApiService apiService;

        public Camera Camera { get; set; }

        public DisplayViewModel (Camera camera)
        {
            this.Camera = camera;
            this.apiService = new ApiService();
            
        }

        public ICommand LoadCommand => new RelayCommand(LoadCameraAsync);

        // todo: hilo que vaya recargando la imagen automaticamente
        private async void LoadCameraAsync ()
        {
            var response = await this.apiService.GetCameraAsync<Camera>(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cameras",
                this.Camera.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if ( !response.IsSuccess )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            Camera = (Camera) response.Result;

        }
    }
}
