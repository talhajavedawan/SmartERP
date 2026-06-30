using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using ERP_BL.Databases;

namespace ZAS_ERP.Reportss
{
    public partial class reportPurchaseOrderSingle : DevExpress.XtraReports.UI.XtraReport
    {
        public reportPurchaseOrderSingle()
        {
            InitializeComponent();
        }
        public reportPurchaseOrderSingle(ERP_BL.Databases.PurchaseOrder purchaseOrder)
        {
            InitializeComponent();
            IList<PurchaseOrder> PurchaseOrderList = new List<PurchaseOrder>();
            PurchaseOrderList.Add(purchaseOrder);
            this.DataSource = PurchaseOrderList;
            this.Name = "reportPurchaseOrderSingle1";
            SYSTEM_STATIC.SetLayoutOfCurrentReport(this);
        }
    }
}
