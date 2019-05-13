using GalaSoft.MvvmLight.Command;
using IPCViewer.Common.Models;
using IPCViewer.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IPCViewer.Forms.ViewModels
{
    public class DisplayViewModel : BaseViewModel
    {
        private string imageUrl;

        public string ImageUrl
        {
            get => this.imageUrl;
            set => this.SetProperty(ref imageUrl, value);
        }

        public Camera Camera { get; set; }

        public DisplayViewModel (Camera camera)
        {
            this.Camera = camera;
            ImageUrl = camera.ImageUrl;
            Action<Task> reloadImageTask = null;

            reloadImageTask = t =>
            {
                var test =
                    "https://media.revistagq.com/photos/5ca5f6a77a3aec0df5496c59/master/w_1280,c_limit/bob_esponja_9564.png";
                ImageUrl = ImageUrl == test ? Camera.ImageUrl : test;

                if (ImageUrl == test)
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
