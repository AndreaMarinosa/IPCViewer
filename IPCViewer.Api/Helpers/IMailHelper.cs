using System;
using System.Collections.Generic;
using System.Text;

namespace IPCViewer.Api.Helpers
{
    public interface IMailHelper
    {
        void SendMail(string to, string subject, string body);
    }

}
