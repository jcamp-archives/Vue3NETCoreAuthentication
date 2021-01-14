using System.Security.Claims;
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
    public class ConfirmEmailChange
    {
        public class Command : IRequest<Result>
        {
            public string UserId { get; set; }
            public string Code { get; set; }
            public string Email { get; set; }
            public bool ClientAuth { get; set; }
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
            public string Token { get; set; }
            public bool RequiresEmailConfirmation { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Result>
        {
            private readonly IJwtHelper _jwtHelper;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly ClaimsPrincipal _user;

            public CommandHandler(IJwtHelper jwtHelper,
                SignInManager<ApplicationUser> signInManager, IUserAccessor user)
            {
                _jwtHelper = jwtHelper;
                _signInManager = signInManager;
                _user = user.User;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _signInManager.UserManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    return new Result().Failed($"Unable to load user with ID '{request.UserId}'.");
                }

                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
                var result = await _signInManager.UserManager.ChangeEmailAsync(user, request.Email, code);

                if (!result.Succeeded)
                {
                    return new Result().Failed("Error confirming your email.");
                }

                // In our UI email and user name are one and the same, so when we update the email
                // we need to update the user name.
                var setUserNameResult = await _signInManager.UserManager.SetUserNameAsync(user, request.Email);
                await _signInManager.RefreshSignInAsync(user);

                if (!setUserNameResult.Succeeded)
                {
                    return new Result().Failed("Error changing user name.");
                }

                if (_user.Identity.IsAuthenticated)
                {
                    var loggedInUser = await _signInManager.UserManager.GetUserAsync(_user);
                    if (loggedInUser.Id == request.UserId)
                    {
                        var roles = await _signInManager.UserManager.GetRolesAsync(user);
                        var token = _jwtHelper.GenerateJwt(user, roles);
                        return (new Result { Token = token }).Succeeded("Thank you for confirming your email change.");
                    }
                }

                return new Result().Succeeded("Thank you for confirming your email change.");
            }
        }
    }
}
