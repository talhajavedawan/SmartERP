using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL
{
    public class AllTransactionsView : INotifyPropertyChanged
    {
        private string _Id { get; set; }
        private string _Parent_Id { get; set; }

        private string _Customer;
        private string _Employee;
        private string _Company;
        private string _TaskGroup;
        private string _Department;
        private double _totalCFRValue { get; set; }
        private double _amountME{ get; set; }
        private double _amountSOC{ get; set; }
        private double _loanAdjustment { get; set; }
        private string _Currency { get; set; } 
        private string _SalesReferenceNo { get; set; }  
        private DateTime? _CreationDate { get; set; }
        private string _Status { get; set; }
        private string _BackColor { get; set; }
        private string _TransactionType { get; set; }
        private string _SyetmReferenceNo { get; set; }

        public bool _Parent { get; set; }


        public string Id
        {
            get { return _Id; }

            set
            {
                _Id = value;
                NotifyPropertyChanged("Id");
            }
        }
        public string Parent_Id
        {
            get { return _Parent_Id; }

            set
            {
                _Parent_Id = value;
                NotifyPropertyChanged("Parent_Id");
            }
        }
        public bool Parent
        {
            get { return _Parent; }

            set
            {
                _Parent = value;
                NotifyPropertyChanged("Parent");
            }
        }
        public string Customer
        {
            get { return _Customer; }

            set
            {
                _Customer = value;
                NotifyPropertyChanged("Customer");
            }
        }

        public string Employee
        {
            get { return _Employee; }

            set
            {
                _Employee = value;
                NotifyPropertyChanged("Employee");
            }
        }
        public string TaskGroup
        {
            get { return _TaskGroup; }
            set
            {
                _TaskGroup = value;
                NotifyPropertyChanged("TaskGroup");
            }
        }
        public string Company
        {
            get { return _Company; }
            set
            {
                _Company = value;
                NotifyPropertyChanged("Company");
            }
        }
        public string Department
        {
            get { return _Department; }
            set
            {
                _Department = value;
                NotifyPropertyChanged("Department");
            }
        }
        public double totalCFRValue
        {
            get { return _totalCFRValue; }
            set
            {
                _totalCFRValue = value;
                NotifyPropertyChanged("totalCFRValue");
            }
        }
        public double amountME
        {
            get { return _amountME; }
            set
            {
                _amountME = value;
                NotifyPropertyChanged("amountME");
            }
        }
        public double amountSOC
        {
            get { return _amountSOC; }
            set
            {
                _amountSOC = value;
                NotifyPropertyChanged("amountSOC");
            }
        }

        public double loanAdjustment
        {
            get { return _loanAdjustment; }
            set
            {
                _loanAdjustment = value;
                NotifyPropertyChanged("loanAdjustment");
            }
        }
        public string Currency
        {
            get { return _Currency; }
            set
            {
                _Currency = value;
                NotifyPropertyChanged("Currency");
            }
        }
        public string SalesReferenceNo
        {
            get { return _SalesReferenceNo; }
            set
            {
                _SalesReferenceNo = value;
                NotifyPropertyChanged("SalesReferenceNo");
            }
        }
        public string SyetmReferenceNo
        {
            get { return _SyetmReferenceNo; }
            set
            {
                _SyetmReferenceNo = value;
                NotifyPropertyChanged("SyetmReferenceNo");
            }
        }
        public DateTime? CreationDate
        {
            get { return _CreationDate; }
            set
            {
                _CreationDate = value;
                NotifyPropertyChanged("CreationDate");
            }
        }
        public string Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public string BackColor
        {
            get { return _BackColor; }
            set
            {
                _BackColor = value;
                NotifyPropertyChanged("BackColor");
            }
        }
                public string TransactionType
        {
            get { return _TransactionType; }
            set
            {
                _TransactionType = value;
                NotifyPropertyChanged("TransactionType");
            }
        }
        


        // Property Change Logic  
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
