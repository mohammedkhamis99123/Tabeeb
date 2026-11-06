namespace Tabeeb.Application.Areas.SupscriptionPackage.Queries.GetSubscriptionPackages
{
    public class SubscriptionPackageDto
    {
        public byte Id { get; set; }
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;

        public string ArDescription { get; set; } = null!;

        public string EnDescription { get; set; } = null!;
        public int DurationInDays { get; set; }

        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}