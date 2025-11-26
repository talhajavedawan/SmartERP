using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities.Base.Contacts.Dtos
{
    public class ContactDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string? WebsiteUrl { get; set; }
        public string? EmergencyPhoneNumber { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? Fax { get; set; }
        public string? LinkedIn { get; set; }
    }
}
