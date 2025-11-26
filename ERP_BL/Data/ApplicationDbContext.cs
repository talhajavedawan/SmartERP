
using ERP_BL.Entities;
using ERP_BL.Entities.Base.Addresses;
using ERP_BL.Entities.Base.Contacts;
using ERP_BL.Entities.Base.Persons;
using ERP_BL.Entities.Company_Center.Companies;
using ERP_BL.Entities.CompanyCenter.Companies;
using ERP_BL.Entities.CompanyCenter.Companies.List;
using ERP_BL.Entities.CompanyCenter.Customers;
using ERP_BL.Entities.CompanyCenter.Departments;
using ERP_BL.Entities.Core.Permissions;
using ERP_BL.Entities.Core.PowerUsers;
using ERP_BL.Entities.Core.Roles;
using ERP_BL.Entities.Core.Users;
using ERP_BL.Entities.Core.Statuses;
using ERP_BL.Entities.HRM.Employees;

using ERP_BL.Entities.HRM.Employees.List.JobTitle;

using ERP_BL.Entities.Leaves;
using ERP_BL.Entities.Locations.Cities;
using ERP_BL.Entities.Locations.Countries;
using ERP_BL.Entities.Locations.States;
using ERP_BL.Entities.Locations.Zones;
using ERP_BL.Entities.Notifications;
using ERP_BL.Migrations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ERP_BL.Entities.Core.StatusClass;
namespace ERP_BL.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<PowerUser> PowerUsers { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<BusinessType> BusinessTypes { get; set; }
        public DbSet<IndustryType> IndustryTypes { get; set; }

        public DbSet<JobTitle> JobTitles { get; set; }

        public DbSet<Status> Statuses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveApplication> LeaveApplications { get; set; }
        public DbSet<EmployeeLeaveBalance> EmployeeLeaveBalances { get; set; }
        public DbSet<LeaveApplicationHistory> LeaveApplicationHistories { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<VendorNature> VendorNatures { get; set; }
        public DbSet<VendorContact> VendorContacts { get; set; }
   public DbSet<StatusClass> StatusClasses { get; set; }
        // Notification Tables
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationFlag> NotificationFlags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>()
                .Property(p => p.BloodGroup)
                .HasConversion<string>();
            modelBuilder.Entity<Person>()
                .Property(p => p.Gender)
                .HasConversion<string>();
            modelBuilder.Entity<Person>()
                .Property(p => p.MaritalStatus)
                .HasConversion<string>();
            modelBuilder.Entity<Address>()
                .Property(a => a.AddressType)
                .HasConversion<string>();
            modelBuilder.Entity<Company>()
                .Property(c => c.CompanyType)
                .HasConversion<string>();




            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithMany(e => e.Users)
                .HasForeignKey(u => u.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Role>()
                .Property(r => r.CreationDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<BusinessType>()
                .Property(b => b.BusinessTypeName)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<IndustryType>()
                .Property(i => i.IndustryTypeName)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Company>()
                .HasOne(c => c.BusinessType)
                .WithMany()
                .HasForeignKey(c => c.BusinessTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Company>()
                .HasOne(c => c.IndustryType)
                .WithMany()
                .HasForeignKey(c => c.IndustryTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<City>()
                .HasIndex(c => c.Name);
            modelBuilder.Entity<City>()
                .HasIndex(c => c.StateId);
            modelBuilder.Entity<Role>()
                .Property(r => r.CreationDate)
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<BusinessType>()
                .Property(b => b.BusinessTypeName)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<IndustryType>()
                .Property(i => i.IndustryTypeName)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Company>()
                .HasOne(c => c.BusinessType)
                .WithMany()
                .HasForeignKey(c => c.BusinessTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Company>()
                .HasOne(c => c.IndustryType)
                .WithMany()
                .HasForeignKey(c => c.IndustryTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<City>()
                .HasIndex(c => c.Name);
            modelBuilder.Entity<City>()
                .HasIndex(c => c.StateId);
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity(j => j.ToTable("PermissionRole"));
            modelBuilder.Entity<UserSetting>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired(false);
                entity.Property(e => e.PowerUserId).IsRequired(false);
                entity.Property(e => e.SettingKey).IsRequired().HasMaxLength(200);
                entity.Property(e => e.SettingValue).IsRequired();
                entity.Property(e => e.LastModified).IsRequired();

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.PowerUser)
                      .WithMany()
                      .HasForeignKey(e => e.PowerUserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasCheckConstraint("CK_UserSetting_SingleUserType",
                    "[UserId] IS NULL OR [PowerUserId] IS NULL");
            });
            modelBuilder.Entity<PowerUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(350);
            });
            modelBuilder.Entity<LeaveApplication>()
            .HasOne(a => a.Employee)
            .WithMany(e => e.LeaveApplications)
           .HasForeignKey(a => a.EmployeeId)
           .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LeaveApplication>()
                .HasOne(a => a.LeaveType)
                .WithMany(l => l.LeaveApplications)
                .HasForeignKey(a => a.LeaveTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LeaveApplication>()
                .Property(a => a.Status)
                .HasConversion<string>()
                .HasDefaultValue(LeaveApplicationStatus.UnderApproval);
            modelBuilder.Entity<LeaveApplicationHistory>()
               .HasOne(h => h.LeaveApplication)
               .WithMany(a => a.Histories)
               .HasForeignKey(h => h.LeaveApplicationId)
               .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.ApplyConfiguration(new Configuration());

            modelBuilder.Entity<Vendor>()
                    .HasOne(v => v.Company)
                    .WithMany(c => c.OwnedVendors)
                    .HasForeignKey(v => v.CompanyId)
                    .OnDelete(DeleteBehavior.NoAction); // prevent cascade

            // --- Vendor ↔ Client Companies ---
            modelBuilder.Entity<Vendor>()
                .HasMany(v => v.ClientCompanies)
                .WithMany(c => c.Vendors)
                .UsingEntity<Dictionary<string, object>>(
                    "CompanyVendor",
                    j => j.HasOne<Company>()
                          .WithMany()
                          .HasForeignKey("ClientCompaniesId")
                          .OnDelete(DeleteBehavior.NoAction), // 🔴 Disable cascade
                    j => j.HasOne<Vendor>()
                          .WithMany()
                          .HasForeignKey("VendorsId")
                          .OnDelete(DeleteBehavior.NoAction)  // 🔴 Disable cascade
                );

            // --- Vendor ↔ Departments ---
            modelBuilder.Entity<Vendor>()
                .HasMany(v => v.Departments)
                .WithMany(d => d.Vendors)
                .UsingEntity<Dictionary<string, object>>(
                    "DepartmentVendor",
                    j => j.HasOne<Department>()
                          .WithMany()
                          .HasForeignKey("DepartmentsId")
                          .OnDelete(DeleteBehavior.NoAction),
                    j => j.HasOne<Vendor>()
                          .WithMany()
                          .HasForeignKey("VendorsId")
                          .OnDelete(DeleteBehavior.NoAction)
                );

            // --- Notification Configuration ---
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.SendingUser)
                .WithMany()
                .HasForeignKey(n => n.SendingUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.CcUser)
                .WithMany()
                .HasForeignKey(n => n.CcUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.NotificationFlag)
                .WithMany(f => f.Notifications)
                .HasForeignKey(n => n.FlagId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Notification>()
                .Property(n => n.TransactionType)
                .HasConversion<string>();

            modelBuilder.Entity<Notification>()
                .Property(n => n.Timestamp)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<NotificationFlag>()
                .Property(f => f.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}