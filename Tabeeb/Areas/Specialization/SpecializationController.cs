using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tabeeb.Application.Areas.City.Commands.CreateCity;
using Tabeeb.Application.Areas.City.Commands.DeleteCity;
using Tabeeb.Application.Areas.City.Commands.UpdateCity;
using Tabeeb.Application.Areas.City.Queries.GetCities;
using Tabeeb.Application.Areas.Specialization.Commands.AddSpecialization;
using Tabeeb.Application.Areas.Specialization.Commands.DeleteSpecialization;
using Tabeeb.Application.Areas.Specialization.Commands.UpdateSpecialization;
using Tabeeb.Application.Areas.Specialization.Queries.DropdownSpecialization;
using Tabeeb.Application.Areas.Specialization.Queries.GetSpecialization;
using Tabeeb.Application.Areas.Specialization.Queries.GetSpecializations;
using Tabeeb.Attributes;
using Tabeeb.Extensions;

namespace Tabeeb.Areas.Specialization
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController(ISender sender) : ControllerBase
    {


        [HttpGet]
        [HasPermission(Domain.Enums.Permission.ViewSpecialization)]

        public async Task<IActionResult> Paginate([FromQuery] GetSpecializations Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }


        [HttpGet("DropDown")]
        [HasPermission(Domain.Enums.Permission.ViewSpecialization)]

        public async Task<IActionResult> DropDown([FromQuery] DropdownSpecialization Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }

        [HttpGet("{Id}")]
        [HasPermission(Domain.Enums.Permission.ViewSpecialization)]

        public async Task<IActionResult> GetById([FromRoute] GetSpecialization Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }



        [HttpPost]
        [HasPermission(Domain.Enums.Permission.AddSpecialization)]

        public async Task<IActionResult> Create(AddSpecialization command)
        {
            return await sender.Send(command).ToGenericReponse();
        }




        [HttpPut]
        [HasPermission(Domain.Enums.Permission.UpdateSpecialization)]

        public async Task<IActionResult> Update(UpdateSpecialization command)
        {
            return await sender.Send(command).ToGenericReponse();
        }


        [HttpDelete("{Id}")]
        [HasPermission(Domain.Enums.Permission.DeleteSpecialization)]

        public async Task<IActionResult> Delete([FromRoute] DeleteSpecialization command)
        {
            return await sender.Send(command).ToGenericReponse();
        }
    }
}

