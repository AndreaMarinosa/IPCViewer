using System.Globalization;

namespace IPCViewer.Forms.Interfaces
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo ();

        void SetLocale (CultureInfo ci);
    }
}