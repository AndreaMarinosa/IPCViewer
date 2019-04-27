namespace IPCViewer.Forms.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class CamerasViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private ObservableCollection<Camera> cameras;
        private bool isRefreshing;

        public ObservableCollection<Camera> Cameras
        {
            get => cameras;
            set => this.SetProperty(ref cameras, value);
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetProperty(ref this.isRefreshing, value);
        }

        public ICommand RefreshCommand => new RelayCommand(this.LoadCamerasAsync);

        public CamerasViewModel()
        {
            this.apiService = new ApiService();
            LoadCamerasAsync();
        }

        private async void LoadCamerasAsync()
        {
            var response = await this.apiService.GetListAsync<Camera>(
                "https://ipcviewerapi2.azurewebsites.net",
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

            var myCameras= (List<Camera>)response.Result;
            this.Cameras = new ObservableCollection<Camera>(myCameras);

        }
    }
}