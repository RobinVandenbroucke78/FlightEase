using FlightEase.Domains.Entities;
using FlightEase.Services.Interfaces;
using FlightEase.Util.Mail.Interfaces;
using FlightEase.Util.PDF.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FlightEase.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailSend _emailSend;
        private readonly ICreatePDF _createPDF;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IService<Ticket> _ticketService;
        private readonly IService<Flight> _flightService;

        public EmailController(
            IEmailSend emailSend,
            ICreatePDF createPDF,
            IWebHostEnvironment hostEnvironment,
            IService<Ticket> ticketService,
            IService<Flight> flightService)
        {
            _emailSend = emailSend;
            _createPDF = createPDF;
            _hostEnvironment = hostEnvironment;
            _ticketService = ticketService;
            _flightService = flightService;
        }

        public async Task SendEmail(Booking booking, string userEmail)
        {
            try
            {
                // Get ticket details
                var ticket = await _ticketService.FindByIdAsync(booking.TicketId);
                if (ticket == null)
                {
                    throw new Exception($"Ticket not found with ID: {booking.TicketId}");
                }

                // Get flight details
                var flight = await _flightService.FindByIdAsync(ticket.FlightId);
                if (flight == null)
                {
                    throw new Exception($"Flight not found with ID: {ticket.FlightId}");
                }

                // Create PDF
                string logoPath = Path.Combine(_hostEnvironment.WebRootPath, "images", "logo.png");
                var pdfStream = _createPDF.CreatePDFDocumentAsync(booking, logoPath, flight);

                // Email subject and message
                string subject = $"FlightEase - Booking Confirmation #{booking.BookingId}";
                string message = $@"
                    <h2>Thank you for your booking with FlightEase!</h2>
                    <p>Your booking has been confirmed. Please find your ticket attached.</p>
                    <p>Booking ID: {booking.BookingId}</p>
                    <p>Booking Date: {booking.BookingDate}</p>
                    <p>Total Price: {booking.Price:C}</p>
                    <p>Thank you for choosing FlightEase!</p>";

                // Send email with attachment
                await _emailSend.SendEmailAttachmentAsync(
                    userEmail,
                    subject,
                    message,
                    pdfStream,
                    $"FlightEase-Ticket-{booking.BookingId}.pdf",
                    true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email for booking {booking.BookingId}: {ex.Message}");
                throw;
            }
        }

        public async Task SendMultipleBookingsEmail(List<Booking> bookings, string userEmail)
        {
            try
            {
                if (bookings == null || !bookings.Any())
                {
                    throw new Exception("No bookings provided to send email");
                }

                StringBuilder messageBuilder = new StringBuilder();
                messageBuilder.Append("<h2>Thank you for your bookings with FlightEase!</h2>");
                messageBuilder.Append("<p>Your bookings have been confirmed. Please find your tickets attached.</p>");
                messageBuilder.Append("<h3>Booking Summary:</h3>");
                messageBuilder.Append("<table border='1' cellpadding='5' style='border-collapse: collapse;'>");
                messageBuilder.Append("<tr><th>Booking ID</th><th>Flight</th><th>From</th><th>To</th><th>Class</th><th>Seat</th><th>Meal</th><th>Price</th></tr>");

                decimal totalPrice = 0m; // Ensure totalPrice is of type decimal
                List<MemoryStream> pdfStreams = new List<MemoryStream>();
                List<string> attachmentNames = new List<string>();

                // Process each booking
                foreach (var booking in bookings)
                {
                    // Get ticket details
                    var ticket = await _ticketService.FindByIdAsync(booking.TicketId);
                    if (ticket == null)
                    {
                        continue;
                    }

                    // Get flight details
                    var flight = await _flightService.FindByIdAsync(ticket.FlightId);
                    if (flight == null)
                    {
                        continue;
                    }

                    // Add booking details to the email message
                    messageBuilder.Append("<tr>");
                    messageBuilder.Append($"<td>{booking.BookingId}</td>");
                    messageBuilder.Append($"<td>{flight.FlightId}</td>");
                    messageBuilder.Append($"<td>{flight.FromAirport.City.CityName}</td>");
                    messageBuilder.Append($"<td>{flight.ToAirport.City.CityName}</td>");
                    messageBuilder.Append($"<td>{ticket.ClassType.ClassName}</td>");
                    messageBuilder.Append($"<td>{ticket.SeatNumber}</td>");
                    messageBuilder.Append($"<td>{ticket.Meal.MealName}</td>");
                    messageBuilder.Append($"<td>{booking.Price:C}</td>");
                    messageBuilder.Append("</tr>");

                    totalPrice += (decimal)booking.Price;

                    // Create PDF for this booking
                    string logoPath = Path.Combine(_hostEnvironment.WebRootPath, "images", "logo.png");
                    var pdfStream = _createPDF.CreatePDFDocumentAsync(booking, logoPath, flight);
                    pdfStreams.Add(pdfStream);
                    attachmentNames.Add($"FlightEase-Ticket-{booking.BookingId}.pdf");
                }

                messageBuilder.Append("</table>");
                messageBuilder.Append($"<p><strong>Total Price: {totalPrice:C}</strong></p>");
                messageBuilder.Append("<p>Thank you for choosing FlightEase!</p>");

                // Email subject
                string subject = $"FlightEase - Your Booking Confirmation";

                // Send a single email with multiple PDFs
                await SendEmailWithMultipleAttachments(
                    userEmail,
                    subject,
                    messageBuilder.ToString(),
                    pdfStreams,
                    attachmentNames,
                    true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email for multiple bookings: {ex.Message}");
                throw;
            }
        }

        private async Task SendEmailWithMultipleAttachments(
            string email,
            string subject,
            string message,
            List<MemoryStream> attachmentStreams,
            List<string> attachmentNames,
            bool isBodyHtml = true)
        {
            try
            {
                if (attachmentStreams.Count != attachmentNames.Count)
                {
                    throw new ArgumentException("The number of attachment streams must match the number of attachment names");
                }

                // Send email with the first attachment
                if (attachmentStreams.Count > 0)
                {
                    await _emailSend.SendEmailAttachmentAsync(
                        email,
                        subject,
                        message,
                        attachmentStreams[0],
                        attachmentNames[0],
                        isBodyHtml);

                    // If there are more attachments, send them as separate emails with minimal content
                    for (int i = 1; i < attachmentStreams.Count; i++)
                    {
                        string additionalMessage = $"<p>Additional ticket #{i + 1} attached.</p>";
                        await _emailSend.SendEmailAttachmentAsync(
                            email,
                            $"{subject} - Additional Ticket #{i + 1}",
                            additionalMessage,
                            attachmentStreams[i],
                            attachmentNames[i],
                            isBodyHtml);
                    }
                }
                else
                {
                    // If no attachments, just send the email
                    await _emailSend.SendEmailAsync(email, subject, message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email with multiple attachments: {ex.Message}");
                throw;
            }
        }
    }
}