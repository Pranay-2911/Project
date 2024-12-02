using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class addanc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agents_Admins_AdminId",
                table: "Agents");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Admins_AdminId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Admins_AdminId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Policies_Admins_AdminId",
                table: "Policies");

            migrationBuilder.DropIndex(
                name: "IX_Policies_AdminId",
                table: "Policies");

            migrationBuilder.DropIndex(
                name: "IX_Employees_AdminId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Customers_AdminId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Agents_AdminId",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Agents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AdminId",
                table: "Policies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdminId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdminId",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdminId",
                table: "Agents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Policies_AdminId",
                table: "Policies",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AdminId",
                table: "Employees",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AdminId",
                table: "Customers",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_AdminId",
                table: "Agents",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agents_Admins_AdminId",
                table: "Agents",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Admins_AdminId",
                table: "Customers",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Admins_AdminId",
                table: "Employees",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_Admins_AdminId",
                table: "Policies",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");
        }
    }
}
