using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using ERP_BL.Databases;
using System.Collections.Generic;

namespace ZAS_ERP.Procurementss.PurchaseOrderss.Cst_Report
{
    public partial class ucCstReport : DevExpress.XtraReports.UI.XtraReport
    {
        List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
        PurchaseOrder purchaseOrder;
        public ucCstReport()
        {
            InitializeComponent();
        }
        public ucCstReport(PurchaseOrder PO)
        {
            InitializeComponent();
            purchaseOrder = PO;
            //this.DisplayName = purchaseOrder.company.CompanyName + " " + "Page1";
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


                            },
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
                    });
                }
            }
            DataSource = procurementProducts;
            if (purchaseOrder.products != null)
            {
                double sumCfr = 0;
                foreach (var _prod in purchaseOrder.products)
                {
                    sumCfr = sumCfr + _prod.value1;
                }
                poTotalCost.Text = purchaseOrder.currency.Symbol + "  " + Convert.ToDecimal(sumCfr).ToString("#,##0.00");
                poTotalTax.Text = purchaseOrder.currency.Symbol + "  " + Convert.ToDecimal(purchaseOrder.totaltaxAmount).ToString("#,##0.00");
                poTotalWithTax.Text = purchaseOrder.currency.Symbol + "  " + Convert.ToDecimal(purchaseOrder.totaltaxAmount+ sumCfr).ToString("#,##0.00");

            }
            if (purchaseOrder.vendors != null && purchaseOrder.vendors.Count > 0)
            {
                var vendorName = purchaseOrder.vendors[0].company.CompanyName;
                poVendorName.Text = vendorName; 
            }
            if (purchaseOrder.POWarranty != null)
            {
                poWarranty.Text = purchaseOrder.POWarranty.name;
            }
            if (purchaseOrder.POPaymentTerm.term != null)
                soPaymentTerm.Text = purchaseOrder.POPaymentTerm.term;
            if (purchaseOrder.incoterm != null)
            {
                if(!string.IsNullOrEmpty(purchaseOrder.incoterm.term))
                incoTerm.Text = purchaseOrder.incoterm.term;
            }
            if (!string.IsNullOrEmpty (purchaseOrder.proforma))
                performaInvoice.Text = purchaseOrder.proforma;
            if (purchaseOrder.origin != null)
                poOrigin.Text = purchaseOrder.origin;
            if (purchaseOrder.DeliveryDate != null)
            {
                var purchaseOrderDeliveryDate = (DateTime)purchaseOrder.DeliveryDate;
                poDeliveryTime.Text = purchaseOrderDeliveryDate.Date.Day.ToString() + "/" + purchaseOrderDeliveryDate.Date.Month.ToString() + "/" + purchaseOrderDeliveryDate.Date.Year.ToString();
            }
            if (purchaseOrder.POReferenceNo != null)
                poOrderNumber.Text = purchaseOrder.POReferenceNo;
            if (purchaseOrder.PurchaseOrderDate != null)
            {
                var purchaseOrderDate = (DateTime)purchaseOrder.PurchaseOrderDate;
                poDate.Text = purchaseOrderDate.Date.Day.ToString() + "/" + purchaseOrderDate.Date.Month.ToString() + "/" + purchaseOrderDate.Date.Year.ToString();
            }
            if (purchaseOrder.maker != null)
                poManufacturer.Text = purchaseOrder.maker;

        }
    }
}
