namespace ERP_BL.Entities.Core.Permissions.Dtos
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? ParentPermissionId { get; set; }
        public bool IsActive { get; set; }
    }
}
