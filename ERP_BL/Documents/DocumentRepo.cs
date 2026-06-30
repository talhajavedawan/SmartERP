using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Documents
{
    public class DocumentRepo
    {
        DBContextERP context = new DBContextERP();

        public ERP_BL.Databases.Employee GetEmployeeForDocuments(int empID)
        {
            return context.Employees
                .FirstOrDefault(x => x.EmpId == empID);
        }

        /// <summary>
        /// Get last Payment
        /// </summary>
        /// <returns></returns>
        public int GetLastTransactionId()
        {
            var document = context.documents.OrderByDescending(q => q.Id).FirstOrDefault();
            if (document == null)
                return 0;
            return document.transactionGroupId;
        }

        /// <summary>
        /// Get last Payment
        /// </summary>
        /// <returns></returns>
        public int GetLargestTransactionId()
        {
            var document = context.documents.OrderByDescending(q => q.transactionGroupId).FirstOrDefault();
            if (document == null)
                return 0;
            return document.transactionGroupId;
        }

        /// <summary>
        /// Get All Documents by Group Id
        /// </summary>
        /// <returns></returns>
        public List<Document> GetDocumentsByGroupId(int transactionGroupId)
        {
            return
            context.documents
            .Where(x => x.transactionGroupId == transactionGroupId)
            .ToList();
        }

       

        /// <summary>
        /// Add Document
        /// </summary>
        /// <param name="document"></param>
        public void AddDocument(List<Document> documents)
        {
            foreach (var _document in documents)
            {
                context.documents.Add(_document);
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Update Document
        /// </summary>
        /// <param name="document"></param>
        public void UpdateDocument(List<Document> documents)
        {
            foreach(var _document in documents)
            {
                if (_document.Id == 0) 
                {
                    context.documents.Add(_document);
                }
                else
                {
                    var document = context.documents.FirstOrDefault(x => x.Id == _document.Id);
                    document = _document;
                }                
            }
            context.SaveChanges();
        }

        public void SetDocumentsToVoid(int groupId, bool isVoid)
        {
            List<Document> documents = GetDocumentsByGroupId(groupId);

            foreach (Document _document in documents)
            {
                var bill = context.documents.FirstOrDefault(x => x.Id == _document.Id);
                bill.isVoid = isVoid;
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Get Document
        /// </summary>
        /// <param name="documentId"></param>
        public Document GetDocument(int documentId)
        {
            return context.documents.FirstOrDefault(x => x.Id == documentId);
        }

        public List<DocumentStatus> GetAllCloseDocumentStatus()
        {
            var statusList = context.documentStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;

        }



        /// <summary>
        /// Add New Document Status
        /// </summary>
        /// <param name="documentStatus"></param>
        public void AddDocumentStatus(DocumentStatus documentStatus)
        {
            context.documentStatuses.Add(documentStatus);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Document Status
        /// </summary>
        /// <param name="billStatus"></param>
        public void UpdateDocumentStatus(DocumentStatus documentStatus)
        {
            DocumentStatus _documentStatus = context.documentStatuses.FirstOrDefault(x => x.Id == documentStatus.Id);
            _documentStatus.Status = documentStatus.Status;
            _documentStatus.isActive = documentStatus.isActive;
            _documentStatus.forecolor = documentStatus.forecolor;
            _documentStatus.backcolor = documentStatus.backcolor;
            context.SaveChanges();
        }

        /// <summary>
        /// Get A bill Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public DocumentStatus GetDocumentStatus(int statusId)
        {
            return context.documentStatuses
                //.Include("Bills")
                .FirstOrDefault(x => x.Id == statusId);
        }


        /// <summary>
        /// Get All Document Statuses
        /// </summary>
        /// <returns></returns>
        public List<DocumentStatus> GetAllDocumentStatuses()
        {
            return context.documentStatuses
                //.Include("Bills")
                .ToList();
        }

        /// <summary>
        /// Get All Document Statuses
        /// </summary>
        /// <returns></returns>
        public List<DocumentStatus> GetAllOpenDocumentStatuses()
        {
            return context.documentStatuses
                //.Include("Bills")
                .Where(x => x.isActive == true)
                .ToList();
        }

        /// <summary>
        /// Get All Loans and Advances
        /// </summary>
        /// <returns></returns>
        public List<Document> GetAllDocuments(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
            {
                companyIds.Add(comp.Id);

            }
            return context.documents

           .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains(x.company.Id) && x.isVoid != true)
           .ToList();


        }


        /// <summary>
        /// Get All Documents
        /// </summary>
        /// <returns></returns>
        public List<Document> GetAllActiveDocuments(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.documents

           .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains(x.company.Id) && x.Status.isActive == true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
           .ToList();


        }


        /// <summary>
        /// Get All Documents
        /// </summary>
        /// <returns></returns>
        public List<Document> GetAllClosedDocuments(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.documents

           .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains(x.company.Id) && x.Status.isActive == true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
           .ToList();


        }


        /// <summary>
        /// Get All Documents
        /// </summary>
        /// <returns></returns>
        public List<Document> GetAllDocumentsByStatusId(int uid, int statusId)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.documents

           .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains(x.company.Id) && x.Status.Id == statusId && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
           .ToList();
        }


        /// <summary>
        /// Get All Pending For Approval Documents
        /// </summary>
        /// <returns></returns>
        public List<Document> GetAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.documents

        .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains(x.company.Id) && x.isApproved == false && x.isVoid != true)
         .ToList();
        }

        /// <summary>
        /// Admin Bills Pending for re-approval
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<Document> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.documents

         .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains(x.company.Id) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
             .ToList();

        }


        /// <summary>
        /// Get all pending for closing Document by Departmental
        /// </summary>
        /// <returns></returns>
        public List<Document> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.documents
            .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains(x.company.Id) && x.PendingForClosing == true && x.isVoid != true)
            .ToList();
        }


        /// <summary>
        /// Get all Void Document own.
        /// </summary>
        /// <returns></returns>
        public List<Document> getVoidRegisterOwn(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.documents
         .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains((int)x.company.Id) && x.isVoid == true)
             .ToList();
        }


        /// <summary>
        /// Get Count pending documents by Departments
        /// <returns></returns>
        public int getAllPendingForApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.documents

                .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains((int)x.company_Id) && x.isApproved == false && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count pending Documents by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.documents
                .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains((int)x.company_Id) && (x.creator.employee.SupervisorId == user.employee.EmpId || x.creator.employee.EmpId == user.employee.EmpId || x.creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.creator.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Own Count pending Documents
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.documents

                .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains((int)x.company_Id) && (x.creator.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
        }



        /// <summary>
        /// Get Count pending Documents by Departments
        /// <returns></returns>
        public int getAllPendingForReApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.documents

                .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains((int)x.company_Id) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending Documents by user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForReApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.documents

                .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains((int)x.company_Id) && (x.creator.employee.SupervisorId == user.employee.EmpId || x.creator.employee.EmpId == user.employee.EmpId || x.creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.creator.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Own Count pending Documents user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForReApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.documents

                .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains((int)x.company_Id) && (x.creator.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
        }

        /// <summary>
        /// Get all Void Documents for user count. 
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.documents
                .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains((int)x.company_Id) && x.isVoid == true)
                .Count();
        }


        /// <summary>
        /// Get all Void Documents.
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.documents
    .Where(x => x.isVoid == true)
    .Count();
        }


        /// <summary>
        /// Get all Documents for user count. 
        /// </summary>
        /// <returns></returns>
        public int getBankTransferRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.documents
                .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains((int)x.company_Id) && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get all Document  own count. 
        /// </summary>
        /// <returns></returns>
        public int getBankTransferRegisterCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.documents
                .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains((int)x.company_Id) && (x.creator.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .Count();
        }

        /// Get Count Document.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.documents

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get all Documents .
        /// </summary>
        /// <returns></returns>
        public int getInterBankTransferAdministratorCount()
        {
            return context.documents
                .Where(x => x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get all pending document by Departmental Count
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.documents

                .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains((int)x.company_Id) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending for closing documents by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.documents

                .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains((int)x.company_Id) && (x.creator.employee.SupervisorId == user.employee.EmpId || x.creator.employee.EmpId == user.employee.EmpId || x.creator.employee.Supervisor.SupervisorId == user.employee.EmpId || x.creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending for closing documents own <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.documents

                .Where(x => (deptIds.Contains(x.department.Id) || x.creator_Id == uid) && companyIds.Contains((int)x.company_Id) && (x.creator.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count documents
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.documents

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }



        /// <summary>
        /// Add Document Type
        /// </summary>
        /// <param name="documentType"></param>
        public void AddDocumentType(DocumentType documentType)
        {
            context.documentTypes.Add(documentType);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Document Type
        /// </summary>
        /// <param name="documentType"></param>
        public void UpdateDocumentType(DocumentType documentType)
        {
            var _documentType = context.documentTypes.FirstOrDefault(x => x.Id == documentType.Id);
            _documentType = documentType;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Document Type
        /// </summary>
        /// <param name="documentTypeId"></param>
        public DocumentType GetDocumentType(int documentTypeId)
        {
            return context.documentTypes.FirstOrDefault(x => x.Id == documentTypeId);
        }

        /// <summary>
        /// Get All Document Types
        /// </summary>
        /// <param name="documentTypeId"></param>
        public List<DocumentType> GetAllDocumentType()
        {
            return context.documentTypes.ToList();
        }


        /// <summary>
        /// Add Document Template
        /// </summary>
        /// <param name="documentTemplate"></param>
        public void AddDocumentTemplate(DocumentTemplate documentTemplate)
        {
            context.documentTemplates.Add(documentTemplate);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Document Template
        /// </summary>
        /// <param name="documentTemplate"></param>
        public void UpdateDocumentTemplate(DocumentTemplate documentTemplate)
        {
            var _documentTemplate = context.documentTemplates.FirstOrDefault(x => x.Id == documentTemplate.Id);
            _documentTemplate = documentTemplate;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Document Template
        /// </summary>
        /// <param name="documentTemplateId"></param>
        public DocumentTemplate GetDocumentTemplate(int documentTemplateId)
        {
            return context.documentTemplates.FirstOrDefault(x => x.Id == documentTemplateId);
        }

        /// <summary>
        /// Get All Document Types
        /// </summary>
        /// <param name="documentTemplateId"></param>
        public List<DocumentTemplate> GetAllDocumentTemplate()
        {
            return context.documentTemplates.ToList();
        }


        /// <summary>
        /// Add Document Authority
        /// </summary>
        /// <param name="documentAuthority"></param>
        public void AddDocumentAuthority(DocumentAuthority documentAuthority)
        {
            context.documentAuthorities.Add(documentAuthority);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Document Authority
        /// </summary>
        /// <param name="documentAuthority"></param>
        public void UpdateDocumentAuthority(DocumentAuthority documentAuthority)
        {
            var _documentAuthority = context.documentAuthorities.FirstOrDefault(x => x.Id == documentAuthority.Id);
            _documentAuthority = documentAuthority;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Document Authority
        /// </summary>
        /// <param name="documentAuthorityId"></param>
        public DocumentAuthority GetDocumentAuthority(int documentAuthorityId)
        {
            return context.documentAuthorities.FirstOrDefault(x => x.Id == documentAuthorityId);
        }

        /// <summary>
        /// Get All Document Authorities
        /// </summary>
        public List<DocumentAuthority> GetAllDocumentAuthority()
        {
            return context.documentAuthorities.ToList();
        }


    }
}
