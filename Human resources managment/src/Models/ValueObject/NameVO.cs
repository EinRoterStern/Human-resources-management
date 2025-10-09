using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_resources_managment.Models.ValueObjectModels
{
    public record NameVO
    {
        private NameVO (string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public static Result<NameVO> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<NameVO>("Название не должно быть пустым!");

            var obj = new NameVO (name);
            return Result.Success<NameVO> (obj);
        }
    }
}
