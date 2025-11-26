namespace ERP_BL.Entities.Core.Permissions.Dtos
{
    public class UserRoleResponseDto
    {
        public int? Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? ParentRoleId { get; set; }
        public bool IsActive { get; set; }
        public List<PermissionDto> Permissions { get; set; } = new();
    }
}
