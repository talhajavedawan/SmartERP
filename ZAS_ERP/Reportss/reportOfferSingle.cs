using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using ERP_BL.Databases;

namespace ZAS_ERP.Reportss
{
    public partial class reportOfferSingle : DevExpress.XtraReports.UI.XtraReport
    {
        public reportOfferSingle()
        {
            InitializeComponent();
        }
        public reportOfferSingle(Offer offer)
        {
            InitializeComponent();
            IList<Offer> offersList = new List<Offer>();
            offersList.Add(offer);
            this.DataSource = offersList;
            this.Name = "reportOfferSingle1";
            SYSTEM_STATIC.SetLayoutOfCurrentReport(this);
        }
    }
}
