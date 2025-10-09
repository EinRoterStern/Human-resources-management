using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Human_resources_managment.Models.ValueObjectModels
{
    public record EmailVO
    {
        private EmailVO(string email) 
        {
            Email = email;
        }

        public string Email { get; private set; }

        private static readonly Regex EmailRegex = new(
               @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$",
               RegexOptions.Compiled | RegexOptions.IgnoreCase);


        public static Result<EmailVO> Create(string email)
        {

            if (string.IsNullOrWhiteSpace(email))
                return Result.Failure<EmailVO>("Почта не должна быть пустой!");

            // Ограничение длины по RFC 5321: максимум 254 символа
            if (email.Length > 254)
                return Result.Failure<EmailVO>("Почта слишком длинная!");

            if(!EmailRegex.IsMatch(email))
                return Result.Failure<EmailVO>("Почта неверного формата!");

            var obj = new EmailVO(email);
            return Result.Success<EmailVO>(obj);
        }
    }
}
