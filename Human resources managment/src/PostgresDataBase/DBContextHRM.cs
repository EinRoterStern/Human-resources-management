using Human_resources_managment.Models.DataBaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_resources_managment.PostgresDataBase
{
    public class DBContextHRM : DbContext
    {

        public DBContextHRM(DbContextOptions<DBContextHRM> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        // Создание миграций по конфигурациям
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DBContextHRM).Assembly);
        }

        public DbSet<Departments> Departments => Set<Departments>();

        public DbSet<Employees> Employees => Set<Employees>();

        public DbSet<Positions> Positions => Set<Positions>();


    }
}
