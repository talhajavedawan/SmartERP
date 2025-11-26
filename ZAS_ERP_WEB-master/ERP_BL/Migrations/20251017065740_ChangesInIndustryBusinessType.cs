using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_BL.Migrations
{
    /// <inheritdoc />
    public partial class ChangesInIndustryBusinessType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "IndustryTypes");

            migrationBuilder.DropColumn(
                name: "IsVoid",
                table: "IndustryTypes");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "BusinessTypes");

            migrationBuilder.DropColumn(
                name: "IsVoid",
                table: "BusinessTypes");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "IndustryTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "IndustryTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "BusinessTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "BusinessTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndustryTypes_IndustryTypeName",
                table: "IndustryTypes",
                column: "IndustryTypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTypes_BusinessTypeName",
                table: "BusinessTypes",
                column: "BusinessTypeName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IndustryTypes_IndustryTypeName",
                table: "IndustryTypes");

            migrationBuilder.DropIndex(
                name: "IX_BusinessTypes_BusinessTypeName",
                table: "BusinessTypes");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "IndustryTypes");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "IndustryTypes");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "BusinessTypes");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "BusinessTypes");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "IndustryTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVoid",
                table: "IndustryTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "BusinessTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVoid",
                table: "BusinessTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
