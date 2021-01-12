using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Blazor5Validation.Shared.Features.Base;
using Blazor5Validation.Server.Extensions;
using System.Threading;
using Blazor5Validation.Shared;

namespace Features.Person
{
    public class CreatePerson
    {
        public class Command : Blazor5Validation.Shared.Person, IRequest<Result>
        {

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator() {
                Include(new PersonValidator());
            }
        }

        public class Result : BaseResult { }

        public class CommandHandler : IRequestHandler<Command, Result>
        {
            public Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = new Result().Success("Looks good to the server (mediatr).");
                return Task.FromResult(result);
            }
        }

    }
}
