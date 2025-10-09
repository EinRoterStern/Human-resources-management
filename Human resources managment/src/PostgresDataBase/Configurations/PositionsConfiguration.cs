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
    public class PositionsConfiguration : IEntityTypeConfiguration<Positions>
    {
        public void Configure(EntityTypeBuilder<Positions> builder)
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

        private static void ConfigureTable(EntityTypeBuilder<Positions> builder)
        {
            builder.ToTable("positions");
            builder.HasKey(b => b.Id).HasName("pk_positions");
        }

        private static void ConfigureProperties(EntityTypeBuilder<Positions> builder)
        {
            builder.Property(b => b.Id)
                .IsRequired()
                .HasColumnName("id");
        }

        private static void ConfigureValueObjects(EntityTypeBuilder<Positions> builder)
        {
            ConfigureDepartName(builder);
        }

        private static void ConfigureDepartName(EntityTypeBuilder<Positions> builder)
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

        private static void ConfigureRelations(EntityTypeBuilder<Positions> builder)
        {
            ConfigureAuthorRelation(builder);
        }

        private static void ConfigureAuthorRelation(EntityTypeBuilder<Positions> builder)
        {

            builder.HasOne(d => d.Employee)
                .WithOne(e => e.Position)
                .HasForeignKey<Employees>(e => e.PositionId) // FK в Employees
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
