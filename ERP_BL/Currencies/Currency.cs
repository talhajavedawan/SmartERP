using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
   public class Currency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CurrencyName { get; set; }
        public string Symbol { get; set; }
        public string Abbrivation { get; set; }
        public string Country { get; set; }
        [InverseProperty("Base_Currency")]
        public virtual List<MarketExchangeRate> Base_CurrencyMarketExchangeRates { get; set; }
        [InverseProperty("Target_Currency")]
        public virtual List<MarketExchangeRate> Target_CurrencyMarketExchangeRates { get; set; }
        [InverseProperty("Target_Currency")]
        public virtual List<SalesExchangeRate> Target_CurrencysalesExchangeRates { get; set; }
        [InverseProperty("Base_Currency")]
        public virtual List<SalesExchangeRate> Base_CurrencysalesExchangeRates { get; set; }

        public bool isVoid { get; set; }




    }

    public interface ICurrencyRepo
    {
        void Add(Currency currency);
        void update(Currency currency);
        void update(int currencyId, Currency currency);


        List<Currency> getAll();
        List<Currency> getAll(string symbol);

        Currency get(int currencyId);
    }
    public class CurrencyRepo : ICurrencyRepo
    {
        DBContextERP context = new DBContextERP();

        public CurrencyRepo()
        {
            SystemLog.LogInfo(this.GetType(), "Instance Created");

        }
        /// <summary>
        /// add new currency object
        /// </summary>
        /// <param name="Target_Currency">Currency Class Object</param>
        /// 
        public void Add(Currency currency)
        {
            context.currencies.Add(currency);
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Added Currency with Name= " + currency.CurrencyName);

        }

        /// <summary>
        /// update currency details
        /// </summary>
        /// <param name="Target_Currency">Currecnt currency Object</param>
        /// <param name="updatedCurrency">Updated Currency Object</param>
        public void update(Currency currency)
        {
            Currency currencytoUpdate = context.currencies.FirstOrDefault(x => x.Id == currency.Id);
            currencytoUpdate=currency;
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated Currency with Name= " + currency.CurrencyName + " Id= "+currency.Id);

        }

        /// <summary>
        /// Updated currency detail against currecny ID
        /// </summary>
        /// <param name="currencyId">Currency ID as int</param>
        /// <param name="Target_Currency">Updated Currency Object</param>
        public void update(int currencyId, Currency currency)
        {
            Currency currencytoUpdate = context.currencies.FirstOrDefault(x => x.Id == currencyId);
            currencytoUpdate = currency;
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated Currency with Name= " + currency.CurrencyName + " Id= " + currencyId);

        }

        /// <summary>
        /// Get Currency Object against Currency ID
        /// </summary>
        /// <param name="currencyId">Currency ID as int</param>
        /// <returns></returns>
        public Currency get(int currencyId)
        {
            SystemLog.LogInfo(this.GetType(), "retrived Currency with Id= " + currencyId);

            return context.currencies.FirstOrDefault(x => x.Id == currencyId);

        }

        /// <summary>
        /// Get all currencies as list
        /// </summary>
        /// <returns></returns>
        public List<Currency> getAll()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of All Currencies" );

            return context.currencies.Where(x=>x.isVoid!=true).ToList();
        }


        /// <summary>
        /// Get all currencies having same symbol
        /// </summary>
        /// <param name="symbol">currency Symbol in string</param>
        /// <returns></returns>
        public List<Currency> getAll(string symbol)
        {
            SystemLog.LogInfo(this.GetType(), "Retrive List of Currencies matching with Symbol = " + symbol);

            return context.currencies.Where(x => x.Symbol == symbol).ToList();
        }
        /// <summary>
        /// add new exchangerate object
        /// </summary>
        /// <param name="exchangerate">exchangerate Class Object</param>
        /// 
        public void Add(MarketExchangeRate exchange)
        {
            context.exchangeRates.Add(exchange);
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Added Exchange Rate with Id= " + exchange.Id);

        }

        /// <summary>
        /// update exchangerate details
        /// </summary>
        /// <param name="exchangerate">exchangerate Object</param>

        public void update(MarketExchangeRate exchange)
        {
            MarketExchangeRate exchangetoUpdate = context.exchangeRates.FirstOrDefault(x => x.Id == exchange.Id);
            exchangetoUpdate = exchange;
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated ExchangeRate with  Id= " + exchange.Id);

        }

        /// <summary>
        /// Updated exchangerate detail against currecny ID
        /// </summary>
        /// <param name="exchangerateId">exchangerate ID as int</param>
        /// <param name="exchangerate">Updated exchangerate Object</param>
        public void update(int exchangerateId, MarketExchangeRate exchange)
        {
            MarketExchangeRate exchangeratetoUpdate = context.exchangeRates.FirstOrDefault(x => x.Id == exchangerateId);
            exchangeratetoUpdate = exchange;
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated Exchange Rate with Id= " + exchangerateId);

        }

        /// <summary>
        /// Get exchangerate Object against exchangerate ID
        /// </summary>
        /// <param name="exchangerateId">exchangerate ID as int</param>
        /// <returns></returns>
        public MarketExchangeRate getMarketexchangerate(int exchangerateId)
        {
            SystemLog.LogInfo(this.GetType(), "retrived exchangerate with Id= " + exchangerateId);

            return context.exchangeRates.FirstOrDefault(x => x.Id == exchangerateId);

        }
        /// <summary>
        /// Get exchangerate Object against exchangerate ID
        /// </summary>
        /// <param name="exchangerateId">exchangerate ID as int</param>
        /// <returns></returns>
        public MarketExchangeRate getMarketexchangerate(int companyId, int currencyId)
        {
            SystemLog.LogInfo(this.GetType(), "retrived exchange rate with Id= " + companyId);

            return context.exchangeRates.OrderByDescending(x => x.effectiveTo).FirstOrDefault(x => x.company_Id == companyId && x.target_currency_Id==currencyId /*&& (x.effectiveFrom<= System.DateTime.Now && System.DateTime.Now<=x.effectiveTo)*/);

        }
        /// <summary>
        /// Get all exchangerates as list
        /// </summary>
        /// <returns></returns>
        public List<MarketExchangeRate> getAllMarketExchangeRatesForToday()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of All exchangerates");

            return context.exchangeRates.OrderByDescending(x => x.effectiveTo).Where(x => (x.effectiveFrom <= System.DateTime.Now && System.DateTime.Now <= x.effectiveTo)).ToList();
        }
        /// <summary>
        /// Get all exchangerates as list
        /// </summary>
        /// <returns></returns>
        public List<MarketExchangeRate> getAllMarketExchangeRates()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of All exchangerates");

            return context.exchangeRates.ToList();
        }
        /// <summary>
        /// Get all exchangerates as list
        /// </summary>
        /// <returns></returns>
        /// <param name="companyId">company Id </param>
        public List<MarketExchangeRate> getAllExchangeRates(int companyId)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of All exchangerates by Company Id=" + companyId);

            return context.exchangeRates.Where(x => x.company_Id == companyId).ToList();
        }
        /// <summary>
        /// Get all exchangerates as list
        /// </summary>
        /// <returns></returns>
        /// <param name="userId">company Id </param>
        public List<MarketExchangeRate> getAllExchangeRatesForCurrentUser(int userId)
        {
            SystemLog.LogInfo(this.GetType(), "Retriving List of All exchangerates for User Id=" + userId);
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.exchangeRates.Where(x => companyIds.Contains((int)x.company_Id) ).ToList();
        }
        /// <summary>
        /// add new SalesExchangeRate object
        /// </summary>
        /// <param name="exchangerate">SalesExchangeRate Class Object</param>
        /// 
        public void Add(SalesExchangeRate exchange)
        {
            context.salesExchangeRates.Add(exchange);
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Added salesExchange Rate with Id= " + exchange.Id);

        }

        /// <summary>
        /// update SalesExchangeRate details
        /// </summary>
        /// <param name="exchangerate">SalesExchangeRate Object</param>

        public void update(SalesExchangeRate exchange)
        {
            SalesExchangeRate exchangetoUpdate = context.salesExchangeRates.FirstOrDefault(x => x.Id == exchange.Id);
            exchangetoUpdate = exchange;
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated salesExchangeRate with  Id= " + exchange.Id);

        }

        /// <summary>
        /// Updated SalesExchangeRate detail against currecny ID
        /// </summary>
        /// <param name="exchangerateId">SalesExchangeRate ID as int</param>
        /// <param name="exchangerate">Updated SalesExchangeRate Object</param>
        public void update(int exchangerateId, SalesExchangeRate exchange)
        {
            SalesExchangeRate exchangeratetoUpdate = context.salesExchangeRates.FirstOrDefault(x => x.Id == exchangerateId);
            exchangeratetoUpdate = exchange;
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated SalesExchangeRate with Id= " + exchangerateId);

        }

        /// <summary>
        /// Get SalesExchangeRate Object against SalesExchangeRate ID
        /// </summary>
        /// <param name="exchangerateId">SalesExchangeRate ID as int</param>
        /// <returns></returns>
        public SalesExchangeRate getsalesExchangeRate(int exchangerateId)
        {
            SystemLog.LogInfo(this.GetType(), "retrived SalesExchangeRate with Id= " + exchangerateId);

            return context.salesExchangeRates.FirstOrDefault(x => x.Id == exchangerateId);

        }
        /// <summary>
        /// Get Salesexchangerate Object against exchangerate ID
        /// </summary>
        /// <param name="exchangerateId">Sales exchangerate ID as int</param>
        /// <returns></returns>
        public SalesExchangeRate getsalesexchangerate(int companyId, int? currencyId)
        {
            SystemLog.LogInfo(this.GetType(), "retrived Sales exchange rate with Id= " + companyId);

            return context.salesExchangeRates.OrderByDescending(x => x.AddedOn).FirstOrDefault(x => x.company_Id == companyId && x.target_currency_Id == currencyId /*&& x.targetYear==System.DateTime.Now.Year*/);

        }
        /// <summary>
        /// Get all SalesExchangeRates as list
        /// </summary>
        /// <returns></returns>
        public List<SalesExchangeRate> getAllsalesExchangeRates()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of All SalesExchangeRates");

            return context.salesExchangeRates.ToList();
        }
        /// <summary>
        /// Get all SalesExchangeRates as list
        /// </summary>
        /// <returns></returns>
        /// <param name="companyId">company Id </param>
        public List<SalesExchangeRate> getAllsalesExchangeRates(int companyId)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of All SalesExchangeRate by Company Id=" + companyId);

            return context.salesExchangeRates.Where(x=>x.company_Id==companyId).ToList();
        }
    }
}
