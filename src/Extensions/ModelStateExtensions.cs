using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blazor5Validation.Server.Extensions
{
    public static class ModelStateExtensions
    {
        public static IDictionary<string, string> ToDictionary(this ModelStateDictionary modelState)
        {
            var Errors = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var keyModelStatePair in modelState)
            {
                var key = keyModelStatePair.Key;

                //camelCase the key
                key = Regex.Replace(key, @"([A-Z])([A-Z]+|[a-z0-9_]+)($|[A-Z]\w*)",
            m =>
            {
                return m.Groups[1].Value.ToLower() + m.Groups[2].Value.ToLower() + m.Groups[3].Value;
            });
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    if (errors.Count == 1)
                    {
                        var errorMessage = GetErrorMessage(errors[0]);
                        Errors.Add(key, errorMessage);
                    }
                    else
                    {
                        var errorMessages = "";
                        for (var i = 0; i < errors.Count; i++)
                        {
                            if (i > 0) {
                              errorMessages += ", ";
                            }
                            errorMessages += GetErrorMessage(errors[i]);
                        }

                        Errors.Add(key, errorMessages);
                    }
                }
            }

            return Errors;

            string GetErrorMessage(ModelError error)
            {
                return string.IsNullOrEmpty(error.ErrorMessage) ? "The input was not valid." : error.ErrorMessage;
            }

        }
    }
}
