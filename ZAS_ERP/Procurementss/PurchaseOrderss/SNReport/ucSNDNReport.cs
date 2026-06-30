using DevExpress.XtraReports.UI;
using ERP_BL.Databases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace ZAS_ERP.Procurementss.saleOrderss.SNReport
{
    public partial class ucSNDNReport : DevExpress.XtraReports.UI.XtraReport
    {
        List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
        SaleOrder saleOrder;
        public ucSNDNReport()
        {
            InitializeComponent();
        }
        public ucSNDNReport(SaleOrder PO)
        {
            InitializeComponent();
            saleOrder = PO;
            this.DisplayName = saleOrder.company.CompanyName + " " + "Delivery Note";
            CalculateProducts();
        }
        public void CalculateProducts()
        {
            if (saleOrder.products != null)
            {
                List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                //grdPOItems.ItemsSource = offer.products;
                foreach (var procurementProduct in saleOrder.products)
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
                txtCustomerName.Text = saleOrder.customerCompany.company.CompanyName;
                txtDepartment.Text = saleOrder.department.DeptName;
                txtDVDate.Text = saleOrder.CreationDate.ToString();
            procurementProducts = procurementProducts.OrderBy(x => x.priority).ToList();
            DataSource = procurementProducts;
        }
    }
}
