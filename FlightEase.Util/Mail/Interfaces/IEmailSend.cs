using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightEase.Util.Mail.Interfaces
{
    public interface IEmailSend
    {
        Task SendEmailAsync(string email, string subject, string message);

        Task SendEmailAttachmentAsync(string email, string subject, string message, Stream attachmentStream, string attachmentName, bool isBodyHtml = false);
    }
}
