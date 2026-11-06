using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tabeeb.Application.Areas.Offer.Commands.AddOffer;
using Tabeeb.Application.Areas.Offer.Commands.DeleteOffer;
using Tabeeb.Application.Areas.Offer.Commands.UpdateOffer;
using Tabeeb.Application.Areas.Offer.Queries.GetOffers;
using Tabeeb.Application.Areas.Specialization.Commands.AddSpecialization;
using Tabeeb.Application.Areas.Specialization.Commands.DeleteSpecialization;
using Tabeeb.Application.Areas.Specialization.Commands.UpdateSpecialization;
using Tabeeb.Application.Areas.Specialization.Queries.DropdownSpecialization;
using Tabeeb.Application.Areas.Specialization.Queries.GetSpecializations;
using Tabeeb.Attributes;
using Tabeeb.Extensions;

namespace Tabeeb.Areas.Offer
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController(ISender sender) : ControllerBase
    {


        [HttpGet]
        [HasPermission(Domain.Enums.Permission.ViewOffer)]

        public async Task<IActionResult> Paginate([FromQuery] GetOffers Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }


       



        [HttpPost]
        [HasPermission(Domain.Enums.Permission.AddOffer)]

        public async Task<IActionResult> Create(AddOffer command)
        {
            return await sender.Send(command).ToGenericReponse();
        }




        [HttpPut]
        [HasPermission(Domain.Enums.Permission.UpdateOffer)]

        public async Task<IActionResult> Update(UpdateOffer command)
        {
            return await sender.Send(command).ToGenericReponse();
        }


        [HttpDelete("{Id}")]
        [HasPermission(Domain.Enums.Permission.DeleteOffer)]

        public async Task<IActionResult> Delete([FromRoute] DeleteOffer command)
        {
            return await sender.Send(command).ToGenericReponse();
        }
    }
}
