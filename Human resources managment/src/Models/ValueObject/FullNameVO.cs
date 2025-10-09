using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Human_resources_managment.Models.ValueObjectModels
{
    public record FullNameVO
    {
        private FullNameVO(string firstName, string lastName, string? midleName = null)
        {
            FirstName = firstName;
            LastName = lastName;
            MidleName = midleName;
        }


        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string? MidleName { get; private set; }

        public static Result<FullNameVO> Create (string firstName, string lastName, string? midlName = null)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return Result.Failure<FullNameVO>("Имя не должно быть пустым!");

            if (string.IsNullOrWhiteSpace(lastName))
                return Result.Failure<FullNameVO>("Фамилия не должно быть пустым!");

            var obj = new FullNameVO(firstName, lastName, midlName);
            return Result.Success<FullNameVO>(obj);
        }
            

    }
}
