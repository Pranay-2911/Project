using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class addgol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PolicyCancelationPenalty",
                table: "GlobalVariables",
                newName: "PolicyCancellationPenalty");

            migrationBuilder.RenameColumn(
                name: "CommisionWithdrawDeduction",
                table: "GlobalVariables",
                newName: "CommissionWithdrawDeduction");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PolicyCancellationPenalty",
                table: "GlobalVariables",
                newName: "PolicyCancelationPenalty");

            migrationBuilder.RenameColumn(
                name: "CommissionWithdrawDeduction",
                table: "GlobalVariables",
                newName: "CommisionWithdrawDeduction");
        }
    }
}
