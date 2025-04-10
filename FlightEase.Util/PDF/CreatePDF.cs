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

namespace FlightEase.Util.PDF
{
    public class CreatePDF : ICreatePDF
    {
        public MemoryStream CreatePDFDocumentAsync(List<Booking> bookings, string logoPath)
        {

            // Genereren van de PDF-factuur

            using (MemoryStream stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                iText.Layout.Document document = new iText.Layout.Document(pdf);

                // Factuurinformatie toevoegen
                //iText.Layout.Element.Image logo = new iText.Layout.Element.Image(ImageDataFactory.Create(logoPath)).ScaleToFit(50, 50);
                //logo.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                //document.Add(logo);
                string companyName = "Hogeschool VIVES";
                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(companyName, QRCodeGenerator.ECCLevel.Q);
                //var qrCode = new QRCode(qrCodeData);
                //var qrCodeImage = qrCode.GetGraphic(3);

                //iText.Layout.Element.Image qrCodeImageElement = new iText.Layout.Element.Image(ImageDataFactory.Create(BitmapToBytes(qrCodeImage))).SetHorizontalAlignment(HorizontalAlignment.CENTER);


                //document.Add(qrCodeImageElement);
                //document.Add(new Paragraph("Factuur").SetFontSize(20));
                //document.Add(new Paragraph("Factuurnummer: 001").SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)).SetFontSize(16).SetFontColor(ColorConstants.BLUE));
                //document.Add(new Paragraph("Datum: " + DateTime.Now.ToShortDateString()));
                //document.Add(new Paragraph(""));


                //// Tabel voor producten
                //Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
                //table.AddHeaderCell("Product");
                //table.AddHeaderCell("Prijs per stuk");
                //table.AddHeaderCell("Totaal");
                //decimal totalPrice = 0;
                //foreach (var product in products)
                //{
                //    table.AddCell(product.Name);
                //    table.AddCell(product.Price.ToString("C"));
                //    decimal totalProductPrice = product.Price * product.Number;
                //    table.AddCell(totalProductPrice.ToString("C"));
                //    totalPrice += totalProductPrice;
                //}
                //document.Add(table);

                //// Totaalbedrag toevoegen
                //document.Add(new Paragraph($"Totaalbedrag: {totalPrice.ToString("C")}"));

                //document.Close();
                return new MemoryStream(stream.ToArray());


            }
        }


        // This method is for converting bitmap into a byte array
        //private static byte[] BitmapToBytes(Bitmap img)
        //{
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //        return stream.ToArray();
        //    }
        //}
    }

}
