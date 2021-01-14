using System;
using System.Linq;
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
    public class Register
    {
        public class Command : IRequest<Result>
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
            public string ReturnUrl { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Email).NotEmpty().EmailAddress();
                RuleFor(p => p.Password).NotEmpty().Length(8, 100);
                RuleFor(p => p.ConfirmPassword).Matches(v => v.Password);
            }
        }

        public class Result : BaseResult
        {
            public bool RequiresEmailConfirmation { get; set; }
        }

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
                var newUser = new ApplicationUser { UserName = request.Email, Email = request.Email };

                var result = await _signInManager.UserManager.CreateAsync(newUser, request.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(x => x.Description);

                    return new Result().WithErrors(errors);
                }

                // Send confirmation email
                var code = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(newUser);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var httpRequest = _contextAccessor.HttpContext.Request;
                var domain = $"{httpRequest.Scheme}://{httpRequest.Host}";

                var callbackUrl = $"{domain}/Account/ConfirmEmail?userId={Uri.EscapeDataString(newUser.Id)}&code={code}";

                await _emailService.SendAsync(newUser.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                // Add all new users to the User role
                await _signInManager.UserManager.AddToRoleAsync(newUser, "User");

                // Add new users whose email starts with 'admin' to the Admin role
                if (newUser.Email.StartsWith("admin"))
                {
                    await _signInManager.UserManager.AddToRoleAsync(newUser, "Admin");
                }

                return new Result().Succeeded();
            }
        }
    }
}
