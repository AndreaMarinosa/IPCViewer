using System.Collections.Generic;

namespace IPCViewer.Forms.ViewModels
{
    public class CameraGroup
    {
        public string CityName { get; set; }

        public List<CameraItemViewModel> Cameras { get; set; }
    }
}
