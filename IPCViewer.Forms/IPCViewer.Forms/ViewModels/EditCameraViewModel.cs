namespace IPCViewer.Forms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Common.Models;
    using Common.Services;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class EditCameraViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService apiService;

        public Camera Camera { get; set; }

        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetProperty(ref this.isRunning, value);
        }

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand DeleteCommand => new RelayCommand(this.Delete);


        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetProperty(ref this.isEnabled, value);
        }

        /**
         * La camara ligada a la MainViewModel es la que se pasa por parametro
         */
        public EditCameraViewModel(Camera camera)
        {
            this.Camera = camera;
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Camera.Name))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a camera name.", "Accept");
                return;
            }

            // Todo: other alerts

            this.IsRunning = true;
            this.IsEnabled = false;

            var response = await this.apiService.PutAsync(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cameras",
                this.Camera.Id,
                this.Camera,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRunning = false;
            this.IsEnabled = true;
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            var modifiedCamera = (Camera)response.Result;
            MainViewModel.GetInstance().Cameras.UpdateCamera(modifiedCamera);
            await App.Navigator.PopAsync();

        }

        private async void Delete()
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure to delete the camera?", "Yes", "No");
            if (!confirm)
            {
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var response = await this.apiService.DeleteAsync(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cameras",
                this.Camera.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            MainViewModel.GetInstance().Cameras.DeleteCamera(this.Camera.Id);
            await App.Navigator.PopAsync();
        }

    }
}