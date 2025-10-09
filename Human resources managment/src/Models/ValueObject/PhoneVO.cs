using CSharpFunctionalExtensions;
using Human_resources_managment.Models.ValueObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Human_resources_managment.Models.ValueObject
{
    public record PhoneVO
    {
        private PhoneVO(string phone) 
        {
            Phone = phone;
        }

        public string Phone { get; private set; }


        // Регулярка для валидации НОРМАЛИЗОВАННОГО номера: +7XXXXXXXXXX (11 цифр после +)
        private static readonly Regex NormalizedPhoneRegex = new(
            @"^\+7\d{10}$",
            RegexOptions.Compiled);

        // Регулярка для "грязного" ввода: разрешаем +, пробелы, скобки, дефисы
        private static readonly Regex InputPhoneRegex = new(
            @"^[\+]?[78]?[\s\-\(\)]?\d{3}[\s\-\(\)]?\d{3}[\s\-\(\)]?\d{2}[\s\-\(\)]?\d{2}$",
            RegexOptions.Compiled);

        public static Result<PhoneVO> Create(string phone)
        {

            if (string.IsNullOrWhiteSpace(phone))
                return Result.Failure<PhoneVO>("Номер телефона не должен быть пустой!");


            if (!NormalizedPhoneRegex.IsMatch(Normalize(phone)))
                return Result.Failure<PhoneVO>("Номер телефона неверного формата!");

            var obj = new PhoneVO(Normalize(phone));
            return Result.Success<PhoneVO>(obj);
        }


        private static string Normalize(string input)
        {
            // Удаляем всё, кроме цифр
            var digitsOnly = Regex.Replace(input, @"[^\d]", "");

            // Если номер начинается с 8 — заменяем на 7
            if (digitsOnly.StartsWith("8") && digitsOnly.Length == 11)
                digitsOnly = "7" + digitsOnly.Substring(1);

            // Если номер без кода страны (10 цифр) — добавляем +7
            if (digitsOnly.Length == 10)
                digitsOnly = "7" + digitsOnly;

            // Добавляем +
            return "+" + digitsOnly;
        }
    }

}
