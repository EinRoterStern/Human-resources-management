using Human_resources_managment.Models.DataBaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_resources_managment.PostgresDataBase.Configurations
{
    public class DepartmentsConfiguration : IEntityTypeConfiguration<Departments>
    {
        public void Configure(EntityTypeBuilder<Departments> builder)
        {
            // создание таблицы
            ConfigureTable(builder);
            // создание полей которые не vo
            ConfigureProperties(builder);
            // создание полей vo
            ConfigureValueObjects(builder);
            // связи между таблицами
            ConfigureRelations(builder);
        }

        private static void ConfigureTable(EntityTypeBuilder<Departments> builder)
        {
            builder.ToTable("departments");
            builder.HasKey(b => b.Id).HasName("pk_departments");
        }

        private static void ConfigureProperties(EntityTypeBuilder<Departments> builder)
        {
            builder.Property(b => b.Id)
                .IsRequired()
                .HasColumnName("id");

            builder.Property(b => b.Description)
                .IsRequired(false)
                .HasColumnName("description");
        }

        private static void ConfigureValueObjects(EntityTypeBuilder<Departments> builder)
        {
            ConfigureDepartName(builder);
        }

        private static void ConfigureDepartName(EntityTypeBuilder<Departments> builder)
        {
            builder.OwnsOne(b => b.Name, nb =>
            {
                nb.Property(b => b.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("name");
            });

            builder.Navigation(b => b.Name).IsRequired();
        }  

        private static void ConfigureRelations(EntityTypeBuilder<Departments> builder)
        {
            ConfigureAuthorRelation(builder);
        }

        private static void ConfigureAuthorRelation(EntityTypeBuilder<Departments> builder)
        {

            builder.HasOne(d => d.Employee)
                .WithOne(e => e.Department)
                .HasForeignKey<Employees>(e => e.DepartmentId) // FK в Employees
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
