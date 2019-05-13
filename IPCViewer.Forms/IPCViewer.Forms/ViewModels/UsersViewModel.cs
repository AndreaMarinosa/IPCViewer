using System;

namespace IPCViewer.Forms.ViewModels
{
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class UsersViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private List<User> myUsers; // La lista original del API

        private ObservableCollection<UserItemViewModel> users;
        private bool isRefreshing;

        public ObservableCollection<UserItemViewModel> Users
        {
            get => users;
            set => this.SetProperty(ref users, value);
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetProperty(ref this.isRefreshing, value);
        }

        public ICommand RefreshCommand => new RelayCommand(this.LoadUsersAsync);

        public UsersViewModel ()
        {
            this.apiService = new ApiService();
            LoadUsersAsync();
        }

        private async void LoadUsersAsync ()
        {
            //var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetUserListAsync<User>(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Account");

            if ( !response.IsSuccess )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            this.myUsers = (List<User>) response.Result;
            RefreshUsersList();
            IsRefreshing = false;
        }

        public void AddUser (UserItemViewModel user)
        {
            this.myUsers.Add(user);
            RefreshUsersList();
        }

        //public void UpdateUser(UserItemViewModel user)
        //{
        //    var oldUser = myUsers.FirstOrDefault(c => c.Id == user.Id);

        //    // Eliminamos la antigua camara
        //    if (oldUser != null)
        //    {
        //        myUsers.Remove(oldUser);
        //    }

        //    // Aniadimos la nueva
        //    myUsers.Add(user);
        //    RefreshUsersList();
        //}

        public void DeleteUser (Guid id)
        {
            var oldUser = myUsers.FirstOrDefault(c => c.Id == id);
            if ( oldUser != null )
            {
                myUsers.Remove(oldUser);
            }

            RefreshUsersList();
        }

        private void RefreshUsersList ()
        {
            // ObservableCollection de la Clase UserItemViewModel -> (User + Comando)
            Users = new ObservableCollection<UserItemViewModel>(
                myUsers.Select(c => new UserItemViewModel // Por cada user se creara una nueva instancia de UserItemViewModel
                {
                    Id = c.Id,
                    Email = c.Email,
                    FirstName = c.FirstName,
                    UserName = c.UserName,
                    CityId = c.CityId,
                    City = c.City
                }).ToList());
        }
    }
}