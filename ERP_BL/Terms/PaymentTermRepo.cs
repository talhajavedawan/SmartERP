using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class PaymentTermRepo
    {
        DBContextERP context = new DBContextERP();

        /// <summary>
        /// Add new PaymentTerm  
        /// </summary>
        /// <param name="paymentTerm">PaymentTerm  Object</param>
        public void Add(PaymentTerm paymentTerm)
        {
            context.paymentTerms.Add(paymentTerm);
            context.SaveChanges();
        }





        /// <summary>
        /// return all PaymentTerm  list.
        /// </summary>
        /// <returns></returns>
        public List<PaymentTerm> getAll()
        {
            return context.paymentTerms
                .ToList();
        }

        /// <summary>
        /// return all PaymentTerm  list for SO.
        /// </summary>
        /// <returns></returns>
        public List<PaymentTerm> getAllForSO()
        {
            return context.paymentTerms
                .Where(x=>x.isSaleOrderType == true && x.isActive == true)
                .ToList();
        }

        /// <summary>
        /// return all PaymentTerm  list for PO.
        /// </summary>
        /// <returns></returns>
        public List<PaymentTerm> getAllForPO()
        {
            return context.paymentTerms
                .Where(x => x.isPurchaseOrderType == true && x.isActive == true)
                .ToList();
        }

        /// <summary>
        /// return all PaymentTerm  list for PO.
        /// </summary>
        /// <returns></returns>
        public List<PaymentTerm> getAllForSI()
        {
            return context.paymentTerms
                .Where(x => x.isPurchaseOrderType == true && x.isActive == true)
                .ToList();
        }

        /// <summary>
        /// return all PaymentTerm  list for PO.
        /// </summary>
        /// <returns></returns>
        public List<PaymentTerm> getAllForPayments()
        {
            return context.paymentTerms
                .Where(x => x.isPaymentType == true && x.isActive == true)
                .ToList();
        }

        /// <summary>
        /// return all PaymentTerm  list for PO.
        /// </summary>
        /// <returns></returns>
        public List<PaymentTerm> getAllForVendorBills()
        {
            return context.paymentTerms
                .Where(x => x.isVnedorBillType == true && x.isActive == true)
                .ToList();
        }

        /// <summary>
        /// return all PaymentTerm  list for Offer.
        /// </summary>
        /// <returns></returns>
        public List<PaymentTerm> getAllForOffers()
        {
            return context.paymentTerms
                .Where(x => x.isOfferType == true && x.isActive == true)
                .ToList();
        }

        /// <summary>
        /// get paymentTerm matching to ID
        /// </summary>
        /// <param name="paymentTermCompID">PaymentTerm ID</param>
        /// <returns></returns>
        public PaymentTerm get(int paymentTermID)
        {
            return context.paymentTerms.FirstOrDefault(x => x.Id == paymentTermID);
        }

        public Warranty getWarrenty(int warrentyID)
        {
            return context.Warranties.FirstOrDefault(x => x.Id == warrentyID);
        }  
        /// <summary>
        /// update PaymentTerm  object details
        /// </summary>
        /// <param name="paymentTerm">PaymentTerm  Object</param>
        public void Update(PaymentTerm paymentTerm)
        {
            PaymentTerm compToUpdate = context.paymentTerms.FirstOrDefault(x => x.Id == paymentTerm.Id);
            compToUpdate = paymentTerm;
            context.SaveChanges();
        }
        /// <summary>
        /// Get All Active PaymentTerms
        /// </summary>
        /// <returns></returns>
        public List<PaymentTerm> getActivePaymentTerm()
        {
            return context.paymentTerms.Where(x => x.isActive == true).ToList();

        }
        /// <summary>
        /// Get All inActive PaymentTerms
        /// </summary>
        /// <returns></returns>
        public List<PaymentTerm> getinActivePaymentTerms()
        {
            return context.paymentTerms.Where(x => x.isActive == false).ToList();

        }
    }
}
