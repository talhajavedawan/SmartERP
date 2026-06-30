using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Grid;
using ERP_BL;
using ERP_BL.Databases;


namespace ZAS_ERP.Procurementss.Inquiriess
{
    /// <summary>
    /// Interaction logic for ucInquiryBuckets.xaml
    /// </summary>
    public partial class ucInquiryBuckets : UserControl
    {
        InquiryRepo inquiryrepo = new InquiryRepo();
        Inquiry inquiry = new Inquiry();
        public static int inquiryid;
        List<ERP_BL.Databases.Inquiry> inquiryrepos = new List<ERP_BL.Databases.Inquiry>();
        public ucInquiryBuckets()
        {
            InitializeComponent();
        }

        private void btnCreateBucket_Click(object sender, RoutedEventArgs e)
        {

        }
        public class cmbitem
        {
            public string name { get; set; }
            public int id { get; set; }

        }
        private void cmbVendor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbVendor.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbVendor.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Vendorss.frmVendoradd vendorAdd = new Vendorss.frmVendoradd();
                    vendorAdd.ShowDialog();
                    loadVendors();
                }
                
                    
            }
        }
        public class datagriditem
        {
            public string Item_Name { get; set; }
            public string Item_Discription { get; set; }
            public string Own_Description { get; set; }
            public string UOM { get; set; }
            public double Quantity { get; set; }


        }
        VendorRepo vendrepo = new VendorRepo();
        List<Vendor> Vendors = new List<Vendor>();
        public void loadVendors()
        {
            Vendors= vendrepo.getAll();
            List<cmbitem> cmbitems = new List<cmbitem>();
            
            foreach (Vendor ven in Vendors)
            {
                
                    cmbitems.Add(new cmbitem() { name = ven.company.CompanyName, id = ven.Id });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbVendor.ItemsSource = cmbitems;
            lookupvendor.ItemsSource = Vendors;

        }
       
        public void loadinquiry()
        {
            //if (inquiry.customerCompany.ParentID != null || inquiry.customerCompany.parentCompany != null)
            //{
            //    cmbCustomerName.Text = inquiry.customerCompany.parentCompany.company.CompanyName;
            //    cmbsubcustomer.Text = inquiry.customerCompany.company.CompanyName;
            //}
            //else
            //{
            //    cmbCustomerName.Text = inquiry.customerCompany.company.CompanyName;
            //    cmbsubcustomer.Text = "N/A";
            //}
            //cmbDepartment.Text = inquiry.department.DeptName;
            //cmbSection.Text = inquiry.section.SectionName;
            //cmbEmployee.Text = inquiry.employee.person.FName + " " + inquiry.employee.person.LName;
            //txtInquiryStatus.Text = inquiry.inquiryStatus.Status;
            //datalertDate.DateTime = inquiry.alertDate;
            //cmbInquiryStatus.Text = inquiry.inquiryStatus.Status;
            //datinquirydate.DateTime = inquiry.inquiryDate;
            //datduedate.DateTime = inquiry.DeliveryDueDate;
            //datclosedate.DateTime = inquiry.lastSubmissionDate;
            //cmbInquiryType.Text = inquiry.inquirytype.ToString();
            //txtfileref.Text = inquiry.SalesReferenceNo;
            //txtinquiryref.Text = inquiry.InquiryReferenceNo;
            List<datagriditem> datagriditems = new List<datagriditem>();
            //foreach (InqueryItem inqueryItem in inquiry.inqueryItems)
            //{

            //    datagriditems.Add(new datagriditem { Item_Name = inqueryItem.itemType.Type, Item_Discription = inqueryItem.item, Own_Description = inqueryItem.itemDescription, UOM = inqueryItem.UOM, Quantity = inqueryItem.Qty });
            //}
            dGitems.ItemsSource = datagriditems;
            //loadInquiryStatus();
            //foreach (cmbitem cmbitem in cmbInquiryStatus.Items)
            //{
            //    if (cmbitem.name == inquiry.inquiryStatus.Status)
            //    {

            //        cmbInquiryStatus.SelectedItem = cmbitem;
            //    }
            //}




        }

        private void loadgrid()

        {
            inquiryrepos = inquiryrepo.getAll();
            this.grdinquiry.ItemsSource = inquiryrepos;

        }

        private void ucInquiryBucket_Loaded(object sender, RoutedEventArgs e)
        {
            loadVendors();
            loadgrid();
        }

       
        private void grdinquiry_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            if (grdinquiry.GetFocusedRowCellValue(grdinquiry.Columns.GetColumnByFieldName("Id")) != null)
            {

                inquiryid = (int)grdinquiry.GetFocusedRowCellValue(grdinquiry.Columns.GetColumnByFieldName("Id"));
                inquiry = inquiryrepo.get(inquiryid);
                loadinquiry();

            }
        }

        private void btnitemadd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<datagriditem> datagriditems = new List<datagriditem>();
                if (dGitems.HasItems)
                {
                    if (dgrdbucketitems.HasItems)
                    {
                        foreach (datagriditem datagriditem in dgrdbucketitems.Items)
                        {
                            if (dGitems.SelectedItem != datagriditem)
                            {
                                //dgrdbucketitems.Items.Add(datagriditem);
                                datagriditems.Add((datagriditem)dGitems.SelectedItem);
                                dgrdbucketitems.ItemsSource = datagriditems;
                            }
                        }

                    }
                    else
                        datagriditems.Add((datagriditem)dGitems.SelectedItem);
                    dgrdbucketitems.ItemsSource = datagriditems;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
            
        }

        private void btnitemremove_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
            if (dgrdbucketitems.HasItems)
            dgrdbucketitems.Items.RemoveAt(dgrdbucketitems.SelectedIndex);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
}
    }
}
