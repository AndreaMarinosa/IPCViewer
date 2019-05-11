using IPCViewer.Common.Models;
using IPCViewer.Common.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace IPCViewer.Forms.ViewModels
{
    public class DisplayViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService apiService;

        public Camera Camera { get; set; }

        public DisplayViewModel (Camera camera)
        {
            this.Camera = camera;
            this.apiService = new ApiService();
        }

        // todo: hilo que vaya recargando la imagen automaticamente
    }
}
