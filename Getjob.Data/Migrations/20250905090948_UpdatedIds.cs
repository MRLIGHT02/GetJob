using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetJob.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployerId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JobseekerId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "JobseekerId",
                table: "Users");
        }
    }
}
