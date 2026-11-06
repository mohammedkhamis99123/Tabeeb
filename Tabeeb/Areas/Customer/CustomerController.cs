using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tabeeb.Application.Areas.Customer.Commands.AddCustomerCommand;
using Tabeeb.Application.Areas.Customer.Commands.UpdateCustomerCommand;
using Tabeeb.Application.Areas.Customer.Queries.GetCustomer;
using Tabeeb.Application.Areas.Customer.Queries.GetCustomerByPhone;
using Tabeeb.Application.Areas.Customer.Queries.GetCustomers;
using Tabeeb.Application.Areas.Employee.Commands.AddEmployee;
using Tabeeb.Application.Areas.Employee.Commands.UpdateEmployee;
using Tabeeb.Application.Areas.Employee.Queries.EmployeeDropDown;
using Tabeeb.Application.Areas.Employee.Queries.GetEmployee;
using Tabeeb.Application.Areas.Employee.Queries.GetEmployees;
using Tabeeb.Attributes;
using Tabeeb.Extensions;

namespace Tabeeb.Areas.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController (ISender sender): ControllerBase
    {

        [HttpGet]
        [HasPermission(Domain.Enums.Permission.ViewCustomer)]

        public async Task<IActionResult> Paginate([FromQuery] GetCustomers Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }


        [HttpGet("GetCustomerByPhone")]
        [HasPermission(Domain.Enums.Permission.ViewCustomer)]

        public async Task<IActionResult> GetCustomerByPhone([FromQuery] GetCustomerByPhone Query)
        {
            return await sender.Send(Query).ToGenericReponse();
        }



        [HttpGet("{Id}")]
        [HasPermission(Domain.Enums.Permission.ViewCustomer)]

        public async Task<IActionResult> GetByID([FromRoute] GetCustomer command)
        {
            return await sender.Send(command).ToGenericReponse();
        }




        [HttpPost]
        [HasPermission(Domain.Enums.Permission.AddCustomer)]

        public async Task<IActionResult> Add(AddCustomerCommand command)
        {
            return await sender.Send(command).ToGenericReponse();
        }


        [HttpPut]
        [HasPermission(Domain.Enums.Permission.UpdateCustomer)]

        public async Task<IActionResult> Update(UpdateCustomerCommand command)
        {
            return await sender.Send(command).ToGenericReponse();
        }
    }
}
