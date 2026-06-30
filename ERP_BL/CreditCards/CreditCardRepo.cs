using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.CreditCards
{
    public class CreditCardRepo
    {
        DBContextERP context = new DBContextERP();

        public void addCardHolder(CardHolder cardHolder)
        {   
            if (cardHolder == null)
                throw new NullReferenceException("Object can not be null");

            context.cardHolders.Add(cardHolder);
            context.SaveChanges();
        }


        public void updateCardHolder(CardHolder cardHolder)
        {
            if (cardHolder == null)
                throw new NullReferenceException("Object can not be null");
            
            var _cardHolder = context.cardHolders.FirstOrDefault(x => x.Id == cardHolder.Id);

            //_cardHolder.cardHolderType = cardHolder.cardHolderType;

            if (cardHolder.Name != null)
                _cardHolder.Name = cardHolder.Name;

            _cardHolder.isActive = cardHolder.isActive;
            //if (cardHolder.ParentCardHolder != null)
            //    _cardHolder.ParentCardHolder = context.cardHolders.FirstOrDefault(x => x.Id == cardHolder.ParentCardHolder.Id);

            context.SaveChanges();
        }


        public CardHolder GetCardHolder(int cardHolder_id)
        {
            if (cardHolder_id == 0)
                throw new NullReferenceException("Object can not be null");
            return context.cardHolders
                .FirstOrDefault(x => x.Id == cardHolder_id);
        }

        public List<CardHolder> GetAllCardHolders()
        {
            return context.cardHolders
                .ToList();
        }


        public void addCreditCard(CreditCard creditCard)
        {
            CreditCard _creditCard = new CreditCard();

            if (creditCard == null)
                throw new NullReferenceException("Object can not be null");

            _creditCard.cardHolderType = creditCard.cardHolderType;
            if (creditCard.company != null)
                _creditCard.company = context.Companies.FirstOrDefault(x=>x.Id == creditCard.company.Id);

            if (creditCard.departments != null)
            {
                _creditCard.departments = new List<Department>();
                foreach (var _dept in creditCard.departments)
                {
                    var dept = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                    _creditCard.departments.Add(dept);
                }
            }

            if (creditCard.PrimaryCardHolder != null)
                _creditCard.PrimaryCardHolder = context.cardHolders.FirstOrDefault(x => x.Id == creditCard.PrimaryCardHolder.Id);

            if (creditCard.bank != null)
                _creditCard.bank = context.banks.FirstOrDefault(x => x.Id == creditCard.bank.Id);

            if (creditCard.CardUser != null)
                _creditCard.CardUser = context.cardHolders.FirstOrDefault(x => x.Id == creditCard.CardUser.Id);

            if (creditCard.SecondaryCardHolder != null)
                _creditCard.SecondaryCardHolder = context.cardHolders.FirstOrDefault(x => x.Id == creditCard.SecondaryCardHolder.Id);


            if(creditCard.cardHolderType == Enums.CardHolderType.Secondary)
            {
                //_creditCard.hasPrimaryCard = true;
                if (creditCard.PrimaryCardNoId != null)
                    _creditCard.PrimaryCardNo = context.creditCards.FirstOrDefault(x=>x.Id == creditCard.PrimaryCardNoId);
                //_creditCard.SecondaryCardNumber = creditCard.SecondaryCardNumber;

                //_creditCard.CardNumber = null;
            }
            else
            {
                //_creditCard.hasPrimaryCard = false;
                _creditCard.PrimaryCardNo = null;
            }
            _creditCard.CardNumber = creditCard.CardNumber;

            _creditCard.IssueDate = creditCard.IssueDate;
            _creditCard.ExpiryDate = creditCard.ExpiryDate;
            _creditCard.CVV = creditCard.CVV;

            if (creditCard.creditCardType != null)
                _creditCard.creditCardType = context.creditCardTypes.FirstOrDefault(x => x.Id == creditCard.creditCardType.Id);

            if (creditCard.currency != null)
                _creditCard.currency = context.currencies.FirstOrDefault(x => x.Id == creditCard.currency.Id);

            _creditCard.LimitAmount = creditCard.LimitAmount;

            context.creditCards.Add(_creditCard);
            context.SaveChanges();
        }


        public void updateCreditCard(CreditCard creditCard)
        {
            if (creditCard == null)
                throw new NullReferenceException("Object can not be null");

            var _creditCard = context.creditCards.FirstOrDefault(x => x.Id == creditCard.Id);

            _creditCard.cardHolderType = creditCard.cardHolderType;

            if (creditCard.company != null)
                _creditCard.company = context.Companies.FirstOrDefault(x => x.Id == creditCard.company.Id);

            _creditCard.departments.Clear();
            if (creditCard.departments != null)
            {
                foreach (var _dept in creditCard.departments)
                {
                    if (_creditCard.departments.Contains(_dept) == false)
                    {
                        var dept = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                        _creditCard.departments.Add(dept);
                    }
                }
            }

            if (creditCard.bank != null)
                _creditCard.bank = context.banks.FirstOrDefault(x=>x.Id == creditCard.bank.Id);

            if (creditCard.PrimaryCardHolder != null)
                _creditCard.PrimaryCardHolder = context.cardHolders.FirstOrDefault(x=>x.Id == creditCard.PrimaryCardHolder.Id);

            if (creditCard.CardUser != null)
                _creditCard.CardUser = context.cardHolders.FirstOrDefault(x => x.Id == creditCard.CardUser.Id);

            



            if (creditCard.cardHolderType == Enums.CardHolderType.Secondary)
            {
                //_creditCard.hasPrimaryCard = true;
                if (creditCard.PrimaryCardNoId != null)
                    _creditCard.PrimaryCardNo = context.creditCards.FirstOrDefault(x => x.Id == creditCard.PrimaryCardNoId);

                if (creditCard.SecondaryCardHolder != null)
                    _creditCard.SecondaryCardHolder = context.cardHolders.FirstOrDefault(x => x.Id == creditCard.SecondaryCardHolder.Id);
                //_creditCard.SecondaryCardNumber = creditCard.SecondaryCardNumber;


            }
            else
            {
                //_creditCard.hasPrimaryCard = false;
                _creditCard.PrimaryCardNo = null;
                _creditCard.SecondaryCardHolder = null;
            }

            _creditCard.CardNumber = creditCard.CardNumber;


            _creditCard.IssueDate = creditCard.IssueDate;
            _creditCard.ExpiryDate = creditCard.ExpiryDate;

            if (creditCard.CVV > 0)
                _creditCard.CVV = creditCard.CVV;

            if (creditCard.creditCardType != null)
                _creditCard.creditCardType = context.creditCardTypes.FirstOrDefault(x => x.Id == creditCard.creditCardType.Id);

            if (creditCard.currency != null)
                _creditCard.currency = context.currencies.FirstOrDefault(x => x.Id == creditCard.currency.Id);

            _creditCard.LimitAmount = creditCard.LimitAmount;

            _creditCard.isActive = creditCard.isActive;
            //if (cardHolder.ParentCardHolder != null)
            //    _cardHolder.ParentCardHolder = context.cardHolders.FirstOrDefault(x => x.Id == cardHolder.ParentCardHolder.Id);

            context.SaveChanges();
        }

        public CreditCard GetCreditCard(int creditCard_id)
        {
            if (creditCard_id == 0)
                throw new NullReferenceException("Object can not be null");

            return context.creditCards
                .FirstOrDefault(x => x.Id == creditCard_id);
        }

        public List<CreditCard> GetAllCreditCards()
        {
            return context.creditCards

                .ToList();
        }

        public List<CreditCard> GetAllPrimaryCards()
        {
            return context.creditCards

                .Where(x=>x.PrimaryCardNo == null)
                .ToList();
        }


        public List<CreditCard> GetCardsByPrimaryCard(/*int cardUser_id, int bankId,*/ int companyId, int deptId, int primaryCardId)
        {
            List<CreditCard> cardsToReturn = new List<CreditCard>();

            var creditCards = context.creditCards

                .Where(x => x.company.Id == companyId && x.PrimaryCardNoId == primaryCardId)
                .ToList();

            foreach (var _card in creditCards)
            {
                foreach (var _dept in _card.departments)
                {
                    if (_dept.Id == deptId)
                    {
                        cardsToReturn.Add(_card);
                        break;
                    }
                }
            }
            return cardsToReturn;
        }

        public List<CreditCard> GetAllPrimaryCardsByCompanyDept(/*int cardUser_id, int bankId,*/ int companyId, int deptId,  int bankId)
        {
            List<CreditCard> cardsToReturn = new List<CreditCard>();

            var creditCards = context.creditCards

                .Where(x=>x.company.Id == companyId && x.bank.Id == bankId && x.PrimaryCardNoId == null)
                .ToList();

            foreach(var _card in creditCards)
            {
                foreach(var _dept in _card.departments)
                {
                    if(_dept.Id == deptId)
                    {
                        cardsToReturn.Add(_card);
                        break;
                    }
                }
            }
            return cardsToReturn;
        }


        public List<CreditCard> GetAllSecondaryCardsByCompanyDept(/*int cardUser_id, int bankId,*/ int companyId, int deptId, int bankId)
        {
            List<CreditCard> cardsToReturn = new List<CreditCard>();

            var creditCards = context.creditCards

                .Where(x => x.company.Id == companyId && x.bank.Id == bankId && x.PrimaryCardNoId != null)
                .ToList();

            foreach (var _card in creditCards)
            {
                foreach (var _dept in _card.departments)
                {
                    if (_dept.Id == deptId)
                    {
                        cardsToReturn.Add(_card);
                        break;
                    }
                }
            }
            return cardsToReturn;
        }

        public List<CreditCard> GetAllByCompanyBankPrimaryHolder(/*int cardUser_id, int bankId,*/ int companyId, int bankId, int primaryHolderId)
        {
            return context.creditCards

                .Where(x => x.company.Id == companyId && x.bank.Id == bankId && x.PrimaryCardHolder.Id == primaryHolderId && x.PrimaryCardNoId == null)
                .ToList();

            
        }


        public List<CreditCard> GetAllPrimaryCardsByCompanyBank(/*int cardUser_id, int bankId,*/ int companyId, int bankId)
        {
            return context.creditCards
  
                .Where(x => x.company.Id == companyId && x.bank.Id == bankId && x.PrimaryCardNoId == null)
                .ToList();


        }


        public void addCreditCardType(CreditCardType cardType)
        {
            if (cardType == null)
                throw new NullReferenceException("Object can not be null");

            context.creditCardTypes.Add(cardType);
            context.SaveChanges();
        }

        public void updateCreditCardType(CreditCardType cardType)
        {
            if (cardType == null)
                throw new NullReferenceException("Object can not be null");

            var _cardType = context.creditCardTypes.FirstOrDefault(x => x.Id == cardType.Id);

            if (cardType.Type != null)
                _cardType.Type = cardType.Type;

            context.SaveChanges();
        }

        public CreditCardType getCreditCardType(int typeId)
        {
            if (typeId == 0)
                throw new NullReferenceException("Object can not be null");

            return context.creditCardTypes
                .FirstOrDefault(x => x.Id == typeId);
        }

        public List<CreditCardType> GetAllCreditCardTypes()
        {
            return context.creditCardTypes
                .ToList();
        }
    }
}
