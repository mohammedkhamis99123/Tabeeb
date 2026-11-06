using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Application.Areas.Clinic.Queries
{
    public  class ClinicDto
    {
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;

        public string CountryArName {  get; set; } = null!;

        public string CountryEnName { get; set; } = null!;
        public DateOnly SubscriptionStartAt { get; set; }

        public DateOnly SubscriptionEndAt {  get; set; }
        public string StreetArName { get; set; } = null!;

        public string StreetEnName { get; set; } = null!;

        public string Langitude { get; set; } = null!;

        public string SnapshatUser { get; set; }

        public string InstgramUser { get; set; } = null!;

        public string WhatsappNumber { get; set; }
        public string Latitude { get; set; } = null!;

        public int CityId { get; set; }


        public string CityArName {  get; set; } = null!;

        public string CityEnName {  get; set; } = null!;

        //public int CountryId { get; set; }
        //public Country Country { get; set; } = new();

        public string? ArDescription { get; set; }

        public string? EnDescription { get; set; }


        public string? ProfileAr { get; set; }


        public string? ProfileEn { get; set; }

        public bool IsActive {  get; set; }

        public int DoctorsCount {  get; set; }

        public int Id { get; set; }

        public byte? SubscriptionPackageId {  get; set; }

        public string? SubscriptionPackageArName { get; set; }

        public string? SubscriptionPackageEnName { get; set; }

        public string OwnerArName { get; set; } = null!;

        public string OwnerEnName { get; set; } = null!;

        public string OwnerEmail { get; set; }
        public string LogoPath {  get; set; }

        public string bannerPath {  get; set; }

        public string invoiceLogoPath {  get; set; }
    }
}
