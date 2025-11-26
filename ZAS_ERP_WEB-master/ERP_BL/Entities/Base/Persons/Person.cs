
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Entities.Base.Persons
{

    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
    public string FirstName { get; set; }


    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
    public string? LastName { get; set; }


    [StringLength(50, ErrorMessage = "Father's name can't exceed 50 characters.")]
    public string? FatherName { get; set; }


    [RegularExpression(@"^\d{5}-\d{7}-\d{1}$", ErrorMessage = "CNIC must be in the format 12345-1234567-1.")]
    public string? CNIC { get; set; }


    [DataType(DataType.Date)]


    public DateTime? DOB { get; set; }
    public string? BloodGroup { get; set; }  

 
    public string? Gender { get; set; }     

    
    public string? MaritalStatus { get; set; } 


    [StringLength(20, ErrorMessage = "Passport number can't exceed 20 characters.")]
    public string? PassportNumber { get; set; }


        [StringLength(50, ErrorMessage = "Nationality can't exceed 50 characters.")]
        public string? Nationality { get; set; }

        [StringLength(50, ErrorMessage = "Religion can't exceed 50 characters.")]
        public string? Religion { get; set; }

    }
}
