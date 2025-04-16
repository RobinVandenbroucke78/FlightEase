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
        public MemoryStream CreatePDFDocumentAsync(Booking booking, string logoPath)
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
                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(companyName, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(qrCodeData);
                var qrCodeImage = qrCode.GetGraphic(3);

                iText.Layout.Element.Image qrCodeImageElement = new iText.Layout.Element.Image(ImageDataFactory.Create(BitmapToBytes(qrCodeImage))).SetHorizontalAlignment(HorizontalAlignment.CENTER);


                document.Add(qrCodeImageElement);
                document.Add(new Paragraph("Booking").SetFontSize(20));
                document.Add(new Paragraph($"BookingId: {booking.BookingId}").SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)).SetFontSize(16).SetFontColor(ColorConstants.BLUE));
                document.Add(new Paragraph($"Datum: {booking.BookingDate}"));
                document.Add(new Paragraph(""));

                // Ticketinformatie toevoegen
                document.Add(new Paragraph("Ticketinformatie").SetFontSize(18));
                document.Add(new Paragraph($"TicketId: {booking.TicketId}"));
                document.Add(new Paragraph($"Vertrek: {booking.Ticket.Flight.FromAirport}"));
                document.Add(new Paragraph($"Bestemming: {booking.Ticket.Flight.ToAirport}"));
                document.Add(new Paragraph($"Vertrekdatum: {booking.Ticket.Flight.DepartureTime}"));
                document.Add(new Paragraph($"Aankomstdatum: {booking.Ticket.Flight.ArrivalTime}"));
                document.Add(new Paragraph($"Seat: {booking.Ticket.SeatNumber}"));
                document.Add(new Paragraph($"Meal: {booking.Ticket.Meal}"));
                document.Add(new Paragraph($"Classtype: {booking.Ticket.ClassType}"));
                document.Add(new Paragraph($"Prijs: {booking.Ticket.Price:C}"));


                //// Totaalbedrag toevoegen
                document.Add(new Paragraph($"Totaalbedrag: {booking.Price:C}"));

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
