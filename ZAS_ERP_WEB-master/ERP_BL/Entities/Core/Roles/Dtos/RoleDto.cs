using ERP_BL.Entities.Core.Permissions.Dtos;
namespace ERP_BL.Entities.Core.Roles.Dtos
{
public class RoleDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
  public int? ParentRoleId { get; set; }
  public bool IsActive { get; set; } = true;
  public bool IsVoid { get; set; } = false;
  public string? CreatedBy { get; set; }
  public DateTime? CreationDate { get; set; }
  public string? LastModifiedBy { get; set; }


  public List<int> PermissionIds { get; set; } = new();
  public List<PermissionDto> Permissions { get; set; } = new();
}
}
