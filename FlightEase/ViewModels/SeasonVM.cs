namespace FlightEase.ViewModels
{
    public class SeasonVM
    {
        public int SeasonId { get; set; }

        public string? Name { get; set; }

        public double Price { get; set; }

        public DateOnly? BeginDate { get; set; }

        public DateOnly? EndDate { get; set; }
    }
}
