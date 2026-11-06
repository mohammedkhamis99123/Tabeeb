using Tabeeb.Domain.Enums;

namespace Tabeeb.Application.Areas.Subscription.Queries.GetSubscriptions
{
    public class SubscriptionDto
    {
        public int ClinicId { get; set; }

        public long Id {  get; set; }

        public byte PackageID { get; set; }

        public string ClinicArName { get; set; } = null!;

        public string ClinicEnName { get; set; } = null!;

        public string PackageArName { get; set; } = null!;

        public string PackageEnName { get; set; } = null!;


        public SubscriptionStatus Status { get; set; }

        public DateOnly? StartAt { get; set; }


        public DateOnly? EndAt { get; set; }
    }
}