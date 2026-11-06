using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tabeeb.Application.Areas.City.Commands.CreateCity;
using Tabeeb.Application.Areas.City.Commands.DeleteCity;
using Tabeeb.Application.Areas.City.Commands.UpdateCity;
using Tabeeb.Application.Areas.City.Queries.GetCities;
using Tabeeb.Application.Areas.Country.Commands.AddCountry;
using Tabeeb.Application.Areas.Country.Commands.DeleteCountry;
using Tabeeb.Application.Areas.Country.Commands.UpdateCountry;
using Tabeeb.Application.Areas.Country.Queries.CountryDropDown;
using Tabeeb.Application.Areas.Country.Queries.GetCountries;
using Tabeeb.Application.Areas.Country.Queries.GetCountry;
using Tabeeb.Attributes;
using Tabeeb.Domain.Enums;
using Tabeeb.Extensions;

namespace Tabeeb.Areas.Country
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountryController(ISender sender) : ControllerBase
    {

        [HttpGet]
        [HasPermission(Permission.ViewCountry)]
        public async Task<IActionResult> Paginate([FromQuery] GetCountries Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }


        [HttpGet("{Id}")]
        [HasPermission(Permission.ViewCountry)]
        public async Task<IActionResult> GetById([FromRoute] GetCountry Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }


        [HttpGet("DropDown")]
        [HasPermission(Permission.ViewCountry)]
        public async Task<IActionResult> Dropdown([FromQuery] CountryDropDown Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }


        [HttpPost]
        [HasPermission([Permission.AddCountry])]
        public async Task<IActionResult> Create(AddCountryCommand command)
        {
            return await sender.Send(command).ToGenericReponse();
        }




        [HttpPut]
        [HasPermission([Permission.UpdateCountry])]
        public async Task<IActionResult> Update(UpdateCountryCommand command)
        {
            return await sender.Send(command).ToGenericReponse();
        }


        [HttpDelete("{Id}")]
        [HasPermission([Permission.DeleteCountry])]
        public async Task<IActionResult> Delete([FromRoute]DeleteCountry command)
        {
            return await sender.Send(command).ToGenericReponse();
        }
    }
}
