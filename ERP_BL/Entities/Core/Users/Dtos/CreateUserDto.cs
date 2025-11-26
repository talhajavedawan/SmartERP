namespace ERP_BL.Entities.Core.Users.Dtos
{
    public class CreateUserDto
    {
        public string UserName { get; set; }
        //public string Email { get; set; }
        public int? EmployeeId { get; set; }
        public string? Password { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
        public List<int> Roles { get; set; } = new();
    }
    //create
}