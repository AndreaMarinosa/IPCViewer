using GalaSoft.MvvmLight.Command;
using IPCViewer.Common.Models;
using IPCViewer.Forms.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Menu = IPCViewer.Common.Models.Menu;

namespace IPCViewer.Forms.ViewModels
{
    internal class MainViewModel
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

        public ICommand AddCameraCommand => new RelayCommand(GoAddCamera);

        public EditCameraViewModel EditCamera { get; set; }

        public UsersViewModel Users { get; set; }

        public EditUserViewModel EditUser { get; set; }


        private void GoAddCamera ()
        {
            AddCamera = new AddCameraViewModel();
            App.Navigator.PushAsync(new AddCameraPage());
        }

        public MainViewModel ()
        {
            _instance = this;
            Login = new LoginViewModel();
            Register = new RegisterViewModel();
            LoadMenus();
        }

        private void LoadMenus ()
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
                    Icon = null,
                    PageName = "UsersPage",
                    Title= "Users"
                },

                new Menu
                {
                    Icon = "exit",
                    PageName = "LoginPage",
                    Title = "Close session"
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(menus.Select(m => new MenuItemViewModel
            {
                Icon = m.Icon,
                PageName = m.PageName,
                Title = m.Title
            }).ToList());
        }

        // Singleton
        public static MainViewModel GetInstance () => _instance ?? new MainViewModel();

    }
}
