using System.IO;
using Xamarin.Forms;

namespace IPCViewer.Forms.Interfaces
{
    public interface ILocation
    {
        void SetLocation(string longitude, string latitude, byte[] imageSource);
    }
}