using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Human_resources_managment.Models.DataBaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Human_resources_managment.PostgresDataBase
{
    public class DBContextHRM : DbContext
    {

        public DBContextHRM(DbContextOptions<DBContextHRM> options)
            : base(options)
        {
        }

        // ⚠️ Этот конструктор нужен ТОЛЬКО для миграций (EF Core Tooling)
        public DBContextHRM()
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Читаем строку подключения из appsettings.json
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var connectionString = configuration.GetConnectionString("HRMConnection");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        // Создание миграций по конфигурациям
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DBContextHRM).Assembly);

            // Seed: Positions
            var positions = new[]
            {
                new { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                new { Id = Guid.Parse("22222222-2222-2222-2222-222222222222") },
                new { Id = Guid.Parse("33222222-2222-2222-2222-222222222222") }
            };

            modelBuilder.Entity<Positions>().HasData(positions);

            var positionNames = new[]
            {
                new { PositionsId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Инженер" },
                new { PositionsId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Инженер-программист" },
                new { PositionsId = Guid.Parse("33222222-2222-2222-2222-222222222222"), Name = "Бухгалтер" }
            };

            modelBuilder.Entity<Positions>()
                .OwnsOne(p => p.Name)
                .HasData(positionNames);


            // Seed: Departments
            var departments = new[]
            {
                new
                {
                    Id = Guid.Parse("aaaa1111-1111-1111-1111-111111111111"),
                    Description = "Технический отдел, отвечающий за все компьютеры в офисе"
                },
                new
                {
                    Id = Guid.Parse("bbbb2222-2222-2222-2222-222222222222"),
                    Description = "Бухгалтерский отдел, отвечающий за деньги"
                },
                new
                {
                    Id = Guid.Parse("cccc3333-3333-3333-3333-333333333333"),
                    Description = "Испытательный центр, испытывает все нововедения"
                }
            };

            modelBuilder.Entity<Departments>().HasData(departments);

            var departmentNames = new[]
            {
                new
                {
                    DepartmentsId = Guid.Parse("aaaa1111-1111-1111-1111-111111111111"),
                    Name = "IT"
                },
                new
                {
                    DepartmentsId = Guid.Parse("bbbb2222-2222-2222-2222-222222222222"),
                    Name = "Бухгалтерский"
                },
                new
                {
                    DepartmentsId = Guid.Parse("cccc3333-3333-3333-3333-333333333333"),
                    Name = "ИЦ"
                }
            };

            modelBuilder.Entity<Departments>()
                .OwnsOne(d => d.Name)
                .HasData(departmentNames);

            // Seed: Employees
            var employees = new[]
            {
                new
                {
                    Id = Guid.Parse("eeee4444-4444-4444-4444-444444444444"),
                    PositionId = Guid.Parse("33222222-2222-2222-2222-222222222222"),
                    DepartmentId = Guid.Parse("bbbb2222-2222-2222-2222-222222222222")
                },
                new
                {
                    Id = Guid.Parse("ffff5555-5555-5555-5555-555555555555"),
                    PositionId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    DepartmentId = Guid.Parse("aaaa1111-1111-1111-1111-111111111111")
                },
                new
                {
                    Id = Guid.Parse("dddd6666-6666-6666-6666-666666666666"),
                    PositionId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    DepartmentId = Guid.Parse("cccc3333-3333-3333-3333-333333333333")
                }
            };

            modelBuilder.Entity<Employees>().HasData(employees);

            var employeeFullNames = new[]
            {
                new
                {
                    EmployeesId = Guid.Parse("eeee4444-4444-4444-4444-444444444444"),
                    FirstName = "Абдула",
                    LastName = "Али",
                    MidleName = (string?)null
                },
                new
                {
                    EmployeesId = Guid.Parse("ffff5555-5555-5555-5555-555555555555"),
                    FirstName = "Резников",
                    LastName = "Константин",
                    MidleName = "Игоревич"
                },
                new
                {
                    EmployeesId = Guid.Parse("dddd6666-6666-6666-6666-666666666666"),
                    FirstName = "Игнатьев",
                    LastName = "Валентайн",
                    MidleName = "Архипович"
                }
            };

            modelBuilder.Entity<Employees>()
                .OwnsOne(e => e.FullName)
                .HasData(employeeFullNames);

            var employeeBirthDates = new[]
            {
                new
                {
                    EmployeesId = Guid.Parse("eeee4444-4444-4444-4444-444444444444"),
                    Date = DateOnly.ParseExact("22.04.2004", "dd.MM.yyyy")
                },
                new
                {
                    EmployeesId = Guid.Parse("ffff5555-5555-5555-5555-555555555555"),
                    Date = DateOnly.ParseExact("01.12.2000", "dd.MM.yyyy")
                },
                new
                {
                    EmployeesId = Guid.Parse("dddd6666-6666-6666-6666-666666666666"),
                    Date = DateOnly.ParseExact("15.08.1980", "dd.MM.yyyy")
                }
            };

            modelBuilder.Entity<Employees>()
                .OwnsOne(e => e.BirthDate)
                .HasData(employeeBirthDates);

            var employeeHireDates = new[]
            {
                new
                {
                    EmployeesId = Guid.Parse("eeee4444-4444-4444-4444-444444444444"),
                    Date = DateOnly.ParseExact("22.04.2025", "dd.MM.yyyy")
                },
                new
                {
                    EmployeesId = Guid.Parse("ffff5555-5555-5555-5555-555555555555"),
                    Date = DateOnly.ParseExact("22.04.2024", "dd.MM.yyyy")
                },
                new
                {
                    EmployeesId = Guid.Parse("dddd6666-6666-6666-6666-666666666666"),
                    Date = DateOnly.ParseExact("01.01.2000", "dd.MM.yyyy")
                }
            };

            modelBuilder.Entity<Employees>()
                .OwnsOne(e => e.HireDate)
                .HasData(employeeHireDates);

            var employeeEmails = new[]
            {
                new
                {
                    EmployeesId = Guid.Parse("eeee4444-4444-4444-4444-444444444444"),
                    Email = "amail@mail.ru"
                },
                new
                {
                    EmployeesId = Guid.Parse("ffff5555-5555-5555-5555-555555555555"),
                    Email = "pamail@mail.ru"
                },
                new
                {
                    EmployeesId = Guid.Parse("dddd6666-6666-6666-6666-666666666666"),
                    Email = "mamail@mail.ru"
                }
            };

            modelBuilder.Entity<Employees>()
                .OwnsOne(e => e.Email)
                .HasData(employeeEmails);

            var employeePhones = new[]
            {
                new
                {
                    EmployeesId = Guid.Parse("eeee4444-4444-4444-4444-444444444444"),
                    Phone = "+79999999999"
                },
                new
                {
                    EmployeesId = Guid.Parse("ffff5555-5555-5555-5555-555555555555"),
                    Phone = "+79889999966"
                },
                new
                {
                    EmployeesId = Guid.Parse("dddd6666-6666-6666-6666-666666666666"),
                    Phone = "+79779999933"
                }
            };

            modelBuilder.Entity<Employees>()
                .OwnsOne(e => e.Phone)
                .HasData(employeePhones);

        }

        public DbSet<Departments> Departments => Set<Departments>();

        public DbSet<Employees> Employees => Set<Employees>();

        public DbSet<Positions> Positions => Set<Positions>();


    }
}
