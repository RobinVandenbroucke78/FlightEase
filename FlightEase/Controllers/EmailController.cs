using FlightEase.Domains.Entities;
using FlightEase.Util.Mail.Interfaces;
using FlightEase.Util.PDF.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlightEase.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailSend _emailSend;
        private readonly ICreatePDF _createPDF;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EmailController(IEmailSend emailSend, ICreatePDF createPDF, IWebHostEnvironment hostEnvironment)
        {
            _emailSend = emailSend;
            _createPDF = createPDF;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(Booking booking, string email)
        {
            try
            {
                // Assuming you have a booking object to pass to the PDF creation
                string logoPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images", "logo.png");
                var pdfStream = _createPDF.CreatePDFDocumentAsync(booking, logoPath);
                string subject = "Your FlightEase Booking Confirmation";
                string message = $@"
                            <h2>Thank you for your booking with FlightEase!</h2>
                            <p>Your booking has been confirmed.</p>
                            <p>Details:</p>
                            <ul>
                                $<li>Booking #{booking.BookingId}: <br> BookingName: {booking.BookingName} <br> Price: ${booking.Price}</li>
                            </ul>
                            <p>Total: ${booking.Price}</p>
                            <p>Thank you for choosing FlightEase for your travel needs!</p>";
                // Send email with attachment
                // maak deze dan aan voordat je het PDF-document opslaat.
                string pdfFolderPath = Path.Combine(_hostEnvironment.WebRootPath, "pdf");
                Directory.CreateDirectory(pdfFolderPath);
                //Combineer het pad naar de wwwroot map met het gewenste subpad en bestandsnaam voor het PDF-document.
                string filePath = Path.Combine(pdfFolderPath, "Booking.pdf");
                await _emailSend.SendEmailAttachmentAsync(email, subject, message, pdfStream, "Booking.pdf");
                ViewBag.Message = "Email sent successfully!";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error sending email: {ex.Message}";
            }
            return View();
        }
    }
}
