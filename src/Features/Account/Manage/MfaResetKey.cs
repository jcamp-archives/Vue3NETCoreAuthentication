using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Blazor5Auth.Server.Extensions;
using Blazor5Auth.Server.Models;
using Features.Base;
using MediatR;

namespace Features.Account.Manage
{
    public class MfaResetKey
    {
        public class Command : IRequest<Result> { }

        public class Result : BaseResult { }

        public class CommandHandler : IRequestHandler<Command, Result>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly ClaimsPrincipal _user;

            public CommandHandler(UserManager<ApplicationUser> userManager, IUserAccessor user, SignInManager<ApplicationUser> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _user = user.User;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.GetUserAsync(_user);
                await _userManager.SetTwoFactorEnabledAsync(user, false);
                await _userManager.ResetAuthenticatorKeyAsync(user);
                await _signInManager.RefreshSignInAsync(user);

                return new Result().Succeeded(
                    "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.");
            }
        }
    }
}
