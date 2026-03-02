using FluentValidation;

namespace SIWES360.Application.Features.Authentication.Commands
{
    public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Firstname).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Lastname).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Role).IsInEnum();
        }
    }
}