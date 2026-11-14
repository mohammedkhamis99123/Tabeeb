using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tabeeb.Application.Areas.City.Queries.GetCities;
using Tabeeb.Application.Areas.ClinicDoctor.Command.AddClinicDoctor;
using Tabeeb.Application.Areas.ClinicDoctor.Command.UpdateClinicDoctor;
using Tabeeb.Application.Areas.ClinicDoctor.Queries.GetClinicDoctor;
using Tabeeb.Application.Areas.ClinicDoctor.Queries.GetClinicDoctors;
using Tabeeb.Attributes;
using Tabeeb.Extensions;

namespace Tabeeb.Areas.ClinicDoctor
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ClinicDoctorController(ISender Sender) : ControllerBase
    {




        [HttpGet("{ClinicId}")]
        [HasPermission(Domain.Enums.Permission.ViewClinicDoctors)]
        public async Task<IActionResult> GetClinicDoctor([FromRoute]GetClinicDoctors Query)
        {
            return await Sender.Send(Query).ToGenericReponse();
        }

        [HttpGet("GetClinicDoctor/{DoctorId}/{ClinicId}")]
        [HasPermission(Domain.Enums.Permission.ViewClinicDoctors)]
        public async Task<IActionResult> GetClinicDoctor([FromRoute] GetClinicDoctor Query)
        {
            return await Sender.Send(Query).ToGenericReponse();
        }

        [HttpPost]
        [HasPermission(Domain.Enums.Permission.AddClinicDoctors)]

        public async Task<IActionResult> AddDoctorClinic([FromForm] AddClinicDoctorCommand Query)
        {
            return await Sender.Send(Query).ToGenericReponse();
        }



        [HttpPut]
        [HasPermission(Domain.Enums.Permission.UpdateClinicDoctors)]
        public async Task<IActionResult> UpdateDoctorClinic([FromForm] UpdateDoctorClinic Query)
        {
            return await Sender.Send(Query).ToGenericReponse();
        }
    }
}
