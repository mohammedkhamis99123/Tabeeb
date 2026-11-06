using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tabeeb.Application.Areas.City.Commands.CreateCity;
using Tabeeb.Application.Areas.City.Commands.DeleteCity;
using Tabeeb.Application.Areas.City.Commands.UpdateCity;
using Tabeeb.Application.Areas.City.Queries.GetCities;
using Tabeeb.Application.Areas.Clinic.Commands.AddClinic;
using Tabeeb.Application.Areas.Clinic.Commands.DeleteClinic;
using Tabeeb.Application.Areas.Clinic.Commands.UpdateClinic;
using Tabeeb.Application.Areas.Clinic.Queries.ClinicsDropDown;
using Tabeeb.Application.Areas.Clinic.Queries.GetClinic;
using Tabeeb.Application.Areas.Clinic.Queries.GetClinics;
using Tabeeb.Attributes;
using Tabeeb.Domain.Enums;
using Tabeeb.Extensions;

namespace Tabeeb.Areas.Clinic
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicController(ISender sender) : ControllerBase
    {


        [HttpGet]
        [HasPermission(Permission.ViewClinic)]
        public async Task<IActionResult> Paginate([FromQuery] GetClinics Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }

        [HttpGet("Dropdown")]
        [HasPermission(Permission.ViewClinic)]
        public async Task<IActionResult> DropDown([FromQuery] ClinicsDropDown Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }

        [HttpGet("{id}")]
        [HasPermission(Permission.ViewClinic)]
        public async Task<IActionResult> GetById([FromRoute] GetClinic Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }



        [HttpPost]
        [HasPermission([Permission.AddClinic])]
        public async Task<IActionResult> Create(AddClinicCommand command)
        {
            return await sender.Send(command).ToGenericReponse();
        }




        [HttpPut]
        [HasPermission([Permission.UpdateClinic])]
        public async Task<IActionResult> Update(UpdateClinic command)
        {
            return await sender.Send(command).ToGenericReponse();
        }


        [HttpDelete("{id}")]
        [HasPermission([Permission.DeleteClinic])]
        public async Task<IActionResult> Delete([FromRoute] DeleteClinic command)
        {
            return await sender.Send(command).ToGenericReponse();
        }
    }
}
