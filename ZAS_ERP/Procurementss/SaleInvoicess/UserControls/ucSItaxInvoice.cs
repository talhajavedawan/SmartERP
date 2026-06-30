using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using ERP_BL.Databases;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Color = System.Drawing.Color;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace ZAS_ERP.Procurementss.SaleInvoicess.UserControls
{
    public partial class ucSItaxInvoice : DevExpress.XtraReports.UI.XtraReport
    {
        List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
        SaleInvoice saleInvoice;
        int rowCount = 0;
        public ucSItaxInvoice()
        {
            InitializeComponent();
        }
        public ucSItaxInvoice(SaleInvoice SI)
        {
            InitializeComponent();
            saleInvoice = SI;
            CalculateProducts();
          
            // CalculateProducts();
        }
        public void CalculateProducts()
        {
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
            DataSource = procurementProducts;
            if (saleInvoice.CreationDate != null)
            {
                var CreationDate = (DateTime)saleInvoice.CreationDate;
                siCreationDate.Text = CreationDate.Date.ToString("dd-MMM-yyyy");
            }
            if (saleInvoice.customerCompany.billingAddres != null)
            {
               
                var address = saleInvoice.customerCompany.billingAddres.Line1;
                siBillTo.Text = address;
                var line2 = saleInvoice.customerCompany.billingAddres.Line2;
                siBillToAddress.Text = line2;
                var city = saleInvoice.customerCompany.billingAddres.City;
                siBillToCity.Text = city;
               


            }
            if (saleInvoice.customerCompany.company.SaleTaxRegistrationNumber != null)
            {
                var SaleTax = saleInvoice.customerCompany.company.SaleTaxRegistrationNumber;
                buyerSaleTaxReg.Text = SaleTax;
            }
            if (saleInvoice.customerCompany.company.EmployeerNo != null)
            {
                var Ntn = saleInvoice.customerCompany.company.EmployeerNo;
                ntnNumber.Text = Ntn;
            }

            if (!string.IsNullOrEmpty(saleInvoice.FinanceRefrenceNo))
                siFinanceRefNo.Text = saleInvoice.FinanceRefrenceNo;

            if (!string.IsNullOrEmpty( saleInvoice.referenceNo))
                soReferenceNo.Text = saleInvoice.referenceNo;

            if (saleInvoice.saleInvoiceDate != null)
            {
                var soDates = (DateTime)saleInvoice.saleInvoiceDate;
                soDate.Text = soDates.Date.ToString("dd-MMM-yyyy");
            }
            if (saleInvoice.BLAWBDate != null)
            {
                var soDatess = (DateTime)saleInvoice.BLAWBDate;
                soDeliveryDate.Text = soDatess.Date.ToString("dd-MMM-yyyy");  
            }
            if (!string.IsNullOrEmpty(saleInvoice.BLdeliveryRefNo))
            {
                siLCno.Text = saleInvoice.BLdeliveryRefNo; 
            }

            // if(SYSTEM_STATIC.currentUser.employee.person.Signature!= null)
            //{ 
            //    var userimage =  SYSTEM_STATIC.currentUser.employee.person.Signature;
            //    var source= ConvertByteArrayToBitmapImage(userimage); 
            //    Image img = (Image)source;
            //    imgSignature.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(img) ; 
            //}

            if (saleInvoice.products != null)
            {
                double sumCfr = 0;
                foreach(var _prod in saleInvoice.products)
                {
                    sumCfr = sumCfr + _prod.NowAmount;
                }
                siSubTotal.Text = saleInvoice.currency.Symbol+"  "+Convert.ToDecimal(sumCfr).ToString("#,##0.00");
                
            }

            if (!string.IsNullOrEmpty( saleInvoice.paymentTerm.term))
            {
                siPaymentTerm.Text = saleInvoice.paymentTerm.term;
            }

            if(saleInvoice.totalInvoiceAmount != 0)
             siTotal.Text = saleInvoice.currency.Symbol + "  " + Convert.ToDouble(saleInvoice.totalInvoiceAmount).ToString("#,##0.00");
        
            if (saleInvoice.salesReceipts != null)
            {
                double sumCollectionAmount = 0; 
                foreach(var _amount in saleInvoice.salesReceipts)
                {
                    sumCollectionAmount = sumCollectionAmount + _amount.CollectionAmount;
                }
                siRemainingSO.Text = saleInvoice.currency.Symbol + "  " + (saleInvoice.totalInvoiceAmount - sumCollectionAmount).ToString("#,##0.00");
            }

            if (saleInvoice.salesTax != 0)
            {
                siTax.Text = saleInvoice.currency.Symbol + "  " + saleInvoice.salesTax.ToString("#,##0.00");
            }
            else
            {
                siTax.Text = "0";
            }

           
        }

   

        private void xrTableCell26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (rowCount % 2 == 0)
            {
                xrTableCell27.BackColor = Color.White;
                xrTableCell26.BackColor = Color.White;
                xrTableCell32.BackColor = Color.White;
                xrTableCell28.BackColor = Color.White;
                xrTableCell10.BackColor = Color.White;
                xrTableCell29.BackColor = Color.White;
                xrTableCell11.BackColor = Color.White;
                xrTableCell13.BackColor = Color.White;
                rowCount++;
            }
            else
            {
                // var color = ColorTranslator.FromHtml("#ebf0f8");#F7F7F7 EBEAEA #F5F5F5 #F7F7F7  #F5F5F5
                var color = ColorTranslator.FromHtml("#F7F7F7");

                xrTableCell27.BackColor = color;
                xrTableCell26.BackColor = color;
                xrTableCell32.BackColor = color;
                xrTableCell28.BackColor = color;
                xrTableCell10.BackColor = color;
                xrTableCell29.BackColor = color;
                xrTableCell11.BackColor = color;
                xrTableCell13.BackColor = color;
                rowCount++;
            }
        }


        //public static Bitmap ConvertByteArrayToBitmapImage(byte[] bytes)
        //{
        //    var stream = new MemoryStream(bytes);
        //    stream.Seek(0, SeekOrigin.Begin);
        //    var image = new Bitmap(stream);          
        //    return image;
        //}




    }
}
