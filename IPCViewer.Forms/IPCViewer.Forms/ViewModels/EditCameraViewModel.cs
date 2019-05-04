namespace IPCViewer.Forms.ViewModels
{
    using IPCViewer.Common.Models;

    public class EditCameraViewModel
    {
        public Camera Camera { get; set; }

        /**
         * La camara ligada a la MainViewModel es la que se pasa por parametro
         */
        public EditCameraViewModel(Camera camera)
        {
            this.Camera = camera;
        }
    }
}
