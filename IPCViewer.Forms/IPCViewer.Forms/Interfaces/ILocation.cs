using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IPCViewer.Forms.Interfaces
{
    public interface ILocation
    {

        void SetLocation(string longitude, string latitude, ImageSource imageSource);
    }
}
