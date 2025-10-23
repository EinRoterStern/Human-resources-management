using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_resources_managment.EmployeeWindow.Model
{
    public class EmployeeDGModel
    {
        public string FIO { get; set; }
        public DateOnly? birthDate { get; set; }
        public DateOnly? hireDate { get; set; }
        public string positionName { get; set; }
        public string departmentName { get; set; }
        public string email {  get; set; }
        public string phone { get; set; }


    }
}
