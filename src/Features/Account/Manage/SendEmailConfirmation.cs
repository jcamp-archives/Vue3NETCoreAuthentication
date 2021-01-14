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
using MediatR;

namespace Features.Account.Manage
{
    public class SendEmailConfirmation
    {
        public class Command : IRequest<Result> { }

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
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var httpRequest = _contextAccessor.HttpContext.Request;
                var domain = $"{httpRequest.Scheme}://{httpRequest.Host}";

                var callbackUrl = $"{domain}/Account/ConfirmEmail?userId={Uri.EscapeDataString(userId)}&code={code}";

                await _emailService.SendAsync(email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                var statusMessage = "Confirmation link sent. Please check your email.";
                return new Result().Succeeded(statusMessage);
            }
        }
    }
}
