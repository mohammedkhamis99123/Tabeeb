using Mapster;
using Tabeeb.Application.Areas.Clinic.Queries;
using Tabeeb.Domain.Entities;
using Tabeeb.Domain.Enums;

namespace Tabeeb.Application.Mapping;

public static class ClinicMapping
{
    public static void Run()
    {
        TypeAdapterConfig<Clinic, ClinicDto>
            .NewConfig()
            .Map(dest => dest.CityArName,
                 src => src.City.ArName)
            .Map(dest => dest.CityEnName,
                 src => src.City.EnName)
            .Map(dest => dest.CountryArName,
                 src => src.City.Country.ArName)
            .Map(dest => dest.CountryEnName,
                 src => src.City.Country.EnName)
            .Map(dest => dest.DoctorsCount,
                 src => src.Doctors.Count())
            .Map(dest => dest.LogoPath,
                 src => src.Images
                     .Where(x => x.ImageType == ImageType.Logo)
                     .Select(x => x.ImagePath)
                     .FirstOrDefault())
            .Map(dest => dest.bannerPath,
                 src => src.Images
                     .Where(x => x.ImageType == ImageType.Banner)
                     .Select(x => x.ImagePath)
                     .FirstOrDefault())
            .Map(dest => dest.invoiceLogoPath,
                 src => src.Images
                     .Where(x => x.ImageType == ImageType.invoiceLogo)
                     .Select(x => x.ImagePath)
                     .FirstOrDefault())
            
            .Map(dest=>dest.SubscriptionPackageId,src=>src.Subscriptions.OrderByDescending(a => a.StartAt).Select(a=>a.PackageID).FirstOrDefault())
            .Map(dest => dest.SubscriptionPackageArName, src => src.Subscriptions.OrderByDescending(a => a.StartAt).Select(a => a.Package.ArName).FirstOrDefault())
            .Map(dest => dest.SubscriptionPackageEnName, src => src.Subscriptions.OrderByDescending(a => a.StartAt).Select(a => a.Package.EnName).FirstOrDefault())
            .Map(dest => dest.SubscriptionStartAt, src => src.Subscriptions.OrderByDescending(a => a.StartAt).Select(a => a.StartAt).FirstOrDefault())
             .Map(dest => dest.SubscriptionEndAt, src => src.Subscriptions.OrderByDescending(a => a.StartAt).Select(a => a.EndAt).FirstOrDefault())

            ;
    }
}