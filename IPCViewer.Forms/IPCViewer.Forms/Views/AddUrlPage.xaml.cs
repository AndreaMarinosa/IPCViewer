﻿using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace IPCViewer.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddUrlPage 
    {
        public AddUrlPage ()
        {
            InitializeComponent();
        }

        // API: STAR WAR

        // frameworks: PRIMS AND MVVMCROSS

        //Invoked when background is clicked
        protected override bool OnBackgroundClicked ()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return false;
        }
    }
}