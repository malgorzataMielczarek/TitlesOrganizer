using FluentValidation.Results;
using FormHelper;
using Microsoft.AspNetCore.Mvc;

namespace TitlesOrganizer.Web.Controllers.Helper
{
    public static class ValidationHelper
    {
        public static JsonResult CreateErrorResult(this ValidationResult validationResult, string message, string? redirectUri = null, int? redirectDelay = null)
        {
            return new JsonResult(
                new FormResult(FormResultStatus.Error)
                {
                    Message = message,
                    RedirectUri = redirectUri,
                    RedirectDelay = redirectDelay,
                    ValidationErrors = validationResult.Errors.Select(err => new FormResultValidationError()
                    {
                        PropertyName = err.PropertyName,
                        Message = err.ErrorMessage
                    }).ToList()
                });
        }
    }
}