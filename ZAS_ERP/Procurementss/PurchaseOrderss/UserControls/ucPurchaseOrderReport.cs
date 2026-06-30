using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using ERP_BL.Databases;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;

namespace ZAS_ERP.Procurementss.PurchaseOrderss.UserControls
{
    public partial class ucPurchaseOrderReport : DevExpress.XtraReports.UI.XtraReport
    {
        List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
        PurchaseOrder purchaseOrder;


        public ucPurchaseOrderReport()
        {
            InitializeComponent();
     
        }
        public ucPurchaseOrderReport(PurchaseOrder PO)
        {
            InitializeComponent();
            purchaseOrder = PO;
            this.DisplayName = purchaseOrder.company.CompanyName+" "+"Page1";
            CalculateProducts();
        }
        public void CalculateProducts()
        {
           
            double TaxAmount = 0;
            double COOAmount = 0;
            double DiscountAmount = 0;
            double FreightAmount = 0;
            double percentAmount = 0;
            double FinalAmountWithoutPercent = 0;
            double amountWithPercent = 0;


            if (purchaseOrder.products != null)
            {
                List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                //grdPOItems.ItemsSource = offer.products;
                foreach (var procurementProduct in purchaseOrder.products)
                {
                  
                    procurementProducts.Add(new ProcurementProduct()
                    {
                        Id = procurementProduct.Id,
                        inquiryProduct = new InquiryProduct()
                        {
                            Id = procurementProduct.inquiryProduct.Id,
                            ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
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
                                isActive = procurementProduct.inquiryProduct.product.isActive

                            }
                                ,
                            product_Id = procurementProduct.inquiryProduct.product.Id

                        },
                        product_Id = procurementProduct.inquiryProduct.Id,
                        unitPrice = procurementProduct.unitPrice,
                        UnInvoicedQuantity = (procurementProduct.UnInvoicedQuantity == 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.InvoicedQuantity : procurementProduct.UnInvoicedQuantity,
                        InvoicedQuantity = (procurementProduct.InvoicedQuantity == 0 && procurementProduct.UnInvoicedQuantity != 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.UnInvoicedQuantity : procurementProduct.InvoicedQuantity,
                        UnInvoicedWeight = (procurementProduct.UnInvoicedWeight == 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.InvoicedWeight : procurementProduct.UnInvoicedWeight,
                        InvoicedWeight = (procurementProduct.InvoicedWeight == 0 && procurementProduct.UnInvoicedWeight != 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.UnInvoicedWeight : procurementProduct.InvoicedWeight,
                        value1 = procurementProduct.value1,
                        value2 = procurementProduct.value2,
                        UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                        priority = procurementProduct.priority
                        //UnInvoicedQuantity= procurementProduct.UnInvoicedQuantity,
                    });

                }
            }
            if (purchaseOrder.currency.Symbol != null)
            {
               // currencySymbol.Text = purchaseOrder.currency.Symbol;
                currencyVatSymbol.Text = purchaseOrder.currency.Symbol;
               // totalPriceSymbol.Text = purchaseOrder.currency.Symbol;
                currencySymbols.Text = purchaseOrder.currency.Symbol;
                currencySymCoo.Text = purchaseOrder.currency.Symbol;
                currencySymFreigh.Text = purchaseOrder.currency.Symbol;
                currencyAmountSym.Text = purchaseOrder.currency.Symbol;
                totalCuSymbol.Text = purchaseOrder.currency.Symbol;
            }
            if (purchaseOrder.COO != 0)
            {
                //totalWithCoo.Text = (purchaseOrder.totalCFRValue + purchaseOrder.COO).ToString("#,##0.00");
                totalWithCoo.Text = purchaseOrder.COO.ToString("#,##0.00");
                COOAmount = purchaseOrder.COO;
            }
            else
            {
                totalWithCoo.Text = "0";
            }
            if (purchaseOrder.Discount != 0)
            {
                // totalWithDiscount.Text = (purchaseOrder.totalCFRValue - purchaseOrder.Discount).ToString("#,##0.00");
                totalWithDiscount.Text = purchaseOrder.Discount.ToString("#,##0.00");
                DiscountAmount = purchaseOrder.Discount;
            }
            else
            {
                totalWithDiscount.Text = "0";
            }
            if (purchaseOrder.Freight != 0)
            {
                //totalWithFreight.Text = (purchaseOrder.totalCFRValue + purchaseOrder.Freight).ToString("#,##0.00");
                totalWithFreight.Text = purchaseOrder.Freight.ToString("#,##0.00");
                FreightAmount = purchaseOrder.Freight;
            }
            else
            {
                totalWithFreight.Text = "0";
            }
            if (purchaseOrder.billWithTax != 0 && purchaseOrder.tax!=null)
            {
                var totalWithoutTax=purchaseOrder.totalCFRValue /*- purchaseOrder.salesTax*/;
                totalPricewithoutVat.Text = totalWithoutTax.ToString("#,##0.00");

               // totalWithVAT.Text = purchaseOrder.billWithTax.Value.ToString("#,##0.00");
                //var amount = Convert.ToDouble(purchaseOrder.totalCFRValue);

               // percentAmount = (purchaseOrder.tax.percentage / 100) * amount;
               // totalTax.Text = percentAmount.ToString("#,##0.00");
             

                FinalAmountWithoutPercent = (purchaseOrder.totalCFRValue + COOAmount + FreightAmount - DiscountAmount);
                amountWithPercent = (purchaseOrder.tax.percentage / 100) * FinalAmountWithoutPercent;
                totalCalAmount.Text = (FinalAmountWithoutPercent + amountWithPercent).ToString("#,##0.00");
                vatAmount.Text = amountWithPercent.ToString("#,##0.00");

            }
            else
            {
                FinalAmountWithoutPercent = (purchaseOrder.totalCFRValue + COOAmount + FreightAmount - DiscountAmount);
                totalCalAmount.Text = FinalAmountWithoutPercent.ToString("#,##0.00");
                
                totalPricewithoutVat.Text = purchaseOrder.totalCFRValue.ToString("#,##0.00");
                vatAmount.Text = "0";
               // totalTax.Text = purchaseOrder.salesTax.ToString("#,##0.00");
               // totalWithVAT.Text = purchaseOrder.totalCFRValue.ToString("#,##0.00");

            }
            if (purchaseOrder.maker != null)   
            poMaker.Text = purchaseOrder.maker;
            if (purchaseOrder.origin != null)
                poOrigin.Text = purchaseOrder.origin;
            if(purchaseOrder.incoterm!=null)
            poDeliveryTerm.Text = purchaseOrder.incoterm.term;
            if (purchaseOrder.DeliveryDate != null)
            {
               var purchaseOrderDeliveryDate = (DateTime)purchaseOrder.DeliveryDate;
                poDeliveryTime.Text = purchaseOrderDeliveryDate.Date.Day.ToString() + "/" + purchaseOrderDeliveryDate.Date.Month.ToString() + "/" + purchaseOrderDeliveryDate.Date.Year.ToString();
            }
            if (purchaseOrder.POPaymentTerm.term != null)
            poPayment.Text = purchaseOrder.POPaymentTerm.term;
            if (purchaseOrder.POReferenceNo != null)
                poRefNo.Text = purchaseOrder.POReferenceNo;
            if (purchaseOrder.PurchaseOrderDate != null)
            {
                var purchaseOrderDate= (DateTime)purchaseOrder.PurchaseOrderDate;
                poDate.Text = purchaseOrderDate.Date.Day.ToString() + "/" + purchaseOrderDate.Date.Month.ToString() + "/" + purchaseOrderDate.Date.Year.ToString();
            }
           
            if (purchaseOrder.CreationDate != null)
            {
                var CreationDate = (DateTime)purchaseOrder.CreationDate;
                poCreationDate.Text = CreationDate.Date.Day.ToString() + "/" + CreationDate.Date.Month.ToString() + "/" + CreationDate.Date.Year.ToString();
            }
            if (purchaseOrder.VendorName != null)
                poVendorName.Text = purchaseOrder.VendorName.ToString();
            if (purchaseOrder.proforma != null)
                poProfoma.Text = purchaseOrder.proforma;
            procurementProducts = procurementProducts.OrderBy(x => x.priority).ToList();           
            DataSource = procurementProducts; 
            if (purchaseOrder.POWarranty!=null)
            {
                poWarranty.Text = purchaseOrder.POWarranty.name;
            }
            
        }
    }
}
