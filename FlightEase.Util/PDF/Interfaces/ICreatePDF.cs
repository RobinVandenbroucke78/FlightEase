using FlightEase.Domains.Entities;

namespace FlightEase.Util.PDF.Interfaces
{
    public interface ICreatePDF
    {
         MemoryStream CreatePDFDocumentAsync(Booking booking, string logoPath, Flight flight);
    }
}
