using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_BL.Enums;
namespace ERP_BL.Databases
{
    [Table("tabPerson")]
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[ForeignKey("Employee"), Column(Order=0)]
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string FatherName { get; set; }
        public byte[] Photo { get; set; }
        public byte[] Signature { get; set; } 
        //public string ImageName { get; set; }
        public string NextKin { get; set; }
        //[Required]
        public string CNIC { get; set; }
        public DateTime? DOB { get; set; }

        public Gender Gender { get; set; }
        public virtual string PassportNo { get; set; }
        public virtual string BloodGroup { get; set; }
        public DateTime? CNICexpiryDate { get; set; }
        public DateTime? passportExpiryDate { get; set; }
        public DateTime? passportIssueDate { get; set; }

        public virtual List<PersonPhoto> personPhotos { get; set; } 

       
    


        public Person()
        {
            // do nothing
        }

        public Person(string _fname,string _lname, string _fatherName, string _cnic, DateTime _dob, Gender _gender, string _nextKin = "")
        {
            this.FName = _fname;
            this.LName = _lname;
            this.FatherName = _fatherName;
            this.CNIC = _cnic;
            this.DOB = _dob;
            this.NextKin = _nextKin;
            this.Gender = _gender;
        }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FName} {LName}";
            }
        }

    }
    public class PersonPhoto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public  byte[] EmployeePhoto { get; set; } 
        public string PhotoName { get; set; }

        public int? Person_Id { get; set; }        [ForeignKey("Person_Id")]        public Person person { get; set; } 

    }


    [Table("tabAddress")]
    public class Address
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public int? Zip { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string region { get; set; }
        public AddressTypes addressType { get; set;}
       public Address()
        { }
       public Address(string _line1, string _line2, string _state, string _country, string _city,AddressTypes _addressType ,int _zip=0)
        {
            this.Line1 = _line1;
            this.Line2 = _line2;
            this.Zip = _zip;
            this.State = _state;
            this.Country = _country;
            this.City = _city;
            this.addressType = _addressType;
        }
    }

    [Table("tabContact")]
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ContactNo { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }
        public string ContactNo3 { get; set; }
        public string SecondaryContact { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Email3 { get; set; }
        public string Website { get; set; }
        public string SMLink1 { get; set; }
        public string SMLink2 { get; set; }
        public string SMLink3 { get; set; }

        public string OfficialSkype { get; set; }
        public string OffSkypePassword { get; set; }
        public string OfficialTeams { get; set; }
        public string OffTeamsPassword { get; set; }

        public string PersonalSkype { get; set; }
        public string PersonalTeams { get; set; }

        public ContactTypes contactType { get; set; }
       public Contact()
        { }
        Contact(string _contactNo, string _fax, string _email, string _website, string _smLink1, string _smLink2, string _smLink3, string _offSkype, string _OffSkypePass, string _offTeams, string _offTeamsPass, string _personSkype, string _personTeams, ContactTypes _type )
        {
            this.ContactNo = _contactNo;
            this.Fax = _fax;
            this.Email = _email;
            this.Website = _website;
            this.SMLink1 = _smLink1;
            this.SMLink2 = _smLink2;
            this.SMLink3 = _smLink3;
            this.contactType = _type;
            this.OfficialSkype = _offSkype;
            this.OffSkypePassword = _OffSkypePass;
            this.OfficialTeams = _offTeams;
            this.OffTeamsPassword = _offTeamsPass;

            this.PersonalSkype = _personSkype;
            this.PersonalTeams = _personTeams;
        }
}
    public class Religion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ReligionName { get; set; }
        public bool IsActive { get; set; }
    }


}
