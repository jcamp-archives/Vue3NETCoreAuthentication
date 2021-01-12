using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Blazor5Validation.Shared
{
    public class Person
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string EmailAddress { get; set; }
    }

    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter a name");
            RuleFor(p => p.Name).MaximumLength(50).WithMessage("Name cannot be longer than 50 characters");
            RuleFor(p => p.Age).NotNull().GreaterThanOrEqualTo(0).WithMessage("Age must be greater than 0");
            RuleFor(p => p.Age).LessThan(150).WithMessage("Age cannot be greater than 150");
            RuleFor(p => p.EmailAddress).NotEmpty().WithMessage("You must enter a email address");
            RuleFor(p => p.EmailAddress).EmailAddress().WithMessage("You must provide a valid email address");

            RuleFor(p => p.EmailAddress).Must(value => !value.Contains("nonymous.com")).WithMessage("We don't accept emails from this domain.");

            RuleFor(x => x.Name).MustAsync(async (name, cancellationToken) => await IsUniqueAsync(name))
                                .WithMessage("Name must be unique")
                                .When(person => !string.IsNullOrEmpty(person.Name));

        }

        private async Task<bool> IsUniqueAsync(string name)
        {
            await Task.Delay(100);
            return name.ToLower() != "test";
        }
    }
}
