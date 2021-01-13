using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Features.Base;
using FluentValidation;
using MediatR;
using Reinforced.Typings.Attributes;

namespace Features.Account
{
    public class LoginPassword
    {
        [TsInterface(Name = "LoginCommand")]
        public class Command : IRequest<Result>
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Email).NotEmpty().EmailAddress();
                RuleFor(p => p.Password).NotEmpty().MinimumLength(8);
            }
        }

        [TsInterface(Name = "LoginResult")]
        public class Result : BaseResult
        {
            public bool RequiresTwoFactor { get; set; }
            public bool IsLockedOut { get; set; }
            public bool RequiresEmailConfirmation { get; set; }
            public string Token { get; set; }
        }

        //this is here for easy navigation with goto implementation
        public interface ICommandHandler : IRequestHandler<Command, Result> { }

    }
}
