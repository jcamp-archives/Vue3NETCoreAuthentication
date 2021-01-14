using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Blazor5Auth.Server.Extensions;
using Blazor5Auth.Server.Models;
using Features.Base;
using MediatR;

namespace Features.Account.Manage
{
    public class MfaForgetBrowser
    {
        public class Command : IRequest<Result> { }

        public class Result : BaseResult { }

        public class CommandHandler : IRequestHandler<Command, Result>
        {
            private readonly SignInManager<ApplicationUser> _signInManager;

            public CommandHandler(SignInManager<ApplicationUser> signInManager)
            {
                _signInManager = signInManager;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                await _signInManager.ForgetTwoFactorClientAsync();

                return new Result().Succeeded(
                    "The current browser has been forgotten. When you login again from this browser you will be prompted for your Mfa code.");
            }
        }
    }
}
