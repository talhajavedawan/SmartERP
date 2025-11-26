using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_BL.Migrations
{
    /// <inheritdoc />
    public partial class ProfilePictureAndChangesInVendor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Addresses_PermanentAddressId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Contacts_ContactId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Persons_PersonId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Companies_CompanyId",
                table: "Vendors");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Contacts_ContactId",
                table: "Vendors");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Persons_ContactPersonId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_ContactId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_ContactPersonId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ContactPersonId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "IsVoid",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "JoinDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "HRManager",
                table: "Employees",
                newName: "ProfilePictureContentType");

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNumber",
                table: "Vendors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PermanentAddressId",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "JobTitle",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ContactId",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "HRManagerId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "Employees",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureFileName",
                table: "Employees",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProfilePictureSize",
                table: "Employees",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompanyVendor",
                columns: table => new
                {
                    ClientCompaniesId = table.Column<int>(type: "int", nullable: false),
                    VendorsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyVendor", x => new { x.ClientCompaniesId, x.VendorsId });
                    table.ForeignKey(
                        name: "FK_CompanyVendor_Companies_ClientCompaniesId",
                        column: x => x.ClientCompaniesId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompanyVendor_Vendors_VendorsId",
                        column: x => x.VendorsId,
                        principalTable: "Vendors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DepartmentVendor",
                columns: table => new
                {
                    DepartmentsId = table.Column<int>(type: "int", nullable: false),
                    VendorsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentVendor", x => new { x.DepartmentsId, x.VendorsId });
                    table.ForeignKey(
                        name: "FK_DepartmentVendor_Departments_DepartmentsId",
                        column: x => x.DepartmentsId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepartmentVendor_Vendors_VendorsId",
                        column: x => x.VendorsId,
                        principalTable: "Vendors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VendorContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    ContactId = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    LastModifiedById = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorContacts_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VendorContacts_AspNetUsers_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VendorContacts_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VendorContacts_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VendorContacts_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_HRManagerId",
                table: "Employees",
                column: "HRManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ManagerId",
                table: "Employees",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyVendor_VendorsId",
                table: "CompanyVendor",
                column: "VendorsId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentVendor_VendorsId",
                table: "DepartmentVendor",
                column: "VendorsId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorContacts_ContactId",
                table: "VendorContacts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorContacts_CreatedById",
                table: "VendorContacts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_VendorContacts_LastModifiedById",
                table: "VendorContacts",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_VendorContacts_PersonId",
                table: "VendorContacts",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorContacts_VendorId",
                table: "VendorContacts",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Addresses_PermanentAddressId",
                table: "Employees",
                column: "PermanentAddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Contacts_ContactId",
                table: "Employees",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_HRManagerId",
                table: "Employees",
                column: "HRManagerId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_ManagerId",
                table: "Employees",
                column: "ManagerId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Persons_PersonId",
                table: "Employees",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Companies_CompanyId",
                table: "Vendors",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Addresses_PermanentAddressId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Contacts_ContactId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_HRManagerId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_ManagerId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Persons_PersonId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Companies_CompanyId",
                table: "Vendors");

            migrationBuilder.DropTable(
                name: "CompanyVendor");

            migrationBuilder.DropTable(
                name: "DepartmentVendor");

            migrationBuilder.DropTable(
                name: "VendorContacts");

            migrationBuilder.DropIndex(
                name: "IX_Employees_HRManagerId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ManagerId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "HRManagerId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ProfilePictureFileName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ProfilePictureSize",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "ProfilePictureContentType",
                table: "Employees",
                newName: "HRManager");

            migrationBuilder.AddColumn<int>(
                name: "ContactId",
                table: "Vendors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContactPersonId",
                table: "Vendors",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PermanentAddressId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "JobTitle",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContactId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVoid",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinDate",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Manager",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_ContactId",
                table: "Vendors",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_ContactPersonId",
                table: "Vendors",
                column: "ContactPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Addresses_PermanentAddressId",
                table: "Employees",
                column: "PermanentAddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Contacts_ContactId",
                table: "Employees",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Persons_PersonId",
                table: "Employees",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Companies_CompanyId",
                table: "Vendors",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Contacts_ContactId",
                table: "Vendors",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Persons_ContactPersonId",
                table: "Vendors",
                column: "ContactPersonId",
                principalTable: "Persons",
                principalColumn: "Id");
        }
    }
}
