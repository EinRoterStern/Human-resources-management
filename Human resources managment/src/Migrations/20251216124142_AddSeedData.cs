using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Human_resources_managment.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "departments",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { new Guid("aaaa1111-1111-1111-1111-111111111111"), "Технический отдел, отвечающий за все компьютеры в офисе", "IT" },
                    { new Guid("bbbb2222-2222-2222-2222-222222222222"), "Бухгалтерский отдел, отвечающий за деньги", "Бухгалтерский" },
                    { new Guid("cccc3333-3333-3333-3333-333333333333"), "Испытательный центр, испытывает все нововедения", "ИЦ" }
                });

            migrationBuilder.InsertData(
                table: "positions",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Инженер" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Инженер-программист" },
                    { new Guid("33222222-2222-2222-2222-222222222222"), "Бухгалтер" }
                });

            migrationBuilder.InsertData(
                table: "employees",
                columns: new[] { "id", "department_id", "position_id", "birth_date", "hire_date", "phone", "email", "first_name", "last_name", "middle_name" },
                values: new object[,]
                {
                    { new Guid("dddd6666-6666-6666-6666-666666666666"), new Guid("cccc3333-3333-3333-3333-333333333333"), new Guid("22222222-2222-2222-2222-222222222222"), new DateOnly(1980, 8, 15), new DateOnly(2000, 1, 1), "+79779999933", "mamail@mail.ru", "Игнатьев", "Валентайн", "Архипович" },
                    { new Guid("eeee4444-4444-4444-4444-444444444444"), new Guid("bbbb2222-2222-2222-2222-222222222222"), new Guid("33222222-2222-2222-2222-222222222222"), new DateOnly(2004, 4, 22), new DateOnly(2025, 4, 22), "+79999999999", "amail@mail.ru", "Абдула", "Али", null },
                    { new Guid("ffff5555-5555-5555-5555-555555555555"), new Guid("aaaa1111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111"), new DateOnly(2000, 12, 1), new DateOnly(2024, 4, 22), "+79889999966", "pamail@mail.ru", "Резников", "Константин", "Игоревич" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "id",
                keyValue: new Guid("dddd6666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "id",
                keyValue: new Guid("eeee4444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "id",
                keyValue: new Guid("ffff5555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "departments",
                keyColumn: "id",
                keyValue: new Guid("aaaa1111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "departments",
                keyColumn: "id",
                keyValue: new Guid("bbbb2222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "departments",
                keyColumn: "id",
                keyValue: new Guid("cccc3333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "positions",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "positions",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "positions",
                keyColumn: "id",
                keyValue: new Guid("33222222-2222-2222-2222-222222222222"));
        }
    }
}
