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
using Human_resources_managment.Models.ValueObject;
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
                    id = d.Id,
                    departmentId = d.DepartmentId,
                    positionId = d.PositionId
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
        public static async Task<(bool success, string message)> UpdateDepartment(Guid id, string newName, string? newDescription = null)
        {
            try
            {
                var nameVO = NameVO.Create(newName);
                if (nameVO.IsFailure)
                    return (false, nameVO.Error);

                using var context = GetContext();

                var department = await context.Departments
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (department == null)
                    return (false, "Департамент не найден");

                var result = department.Update(nameVO.Value, newDescription);
                if (result.IsFailure)
                    return (false, result.Error);

                await context.SaveChangesAsync();
                return (true, "Департамент успешно обновлён");

            }
            catch (Exception ex)
            {
                return (false, $"Ошибка при обновлении: {ex.Message}");
            }
        }
        public static async Task<(bool success, string message)> DeleteDepartment(Guid id)
        {
            try
            {
                using var context = GetContext();

                var department = await context.Departments
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (department == null)
                    return (false, "Департамент не найден");

                context.Departments.Remove(department);
                await context.SaveChangesAsync();

                return (true, "Департамент успешно удалён");
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка при удалении: {ex.Message}");
            }
        }

        public static async Task<(bool success, string message)> AddPosition(string name)
        {
            try
            {
                var nameResult = NameVO.Create(name);
                if (nameResult.IsFailure)
                    return (false, nameResult.Error);

                var posResult = Positions.Create(nameResult.Value);
                if (posResult.IsFailure)
                    return (false, posResult.Error);

                var position = posResult.Value;

                using var context = GetContext(); 
                context.Positions.Add(position);
                await context.SaveChangesAsync();

                return (true, "Должность успешно добавлена");
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка при добавлении должности: {ex.Message}");
            }
        }
        public static async Task<(bool success, string message)> UpdatePosition(Guid id, string newName)
        {
            try
            {
                var nameVO = NameVO.Create(newName);
                if (nameVO.IsFailure)
                    return (false, nameVO.Error);

                using var context = GetContext();

                var position = await context.Positions
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (position == null)
                    return (false, "Должность не найдена");

                var result = position.Update(nameVO.Value);
                if (result.IsFailure)
                    return (false, result.Error);

                await context.SaveChangesAsync();
                return (true, "Должность успешно обновлёна");

            }
            catch (Exception ex)
            {
                return (false, $"Ошибка при обновлении: {ex.Message}");
            }
        }
        public static async Task<(bool success, string message)> DeletePosition(Guid id)
        {
            try
            {
                using var context = GetContext();

                var position = await context.Positions
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (position == null)
                    return (false, "Должность не найдена");

                context.Positions.Remove(position);
                await context.SaveChangesAsync();

                return (true, "Должность успешно удалёна");
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка при удалении: {ex.Message}");
            }
        }

        public static async Task<(bool success, string message)> AddEmployee(string firstName, string lastName, string? middleName, DateOnly? birthDate, DateOnly? hireDate, string email, string phone, Guid positionId, Guid departmentId)
        {
            try
            {
                var fullNameResult = FullNameVO.Create(firstName, lastName, middleName);
                if (fullNameResult.IsFailure)
                    return (false, $"Ошибка ФИО: {fullNameResult.Error}");

                var emailResult = EmailVO.Create(email.Trim());
                if (emailResult.IsFailure)
                    return (false, $"Ошибка email: {emailResult.Error}");

                var phoneResult = PhoneVO.Create(phone);
                if (phoneResult.IsFailure)
                    return (false, $"Ошибка телефона: {phoneResult.Error}");

                var birthDateResult = DateVO.Create(birthDate);
                if (birthDateResult.IsFailure)
                    return (false, $"Ошибка даты рождения: {birthDateResult.Error}");

                var hireDateResult = DateVO.Create(hireDate);
                if (hireDateResult.IsFailure)
                    return (false, $"Ошибка даты приёма: {hireDateResult.Error}");

                var employeeResult = Employees.Create(fullNameResult.Value, birthDateResult.Value, hireDateResult.Value, positionId, departmentId, emailResult.Value, phoneResult.Value);

                if (employeeResult.IsFailure)
                    return (false, $"Ошибка создания сотрудника: {employeeResult.Error}");

                using var context = GetContext();
                context.Employees.Add(employeeResult.Value);
                await context.SaveChangesAsync();

                return (true, "Сотрудник успешно добавлен");

            }
            catch (Exception ex)
            {
                return (false, $"Ошибка: {ex.Message}\n\nВнутренняя ошибка: {ex.InnerException?.Message}");
            }
        }

        public static async Task<(bool success, string message)> UpdateEmployee(Guid id, string newEmail, string newPhone, Guid newDepartId, Guid newPosId)
        {
            try
            {
                var emailVO = EmailVO.Create(newEmail);
                if (emailVO.IsFailure)
                    return (false, emailVO.Error);

                var phoneVO = PhoneVO.Create(newPhone);
                if (phoneVO.IsFailure)
                    return (false, phoneVO.Error);

                using var context = GetContext();

                var employee = await context.Employees
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (employee == null)
                    return (false, "Сотрудник не найден");

                var result = employee.Update(emailVO.Value, phoneVO.Value, newDepartId, newPosId);
                if (result.IsFailure)
                    return (false, result.Error);

                await context.SaveChangesAsync();
                return (true, "Сотрудник успешно обновлён");

            }
            catch (Exception ex)
            {
                return (false, $"Ошибка при обновлении: {ex.Message}");
            }
        }
        public static async Task<(bool success, string message)> DeleteEmployee(Guid id)
        {
            try
            {
                using var context = GetContext();

                var employee = await context.Employees
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (employee == null)
                    return (false, "Сотрудник не найден");

                context.Employees.Remove(employee);
                await context.SaveChangesAsync();

                return (true, "Сотрудник успешно удалён");
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка при удалении: {ex.Message}");
            }
        }

        public static (string firstName, string lastName, string? middleName) ParseFio(string fio)
        {
            if (string.IsNullOrWhiteSpace(fio))
                throw new ArgumentException("ФИО не может быть пустым", nameof(fio));

            var parts = fio.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return parts.Length switch
            {
                1 => (parts[0], "", null), 
                2 => (parts[1], parts[0], null), 
                >= 3 => (parts[1], parts[0], string.Join(" ", parts.Skip(2))), 
                _ => throw new ArgumentException("Некорректный формат ФИО", nameof(fio))
            };
        }

    }
}
