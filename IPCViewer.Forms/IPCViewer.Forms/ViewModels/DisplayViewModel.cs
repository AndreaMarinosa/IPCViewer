﻿using IPCViewer.Common.Models;
using System;
using System.Threading.Tasks;

namespace IPCViewer.Forms.ViewModels
{
    public class DisplayViewModel : BaseViewModel
    {
        private string imageUrl;
        private DateTime date;

        public DateTime Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }

        public string ImageUrl
        {
            get => this.imageUrl;
            set => this.SetProperty(ref imageUrl, value);
        }

        public Camera Camera { get; set; }

        public DisplayViewModel (Camera camera)
        {
            this.Camera = camera;
            Date = DateTime.Now;
            ImageUrl = camera.ImageFullPath;
            Action<Task> reloadImageTask = null;

            if ( !ImageUrl.Contains("ipcviewerapi") )
            {
                reloadImageTask = t =>
                {
                    Date = DateTime.Now;

                    var test =
                        "https://media.revistagq.com/photos/5ca5f6a77a3aec0df5496c59/master/w_1280,c_limit/bob_esponja_9564.png";
                    ImageUrl = ImageUrl == test ? Camera.ImageUrl : test;

                    if ( ImageUrl == test )
                    {
                        Task.Delay(5).ContinueWith(r => reloadImageTask(r));
                    }
                    else
                    {
                        Task.Delay(350).ContinueWith(r => reloadImageTask(r));
                    }
                };

                Task.Delay(500).ContinueWith(reloadImageTask);
            }
        }

    }
}