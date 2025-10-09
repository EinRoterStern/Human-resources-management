using Human_resources_managment.Models.DataBaseModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_resources_managment.PostgresDataBase.Configurations
{
    public class EmployeesConfiguration : IEntityTypeConfiguration<Employees>
    {
        public void Configure(EntityTypeBuilder<Employees> builder)
        {
            ConfigureTable(builder);
            ConfigureProperties(builder);
            ConfigureValueObjects(builder);
            ConfigureRelations(builder);
        }

        private static void ConfigureTable(EntityTypeBuilder<Employees> builder)
        {
            builder.ToTable("employees");
            builder.HasKey(e => e.Id).HasName("pk_employees");
        }

        private static void ConfigureProperties(EntityTypeBuilder<Employees> builder)
        {
            builder.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("id");

            builder.Property(e => e.PositionId)
                .IsRequired()
                .HasColumnName("position_id");

            builder.Property(e => e.DepartmentId)
                .IsRequired()
                .HasColumnName("department_id");
        }

        private static void ConfigureValueObjects(EntityTypeBuilder<Employees> builder)
        {
            ConfigureFullName(builder);
            ConfigureDates(builder);
            ConfigureEmail(builder);
            ConfigurePhone(builder);
        }

        private static void ConfigureFullName(EntityTypeBuilder<Employees> builder)
        {
            builder.OwnsOne(e => e.FullName, fn =>
            {
                fn.Property(f => f.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("first_name");

                fn.Property(f => f.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("last_name");

                fn.Property(f => f.MidleName)
                    .HasMaxLength(50)
                    .HasColumnName("middle_name");
            });

            builder.Navigation(e => e.FullName).IsRequired();
        }

        private static void ConfigureDates(EntityTypeBuilder<Employees> builder)
        {
            builder.OwnsOne(e => e.BirthDate, bd =>
            {
                bd.Property(d => d.Date)
                    .IsRequired()
                    .HasColumnName("birth_date");
            });

            builder.OwnsOne(e => e.HireDate, hd =>
            {
                hd.Property(d => d.Date)
                    .IsRequired()
                    .HasColumnName("hire_date");
            });
        }

        private static void ConfigureEmail(EntityTypeBuilder<Employees> builder)
        {
            builder.OwnsOne(e => e.Email, em =>
            {
                em.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("email");
            });

            builder.Navigation(e => e.Email).IsRequired();
        }

        private static void ConfigurePhone(EntityTypeBuilder<Employees> builder)
        {
            builder.OwnsOne(e => e.Phone, ph =>
            {
                ph.Property(p => p.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("phone");
            });

            builder.Navigation(e => e.Phone).IsRequired();
        }

        private static void ConfigureRelations(EntityTypeBuilder<Employees> builder)
        {
            // связь с Departments (1:1)
            builder.HasOne(e => e.Department)
                .WithOne(d => d.Employee)
                .HasForeignKey<Employees>(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // связь с Positions (1:1)
            builder.HasOne(e => e.Position)
                .WithOne(p => p.Employee)
                .HasForeignKey<Employees>(e => e.PositionId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
