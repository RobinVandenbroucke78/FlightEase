﻿using FlightEase.Util.Mail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace FlightEase.Util.Mail
{
    public class EmailSend : IEmailSend
    {
        private readonly EmailSettings _emailSettings;
        public EmailSend(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = new MailMessage();
            mail.To.Add(new MailAddress(email));
            mail.From = new MailAddress(_emailSettings.Sender, _emailSettings.SenderName);
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;
            try
            {
                using (var smtp = new SmtpClient(_emailSettings.MailServer))
                {
                    smtp.Port = _emailSettings.MailPort;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password);
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                // Better error handling
                throw new Exception($"Failed to send email: {ex.Message}", ex);
            }
        }

        public async Task SendEmailAttachmentAsync(string email, string subject, string message, Stream attachmentStream, string attachmentName, bool isBodyHtml = false)
        {
            var mail = new MailMessage();
            mail.To.Add(new MailAddress(email));
            mail.From = new MailAddress(_emailSettings.Sender, _emailSettings.SenderName);
            mail.Subject = subject;
            mail.Body = message;
            mail.Attachments.Add(new Attachment(attachmentStream, attachmentName));
            mail.IsBodyHtml = isBodyHtml;
            try
            {
                using (var smtp = new SmtpClient(_emailSettings.MailServer))
                {
                    smtp.Port = _emailSettings.MailPort;
                    smtp.EnableSsl = true;
                    smtp.Credentials =
                    new NetworkCredential(_emailSettings.Sender,
                    _emailSettings.Password);
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email with attachment: {ex.Message}", ex);
            }
        }

        public async Task SendEmailWithMultipleAttachmentsAsync(string email, string subject, string message, List<Stream> attachmentStreams, List<string> attachmentNames, bool isBodyHtml = false)
        {
            if (attachmentStreams.Count != attachmentNames.Count)
            {
                throw new ArgumentException("The number of attachment streams must match the number of attachment names");
            }

            var mail = new MailMessage();
            mail.To.Add(new MailAddress(email));
            mail.From = new MailAddress(_emailSettings.Sender, _emailSettings.SenderName);
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = isBodyHtml;

            // Add all attachments
            for (int i = 0; i < attachmentStreams.Count; i++)
            {
                mail.Attachments.Add(new Attachment(attachmentStreams[i], attachmentNames[i]));
            }

            try
            {
                using (var smtp = new SmtpClient(_emailSettings.MailServer))
                {
                    smtp.Port = _emailSettings.MailPort;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password);
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email with multiple attachments: {ex.Message}", ex);
            }
        }
    }
}