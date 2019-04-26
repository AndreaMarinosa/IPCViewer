using System;
using System.Collections.Generic;
using System.Text;

namespace IPCViewer.Forms.ViewModels
{
    class MainViewModel
    {
        public LoginViewModel Login { get; set; }

        public MainViewModel()
        {
            this.Login = new LoginViewModel();
        }

    }
}
