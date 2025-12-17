using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CSharpFunctionalExtensions;
using Human_resources_managment.Models.ValueObject;
using Human_resources_managment.Models.ValueObjectModels;

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

        private Employees() { }
        public Guid Id { get; private set; }

        public FullNameVO FullName { get; private set; }

        public DateVO BirthDate { get; private set; }

        public DateVO HireDate { get; private set; }

        public Guid PositionId { get; private set; }
        public Positions Position { get; private set; }

        public Guid DepartmentId { get; private set; }

        public Departments Department { get; private set; }

        public EmailVO Email { get; private set; }

        public PhoneVO Phone { get; private set; }

        public static Result<Employees> Create(FullNameVO fullNameVO, DateVO birthDate, DateVO hireDate, Guid positionId, Guid departmentId, EmailVO email, PhoneVO phone)
        {
            return Result.Success(new Employees(fullNameVO, birthDate, hireDate, positionId, departmentId, email, phone));
        }

        public Result Update(EmailVO newEmail, PhoneVO newPhone, Guid newDepartId, Guid newPosId)
        {
            Email = newEmail;
            Phone = newPhone;
            DepartmentId = newDepartId;
            PositionId = newPosId;

            return Result.Success();
        }

    }
}
