
using IPCViewer.Common.Models;

namespace IPCViewer.Forms.ViewModels
{
    public class EditCameraViewModel
    {
        public Camera Camera { get; set; }

        /**
         * El producto ligado a la MainViewModel es el que se pasa por parametro
         */
        public EditCameraViewModel(Camera camera)
        {
            this.Camera = camera;
        }
    }
}
