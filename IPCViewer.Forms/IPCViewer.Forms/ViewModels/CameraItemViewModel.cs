﻿using System;

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

        void SelectCamera ()
        {
            Extensions.SelectCamera(this, 0);
        }
    }
}