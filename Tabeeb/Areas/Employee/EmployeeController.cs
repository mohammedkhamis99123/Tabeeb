using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tabeeb.Application.Areas.City.Commands.CreateCity;
using Tabeeb.Application.Areas.City.Commands.DeleteCity;
using Tabeeb.Application.Areas.City.Commands.UpdateCity;
using Tabeeb.Application.Areas.City.Queries.GetCities;
using Tabeeb.Application.Areas.Employee.Commands.AddEmployee;
using Tabeeb.Application.Areas.Employee.Commands.UpdateEmployee;
using Tabeeb.Application.Areas.Employee.Queries.EmployeeDropDown;
using Tabeeb.Application.Areas.Employee.Queries.GetEmployee;
using Tabeeb.Application.Areas.Employee.Queries.GetEmployees;
using Tabeeb.Attributes;
using Tabeeb.Extensions;

namespace Tabeeb.Areas.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(ISender sender) : ControllerBase
    {


        [HttpGet]
        [HasPermission(Domain.Enums.Permission.ViewEmployee)]

        public async Task<IActionResult> Paginate([FromQuery] GetEmployees Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }

        [HttpGet("DropDown")]
        [HasPermission(Domain.Enums.Permission.ViewEmployee)]

        public async Task<IActionResult> DropDown([FromQuery] EmployeeDropDown Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }


        [HttpGet("{Id}")]
        [HasPermission(Domain.Enums.Permission.ViewEmployee)]

        public async Task<IActionResult> GetByID([FromRoute]GetEmployee command)
        {
            return await sender.Send(command).ToGenericReponse();
        }




        [HttpPost]
        [HasPermission(Domain.Enums.Permission.AddEmployee)]

        public async Task<IActionResult> Add(AddEmployeeCommand command)
        {
            return await sender.Send(command).ToGenericReponse();
        }


        [HttpPut]
        [HasPermission(Domain.Enums.Permission.UpdateEmployee)]

        public async Task<IActionResult> Update(UpdateEmployeeCommand command)
        {
            return await sender.Send(command).ToGenericReponse();
        }

     
    }
}
