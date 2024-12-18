using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class addDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Documents_PolicyAccountId",
                table: "Documents",
                column: "PolicyAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_PolicyAccounts_PolicyAccountId",
                table: "Documents",
                column: "PolicyAccountId",
                principalTable: "PolicyAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_PolicyAccounts_PolicyAccountId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_PolicyAccountId",
                table: "Documents");
        }
    }
}
