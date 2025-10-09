using CSharpFunctionalExtensions;
using Human_resources_managment.Models.ValueObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_resources_managment.Models.DataBaseModels
{
    public class Positions
    {
        private Positions(NameVO name) 
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Guid Id { get; private set; }

        public NameVO Name { get; private set; }

        public Employees Employee { get; private set; }

        public Result<Positions> Create(NameVO name)
        {
            return Result.Success(new Positions(name));
        }
    }
}
