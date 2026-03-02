using MediatR;
using Microsoft.AspNetCore.Mvc;
using SIWES360.Application.Features.Authentication.Commands;
using SIWES360.Application.Features.Authentication.Commands.LoginUser;

namespace SIWES360.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}