namespace TechExpoWorld.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public class IsDateTimeBeforeAttribute : ValidationAttribute, IClientModelValidator
    {
        private const string Error = "The Start Date must be before the End Date!";
        private readonly string endDate;

        public IsDateTimeBeforeAttribute(string endDate)
        {
            this.endDate = endDate;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var endDateProperty = validationContext.ObjectType.GetProperty(this.endDate);

            if (endDateProperty == null)
            {
                throw new ArgumentNullException(nameof(endDateProperty));
            }

            var endDateTime = (DateTime)endDateProperty.GetValue(validationContext.ObjectInstance);

            if ((DateTime)value > endDateTime)
            {
                return new ValidationResult(Error);
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-isdatetimebefore", Error);
        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value);
            return true;
        }
    }
}
