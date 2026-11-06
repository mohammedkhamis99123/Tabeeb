using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tabeeb.Application.Areas.City.Commands.CreateCity;
using Tabeeb.Application.Areas.City.Commands.DeleteCity;
using Tabeeb.Application.Areas.City.Commands.UpdateCity;
using Tabeeb.Application.Areas.City.Queries.GetCities;
using Tabeeb.Application.Areas.ClinicImage.Commands;
using Tabeeb.Application.Areas.ClinicImage.Commands.UpdateClinicImage;
using Tabeeb.Application.Areas.ClinicImage.Queries.GetClinicImages;
using Tabeeb.Attributes;
using Tabeeb.Extensions;

namespace Tabeeb.Areas.ClinicImage
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicImageController (ISender sender): ControllerBase
    {

        [HttpGet]
        [HasPermission(Domain.Enums.Permission.ViewClinicImages)]
        public async Task<IActionResult> GetImages([FromQuery] GetClinicImages Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }





        [HttpPut]
        [HasPermission(Domain.Enums.Permission.UpdateClinicImages)]

        public async Task<IActionResult> Update(UpdateClinicImages command)
        {
            return await sender.Send(command).ToGenericReponse();
        }


     
    }
}
