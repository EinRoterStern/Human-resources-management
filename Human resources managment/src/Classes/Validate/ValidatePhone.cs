using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Human_resources_managment.Classes.Validate
{
    public class ValidatePhone : ValidationRule
    {

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            string digits = Regex.Replace(phone, @"[^\d]", "");
            if (digits.Length != 11)
                return false;

            return digits[0] == '7' || digits[0] == '8';
        }

        public static string GetValidationError(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return "Телефон не может быть пустым.";

            string digits = Regex.Replace(phone, @"[^\d]", "");
            if (digits.Length != 11)
                return "Номер должен содержать 11 цифр.";

            if (digits[0] != '7' && digits[0] != '8')
                return "Номер должен быть российским (+7 или 8).";

            return null;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string phone = value as string;

            string error = GetValidationError(phone);
            if (error != null)
                return new ValidationResult(false, error);

            return ValidationResult.ValidResult;
        }
    }
}
