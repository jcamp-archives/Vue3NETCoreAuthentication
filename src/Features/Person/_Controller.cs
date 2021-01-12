using System;
using System.Threading.Tasks;
using Blazor5Validation.Server.Features.Base;
using Blazor5Validation.Shared.Features.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Features.Person
{
    public class PersonMediatrController : MediatrControllerBase
    {
        public PersonMediatrController(ISender sender) : base(sender) { }
        
        [HttpPost]
        public async Task<IActionResult> Index(CreatePerson.Command model) => await Send(model);
    }
}
