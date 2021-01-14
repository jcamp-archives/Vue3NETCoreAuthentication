using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Blazor5Auth.Server.Extensions;
using Blazor5Auth.Server.Models;
using Features.Base;
using FluentValidation;
using MediatR;
using Reinforced.Typings.Attributes;

namespace Features.Account
{
    public class ResetPassword
    {
        [TsInterface(Name = "ResetPasswordCommand")]
        public class Command : IRequest<Result>
        {
            public string Code { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Code).NotEmpty();
                RuleFor(p => p.Email).NotEmpty().EmailAddress();
                RuleFor(p => p.Password).NotEmpty().MinimumLength(8);
                RuleFor(p => p.ConfirmPassword).Matches(v => v.Password);
            }
        }

        public class Result : BaseResult { }

        public class CommandHandler : IRequestHandler<Command, Result>
        {
            private readonly UserManager<ApplicationUser> _userManager;

            public CommandHandler(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null) return new Result().Succeeded();

                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));

                var result = await _userManager.ResetPasswordAsync(user, code, request.Password);

                if (!result.Succeeded)
                {
                    return new Result().Failed(result.Errors.First().Description);
                }

                return new Result().Succeeded();
            }
        }
    }
}
