using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Blazor5Auth.Server.Extensions;
using Blazor5Auth.Server.Models;
using Features.Base;
using FluentValidation;
using MediatR;
using Reinforced.Typings.Attributes;

namespace Features.Account.Manage
{
    public class UserProfile
    {
        public class Query : IRequest<Command> { }

        [TsInterface(Name = "UserProfileCommand")]
        public class Command : IRequest<Result>
        {
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public bool IsEmailConfirmed { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Email).NotEmpty().EmailAddress();
            }
        }

        public class Result : BaseResult { }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ClaimsPrincipal _user;

            public QueryHandler(UserManager<ApplicationUser> userManager, IUserAccessor user)
            {
                _userManager = userManager;
                _user = user.User;
            }

            public async Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.GetUserAsync(_user);
                var model = new Command { Email = user.Email, PhoneNumber = user.PhoneNumber };
                model.IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                return model;
            }
        }

        public class CommandHandler : IRequestHandler<Command, Result>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ClaimsPrincipal _user;

            public CommandHandler(UserManager<ApplicationUser> userManager, IUserAccessor user)
            {
                _userManager = userManager;
                _user = user.User;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.GetUserAsync(_user);
                var statusMessage = "";
                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

                if (request.PhoneNumber != phoneNumber)
                {
                    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
                    statusMessage = setPhoneResult.Succeeded ? "Your profile has been updated" : "Unexpected error when trying to set phone number.";
                }

                return new Result().Succeeded(statusMessage);
            }
        }
    }
}
