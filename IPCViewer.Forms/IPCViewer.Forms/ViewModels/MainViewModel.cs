using GalaSoft.MvvmLight.Command;
using IPCViewer.Common.Models;
using IPCViewer.Forms.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Menu = IPCViewer.Common.Models.Menu;

namespace IPCViewer.Forms.ViewModels
{
    class MainViewModel
    {
        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private static MainViewModel _instance;

        public RegisterViewModel Register { get; set; }

        public string UserEmail { get; set; }

        public string UserPassword { get; set; }

        public TokenResponse Token { get; set; }

        public LoginViewModel Login { get; set; }

        public ControlUsersPage ControlUsersPage { get; set; }

        public CamerasViewModel Cameras { get; set; }

        public AddCameraViewModel AddCamera { get; set; }

        public ICommand AddCameraCommand => new RelayCommand(this.GoAddCamera);

        public EditCameraViewModel EditCamera { get; set; }

        public UsersViewModel Users { get; set; }

        public EditUserViewModel EditUser { get; set; }

        private void GoAddCamera()
        {
            this.AddCamera = new AddCameraViewModel();
            App.Navigator.PushAsync(new AddCameraPage());
        }

        public MainViewModel()
        {
            _instance = this;
            this.Login = new LoginViewModel();
            this.Register = new RegisterViewModel();
            LoadMenus();
        }

        private void LoadMenus()
        {
            var menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "about",
                    PageName = "AboutPage",
                    Title = "About"
                },

                new Menu
                {
                    Icon = "setup",
                    PageName = "SetupPage",
                    Title = "Setup"
                },

                new Menu
                {
                    Icon = null,
                    PageName = "MapsPage",
                    Title= "Map"
                },

                new Menu
                {
                    Icon = "exit",
                    PageName = "LoginPage",
                    Title = "Close session"
                }
            };

            this.Menus = new ObservableCollection<MenuItemViewModel>(menus.Select(m => new MenuItemViewModel
            {
                Icon = m.Icon,
                PageName = m.PageName,
                Title = m.Title
            }).ToList());
        }

    public static MainViewModel GetInstance()
    {
        if (_instance == null)
        {
            return new MainViewModel();
        }

        return _instance;
    }

}
}
