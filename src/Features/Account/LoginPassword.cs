using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Blazor5Auth.Server.Extensions;
using Blazor5Auth.Server.Models;
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

        public class CommandHandler : IRequestHandler<Command, Result>
        {
            private readonly IJwtHelper _jwtHelper;
            private readonly SignInManager<ApplicationUser> _signInManager;

            public CommandHandler(IJwtHelper jwtHelper,
                SignInManager<ApplicationUser> signInManager)
            {
                _jwtHelper = jwtHelper;
                _signInManager = signInManager;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

                if (result.RequiresTwoFactor) return new Result { IsSuccessful = false, RequiresTwoFactor = true };
                if (result.IsLockedOut) return new Result { IsSuccessful = false, IsLockedOut = true };

                if (result.IsNotAllowed)
                {
                    var user2 = await _signInManager.UserManager.FindByEmailAsync(request.Email);
                    if (!(await _signInManager.UserManager.IsEmailConfirmedAsync(user2)))
                    {
                        return new Result { IsSuccessful = false, RequiresEmailConfirmation = true };
                    }
                }

                if (!result.Succeeded) return new Result().Failed("Username and password are invalid.");

                var user = await _signInManager.UserManager.FindByEmailAsync(request.Email);
                var roles = await _signInManager.UserManager.GetRolesAsync(user);

                var token = _jwtHelper.GenerateJwt(user, roles);

                return new Result { IsSuccessful = true, Token = token };
            }
        }
    }
}
