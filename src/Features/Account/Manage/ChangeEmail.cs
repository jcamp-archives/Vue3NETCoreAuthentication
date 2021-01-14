using System;
using System.Security.Claims;
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

namespace Features.Account.Manage
{
    public class ChangeEmail
    {
        public class Command : IRequest<Result>
        {
            public string NewEmail { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.NewEmail).NotNull().NotEmpty().EmailAddress();
            }
        }

        public class Result : BaseResult { }

        public class CommandHandler : IRequestHandler<Command, Result>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ClaimsPrincipal _user;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IEmailService _emailService;

            public CommandHandler(UserManager<ApplicationUser> userManager, IUserAccessor user, IEmailService emailService, IHttpContextAccessor contextAccessor)
            {
                _userManager = userManager;
                _user = user.User;
                _emailService = emailService;
                _contextAccessor = contextAccessor;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.GetUserAsync(_user);
                var email = await _userManager.GetEmailAsync(user);
                string statusMessage;

                if (request.NewEmail != email)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);

                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var httpRequest = _contextAccessor.HttpContext.Request;
                    var domain = $"{httpRequest.Scheme}://{httpRequest.Host}";

                    var callbackUrl = $"{domain}/Account/ConfirmEmailChange?userId={Uri.EscapeDataString(userId)}&code={code}&email={request.NewEmail}";

                    await _emailService.SendAsync(request.NewEmail, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    statusMessage = "Confirmation link to change email sent. Please check your email.";
                }
                else
                {
                    statusMessage = "Your email is unchanged.";
                }

                return new Result().Succeeded(statusMessage);
            }
        }
    }
}
