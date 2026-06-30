using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class AttachmentsRepo
    {
        DBContextERP context = new DBContextERP();

        public AttachmentsRepo()
        {
            SystemLog.LogInfo(this.GetType(), "Instance Created");

        }
        /// <summary>
        /// Add new Attachment 
        /// </summary>
        /// <param name="attachment">Attachment  Object</param>
        public void Add(Attachment attachment)
        {
            context.Attachments.Add(attachment);
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Added Attachment with Name= " + attachment.fileName + " Id= " + attachment.Id);

        }


        /// <summary>
        /// return all Attachment Company list.
        /// </summary>
        /// <returns></returns>
        public List<Attachment> getAll()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of All Attachment");
            return context.Attachments
                
                .ToList();
        }

        /// <summary>
        /// get Attachment matching to ID
        /// </summary>
        /// <param name="attachmentID">Attachment ID</param>
        /// <returns></returns>
        public Attachment get(int attachmentID)
        {
            SystemLog.LogInfo(this.GetType(), "Retrive Attachment with  Id= " + attachmentID);
            return context.Attachments
                
                .FirstOrDefault(x => x.Id == attachmentID);
        }
        /// <summary>
        /// get Attachment matching to userID
        /// </summary>
        /// <param name="userID">userID</param>
        /// <returns></returns>
        public List<Attachment> getAllForUser(int userId)
        {
            SystemLog.LogInfo(this.GetType(), "Retrive Attachment for  userId= " + userId);
            return context.Attachments
                
                .Where(x => x.Id == userId).ToList();
        }


        /// <summary>
        /// update Attachment  object details
        /// </summary>
        /// <param name="attachment">Attachment Object</param>
        public void Update(Attachment attachment)
        {
            Attachment attachmentToUpdate = context.Attachments.FirstOrDefault(x => x.Id == attachment.Id);
            attachmentToUpdate = attachment;
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated Attachment with title= " + attachment.fileName + " Id= " + attachment.Id);
        }

        /// <summary>
        /// update Attachment  object details
        /// </summary>
        /// <param name="attachment">Attachment Object</param>
        public void MarkasRead(int id)
        {
            Attachment attachmentToUpdate = context.Attachments.FirstOrDefault(x => x.Id == id);
            if (attachmentToUpdate != null)
            {
                attachmentToUpdate.lastOpendate = System.DateTime.Now;
                
                context.SaveChanges();
                SystemLog.LogInfo(this.GetType(), "Updated Attachment with title= " + attachmentToUpdate.fileName + " Id= " + attachmentToUpdate.Id);
            }
        }
        /// <summary>
        /// Add new attachment for transaction
        /// </summary>
        /// <param name="attachment">attachment  Object for Inquiry</param>
        public void Add(string name, int Transactionid, ERP_BL.Enums.TransactionItemType Transactiontype,string description,int userid,string comment,string localaddress, string serveraddress,int CategoryId)
        {
            Attachment attachment = new Attachment();
            if (SystemLog.CurrentUserId != 0)
            {
                attachment.userId =userid;
                attachment.transactionId = Transactionid;
                attachment.transactionType = Transactiontype;
                attachment.additionDate = System.DateTime.Now;
                attachment.lastOpendate= System.DateTime.Now;
                attachment.currentStatus = Enums.UploadFlag.Uploaded;
                attachment.comment = comment;
                attachment.fileName = name;
                attachment.fileLocalAdress = localaddress;
                attachment.fileServerAdress = serveraddress;
                attachment.categoryId = CategoryId;
                context.Attachments.Add(attachment);

                context.SaveChanges();
                SystemLog.LogInfo(this.GetType(), "Added attachment  Id= " + attachment.Id);
            }
        }

        /// <summary>
        /// Get List of Attachment for user
        /// </summary>
        /// 
        public List<Attachment> getUsersAttachmentOrderAsc(int UserId)
        {
            return context.Attachments.OrderBy(x => x.additionDate).Where(x => x.userId == UserId).ToList();

        }
        /// <summary>
        /// Get List of Attachment for user

        /// </summary>
        /// 
        public List<Attachment> getUsersAttachmentOrderDsc(int UserId)
        {
            return context.Attachments.OrderByDescending(x => x.additionDate).Where(x => x.userId == UserId ).ToList();

        }
        /// <summary>
        /// Get List of Attachment for cat

        /// </summary>
        /// 
        public List<Attachment> getAttachmentOrderDsc(int catId,int Transactionid,ERP_BL.Enums.TransactionItemType type)
        {
            return context.Attachments.OrderByDescending(x => x.additionDate).Where(x => x.categoryId ==catId && x.transactionId==Transactionid && x.transactionType==type).ToList();

        }
        /// <summary>
        /// Get List of Attachment by transaction Id

        /// </summary>
        /// 
        public List<Attachment> getAttachmentOrderDsc( int Transactionid, ERP_BL.Enums.TransactionItemType type)
        {
            return context.Attachments.OrderByDescending(x => x.additionDate).Where(x =>  x.transactionId == Transactionid && x.transactionType == type).ToList();

        }
        /// <summary>
        /// Get list of all Attachment Categories in DB
        /// </summary>
        /// <returns>List of AttachmentCategorys Objects</returns>
        public List<AttachmentCategory> getallAttachmentCategories()
        {
            return context.AttachmentCategories.ToList();

        }
        /// <summary>
        /// Get All Active CommentCategory
        /// </summary>
        /// <returns></returns>
        public List<AttachmentCategory> getCurrentTransactionAttachmentCategories(TransactionItemType type)
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.TransactionTypes.FirstOrDefault(y => y.TransactionType == type) != null).ToList();

        }
        /// <summary>
        /// Get All Active attachmentCategories
        /// </summary>
        /// <returns></returns>
        public List<AttachmentCategory> getActiveAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true).ToList();

        }

        /// <summary>
        /// Get All inActive attachmentCategories
        /// </summary>
        /// <returns></returns>
        public List<AttachmentCategory> getinActiveAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == false).ToList();

        }
        /// <summary>
        /// Add new AttachmentCategory
        /// </summary>
        /// <param name="attachmentCategory">AttachmentCategory Object</param>
        public void AddAttachmentCategory(AttachmentCategory attachmentCategory)
        {
            context.AttachmentCategories.Add(attachmentCategory);
            context.SaveChanges();
        }
        /// <summary>
        /// get Product nature by ID
        /// </summary>
        /// <param name="Natureid">Product nature ID</param>
        /// <returns></returns>
        public AttachmentCategory getAttachmentCategory(int categoryId)
        {
            return context.AttachmentCategories
                .FirstOrDefault(x => x.Id == categoryId);
        }
        /// <summarAttachmentCategory
        /// </summary>
        /// <param name="product">AttachmentCategory  Object</param>
        public void UpdateAttachmentCategory(AttachmentCategory attachmentCategory)
        {
            AttachmentCategory prod = context.AttachmentCategories.FirstOrDefault(x => x.Id == attachmentCategory.Id);
            prod = attachmentCategory;
            context.SaveChanges();
        }

        public List<AttachmentCategory> getActiveDocumentAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.Document == TransactionItemType.Document).ToList();

        }

        public List<AttachmentCategory> getActiveSOAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.SO==TransactionItemType.Sale_Order) .ToList();

        }
        public List<AttachmentCategory> getActiveRentalContractAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.RentalContract == TransactionItemType.RentalContract).ToList();

        }
        public List<AttachmentCategory> getActiveRentalOrderAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.RentalOrder == TransactionItemType.RentalOrder).ToList();

        }
        public List<AttachmentCategory> getActiveAssetAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.AssetRental == TransactionItemType.AssetRental).ToList();

        }
        public List<AttachmentCategory> getActiveTenantAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.TenantRental == TransactionItemType.TenantRental).ToList();

        }
        public List<AttachmentCategory> getActiveRentalInvoiceAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.RentalInvoice == TransactionItemType.RentalInvoice).ToList();

        }
        public List<AttachmentCategory> getActiveSTLAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.STL==TransactionItemType.STL) .ToList();

        }
        public List<AttachmentCategory> getActiveSIAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.SI==TransactionItemType.Sale_Invoice) .ToList();

        }
        public List<AttachmentCategory> getActivePOAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.PO == TransactionItemType.Purchase_Order).ToList();

        }
        public List<AttachmentCategory> getActivePIAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.PI == TransactionItemType.Purchase_Invoice).ToList();

        }
        public List<AttachmentCategory> getActiveSRAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.SR == TransactionItemType.Sale_Receipt).ToList();

        }
        public List<AttachmentCategory> getActiveVBAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.VBill == TransactionItemType.Bill).ToList();

        }
        public List<AttachmentCategory> getActiveTaskAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.Tasks == TransactionItemType.Tasks).ToList();

        }
        public List<AttachmentCategory> getActiveMemoAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.Memo == TransactionItemType.Memo).ToList();

        }
        public List<AttachmentCategory> getActivePaymentAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.Payment == TransactionItemType.Payments).ToList();

        }
        public List<AttachmentCategory> getActiveRewardAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.TargetReward == TransactionItemType.TargetReward).ToList();

        }
        public List<AttachmentCategory> getActiveABAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.ABill == TransactionItemType.Admin_Bill).ToList();

        }
        public List<AttachmentCategory> getActiveVehicleExpensesAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.VehicleExpenses == TransactionItemType.VehicleExpenses).ToList();

        }
        public List<AttachmentCategory> getActiveIBTAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.IBT == TransactionItemType.InterBank_Transfer).ToList();

        }
        public List<AttachmentCategory> getActiveOfferAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.Offer == TransactionItemType.Offer).ToList();

        } 
        public List<AttachmentCategory> getActiveModuleContractAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.Offer == TransactionItemType.ModuleContract).ToList();

        }
        public List<AttachmentCategory> getActiveModuleContractrAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.ModuleContract == TransactionItemType.ModuleContract).ToList();

        }
        public List<AttachmentCategory> getActiveInquiryAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.Inquiry == TransactionItemType.Inquiry).ToList();

        }
        public List<AttachmentCategory> getActiveIBCTAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.ICBT == TransactionItemType.InterCompanyBank_Transfer).ToList();

        }

        public List<AttachmentCategory> getActiveLAAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.LoansAdvances == TransactionItemType.LoansAdvances).ToList();

        }


        public List<AttachmentCategory> getActiveTravelingRecordAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.TravelingRecord == TransactionItemType.TravelingRecord).ToList();

        }

        public List<AttachmentCategory> getActiveProcurementProductsAttachmentCategories()
        {
            return context.AttachmentCategories.Where(x => x.isActive == true && x.ProcurementProducts == TransactionItemType.ProcurementProducts).ToList();

        }
    }
}
