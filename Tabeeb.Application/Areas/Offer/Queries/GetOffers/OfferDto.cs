namespace Tabeeb.Application.Areas.Offer.Queries.GetOffers
{
    public class OfferDto
    {
        public int Id { get; set; }
        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public decimal Percentage { get; set; }


        public bool IsActive { get; set; }

        public int DoctorId { get; set; }


        public int ClinicId { get; set; }
    }
}