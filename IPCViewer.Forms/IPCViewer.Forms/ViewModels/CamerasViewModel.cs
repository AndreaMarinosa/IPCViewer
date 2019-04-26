namespace IPCViewer.Forms.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Common.Models;
    using Common.Services;
    using Xamarin.Forms;

    public class CamerasViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<Camera> cameras;

        public ObservableCollection<Camera> Cameras
        {
            get => cameras;
            set => this.SetProperty(ref cameras, value);
        }

        public CamerasViewModel()
        {
            Title = "";
            this.apiService = new ApiService();
            LoadCamerasAsync();
        }

        private async void LoadCamerasAsync()
        {
            var response = await this.apiService.GetListAsync<Camera>(
                "https://ipcviewer.azurewebsites.net",
                "/api",
                "/Cameras");


            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            var cameras= (List<Camera>)response.Result;
            this.Cameras = new ObservableCollection<Camera>(cameras);

        }
    }
}