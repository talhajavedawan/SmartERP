using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using ERP_BL.Databases;
using System.Collections.Generic;
using System.Text;

namespace ZAS_ERP.Procurementss.SaleInvoicess.UserControls
{
    public partial class ucCommissionInvoicePage2 : DevExpress.XtraReports.UI.XtraReport
    {
        List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
        SaleInvoice saleInvoice;
        public ucCommissionInvoicePage2()
        {
            InitializeComponent();
        }
        public ucCommissionInvoicePage2(SaleInvoice SI)
        {
            InitializeComponent();
            saleInvoice = SI;
            CalculateProducts();
        }
        public void CalculateProducts()
        {
            try
            {
                //var Declare region
                var customerName = "";
                var invoiceRefNum = "";
                var dte = "";
                var commissionRefNum = "";
                var dteBl = "";
                var Nowinvoice = "";
                var convertToWord = "";
                var nowSymbol = "";

                if (!string.IsNullOrEmpty(saleInvoice.CISubject))
                {
                    siSubject.Text = saleInvoice.CISubject;
                }

                if (saleInvoice.customerCompany.company != null)
                {
                    customerName = saleInvoice.customerCompany.company.CompanyName;
                }
                if (saleInvoice.BLdeliveryRefNo != null)
                {
                    invoiceRefNum = saleInvoice.BLdeliveryRefNo;
                }
                if (saleInvoice.BLAWBDate != null)
                {
                    var BlDeliveryDate = (DateTime)saleInvoice.BLAWBDate;
                    dte = BlDeliveryDate.Date.ToString("dd-MMM-yyyy");
                }
                if (saleInvoice.commisionRefrenceNo != null)
                {
                    commissionRefNum = saleInvoice.commisionRefrenceNo;
                    siCommissionRefNum.Text = saleInvoice.commisionRefrenceNo;
                }
                if (saleInvoice.BLAWBDate != null)
                {
                    var BlDeliveryDate = (DateTime)saleInvoice.BLAWBDate;
                    dteBl = BlDeliveryDate.Date.ToString("dd-MMM-yyyy");
                }
                if (saleInvoice.products != null)
                {
                    double sumCfr = 0;
                    foreach (var _prod in saleInvoice.products)
                    {
                        sumCfr = sumCfr + _prod.NowAmount;
                    }
                    nowSymbol = saleInvoice.currency.Symbol;
                    Nowinvoice = Convert.ToDecimal(sumCfr).ToString("#,##0.00");


                }
                if (saleInvoice.principal.company != null)
                {
                    siPrinciple.Text = saleInvoice.principal.company.CompanyName;
                }
                if (saleInvoice.principal.contactPerson != null)
                {
                    siAttention.Text = saleInvoice.principal.contactPerson.FName;
                }
                if (saleInvoice.BLdeliveryRefNo != null)
                {
                    siBlDeliveryRefNumber.Text = saleInvoice.BLdeliveryRefNo;
                }
                if (saleInvoice.BLAWBDate != null)
                {
                    var BlDeliveryDate = (DateTime)saleInvoice.BLAWBDate;
                    
                    siBlDeliveryDate.Text = BlDeliveryDate.Date.ToString("dd-MMM-yyyy");
                    siDateBl.Text = BlDeliveryDate.Date.ToString("MMM-yyyy");
                }
             


                string result;
                var num = Nowinvoice;
                int pos = num.LastIndexOf(".");           
             
                    if (pos != -1 && pos < 10)
                    {
                        string _str = num.Substring(0, pos);
                        var newStr =_str.Replace(",", "");
                        int n = int.Parse(newStr);
                        result = NumberToWords(n);
                        string stringAfterChar = num.Substring(num.IndexOf(".") + 1);
                        if (stringAfterChar != "" && stringAfterChar.Length > 1)
                        {
                            string strAfterDot = stringAfterChar.Substring(0, 2);
                            if (strAfterDot != "")
                            {
                            convertToWord = result + " " + "&" + " " + strAfterDot + "/100" + " " + "only ";
                            }
                            else
                            {
                            convertToWord = result + " Only";
                            }
                        }
                    }
                    else
                    {
                        if (num != "" && num.Length < 10)
                        {
                        var newStr = num.Replace(",", "");
                        int number = int.Parse(newStr);
                            result = NumberToWords(number);
                            convertToWord = result + " Only";
                        }

                    }

                
                    var displayText = "Please refer to our client ";
                    var displaytext1 = "order for supply of Refractory Material and your Invoice Ref No: ";
                    var DisplayDate = "Dated:";
                    var results = displayText + " " + customerName + " " + displaytext1 + " " + invoiceRefNum + " "+ DisplayDate +" " + dte +".";
                    xrLabel7.Text = results;

                    var display = "Attached please find our Commission Invoice ref No:";
                    var display1 = "Dated:";
                    var display2 = "of value";
                    var display3 = "concerning our commission against subject order.";
                    var res = display + " " + commissionRefNum + " " + display1 + " " + dteBl + " " + display2 + " " + nowSymbol + " " + Nowinvoice + "( " + convertToWord + " ) " + display3;
                    xrLabel11.Text = res;
            }
            catch (Exception)
            {
                
            }
        }
        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
    }
}
