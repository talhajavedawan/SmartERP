using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_BL.Migrations
{
    /// <inheritdoc />
    public partial class CompanyDeptSelection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "IndustryTypes");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "IndustryTypes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BusinessTypes");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "BusinessTypes");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "IndustryTypes",
                newName: "LastModifiedDate");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "IndustryTypes",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "Employees",
                newName: "LastModifiedDate");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Employees",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "ModificationDate",
                table: "Departments",
                newName: "LastModifiedDate");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Departments",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "BusinessTypes",
                newName: "LastModifiedDate");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "BusinessTypes",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "IndustryTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedByUserId",
                table: "IndustryTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedByUserId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedByUserId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "BusinessTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedByUserId",
                table: "BusinessTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndustryTypes_CreatedByUserId",
                table: "IndustryTypes",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_IndustryTypes_LastModifiedByUserId",
                table: "IndustryTypes",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatedByUserId",
                table: "Employees",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LastModifiedByUserId",
                table: "Employees",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CreatedByUserId",
                table: "Departments",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_LastModifiedByUserId",
                table: "Departments",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTypes_CreatedByUserId",
                table: "BusinessTypes",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTypes_LastModifiedByUserId",
                table: "BusinessTypes",
                column: "LastModifiedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessTypes_AspNetUsers_CreatedByUserId",
                table: "BusinessTypes",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessTypes_AspNetUsers_LastModifiedByUserId",
                table: "BusinessTypes",
                column: "LastModifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_AspNetUsers_CreatedByUserId",
                table: "Departments",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_AspNetUsers_LastModifiedByUserId",
                table: "Departments",
                column: "LastModifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_CreatedByUserId",
                table: "Employees",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_LastModifiedByUserId",
                table: "Employees",
                column: "LastModifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IndustryTypes_AspNetUsers_CreatedByUserId",
                table: "IndustryTypes",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IndustryTypes_AspNetUsers_LastModifiedByUserId",
                table: "IndustryTypes",
                column: "LastModifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessTypes_AspNetUsers_CreatedByUserId",
                table: "BusinessTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessTypes_AspNetUsers_LastModifiedByUserId",
                table: "BusinessTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_AspNetUsers_CreatedByUserId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_AspNetUsers_LastModifiedByUserId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_CreatedByUserId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_LastModifiedByUserId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_IndustryTypes_AspNetUsers_CreatedByUserId",
                table: "IndustryTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_IndustryTypes_AspNetUsers_LastModifiedByUserId",
                table: "IndustryTypes");

            migrationBuilder.DropIndex(
                name: "IX_IndustryTypes_CreatedByUserId",
                table: "IndustryTypes");

            migrationBuilder.DropIndex(
                name: "IX_IndustryTypes_LastModifiedByUserId",
                table: "IndustryTypes");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CreatedByUserId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_LastModifiedByUserId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Departments_CreatedByUserId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_LastModifiedByUserId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_BusinessTypes_CreatedByUserId",
                table: "BusinessTypes");

            migrationBuilder.DropIndex(
                name: "IX_BusinessTypes_LastModifiedByUserId",
                table: "BusinessTypes");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "IndustryTypes");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                table: "IndustryTypes");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "BusinessTypes");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                table: "BusinessTypes");

            migrationBuilder.RenameColumn(
                name: "LastModifiedDate",
                table: "IndustryTypes",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "IndustryTypes",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "LastModifiedDate",
                table: "Employees",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Employees",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "LastModifiedDate",
                table: "Departments",
                newName: "ModificationDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Departments",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "LastModifiedDate",
                table: "BusinessTypes",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "BusinessTypes",
                newName: "CreationDate");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "IndustryTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "IndustryTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "BusinessTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "BusinessTypes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
