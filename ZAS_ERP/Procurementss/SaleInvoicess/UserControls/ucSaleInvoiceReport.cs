using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using ERP_BL.Databases;
using System.Collections.Generic;

namespace ZAS_ERP.Procurementss.SaleInvoicess.UserControls
{
    public partial class ucSaleInvoiceReport : DevExpress.XtraReports.UI.XtraReport
    {
        List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
        SaleInvoice saleInvoice;
        int rowCount = 0;
        public ucSaleInvoiceReport()
        {
            InitializeComponent();
        }
        public ucSaleInvoiceReport(SaleInvoice SI) 
        {
            InitializeComponent();
            saleInvoice = SI;
            CalculateProducts();
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
            if (saleInvoice.BLAWBDate!= null)
            {
                var CreationDate = (DateTime)saleInvoice.BLAWBDate;
                poDate.Text = CreationDate.Date.ToString("dd-MMM-yyyy");
            }
            if (saleInvoice.customerCompany.billingAddres!= null && saleInvoice.customerCompany.shippingAddress!=null)
            {
                var address = saleInvoice.customerCompany.billingAddres.Line1;
                siBillTo.Text = address;
                var address2 = saleInvoice.customerCompany.billingAddres.Line2;
                siBillToLine2.Text = address2;
                var city = saleInvoice.customerCompany.billingAddres.City;
                siBillToCity.Text = city;

                var sAddres = saleInvoice.customerCompany.shippingAddress.Line1;
                siShiplTo.Text = sAddres;
                var sAddresS2 = saleInvoice.customerCompany.shippingAddress.Line2;
                siShiplToLine2.Text = sAddresS2;
                var cit = saleInvoice.customerCompany.shippingAddress.City;
                siShiplToCity.Text = cit;
            }
            if (saleInvoice.BLdeliveryRefNo != null) 
                siRefNO.Text = saleInvoice.BLdeliveryRefNo;           
            if(saleInvoice.BLAWBDate!= null)
            {
                var shippingDate = (DateTime)saleInvoice.BLAWBDate;
                billOfLandingDate.Text = shippingDate.Date.ToString("dd-MMM-yyyy");
            }               
            if (saleInvoice.referenceNo != null)
                soReferenceNo.Text = saleInvoice.referenceNo;
           // if (saleInvoice.allocation_Id != 0)
               // allocateTo.Text = saleInvoice.allocation_Id.ToString();
            if (saleInvoice.employee != null)
                allocateTo.Text = saleInvoice.employee.person.FName + " " + saleInvoice.employee.person.LName;
        }

        private void xrTableCell26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (rowCount % 2 == 0)
            {
                xrTableCell27.BackColor = Color.White;
                xrTableCell26.BackColor = Color.White;
                xrTableCell32.BackColor = Color.White;
                xrTableCell28.BackColor = Color.White;
                xrTableCell12.BackColor = Color.White;
                xrTableCell29.BackColor = Color.White;
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
                xrTableCell12.BackColor = color;
                xrTableCell29.BackColor = color;
                rowCount++;
            }
        }
    }
}
