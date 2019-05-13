using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using IPCViewer.Forms.Interfaces;

namespace IPCViewer.Forms.ViewModels
{
    public class AddUrlViewModel : BaseViewModel
    {
        public string UrlCamera { get; set; }

        private readonly IClosePopup closePopup;

        public ICommand AddUrlCommand => new RelayCommand(this.AddUrl);

        public AddUrlViewModel(IClosePopup closePopup)
        {
            this.closePopup = closePopup;
        }

        private async void AddUrl()
        {
            closePopup.OnClose(UrlCamera);

            // Close the last PopupPage int the PopupStack
            await App.Navigator.PopAsync();
        }
    }
}
