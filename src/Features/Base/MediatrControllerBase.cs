using System.Threading.Tasks;
using Blazor5Validation.Shared.Features.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blazor5Validation.Server.Features.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediatrControllerBase : ControllerBase
    {
     
        protected readonly ISender _sender;
        
        public MediatrControllerBase(ISender sender)
        {
            _sender = sender;
        }

        protected async Task<IActionResult> Send<T>(IRequest<T> request)
        {
            var result = await _sender.Send(request);
            if (result is BaseResult baseResult)
            {
                if (!baseResult.IsSuccessful)
                {
                    return BadRequest(result);
                }
            }
            return Ok(result);
        }   
    }
}
