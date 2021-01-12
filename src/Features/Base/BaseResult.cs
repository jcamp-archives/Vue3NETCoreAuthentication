using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor5Validation.Shared.Features.Base
{
    public class BaseResult
    {
        public bool IsSuccessful { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public IDictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    }
}
