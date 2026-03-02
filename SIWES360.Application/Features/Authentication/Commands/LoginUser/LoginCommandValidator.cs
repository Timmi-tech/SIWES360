using FluentValidation;
using SIWES360.Application.Features.Authentication.Commands.LoginUser;

namespace SIWES360.Application.Features.Authentication.Commands
{
    public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.identifier).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}