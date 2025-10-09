using CSharpFunctionalExtensions;
using Human_resources_managment.Models.ValueObject;
using Human_resources_managment.Models.ValueObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_resources_managment.Models.DataBaseModels
{
    public class Employees
    {
        private Employees(FullNameVO fullNameVO, DateVO birthDate, DateVO hireDate, Guid positionId, Guid departmentId, EmailVO email, PhoneVO phone) 
        {
            Id = Guid.NewGuid();
            FullName = fullNameVO;
            BirthDate = birthDate;
            HireDate = hireDate;
            PositionId = positionId;
            DepartmentId = departmentId;
            Email = email;
            Phone = phone;
        }

        public Guid Id { get; private set; }

        public FullNameVO FullName { get; private set; }

        public DateVO BirthDate { get; private set; }

        public DateVO HireDate { get; private set; }

        public Guid PositionId { get; private set; }

        public Guid DepartmentId { get; private set; }

        public EmailVO Email { get; private set; }

        public PhoneVO Phone { get; private set; }

        public Result<Employees> Create(FullNameVO fullNameVO, DateVO birthDate, DateVO hireDate, Guid positionId, Guid departmentId, EmailVO email, PhoneVO phone)
        {
            return Result.Success(new Employees(fullNameVO, birthDate, hireDate, positionId, departmentId, email, phone));
        }

    }
}
