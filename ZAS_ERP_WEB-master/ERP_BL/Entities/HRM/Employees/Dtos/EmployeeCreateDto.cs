using ERP_BL.Entities.Base.Addresses;
using ERP_BL.Entities.Base.Contacts;
using ERP_BL.Entities.Base.Persons;
using ERP_BL.Entities.HRM.Employees.Dtos;
using ERP_BL.Entities.Locations.Cities.Dtos;
using ERP_BL.Entities.Locations.Countries.Dtos;
using ERP_BL.Entities.Locations.States.Dtos;
using ERP_BL.Entities.Locations.Zones.Dtos;
using ERP_BL.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ERP_BL.Entities.HRM.Employees.Dtos
{
    public class EmployeeCreateDto
    {
        [Required] public string SystemDisplayName { get; set; } = null!;
        public string? JobTitle { get; set; }
        [Required] public DateTime HireDate { get; set; }
        public DateTime? ProbationPeriodEndDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        [Required] public string EmploymentType { get; set; } = null!;
        [Required] public string EmployeeStatus { get; set; }
        [Required] public string EmployeeStatusClass { get; set; }
        public int? ManagerId { get; set; }          
        public int? HRManagerId { get; set; }       
        public bool IsActive { get; set; } = true;
        public string? PayGrade { get; set; }

        // Profile Picture
        public IFormFile? ProfilePictureFile { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public string? ProfilePictureContentType { get; set; }
        public long? ProfilePictureSize { get; set; }
        public string? ProfilePictureFileName { get; set; }


        public Person? Person { get; set; }
        public Contact? Contact { get; set; }
        public Address? PermanentAddress { get; set; }
        public Address? TemporaryAddress { get; set; }

        public List<int> CompanyIds { get; set; } = new();
        public List<int> DepartmentIds { get; set; } = new();
    }
}

public class EmployeeUpdateDto : EmployeeCreateDto
{
    public int Id { get; set; }
    public bool RemoveProfilePicture { get; set; }
}

public class ContactDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string? LinkedIn { get; set; }
}
public class AddressDto
{
    public int Id { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? Zipcode { get; set; }
    public CountryDto? Country { get; set; }
    public StateAddressDto? State { get; set; }
    public CityDto? City { get; set; }
    public ZoneDto? Zone { get; set; }
    public Enum AddressType { get; set; } = Enums.Employee;
}
public class PersonDto
{
    public int? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FatherName { get; set; }
    public string CNIC { get; set; }
    public DateTime DOB { get; set; }
    public string? BloodGroup { get; set; }
    public string Gender { get; set; }
    public string MaritalStatus { get; set; }
    public string? PassportNumber { get; set; }
    public string? Nationality { get; set; }
    public string? Religion { get; set; }
}