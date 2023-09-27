// Ignore Spelling: Validator

using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TitlesOrganizer.Application.ViewModels.Common
{
    public static class ValidatorExtensions
    {
        public static void AddToModelState(this ValidationResult validationResult, ModelStateDictionary modelState)
        {
            modelState.RemoveErrorsRepetedInValidator(validationResult);

            foreach (var error in validationResult.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }

        private static void RemoveErrorsRepetedInValidator(this ModelStateDictionary modelState, ValidationResult validationResult)
        {
            if (!modelState.IsValid && !validationResult.IsValid)
            {
                foreach (var key in validationResult.Errors.Select(e => e.PropertyName))
                {
                    if (modelState[key]?.Errors.Any() ?? false)
                    {
                        modelState[key]!.Errors.Clear();
                    }
                }
            }
        }
    }
}