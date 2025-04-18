using FlightEase.Domains.Entities;
using FlightEase.Util.PDF.Interfaces;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using QRCoder;
using System.Drawing;

namespace FlightEase.Util.PDF
{
    public class CreatePDF : ICreatePDF
    {
        public MemoryStream CreatePDFDocumentAsync(Booking booking, string logoPath, Flight flight)
        {
            // Genereren van de PDF-factuur
            using (MemoryStream stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                iText.Layout.Document document = new iText.Layout.Document(pdf);

                // Factuurinformatie toevoegen
                iText.Layout.Element.Image logo = new iText.Layout.Element.Image(ImageDataFactory.Create(logoPath)).ScaleToFit(50, 50);
                logo.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                document.Add(logo);
                string companyName = "FlightEase";

                document.Add(new Paragraph(companyName).SetFontSize(24));
                document.Add(new Paragraph($"Booking: {booking.BookingName}").SetFontSize(18));
                document.Add(new Paragraph($"BookingId: {booking.BookingId}"));
                document.Add(new Paragraph($"Datum: {booking.BookingDate}"));
                document.Add(new Paragraph(""));

                // Ticketinformatie toevoegen
                document.Add(new Paragraph("Ticketinformatie").SetFontSize(18));
                document.Add(new Paragraph($"TicketId: {booking.TicketId}"));
                document.Add(new Paragraph($"Seat: {booking.Ticket.SeatNumber}"));
                document.Add(new Paragraph($"Meal: {booking.Ticket.Meal.MealName}"));
                document.Add(new Paragraph($"Classtype: {booking.Ticket.ClassType.ClassName}"));

                //Flight informatie toevoegen
                document.Add(new Paragraph("Flightinformatie").SetFontSize(18));
                document.Add(new Paragraph($"Vertrek: {flight.FromAirport?.City?.CityName ?? "N/A"}"));
                document.Add(new Paragraph($"Bestemming: {flight.ToAirport?.City?.CityName ?? "N/A"}"));

                // Safely handle transfers - check if transfer exists
                string transferInfo = "Geen tussenstoppen";
                if (flight.Transfer != null)
                {
                    string firstStop = flight.Transfer.FirstAirport?.City?.CityName ?? "Onbekend";
                    string secondStop = flight.Transfer.SecondAirport?.City?.CityName ?? "Onbekend";

                    if (firstStop != "Onbekend" || secondStop != "Onbekend")
                    {
                        transferInfo = $"{firstStop} - {secondStop}";
                    }
                }
                document.Add(new Paragraph($"Tussenstoppen: {transferInfo}"));

                document.Add(new Paragraph($"Vertrekdatum: {flight.DepartureTime}"));
                document.Add(new Paragraph($"Aankomstdatum: {flight.ArrivalTime}"));

                // Totaalbedrag toevoegen
                document.Add(new Paragraph($"Totaalbedrag: {booking.Price:C}"));

                //QR-code
                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(companyName, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(qrCodeData);
                var qrCodeImage = qrCode.GetGraphic(3);

                iText.Layout.Element.Image qrCodeImageElement = new iText.Layout.Element.Image(ImageDataFactory.Create(BitmapToBytes(qrCodeImage))).SetHorizontalAlignment(HorizontalAlignment.CENTER);

                document.Add(qrCodeImageElement);

                document.Close();
                return new MemoryStream(stream.ToArray());
            }
        }

        // This method is for converting bitmap into a byte array
        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}