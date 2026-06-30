using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using ERP_BL.Databases;
using System.Collections.Generic;

namespace ZAS_ERP.Procurementss.PurchaseOrderss.UserControls
{
    public partial class ucFrmPoReportsAbtUk : DevExpress.XtraReports.UI.XtraReport
    {
        List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
        PurchaseOrder purchaseOrder;
        public ucFrmPoReportsAbtUk()
        {
            InitializeComponent();
        }
      
        public ucFrmPoReportsAbtUk(PurchaseOrder PO)
        {
            InitializeComponent();
            purchaseOrder = PO;
           // this.DisplayName = purchaseOrder.company.CompanyName + " " + "Page1";
            CalculateProducts();
        }
        public void CalculateProducts()
        {
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
                        priority=procurementProduct.priority
                        //UnInvoicedQuantity= procurementProduct.UnInvoicedQuantity,
                    });
                }
            }
            DataSource = procurementProducts;
            if (purchaseOrder.currency.Symbol != null)
            {
                
            }

            if (purchaseOrder.billWithTax != 0 && purchaseOrder.tax != null)
            {
               
            }
            else
            {
            }
            if (purchaseOrder.maker != null)
                soMaker.Text = purchaseOrder.maker;
            if (purchaseOrder.origin != null)
                soOrigin.Text = purchaseOrder.origin;
            if (purchaseOrder.incoterm != null)
                soIncoTerm.Text = purchaseOrder.incoterm.term;

            if (purchaseOrder.DeliveryDate != null)
            {
                var purchaseOrderDeliveryDate = (DateTime)purchaseOrder.DeliveryDate;
                soDeliverytime.Text = purchaseOrderDeliveryDate.Date.Day.ToString() + "/" + purchaseOrderDeliveryDate.Date.Month.ToString() + "/" + purchaseOrderDeliveryDate.Date.Year.ToString();
            }
            if (purchaseOrder.POPaymentTerm.term != null)
                soPaymentterm.Text = purchaseOrder.POPaymentTerm.term;

            if (purchaseOrder.POReferenceNo != null)
                soPoNumber.Text = purchaseOrder.POReferenceNo;
            if (purchaseOrder.PurchaseOrderDate != null)
            {
                var purchaseOrderDate = (DateTime)purchaseOrder.PurchaseOrderDate;
                soCreationDate.Text = purchaseOrderDate.Date.Day.ToString() + "/" + purchaseOrderDate.Date.Month.ToString() + "/" + purchaseOrderDate.Date.Year.ToString();
            }

            if (purchaseOrder.proforma != null)
                soInvoiceNo.Text = purchaseOrder.proforma;
            
           
            if (purchaseOrder.POWarranty != null)
            {
                soPartialShipment.Text = purchaseOrder.POWarranty.name;
            }
            if(purchaseOrder.vendors != null && purchaseOrder.vendors.Count > 0)
            {
                var vendorName = purchaseOrder.vendors[0].company.CompanyName;
                var vendorAddress = purchaseOrder.vendors[0].billingAddres.Line1;
                var vendorAtt = purchaseOrder.vendors[0].contactPerson.FName;
                soSupplierName.Text = vendorName;
                soSupplierAddress.Text = vendorAddress;
                soSupplierAtt.Text = vendorAtt;

            }
            if (purchaseOrder.products != null)
            {
                double sumCfr = 0;
                foreach (var _prod in purchaseOrder.products)
                {
                    sumCfr = sumCfr + _prod.value1;
                }
                soTotalCost.Text = purchaseOrder.currency.Symbol + "  " + Convert.ToDecimal(sumCfr).ToString("#,##0.00");

            }
         
            if (purchaseOrder.billWithTax != 0 && purchaseOrder.tax != null)
            {
                var amount = Convert.ToDouble(purchaseOrder.totalCFRValue);
                var percentAmount = (purchaseOrder.tax.percentage / 100) * amount;
                soVat.Text = purchaseOrder.currency.Symbol + "  " + percentAmount.ToString("#,##0.00");
            }
            else
            {
                soVat.Text = "0";
                
            }
            if (purchaseOrder.billAfterTax != null)
            {
                soTotalWithVat.Text = purchaseOrder.currency.Symbol + "  " + Convert.ToDouble(purchaseOrder.billAfterTax).ToString("#,##0.00");
            }

            if (purchaseOrder.COO != 0)
            {
                totalWithCoo.Text = (purchaseOrder.totalCFRValue + purchaseOrder.COO).ToString("#,##0.00");
            }
            else
            {
                totalWithCoo.Visible = false;
                
                xrTableCell8.Visible = false;
            }
            if (purchaseOrder.Discount != 0)
            {
                totalWithDiscount.Text = (purchaseOrder.totalCFRValue - purchaseOrder.Discount).ToString("#,##0.00");
            }
            else
            {
                totalWithDiscount.Visible = false;
               
                xrTableCell6.Visible = false;
            }
            if (purchaseOrder.Freight != 0)
            {
                totalWithFreight.Text = (purchaseOrder.totalCFRValue + purchaseOrder.Freight).ToString("#,##0.00");
            }
            else
            {
                totalWithFreight.Visible = false;
              
                xrTableCell10.Visible = false;
            }

        }

    }
}
