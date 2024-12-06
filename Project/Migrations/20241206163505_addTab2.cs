using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class addTab2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agents_Employees_EmployeeId",
                table: "Agents");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Employees_EmployeeId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_EmployeeId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Agents_EmployeeId",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Agents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Agents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_EmployeeId",
                table: "Customers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_EmployeeId",
                table: "Agents",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agents_Employees_EmployeeId",
                table: "Agents",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Employees_EmployeeId",
                table: "Customers",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
