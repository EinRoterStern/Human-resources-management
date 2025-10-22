using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Extensions.Options;

namespace Human_resources_managment.Classes.Validate
{
    public class ValidateEmail : ValidationRule
    {

        private static readonly Regex EmailRegex = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
        }

        public static string GetValidationError(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "Email не может быть пустым!";

            if (!EmailRegex.IsMatch(email))
                return "Некорректный формат email!";

            return null; // валидно
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string email = value as string;


            string error = GetValidationError(email);
            if (error != null)
                return new ValidationResult(false, error);

            return ValidationResult.ValidResult;
        }
    }
}
