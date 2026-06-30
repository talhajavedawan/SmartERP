using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ExchangeRates
{
    public interface IExchangeRateGroup
    {
        List<ExchangeRateGroup> GetAll();
        void Add(ExchangeRateGroup exchangeRateGroup);
        void Update(ExchangeRateGroup exchangeRateGroup);
        ExchangeRateGroup Get(int exchangeRateId);
        ExchangeRate GetRate(int exchangeRateId);
        List<ExchangeRateGroup> GetAllVoid();
        ExchangeRateGroup GetGroupByCurrenciesSER(int transactionCurrencyId, int baseCurrencyId, int targetYear);
        ExchangeRateGroup GetGroupByCurrenciesMER(int transactionCurrencyId, int baseCurrencyId, int targetYear);
        ExchangeRateGroup GetGroupByTypeTargetYear(ExchangeRateType type, int targetYear, int baseCurrency_id, int targetCurrency_id);
        List<ExchangeRateGroup> GetAllSER();
        List<ExchangeRateGroup> GetAllMER();

    }
    public class ExchangeRateGroupRepo : IExchangeRateGroup
    {
        DBContextERP context = new DBContextERP();
        public List<ExchangeRateGroup> GetAll()
        {
            return context.exchangeRateGroups
                .Where(x => x.isVoid != true).ToList();
        }

        public void Update(ExchangeRateGroup exchangeRateGroup)
        {
            var dbExchangeGroup = context.exchangeRateGroups.FirstOrDefault(x => x.Id == exchangeRateGroup.Id);
            dbExchangeGroup = exchangeRateGroup;
            context.SaveChanges();
        }
        public void Add(ExchangeRateGroup exchangeRateGroup )
        {
            context.exchangeRateGroups.Add(exchangeRateGroup);
            context.SaveChanges();
        }

        public ExchangeRateGroup Get(int exchangeRateId)
        {
            return context.exchangeRateGroups
                .FirstOrDefault(x => x.Id == exchangeRateId);
        }

        public ExchangeRate GetRate(int exchangeRateId)
        {
            return context.excRates.FirstOrDefault(x => x.Id == exchangeRateId); 
        }

        public List<ExchangeRateGroup> GetAllVoid()
        {
            return context.exchangeRateGroups
               .Where(x => x.isVoid == true).ToList();
        }

        public ExchangeRateGroup GetGroupByCurrenciesSER(int transactionCurrencyId, int baseCurrencyId, int targetYear)
        {
            return context.exchangeRateGroups
                  .FirstOrDefault(x => x.transaction_currency_Id == transactionCurrencyId && x.TargetYear == targetYear && x.base_currency_Id == baseCurrencyId && x.TargetYear == targetYear && x.ExchangeType == Enums.ExchangeRateType.SER && x.isVoid!=true);
        }
        public ExchangeRateGroup GetGroupByCurrenciesMER(int transactionCurrencyId, int baseCurrencyId, int targetYear)
        {
            return context.exchangeRateGroups
                .FirstOrDefault(x => x.transaction_currency_Id == transactionCurrencyId && x.TargetYear==targetYear && x.base_currency_Id == baseCurrencyId && x.ExchangeType == Enums.ExchangeRateType.MER && x.isVoid != true);
        }

        public ExchangeRateGroup GetGroupByTypeTargetYear(ExchangeRateType type, int targetYear, int baseCuurency_id, int transactionCurrency_id)
        {
            return context.exchangeRateGroups
              .FirstOrDefault(x => x.ExchangeType == type && x.TargetYear == targetYear && x.isVoid != true && x.base_currency_Id== baseCuurency_id && x.transaction_currency_Id==transactionCurrency_id);
        }

        public List<ExchangeRateGroup> GetAllSER()
        {
            return context.exchangeRateGroups
            .Where(x => x.isVoid != true && x.ExchangeType==ExchangeRateType.SER).ToList();
        }

        public List<ExchangeRateGroup> GetAllMER()
        {
            return context.exchangeRateGroups
           .Where(x => x.isVoid != true && x.ExchangeType == ExchangeRateType.MER).ToList();
        }
        public List<ExchangeRateGroup> GetAllMERVoid()
        {
            return context.exchangeRateGroups
               .Where(x => x.isVoid == true && x.ExchangeType==ExchangeRateType.MER).ToList();
        }
        public List<ExchangeRateGroup> GetAllSERVoid()
        {
            return context.exchangeRateGroups
               .Where(x => x.isVoid == true && x.ExchangeType == ExchangeRateType.SER).ToList();
        }
    }

}

