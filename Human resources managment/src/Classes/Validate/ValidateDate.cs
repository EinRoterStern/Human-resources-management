using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Human_resources_managment.Classes.Validate
{
    public class ValidateDate : ValidationRule
    {

        public static bool IsValidDate(DateTime? date)
        {
            if (date == null) return false;
            return date.Value.Date <= DateTime.Today;
        }

        public static string GetValidationError(DateTime? date)
        {
            if (date == null)
                return "Дата не может быть пустой.";

            if (date.Value.Date > DateTime.Today)
                return "Дата не может быть в будущем.";

            return null; 
        }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(value == null)
                return new ValidationResult(false, "Дата не может быть пустой");

            DateTime? date = value as DateTime?;

            string error = GetValidationError(date);
            if (error != null)
                return new ValidationResult(false, error);

            return ValidationResult.ValidResult;
        }
    }
}
