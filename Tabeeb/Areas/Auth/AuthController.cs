using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Tabeeb.Application.Areas.Auth.Commands.LoginCommand;
using Tabeeb.Application.Areas.Auth.Commands.RefreshTokenCommand;
using Tabeeb.Application.Areas.Auth.Commands.SelectClinicCommand;
using Tabeeb.Extensions;

namespace Tabeeb.Areas.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController(ISender Sender) : ControllerBase
    {
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login command)
        {
            return await Sender.Send(command).ToGenericReponse();
        }

        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        {
            return await Sender.Send(command).ToGenericReponse();
        }


       [HttpPost("SelectClinic")]
        public async Task<IActionResult> SelectClinic([FromBody]SelectClinicCommand command)
        {
            return await Sender.Send(command).ToGenericReponse();
        }
    }
}
