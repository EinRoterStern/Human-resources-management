using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Human_resources_managment.DepartmentWindow.Model;
using Human_resources_managment.EmployeeWindow.Model;
using Human_resources_managment.Models.DataBaseModels;
using Human_resources_managment.Models.ValueObjectModels;
using Human_resources_managment.PositionWindow.Model;
using Human_resources_managment.PostgresDataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Human_resources_managment.Classes
{
    public class DataBaseHelper
    {
        private static DBContextHRM GetContext()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

            var connectionString = config.GetConnectionString("HRMConnection");

            var optionsBuilder = new DbContextOptionsBuilder<DBContextHRM>();
            optionsBuilder.UseNpgsql(connectionString);

            return new DBContextHRM(optionsBuilder.Options);
        }

        public static async Task<(List<DepartmentDGModel>, string message)> GetDepartmentTable()
        {
            try
            {
                using var context = GetContext();

                var departments = await context.Departments.ToListAsync();

                var dgModels = departments.Select(d => new DepartmentDGModel
                {
                    name = d.Name.Name,
                    description = d.Description,
                    id = d.Id

                }).ToList();

                return (dgModels, string.Empty);
            }
            catch (Exception ex)
            {
                return(null, ex.Message);
            }
            
        }

        public static async Task<(List<PositionDGModel>, string message)> GetPositionTable()
        {
            try
            {
                using var context = GetContext();

                var position = await context.Positions.ToListAsync();

                var dgPosition = position.Select(d => new PositionDGModel
                {
                    name = d.Name.Name,
                    id = d.Id

                }).ToList();

                return (dgPosition, string.Empty);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
           
        }

        public static async Task<(List<EmployeeDGModel>, string message)> GetEmployeeTable()
        {
            try
            {
                using var context = GetContext();

                var employees = await context.Employees
                   .Include(e => e.FullName)
                   .Include(e => e.BirthDate)
                   .Include(e => e.HireDate)
                   .Include(e => e.Email)
                   .Include(e => e.Phone)
                   .Include(e => e.Position)      // ← Загружаем Position
                       .ThenInclude(p => p.Name)  // ← и его Name (NameVO)
                   .Include(e => e.Department)    // ← Загружаем Department
                       .ThenInclude(d => d.Name)  // ← и его Name
                   .ToListAsync();

                var dgEmployee = employees.Select(d => new EmployeeDGModel
                {
                    FIO = string.Join(" ", new[]
                     {
                        d.FullName.FirstName,
                        d.FullName.LastName,
                        d.FullName.MidleName
                    }.Where(part => !string.IsNullOrEmpty(part))),

                    birthDate = d.BirthDate.Date,
                    hireDate = d.HireDate.Date,
                    positionName = d.Position?.Name?.Name ?? "—", // ← защита от null
                    departmentName = d.Department?.Name?.Name ?? "—",
                    email = d.Email.Email,
                    phone = d.Phone.Phone,
                    id = d.Id
                }).ToList();


                return (dgEmployee, string.Empty);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
                //MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        public static async Task<(bool success, string message)> AddDepartment(string name, string? description = null)
        {
            try
            {
                // 1. Создаём NameVO
                var nameResult = NameVO.Create(name);
                if (nameResult.IsFailure)
                    return (false, nameResult.Error);

                // 2. Создаём сущность через DDD-фабрику
                var deptResult = Departments.Create(nameResult.Value, description);
                if (deptResult.IsFailure)
                    return (false, deptResult.Error);

                var department = deptResult.Value;

                // 3. Сохраняем в БД
                using var context = GetContext(); // ваша фабрика контекста
                context.Departments.Add(department);
                await context.SaveChangesAsync();

                return (true, "Департамент успешно добавлен");
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка при добавлении департамента: {ex.Message}");
            }
        }

    }
}
