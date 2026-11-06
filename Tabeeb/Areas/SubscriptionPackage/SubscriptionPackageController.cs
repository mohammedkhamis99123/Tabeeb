using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabeeb.Application.Areas.Country.Commands.AddCountry;
using Tabeeb.Application.Areas.Country.Commands.DeleteCountry;
using Tabeeb.Application.Areas.Country.Commands.UpdateCountry;
using Tabeeb.Application.Areas.Country.Queries.CountryDropDown;
using Tabeeb.Application.Areas.Country.Queries.GetCountries;
using Tabeeb.Application.Areas.Country.Queries.GetCountry;
using Tabeeb.Application.Areas.SupscriptionPackage.commands.AddSubscriptionPackage;
using Tabeeb.Application.Areas.SupscriptionPackage.commands.DeleteSupscriptionPackage;
using Tabeeb.Application.Areas.SupscriptionPackage.commands.UpdateSubscriptionPackage;
using Tabeeb.Application.Areas.SupscriptionPackage.Queries.DropdownSubscriptionPackages;
using Tabeeb.Application.Areas.SupscriptionPackage.Queries.GetSubscriptionPackage;
using Tabeeb.Application.Areas.SupscriptionPackage.Queries.GetSubscriptionPackages;
using Tabeeb.Attributes;
using Tabeeb.Domain.Enums;
using Tabeeb.Extensions;

namespace Tabeeb.Areas.SubscriptionPackage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscriptionPackageController(ISender sender) : ControllerBase
    {
        [HttpGet]
        [HasPermission(Permission.ViewSubscriptionPackage)]
        public async Task<IActionResult> Paginate([FromQuery] GetSubscriptionPackages Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }


        [HttpGet("{Id}")]
        [HasPermission(Permission.ViewSubscriptionPackage)]
        public async Task<IActionResult> GetById([FromRoute] GetSubscriptionPackage Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }


        [HttpGet("DropDown")]
        [HasPermission(Permission.ViewSubscriptionPackage)]
        public async Task<IActionResult> Dropdown([FromQuery] DropdownSubscriptionPackages Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }


        [HttpPost]
        [HasPermission([Permission.AddSubscriptionPackage])]
        public async Task<IActionResult> Create(AddSubscriptionPackage command)
        {
            return await sender.Send(command).ToGenericReponse();
        }




        [HttpPut]
        [HasPermission([Permission.UpdateSubscriptionPackage])]
        public async Task<IActionResult> Update(UpdateSubscriptionPackage command)
        {
            return await sender.Send(command).ToGenericReponse();
        }


        [HttpDelete("{Id}")]
        [HasPermission([Permission.DeleteSubscriptionPackage])]
        public async Task<IActionResult> Delete([FromRoute] DeleteSupscriptionPackage command)
        {
            return await sender.Send(command).ToGenericReponse();
        }
    }
}
