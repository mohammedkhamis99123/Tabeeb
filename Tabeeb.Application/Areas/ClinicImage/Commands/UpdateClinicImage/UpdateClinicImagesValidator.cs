//using FluentValidation;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Localization;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Tabeeb.Application.Common.Services;
//using Tabeeb.Resources;

//namespace Tabeeb.Application.Areas.ClinicImage.Commands.UpdateClinicImage
//{
//    public  class UpdateClinicImagesValidator:AbstractValidator<UpdateClinicImages>
//    {
//        private readonly IAppDbContext appDbContext;
//        private readonly IStringLocalizer<SharedResource> localizer;

//        public UpdateClinicImagesValidator(IAppDbContext appDbContext,IStringLocalizer<SharedResource> localizer)
//        {
//            this.appDbContext = appDbContext;
//            this.localizer = localizer;

    

//            RuleFor(x => x)
//                .MustAsync((command, CancellationToken) => _ValidateImages(command))
//                .WithMessage(localizer["InvalidImages"]);


//        }

//        private async Task<bool> _ValidateImages(UpdateClinicImages command)
//        {
           
//            if(command.DeleteBannerImage.HasValue && command.DeleteBannerImage.Value  && command.BannerImage is null)
//            {
//                return false;
//            }
//            if (command.DeleteLogoImage.HasValue && command.DeleteLogoImage.Value && command.LogoImage is null)
//            {
//                return false;
//            }
//            if (command.DeleteInvoiceLogoImage.HasValue && command.DeleteInvoiceLogoImage.Value && command.InvoiceImage is null)
//            {
//                return false;
//            }

//            return true;

//        }
//    }
//}
