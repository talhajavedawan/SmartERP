using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Bl.Entities.Base.Persons.Dtos
{
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
}

