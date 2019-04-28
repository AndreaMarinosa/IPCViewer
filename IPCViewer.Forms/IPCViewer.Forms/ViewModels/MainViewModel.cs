using IPCViewer.Common.Models;
using IPCViewer.Forms.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace IPCViewer.Forms.ViewModels
{
    class MainViewModel
    {
        private static MainViewModel _instance;

        public RegisterViewModel Register { get; set; }

        public string UserEmail { get; set; }

        public string UserPassword { get; set; }

        public TokenResponse Token { get; set; }

        public LoginViewModel Login { get; set; }

        public ControlUsersPage ControlUsersPage { get; set; }

        public CamerasViewModel Cameras{ get; set; }

        public MainViewModel()
        {
            _instance = this;
            this.Login = new LoginViewModel();
            this.Register = new RegisterViewModel();
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
