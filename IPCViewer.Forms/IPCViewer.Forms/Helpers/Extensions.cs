﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IPCViewer.Forms.Helpers
{
    public static class Extensions
    {
        public static byte[] ToByteArray (this Stream stream)
        {
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            for ( int totalBytesCopied = 0; totalBytesCopied < stream.Length; )
                totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
            return buffer;
        }
    }
}
