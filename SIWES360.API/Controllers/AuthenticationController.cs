using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIWES360.Application.Features.Authentication.Commands;
using SIWES360.Application.Features.Authentication.Commands.LoginUser;
using SIWES360.Application.Features.Authentication.Commands.RefreshToken;
using SIWES360.Application.Features.Authentication.Commands.RevokeToken;

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
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [Authorize]
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(CancellationToken ct)
        {
            var result = await _mediator.Send(new RevokeTokenCommand(), ct);
            return result.IsSuccess ? Ok("Logged out successfully") : BadRequest(result.Error);
        }
    }
}