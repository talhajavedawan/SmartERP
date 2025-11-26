using ERP_BL.Entities.CompanyCenter.Companies;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Entities.Base.Contacts
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
        public string PhoneNumber { get; set; }
        [StringLength(200, ErrorMessage = "Website URL cannot exceed 200 characters.")]
        public string? WebsiteUrl { get; set; }
        [StringLength(20, ErrorMessage = "Emergency phone number cannot exceed 20 characters.")]
        public string? EmergencyPhoneNumber { get; set; }
        [StringLength(20, ErrorMessage = "WhatsApp number cannot exceed 20 characters.")]
        public string? WhatsAppNumber { get; set; }
        [StringLength(20, ErrorMessage = "Fax number cannot exceed 20 characters.")]
        public string? Fax { get; set; }
        [StringLength(200, ErrorMessage = "LinkedIn URL cannot exceed 200 characters.")]
        public string? LinkedIn { get; set; }
      
    }
}
