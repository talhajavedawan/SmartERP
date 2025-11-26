using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using ERP_BL.Entities.Core.Permissions;

namespace ERP_BL.Migrations
{
  public class Configuration : IEntityTypeConfiguration<Permission>
  {
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
      builder.HasData(
          new Permission
          {
            Id = 3100,
            Name = "HRM",
            Description = "Leaves Related HRM",
            ParentPermissionId = null,   // Top-level parent
            CreatedBy = "System",
            LastModifiedBy = null,
            CreationDate = new DateTime(2025, 8, 27),
            LastModified = new DateTime(2025, 8, 27),
            IsActive = true,
            IsVoid = false
          },
          new Permission
          { 
            Id = 3101,
            Name = "Access Employment Centre",
            Description = "Access to Employment Centre",
            ParentPermissionId = 3100,   // child of HRM
            CreatedBy = "System",
              LastModifiedBy = null,
            CreationDate = new DateTime(2025, 8, 27),
            LastModified = new DateTime(2025, 8, 27),
            IsActive = true,
            IsVoid = false
          },
          new Permission
          {
            Id = 3102,
            Name = "Add New Employee",
            Description = "Permission to add a new employee",
            ParentPermissionId = 3100,
            CreatedBy = "System",
              LastModifiedBy = null,
            CreationDate = new DateTime(2025, 8, 27),
            LastModified = new DateTime(2025, 8, 27),
            IsActive = true,
            IsVoid = false
          },
          new Permission
          {
            Id = 3103,
            Name = "View Employee Register",
            Description = "Permission to view employee register",
            ParentPermissionId = 3100,
            CreatedBy = "System",
              LastModifiedBy = null,
            CreationDate = new DateTime(2025, 8, 27),
            LastModified = new DateTime(2025, 8, 27),

            IsActive = true,
            IsVoid = false
          },
          new Permission
          {
            Id = 3104,
            Name = "View Employee",
            Description = "Permission to view employee details",
            ParentPermissionId = 3100,
            CreatedBy = "System",
              LastModifiedBy = null, 
            CreationDate = new DateTime(2025, 8, 27),
            LastModified = new DateTime(2025, 8, 27),

            IsActive = true,
            IsVoid = false
          }
      );
    }
  }
}
