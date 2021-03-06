﻿namespace IPCViewer.Forms.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool isBusy = false;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private string title = string.Empty;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected void SetProperty<T> (ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if ( EqualityComparer<T>.Default.Equals(backingStore, value) )
                return;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
        }

        //protected void SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        //{
        //    if (EqualityComparer<T>.Default.Equals(backingField, value))
        //    {
        //        return;
        //    }

        //    backingField = value;
        //    OnPropertyChanged(propertyName);
        //}

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged ([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged
    }
}