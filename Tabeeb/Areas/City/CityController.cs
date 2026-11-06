using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tabeeb.Application.Areas.City.Commands.CreateCity;
using Tabeeb.Application.Areas.City.Commands.DeleteCity;
using Tabeeb.Application.Areas.City.Commands.UpdateCity;
using Tabeeb.Application.Areas.City.Queries.CityDropDpwn;
using Tabeeb.Application.Areas.City.Queries.GetCities;
using Tabeeb.Application.Areas.City.Queries.GetCity;
using Tabeeb.Attributes;
using Tabeeb.Domain.Enums;
using Tabeeb.Extensions;

namespace Tabeeb.Areas.City
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CityController(ISender sender) : ControllerBase
    {


        [HttpGet]
        [HasPermission(Permission.ViewCity)]
        public async Task<IActionResult> Paginate([FromQuery]GetCities Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }

        [HttpGet("DropDown")]
        [HasPermission(Permission.ViewCity)]
        public async Task<IActionResult> DropDown()
        {
            return await sender.Send(new CityDropDown()).ToGenericReponse();
        }

        [HttpPost]
        [HasPermission([Permission.AddCity])]
        public async Task<IActionResult> Create(CreateCityCommand command)
        {
            return await sender.Send(command).ToGenericReponse();
        }




        [HttpPut]
        [HasPermission([Permission.UpdateCity])]
        public async Task<IActionResult> Update(UpdateCityCommand command)
        {
            return await sender.Send(command).ToGenericReponse();
        }


        [HttpDelete("{Id}")]
        [HasPermission([Permission.DeleteCity])]
        public async Task<IActionResult> Delete([FromRoute]DeleteCityCommand command)
        {
            return await sender.Send(command).ToGenericReponse();
        }

        [HttpGet("{Id}")]
        [HasPermission([Permission.ViewCity])]
        public async Task<IActionResult> GetById([FromRoute] GetCity command)
        {
            return await sender.Send(command).ToGenericReponse();
        }
    }
}
