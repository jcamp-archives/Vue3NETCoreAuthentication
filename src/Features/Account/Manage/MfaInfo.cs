using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Blazor5Auth.Server.Models;
using Features.Base;
using MediatR;
using Reinforced.Typings.Attributes;

namespace Features.Account.Manage
{
    public class MfaInfo
    {
        public class Query : IRequest<Result> { }

        [TsInterface(Name = "MfaInfo")]
        public class Result : BaseResult
        {
            public bool HasAuthenticator { get; set; }
            public int RecoveryCodesLeft { get; set; }
            public bool IsMfaEnabled { get; set; }
            public bool IsMachineRemembered { get; set; }
        }


        public class QueryHandler : IRequestHandler<Query, Result>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly ClaimsPrincipal _user;

            public QueryHandler(UserManager<ApplicationUser> userManager, IUserAccessor user, SignInManager<ApplicationUser> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _user = user.User;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.GetUserAsync(_user);

                var result = new Result
                {
                    HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
                    IsMfaEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
                    IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                    RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
                    IsSuccessful = true
                };
                return result;
            }
        }

    }
}
