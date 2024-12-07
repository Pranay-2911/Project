using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class updateComm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "CommissionRequests");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CommissionRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "CommissionRequests");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "CommissionRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
