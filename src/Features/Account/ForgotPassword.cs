using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Blazor5Auth.Server.Extensions;
using Blazor5Auth.Server.Models;
using Blazor5Auth.Server.Services;
using Features.Base;
using FluentValidation;
using MediatR;

namespace Features.Account
{
    public class ForgotPassword
    {
        public class Command : IRequest<Result>
        {
            public string Email { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Email).NotEmpty().EmailAddress();
            }
        }

        public class Result : BaseResult { }

        public class CommandHandler : IRequestHandler<Command, Result>
        {
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IEmailService _emailService;

            public CommandHandler(SignInManager<ApplicationUser> signInManager, IHttpContextAccessor contextAccessor, IEmailService emailService)
            {
                _signInManager = signInManager;
                _contextAccessor = contextAccessor;
                _emailService = emailService;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(request.Email);
                if (user == null || !(await _signInManager.UserManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return new Result().Succeeded();
                }

                var code = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var httpRequest = _contextAccessor.HttpContext.Request;
                var domain = $"{httpRequest.Scheme}://{httpRequest.Host}";

                var callbackUrl = $"{domain}/Account/ResetPassword?code={code}&email={user.Email}";

                await _emailService.SendAsync(user.Email, "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return new Result().Succeeded();
            }
        }
    }
}
