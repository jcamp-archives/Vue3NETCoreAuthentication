using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Features.Base;

namespace Blazor5Auth.Server.Extensions
{
    public static class BaseResultExtensions
    {
        public static T Succeeded<T>(this T result, string message = null) where T : BaseResult
        {
            result.IsSuccessful = true;
            if (message != null)
            {
                result.Message = message;
            }
            return result;
        }

        public static T Failed<T>(this T result, string message = null) where T : BaseResult
        {
            result.IsSuccessful = false;
            if (message != null)
            {
                result.Message = message;
            }
            return result;
        }

        public static T WithErrors<T>(this T result, ModelStateDictionary modelState) where T : BaseResult
        {
            result.IsSuccessful = false;
            result.Errors = modelState.ToDictionary();
            return result;
        }

        public static T WithErrors<T>(this T result, IEnumerable<string> errors) where T : BaseResult
        {
            result.IsSuccessful = false;
            foreach (var error in errors)
            {
                result.Errors.Add("", error);
            }
            return result;
        }

        public static T WithError<T>(this T result, string field, string error) where T : BaseResult
        {
            result.IsSuccessful = false;
            result.Errors.Add(new KeyValuePair<string, string>(field, error));
            return result;
        }

    }
}
