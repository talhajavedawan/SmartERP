using ERP_BL.Databases;
using ERP_BL.Procurements.StatusClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Documents
{
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? CreationDate { get; set; }
        public int? documentType_Id { get; set; }
        [ForeignKey("documentType_Id")]
        public virtual DocumentType documentType { get; set; }

        public int? documentTemplate_Id { get; set; }
        [ForeignKey("documentTemplate_Id")]
        public virtual DocumentTemplate documentTemplate { get; set; }

        public int? documentAuthority_Id { get; set; }
        [ForeignKey("documentAuthority_Id")]
        public virtual DocumentAuthority documentAuthority { get; set; }

        public int? dept_Id { get; set; }
        [ForeignKey("dept_Id")]
        [InverseProperty("DepartmentDocuments")]
        public virtual Department department { get; set; }

        public int? company_Id { get; set; }
        [ForeignKey("company_Id")]
        [InverseProperty("CompanyDocuments")]
        public virtual Company company { get; set; }

        public int transactionGroupId { get; set; }
        public string SystemRefNo { get; set; }
        public string DocumentNo { get; set; }

        public int? country_Id { get; set; }
        [ForeignKey("country_Id")]
        public virtual Countryy.Country country { get; set; }

        public int? creator_Id { get; set; }
        [ForeignKey("creator_Id")]
        public virtual ERP_BL.Databases.User creator { get; set; }

        public int? status_Id { get; set; }
        [ForeignKey("status_Id")]
        public virtual DocumentStatus Status { get; set; }

        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual StatusClass StatusClass { get; set; }

        public int? employee_Id { get; set; }
        [ForeignKey("employee_Id")]
        public virtual ERP_BL.Databases.Employee employee { get; set; }

        public string Employee { get; set; }

        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool isOpen { get; set; }

        public string stage { get; set; }
        public bool isVoid { get; set; }
        public bool? isReviewed { get; set; }
        public bool? needReview { get; set; }
        public bool? PendingForClosing { get; set; }
        public bool? PendingForReApproval { get; set; }
        public bool? isApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? ReApprovalDate { get; set; }
        public bool? isReApproved { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }

        //public event PropertyChangedEventHandler PropertyChanged;
        //public object GetPropertyValue(string propertyName)
        //{
        //    return this.GetType().GetProperty(propertyName).GetValue(this, null);
        //}
    }

    public class DocumentStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<Document> Documents { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("documentStatuses")]
        public virtual List<StatusClass> documentStatusSubClasses { get; set; }
    }

    public class DocumentType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TypeName { get; set; }


        [InverseProperty("DocumentTypes")]
        public virtual List<DocumentTemplate> DocumentTemplates { get; set; }

        [InverseProperty("DocumentTypes")]
        public virtual List<Company> companies { get; set; }

        [InverseProperty("DocumentTypes")]
        public virtual List<Department> departments { get; set; }
    }

    public class DocumentAuthority
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string AuthorityName { get; set; }

        public int? documentTemplate_Id { get; set; }
        [ForeignKey("documentTemplate_Id")]
        public virtual DocumentTemplate documentTemplate { get; set; }
    }

    public class DocumentTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TemplateName { get; set; }

        [InverseProperty("DocumentTemplates")]
        public virtual List<DocumentType> DocumentTypes { get; set; }

        [InverseProperty("DocumentTemplates")]
        public virtual List<Company> companies { get; set; }

        [InverseProperty("DocumentTemplates")]
        public virtual List<Department> departments { get; set; }
    }
}
