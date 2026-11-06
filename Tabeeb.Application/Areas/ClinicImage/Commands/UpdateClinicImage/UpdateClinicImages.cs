using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Domain.Enums;

namespace Tabeeb.Application.Areas.ClinicImage.Commands.UpdateClinicImage
{
    public class UpdateClinicImages:IRequest
    {
        public int ClinicId {  get; set; }

        public bool? DeleteBannerImage { get; set; }

        public bool? DeleteLogoImage { get; set; }

        public bool? DeleteInvoiceLogoImage { get; set; }
        public IFormFile? LogoImage { get; set; }

        public IFormFile? BannerImage { get; set; }

        public IFormFile? InvoiceImage { get; set; }


    }
    public class UpdateClinicImagesHandler 
        (IAppDbContext appDbContext,
        IFileService fileService,
        ICurrentUserService currentUser)
        : IRequestHandler<UpdateClinicImages>
    {
        public async Task Handle(UpdateClinicImages request, CancellationToken cancellationToken)
        {

            var clinicID = request.ClinicId;

            var clinicImages =  appDbContext.ClinicImages.Where(a => a.ClinicId == clinicID);

            if (request.DeleteBannerImage.HasValue && request.DeleteBannerImage.Value)
            {
               var banner= clinicImages.Where(A => A.ImageType == ImageType.Banner).FirstOrDefault();
                appDbContext.ClinicImages.Remove(banner);
                await appDbContext.SaveChangesAsync();
            }

            if (request.DeleteLogoImage.HasValue && request.DeleteLogoImage.Value)
            {
                var logo = clinicImages.Where(A => A.ImageType == ImageType.Logo).FirstOrDefault();
                appDbContext.ClinicImages.Remove(logo);
                await appDbContext.SaveChangesAsync();
            }

            if (request.DeleteInvoiceLogoImage.HasValue && request.DeleteInvoiceLogoImage.Value)
            {
                var InvoiceLogoImage = clinicImages.Where(A => A.ImageType == ImageType.invoiceLogo).FirstOrDefault();
                appDbContext.ClinicImages.Remove(InvoiceLogoImage);
                await appDbContext.SaveChangesAsync();
            }

            if (request.BannerImage!=null)
            {

                await ReplaceImage(clinicID,request.BannerImage,ImageType.Banner);
            }


            if (request.LogoImage is not null)
            {
                await ReplaceImage(clinicID, request.LogoImage, ImageType.Logo);
            }


            if (request.InvoiceImage!=null)
            {
                await ReplaceImage(clinicID, request.InvoiceImage, ImageType.invoiceLogo);
            }

           

        }

        private async Task ReplaceImage(int clinicId,IFormFile File,ImageType type)
        {
            var Image = appDbContext.ClinicImages
                                   .Where(a => a.ClinicId == clinicId && a.ImageType == type)
                                   .FirstOrDefault();

            if(Image is not null)
            {
                fileService.DeleteFile(Image.ImagePath);


                appDbContext.ClinicImages.Remove(Image);
            }
         

            var FolderName=type==ImageType.Banner ?"Banners" :type==ImageType.Logo ?"Logos" :"InvoiceLogos";
            var ImagePath = fileService.UploadFile(File, FolderName);

            var NewBanner = new Domain.Entities.ClinicImage
            {
                ClinicId = clinicId,
                ImagePath = ImagePath,
                ImageType = type

            };


            appDbContext.ClinicImages.Add(NewBanner);
            await appDbContext.SaveChangesAsync();


        }


    }

}
