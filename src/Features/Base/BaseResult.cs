using System.Collections.Generic;
using Reinforced.Typings.Attributes;

namespace Features.Base
{
    [TsInterface]
    public class BaseResult
    {
        public bool IsSuccessful { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public IDictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    }
}
