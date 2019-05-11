using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPCViewer.Api.Helpers
{
    using System.IO;


    public class FilesHelper
    {
        // Le mandamos desde la aplicacion una coleccion de bits en memoria que almacena la imagen (stream)
        // La carpeta donde se va a guardar (folder)
        // el nombre de la imagen (name)
        public static bool UploadPhoto (MemoryStream stream, string folder, string name)
        {
            try
            {
                stream.Position = 0;
                var path = Path.Combine(Directory.GetCurrentDirectory(), folder, name);
                File.WriteAllBytes(path, stream.ToArray());
            }
            catch
            {
                return false;
            }

            return true;
        }
    }

}
