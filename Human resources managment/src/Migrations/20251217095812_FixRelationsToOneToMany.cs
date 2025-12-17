using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Human_resources_managment.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationsToOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_employees_department_id",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "IX_employees_position_id",
                table: "employees");

            migrationBuilder.CreateIndex(
                name: "IX_employees_department_id",
                table: "employees",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_position_id",
                table: "employees",
                column: "position_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_employees_department_id",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "IX_employees_position_id",
                table: "employees");

            migrationBuilder.CreateIndex(
                name: "IX_employees_department_id",
                table: "employees",
                column: "department_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_position_id",
                table: "employees",
                column: "position_id",
                unique: true);
        }
    }
}
