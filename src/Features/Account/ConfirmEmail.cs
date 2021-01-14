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

namespace Features.Account
{
    public class ConfirmEmail
    {
        public class Command : IRequest<Result>
        {
            public string UserId { get; set; }
            public string Code { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.UserId).NotEmpty();
                RuleFor(p => p.Code).NotEmpty();
            }
        }

        public class Result : BaseResult
        {
            public bool RequiresEmailConfirmation { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Result>
        {
            private readonly SignInManager<ApplicationUser> _signInManager;

            public CommandHandler(SignInManager<ApplicationUser> signInManager)
            {
                _signInManager = signInManager;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _signInManager.UserManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    return new Result().Failed($"Unable to load user with ID '{request.UserId}'.");
                }

                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
                var result = await _signInManager.UserManager.ConfirmEmailAsync(user, code);

                if (!result.Succeeded)
                {
                    return new Result().Failed("Error confirming your email.");
                }

                return new Result().Succeeded("Thank you for confirming your email. You may now login.");
            }
        }


    }
}
