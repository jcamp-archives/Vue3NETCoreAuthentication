using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Blazor5Auth.Server.Extensions;
using Blazor5Auth.Server.Models;
using Features.Base;
using FluentValidation;
using MediatR;

namespace Features.Account.Manage
{
    public class PersonalData
    {
        public class Query : IRequest<QueryResult> { }

        public class QueryResult : BaseResult
        {
            public string JsonData { get; set; }
        }

        public class Command : IRequest<Result>
        {
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Password).NotEmpty().MinimumLength(8);
            }
        }

        public class Result : BaseResult { }

        public class QueryHandler : IRequestHandler<Query, QueryResult>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ClaimsPrincipal _user;

            public QueryHandler(UserManager<ApplicationUser> userManager, IUserAccessor user)
            {
                _userManager = userManager;
                _user = user.User;
            }

            public async Task<QueryResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.GetUserAsync(_user);

                // Only include personal data for download
                var personalData = new Dictionary<string, string>();
                var personalDataProps = typeof(ApplicationUser).GetProperties().Where(
                    prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
                foreach (var p in personalDataProps)
                {
                    personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
                }

                var logins = await _userManager.GetLoginsAsync(user);
                foreach (var l in logins)
                {
                    personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
                }

                var result = new QueryResult().Succeeded();
                result.JsonData = JsonSerializer.Serialize(personalData);

                return result;

                // Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
                // return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");

            }

        }

        public class CommandHandler : IRequestHandler<Command, Result>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ClaimsPrincipal _user;
            private readonly SignInManager<ApplicationUser> _signInManager;

            public CommandHandler(UserManager<ApplicationUser> userManager, IUserAccessor user, SignInManager<ApplicationUser> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _user = user.User;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.GetUserAsync(_user);

                if (!await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    return new Result().Failed("Incorrect password.");
                }

                var result = await _userManager.DeleteAsync(user);
                var userId = await _userManager.GetUserIdAsync(user);
                if (!result.Succeeded)
                {
                    return new Result().Failed($"Unexpected error occurred deleting user with ID '{userId}'.");
                }

                await _signInManager.SignOutAsync();

                return new Result().Succeeded();
            }
        }
    }
}
