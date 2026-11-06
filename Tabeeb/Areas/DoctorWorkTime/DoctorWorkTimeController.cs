using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tabeeb.Application.Areas.Customer.Commands.AddCustomerCommand;
using Tabeeb.Application.Areas.Customer.Commands.UpdateCustomerCommand;
using Tabeeb.Application.Areas.Customer.Queries.GetCustomer;
using Tabeeb.Application.Areas.Customer.Queries.GetCustomers;
using Tabeeb.Application.Areas.DoctorWorkTime.Commands.UpdateDoctorWorkTime;
using Tabeeb.Application.Areas.DoctorWorkTime.Queries.GetDoctorWorkTime;
using Tabeeb.Attributes;
using Tabeeb.Extensions;

namespace Tabeeb.Areas.DoctorWorkTime
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorWorkTimeController (ISender sender): ControllerBase
    {
       



        [HttpPut]
        [HasPermission(Domain.Enums.Permission.UpdateDoctorWorkTime)]

        public async Task<IActionResult> Update( UpdateDoctorWorkTime command)
        {
            return await sender.Send(command).ToGenericReponse();
        }




        [HttpGet]
        [HasPermission(Domain.Enums.Permission.ViewDoctorWorkTime)]

        public async Task<IActionResult> Get([FromQuery]GetDoctorWorkTime command)
        {
            return await sender.Send(command).ToGenericReponse();
        }


     
    }
}
