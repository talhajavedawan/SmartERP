using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using ERP_BL.Databases;

namespace ZAS_ERP.Procurementss.SaleInvoicess.UserControls
{
	public partial class ucSIcommissionInvoice : DevExpress.XtraReports.UI.XtraReport
	{
        List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
        SaleInvoice saleInvoice;
        public ucSIcommissionInvoice()
		{
			InitializeComponent();
		}
        public ucSIcommissionInvoice(SaleInvoice SI)
        {
            InitializeComponent();
            saleInvoice = SI;
            CalculateProducts();
        }
        public void CalculateProducts()
        {
            try
            {
                string displayText = "As Per";
                var principle = "";
                string displayCommercial = "Commercial Invoice Ref No:";
                var blDeliverRefNum = "";
                var displayDate = "Dated:";
                var Bldte = "";

                var commission = "ZAS Commission of";
                var symbol = "";
                var totalAmount = "";
                var convertToWord = "";
                var transferText = "be wire transferred to under mentioned account number.";
                var billingAddress = "";
                var billingCity = "";
                var billingCountry = "";
               
                if (saleInvoice.products != null)
                {
                    List<ProcurementProduct> procItems = new List<ProcurementProduct>();

                    foreach (var procurementProduct in saleInvoice.products)
                    {
                        procurementProducts.Add(new ProcurementProduct()
                        {
                            Id = procurementProduct.Id,

                            inquiryProduct = new InquiryProduct()
                            {
                                Id = procurementProduct.inquiryProduct.Id,
                                ownDiscription = procurementProduct.inquiryProduct.ownDiscription,
                                UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                quantity = procurementProduct.inquiryProduct.quantity,
                                Weight = procurementProduct.inquiryProduct.Weight,
                                product = new Product()
                                {
                                    Id = procurementProduct.inquiryProduct.product.Id,
                                    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                    item = procurementProduct.inquiryProduct.product.item,
                                    code = procurementProduct.inquiryProduct.product.code,
                                    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                    unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                    nature = procurementProduct.inquiryProduct.product.nature,
                                    category = procurementProduct.inquiryProduct.product.category,

                                    isActive = procurementProduct.inquiryProduct.product.isActive,
                                    journalTransactions = procurementProduct.inquiryProduct.product.journalTransactions,
                                    incomeAccount = procurementProduct.inquiryProduct.product.incomeAccount,

                                },
                                product_Id = procurementProduct.inquiryProduct.product.Id
                            },
                            product_Id = procurementProduct.inquiryProduct.Id,
                            unitPrice = procurementProduct.unitPrice,
                            UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                            InvoicedQuantity = procurementProduct.InvoicedQuantity,
                            TotalInvoicedQuantity = procurementProduct.TotalInvoicedQuantity,
                            UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                            InvoicedWeight = procurementProduct.InvoicedWeight,
                            TotalInvoicedWeight = procurementProduct.TotalInvoicedWeight,
                            value2 = procurementProduct.value2,
                            totalInvoicedSoAmount = procurementProduct.totalInvoicedSoAmount,
                            UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                            NowAmount = procurementProduct.NowAmount,

                        });
                    }
                }
                //DataSource = procurementProducts;
                if (!string.IsNullOrEmpty(saleInvoice.CISubject))
                {
                    siSubject.Text = saleInvoice.CISubject;
                }
                //if (!string.IsNullOrEmpty(saleInvoice.Commission))
                //{
                //    siZasCommission.Text = saleInvoice.Commission;
                //}
                if (saleInvoice.principal.billingAddres != null)
                {
                    billingAddress = saleInvoice.principal.billingAddres.Line1;
                }
                if (saleInvoice.principal.billingAddres != null)
                {
                    billingCity = saleInvoice.principal.billingAddres.City;
                }
                if (saleInvoice.principal.billingAddres != null)
                {
                    billingCountry = saleInvoice.principal.billingAddres.Country;
                }
               
                if (saleInvoice.principal.company != null)
                {
                    siPrincipleCompanyName.Text = saleInvoice.principal.company.CompanyName + " " + billingAddress + " " + billingCity + " " + billingCountry; 
                }
                if (saleInvoice.principal.contactPerson != null)
                {
                    siPrinciple.Text = saleInvoice.principal.contactPerson.FName;
                    principle = saleInvoice.principal.contactPerson.FName; 
                }
                if (saleInvoice.BLdeliveryRefNo != null)
                {
                    siRefNO.Text = saleInvoice.BLdeliveryRefNo;

                    blDeliverRefNum = saleInvoice.BLdeliveryRefNo;
                }
                if (saleInvoice.BLAWBDate != null)
                {
                    var CreationDate = (DateTime)saleInvoice.BLAWBDate;
                    siBLDeliveryDate.Text = CreationDate.Date.ToString("dd-MMM-yyyy");
                    Bldte= CreationDate.Date.ToString("dd-MMM-yyyy");
                }
                if (saleInvoice.CreationDate != null)
                {
                    var CreationDate = (DateTime)saleInvoice.CreationDate;
                    blDeliveryDateOrETD.Text = CreationDate.Date.ToString("dd-MMM-yyyy");
                }
                if (saleInvoice.commisionRefrenceNo != null)
                {
                    xrTableCell8.Text = saleInvoice.commisionRefrenceNo; 
                }
                if (saleInvoice.customerCompany.company != null)
                {
                    siCustomerName.Text = saleInvoice.customerCompany.company.CompanyName;
                }
                if (saleInvoice.customerCompany.contactPerson != null)
                {
                    siPrincipleClient.Text = saleInvoice.customerCompany.contactPerson.FName;
                }
                //if (saleInvoice.products != null)
                //{
                //    if (procurementProducts.Count > 0)
                //    {
                //        double sum = 0;
                //        for (int i = 0; i < procurementProducts.Count; i++)
                //        {
                //            sum = sum + procurementProducts[i].value2;
                //        }
                //        siSoAmount.Text = sum.ToString("#,##0.00");
                //    }
                //}

                if (saleInvoice.products != null)
                {
                    double sumCfr = 0;
                    foreach (var _prod in saleInvoice.products)
                    {
                        sumCfr = sumCfr + _prod.NowAmount;
                    }
                    siSoAmount.Text = saleInvoice.currency.Symbol + "  " + Convert.ToDecimal(sumCfr).ToString("#,##0.00");
                    symbol = saleInvoice.currency.Symbol;
                    totalAmount = Convert.ToDecimal(sumCfr).ToString("#,##0.00");
                }

                if (saleInvoice.totalInvoiceAmount != 0)
                {
                    siTotal.Text = saleInvoice.currency.Symbol + "  " + Convert.ToDouble(saleInvoice.totalInvoiceAmount).ToString("#,##0.00");
                }
                if (saleInvoice.bank != null)
                {
                    siBanker.Text = saleInvoice.bank.BankName;
                    siSwift.Text = saleInvoice.bank.SwiftCode;
                }

                if (saleInvoice.account != null)
                {
                    siIBAN.Text = saleInvoice.account.IBAN;
                    siAccountTitle.Text = saleInvoice.account.AccountNick;
                }
                string result;
                var num = totalAmount;
                int pos = num.LastIndexOf(".");
                    if (pos != -1 && pos < 10)
                    {
                        string _str = num.Substring(0, pos);
                        var newStr = _str.Replace(",", "");
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
                    var results = displayText + " " + principle + " " + displayCommercial + " " + blDeliverRefNum + " " + displayDate+ " " + Bldte ;
                    xrLabel18.Text = results;
                    xrLabel7.Text = "(Supply Of Refractory Material)";
                    var res = commission + " " + symbol + " " + totalAmount + " ( " + convertToWord + " ) " + transferText;
                    siZasCommission.Text = res;
            }
            catch (Exception ex)
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
