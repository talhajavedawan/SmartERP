using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using ERP_BL.Databases;
using System.Collections.Generic;

namespace ZAS_ERP.Reportss
{
    public partial class reportInquirySingle : DevExpress.XtraReports.UI.XtraReport
    {
        public reportInquirySingle()
        {
            InitializeComponent();
        }
        public reportInquirySingle(Inquiry inquiry)
        {
            InitializeComponent();
            IList<Inquiry> inquirysList = new List<Inquiry>();
            inquirysList.Add(inquiry);
            this.DataSource = inquirysList;
            this.Name = "reportInquirySingle1";
            SYSTEM_STATIC.SetLayoutOfCurrentReport(this);
            
        }
    }
}
