
using System.ComponentModel.DataAnnotations;


namespace ERP_BL.Enums
{

    public enum Enums
    {

        Company = 1,
        Person = 2,
        Shipping = 3,

        Billing = 4,
        Employee = 5
    }
    public enum CompanyType
    {
        [Display(Name = "Group Company")]

        GroupCompany = 1,

        [Display(Name = "Individual Company")]

        IndividualCompany = 2,

        [Display(Name = "Customer Company")]
        CustomerCompany = 3,

        [Display(Name = "Vendor Company")]
        VendorCompany = 4,

        [Display(Name = "Principal Company")]
        PrincipalCompany = 5
    }
    public enum BloodGroup

    {
        [Display(Name = "A+")]
        A_Positive,


        [Display(Name = "A-")]
        A_Negative,


        [Display(Name = "B+")]
        B_Positive,


        [Display(Name = "B-")]
        B_Negative,


        [Display(Name = "AB+")]
        AB_Positive,


        [Display(Name = "AB-")]
        AB_Negative,


        [Display(Name = "O+")]
        O_Positive,


        [Display(Name = "O-")]
        O_Negative
    }


    public enum Gender
    {
        Male,
        Female,
        Other
    }


    public enum MaritalStatus
    {
        Single,
        Married,
        Divorced,
        Widowed
    }
    public enum TransactionItemType
    {
        Undefined = 1,
        Employee = 2,
        Inquiry = 3,
        SaleOrder = 4,
    }
}

