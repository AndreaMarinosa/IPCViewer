namespace IPCViewer.Forms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using IPCViewer.Common.Models;
    using IPCViewer.Forms.Views;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Menu = IPCViewer.Common.Models.Menu;

    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private static MainViewModel _instance;
        private User user;

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

        public AddUrlViewModel AddUrl { get; set; }

        public DisplayViewModel DisplayCamera { get; set; }

        public AddLocationViewModel AddLocation { get; set; }

        public ProfileViewModel Profile { get; set; }

        public MapViewModel Maps { get; set; }


        public User User
        {
            get => this.user;
            set => this.SetProperty(ref this.user, value);
        }

        private void GoAddCamera ()
        {
            this.AddCamera = new AddCameraViewModel();
            App.Navigator.PushAsync(new AddCameraPage());
        }

        public MainViewModel ()
        {
            _instance = this;
            this.Login = new LoginViewModel();
            this.Register = new RegisterViewModel();
            LoadMenus();
        }

        private void LoadMenus ()
        {
            var menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_mapPink",
                    PageName = "MapsPage",
                    Title= "Maps"
                },

                 new Menu
                {
                    Icon = "ic_settingsPink",
                    PageName = "ProfilePage",
                    Title = "Modify User"
                },

                new Menu
                {
                    Icon = "ic_aboutPink",
                    PageName = "AboutPage",
                    Title = "About"
                },

                new Menu
                {
                    Icon = "ic_logoutPink",
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

        // Singleton
        public static MainViewModel GetInstance () => _instance ?? new MainViewModel();
    }
}