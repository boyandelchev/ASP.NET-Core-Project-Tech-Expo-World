namespace TechExpoWorld.Areas.Admin.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using static TechExpoWorld.Data.DataConstants.Event;

    public class ValidDateTime : ValidationAttribute
    {
        private const string Error = "The date must be in format '15/08/2021', '15-08-2021' or '15.08.2021'!";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isValid = Regex.IsMatch(value.ToString(), DateRegularExpression);

            if (!isValid)
            {
                return new ValidationResult(Error);
            }

            var isDate = DateTime.TryParseExact(
                value.ToString(),
                DateFormatOne,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dateTimeOne);

            if (isDate)
            {
                return ValidationResult.Success;
            }

            isDate = DateTime.TryParseExact(
                value.ToString(),
                DateFormatTwo,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dateTimeTwo);

            if (isDate)
            {
                return ValidationResult.Success;
            }

            isDate = DateTime.TryParseExact(
                value.ToString(),
                DateFormatThree,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dateTimeThree);

            if (isDate)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(Error);
        }
    }
}
