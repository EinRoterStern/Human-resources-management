using CSharpFunctionalExtensions;
using Human_resources_managment.Models.ValueObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_resources_managment.Models.DataBaseModels
{
    public class Departments
    {

        private Departments(NameVO name, string? description = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
        }

        public Guid Id { get; private set; }

        public NameVO Name { get; private set; }

        public string? Description { get; private set; }

        public Employees Employee { get; private set; }

        public Result<Departments> Create(NameVO name, string? description = null)
        {
            return Result.Success(new Departments(name, description));
        }
    }
}
