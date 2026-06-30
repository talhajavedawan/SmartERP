using ERP_BL.Databases;
using ERP_BL.Procurements.InterBankTransfers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Tax
{
    public class TaxRepo
    {
        DBContextERP context = new DBContextERP();


        /// <summary>
        /// Add new Tax Type
        /// </summary>
        /// <param name="taxType"></param>
        public void addTaxType(TaxType taxType)
        {
            if (taxType == null)
                throw new NullReferenceException("Object cannot be null");

            context.taxTypes.Add(taxType);
            context.SaveChanges();
        }
        public void addInterestType(STLInterestType interestType)
        {
            if (interestType == null)
                throw new NullReferenceException("Object cannot be null");

            context.interestTypes.Add(interestType);
            context.SaveChanges();
        }
        public void addCashMarginType(MarginPercentageType marginPercType)
        {
            if (marginPercType == null)
                throw new NullReferenceException("Object cannot be null");

            context.marginPercentageTypes.Add(marginPercType);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Tax Type
        /// </summary>
        /// <param name="taxType"></param>
        public void updateTaxType(TaxType taxType)
        {
            if (taxType == null)
                throw new NullReferenceException("Object cannot be null");

            TaxType _taxType = context.taxTypes.FirstOrDefault(x=>x.Id == taxType.Id);
            _taxType = taxType;

            context.SaveChanges();
        }
        public void updateInterestType(STLInterestType interestType)
        {
            if (interestType == null)
                throw new NullReferenceException("Object cannot be null");
            STLInterestType _interestType = context.interestTypes.FirstOrDefault(x => x.Id == interestType.Id);
            _interestType = interestType;
            context.SaveChanges();
        }
        public void updateMarginPercType(MarginPercentageType marginPercType)
        {
            if (marginPercType == null)
                throw new NullReferenceException("Object cannot be null");
            MarginPercentageType _marginPercType = context.marginPercentageTypes.FirstOrDefault(x => x.Id == marginPercType.Id);
            _marginPercType = marginPercType;
            context.SaveChanges();
        }

        /// <summary>
        /// Get one Tax Type Using Tax type Id
        /// </summary>
        /// <param name="taxTypeId"></param>
        /// <returns></returns>
        public TaxType getTaxType(int taxTypeId)
        {
            if (taxTypeId == 0)
                throw new NullReferenceException("Object cannot be null");

            return context.taxTypes
                .FirstOrDefault(x=>x.Id == taxTypeId);
        }

        public STLInterestType getInterestType(int interestTypeId)
        {
            if (interestTypeId == 0)
                throw new NullReferenceException("Object cannot be null");

            return context.interestTypes
                .FirstOrDefault(x => x.Id == interestTypeId);
        }
        public MarginPercentageType getMarginPercType(int marginPercTypeId)
        {
            if (marginPercTypeId == 0)
                throw new NullReferenceException("Object cannot be null");

            return context.marginPercentageTypes
                .FirstOrDefault(x => x.Id == marginPercTypeId);
        }

        /// <summary>
        /// Get All Tax Types
        /// </summary>
        /// <returns></returns>
        public List<TaxType> getAllTaxType()
        {
            return context.taxTypes
                .ToList();
        }
        public List<STLInterestType> getAllInterestType()
        {
            return context.interestTypes
                .ToList();
        }
        public List<MarginPercentageType> getAllMarginPercType()
        {
            return context.marginPercentageTypes
                .ToList();
        }


        /// <summary>
        /// Add new Tax Name
        /// </summary>
        /// <param name="taxType"></param>
        public void addTax(TaxName taxName)
        {
            if (taxName == null)
                throw new NullReferenceException("Object cannot be null");

            context.taxNames.Add(taxName);
            context.SaveChanges();
        }
        public void addInterest(STLInterest interestName)
        {
            if (interestName == null)
                throw new NullReferenceException("Object cannot be null");
            context.interests.Add(interestName);
            context.SaveChanges();
        }
        public void addMarginPercentage(MarginPercentage marginPercentageName)
        {
            if (marginPercentageName == null)
                throw new NullReferenceException("Object cannot be null");
            context.marginPercentages.Add(marginPercentageName);
            context.SaveChanges();
        }

        /// <summary>
        /// Add new Tax Name
        /// </summary>
        /// <param name="taxType"></param>
        public void updateTax(TaxName taxName)
        {
            if (taxName == null)
                throw new NullReferenceException("Object cannot be null");

            TaxName _taxName = context.taxNames.FirstOrDefault(x=>x.Id == taxName.Id);
            _taxName = taxName;

            context.SaveChanges();
        }
        public void updateInterest(STLInterest interestName)
        {
            if (interestName == null)
                throw new NullReferenceException("Object cannot be null");

            STLInterest _InterestName = context.interests.FirstOrDefault(x => x.Id == interestName.Id);
            _InterestName = interestName;

            context.SaveChanges();
        }
        public void updateMarginPerc(MarginPercentage marginName)
        {
            if (marginName == null)
                throw new NullReferenceException("Object cannot be null");
            MarginPercentage _MarginName = context.marginPercentages.FirstOrDefault(x => x.Id == marginName.Id);
            _MarginName = marginName;
            context.SaveChanges();
        }


        /// <summary>
        /// Get one Tax Using Tax Id
        /// </summary>
        /// <param name="taxTypeId"></param>
        /// <returns></returns>
        public TaxName getTaxName(int taxId)
        {
            if (taxId == 0)
                throw new NullReferenceException("Object cannot be null");

            return context.taxNames
               
                .FirstOrDefault(x => x.Id == taxId);
        }
        public STLInterest getInterestName(int taxId)
        {
            if (taxId == 0)
                throw new NullReferenceException("Object cannot be null");

            return context.interests

                .FirstOrDefault(x => x.Id == taxId);
        }
        public MarginPercentage getMarginName(int taxId)
        {
            if (taxId == 0)
                throw new NullReferenceException("Object cannot be null");

            return context.marginPercentages

                .FirstOrDefault(x => x.Id == taxId);
        }

        /// <summary>
        /// Get All Taxes
        /// </summary>
        /// <returns></returns>

        public List<TaxName> getAllTaxes()
        {
            return context.taxNames
               
                .ToList();
        }
        public List<STLInterest> getAllInterests()
        {
            return context.interests

                .ToList();
        }
        public List<MarginPercentage> getAllMarginPerc()
        {
            return context.marginPercentages

                .ToList();
        }

        /// <summary>
        /// Get All Taxes
        /// </summary>
        /// <returns></returns>

        public List<TaxName> getAllTaxesByTypeId(int typeId)
        {
            return context.taxNames
             
                .Where(x=>x.taxTypeId == typeId)
                .ToList();
        }

        public TaxName getTaxtById(int id )
        {
           return context.taxNames
            
                .FirstOrDefault(x => x.Id == id);
        }
        public TaxName getTaxtByName(string name)
        {
            return context.taxNames

                 .FirstOrDefault(x => x.Name == name);
        }
        public List<TaxName> getAllAdjustedTaxes()
        {
            return context.taxNames
           
                .Where(x=>x.isAdjusted==true)
                .ToList();
        }
        public List<TaxName> getAllUnAdjustedTaxes()
        {
            return context.taxNames
              
                .Where(x => x.isAdjusted == false)
                .ToList();
        }
    }
}
