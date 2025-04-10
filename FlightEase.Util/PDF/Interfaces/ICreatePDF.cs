using FlightEase.Domains.Entities;

namespace FlightEase.Util.PDF.Interfaces
{
    public interface ICreatePDF
    {
         MemoryStream CreatePDFDocumentAsync(List<Booking> bookings, string logoPath);
    }
}
