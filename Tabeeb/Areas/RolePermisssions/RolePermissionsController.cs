using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tabeeb.Application.Areas.DoctorWorkTime.Commands.UpdateDoctorWorkTime;
using Tabeeb.Application.Areas.DoctorWorkTime.Queries.GetDoctorWorkTime;
using Tabeeb.Application.Areas.RolePermissions.Commands.UpdateRolePermissions;
using Tabeeb.Application.Areas.RolePermissions.Queries.GetRolePermissions;
using Tabeeb.Attributes;
using Tabeeb.Extensions;

namespace Tabeeb.Areas.RolePermisssions
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionsController(ISender sender) : ControllerBase
    {




        [HttpPut]
        [HasPermission(Domain.Enums.Permission.UpdateRolePermissions)]

        public async Task<IActionResult> Update(UpdateRolePermissions command)
        {
            return await sender.Send(command).ToGenericReponse();
        }




        [HttpGet]
        [HasPermission(Domain.Enums.Permission.ViewRolePermissions)]

        public async Task<IActionResult> Get([FromQuery]GetRolePermissions command)
        {
            return await sender.Send(command).ToGenericReponse();
        }



    }
}