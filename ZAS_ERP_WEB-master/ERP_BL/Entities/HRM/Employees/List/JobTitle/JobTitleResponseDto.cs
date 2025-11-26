namespace ERP_BL.Entities.HRM.Employees.List.JobTitle
{
    public class JobTitleResponseDto
    {
        public int Id { get; set; }
        public string JobTitleName { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public string? CreatedByUserName { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? LastModifiedByUserName { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
