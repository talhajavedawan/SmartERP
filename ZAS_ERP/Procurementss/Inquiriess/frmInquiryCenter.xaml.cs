using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
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
using System.Windows.Shapes;

namespace ZAS_ERP.Inquiriess
{
    /// <summary>
    /// Interaction logic for frmInquiryCenter.xaml
    /// </summary>
    public partial class frmInquiryCenter : Window
    {

        public static int Editit;
        public static int inquiryid;
        public frmInquiryCenter()
        {
            InitializeComponent();
            
        }

        public class datagriditem
        {
            public string Item_Name { get; set; }
            public string Item_Discription { get; set; }
            public string Own_Description { get; set; }
            public string UOM { get; set; } 
            public double Quantity { get; set; }
            

        }
        private void mbtnaddcomp_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            frmInquiryadd inquiryadd = new frmInquiryadd();
            inquiryadd.ShowDialog();
            loadgrid();
        }
        InquiryRepo inquiryrepo = new InquiryRepo();
        Inquiry inquiry = new Inquiry();
        List<ERP_BL.Databases.Inquiry> inquiryrepos = new List<ERP_BL.Databases.Inquiry>();
        private void loadgrid()

        {
            Editit = 0;
            
            
            
            inquiryrepos = inquiryrepo.getAll();
            this.grdinquiry.ItemsSource = inquiryrepos;
            

            //// grdcutomercompanies.AutoGenerateColumns = AutoGenerateColumnsMode.AddNew;
            //grdcutomercompanies.Columns.Add(new GridColumn() { FieldName = "Id" });
            //grdcutomercompanies.Columns.GetColumnByFieldName("Id").Visible = false;
            //grdcutomercompanies.Columns.Add(new GridColumn() { FieldName = "company.CompanyName" });
            //grdcutomercompanies.Columns.GetColumnByFieldName("company.CompanyName").Header = "Company Name";
            //grdcutomercompanies.Columns.Add(new GridColumn() { FieldName = "contactPerson.FName" });
            //grdcutomercompanies.Columns.GetColumnByFieldName("contactPerson.FName").Header = "First Name";
            //grdcutomercompanies.Columns.Add(new GridColumn() { FieldName = "contactPerson.LName" });
            //grdcutomercompanies.Columns.GetColumnByFieldName("contactPerson.LName").Header = "Last Name";
            //grdcutomercompanies.Columns.Add(new GridColumn() { FieldName = "ParentID" });
            //           grdcutomercompanies.Columns.GetColumnByFieldName("ParentID").Header = "Parent";
            //grdcutomercompanies.Columns.GetColumnByFieldName("ParentID").Visible = false;
            
            // inquiry.
            //grdcutomercompanies.Columns["inquiry.ParentID"].GroupIndex = 0;
            //grdcutomercompanies.GroupBy("inquiry.ParentID");

        }

        private void winInquiryCenter_Loaded(object sender, RoutedEventArgs e)
        {
            loadgrid();
            
        }
        public void loadinquiry()
        {
            if (inquiry.customerCompany.ParentID != null || inquiry.customerCompany.parentCompany != null)
            {
                cmbCustomerName.Text = inquiry.customerCompany.parentCompany.company.CompanyName;
                cmbsubcustomer.Text = inquiry.customerCompany.company.CompanyName;
            }
            else
            {
                cmbCustomerName.Text = inquiry.customerCompany.company.CompanyName;
                cmbsubcustomer.Text = "N/A";
            }
            cmbDepartment.Text = inquiry.department.DeptName;
            //cmbSection.Text = inquiry.section.SectionName;
            cmbEmployee.Text = inquiry.employee.person.FName + " " + inquiry.employee.person.LName;
            txtInquiryStatus.Text = inquiry.inquiryStatus.Status;
            datalertDate.DateTime = inquiry.alertDate;
            cmbInquiryStatus.Text = inquiry.inquiryStatus.Status;
            datinquirydate.DateTime = inquiry.inquiryDate;
            datduedate.DateTime = inquiry.DeliveryDueDate;
            datclosedate.DateTime = inquiry.lastSubmissionDate;
            cmbInquiryType.Text = inquiry.inquirytype.ToString();
            txtfileref.Text = inquiry.SalesReferenceNo;
            txtinquiryref.Text = inquiry.referenceNo;
            List<datagriditem> datagriditems = new List<datagriditem>();
            //foreach (InqueryItem inqueryItem in inquiry.inqueryItems)
            //{

            //    datagriditems.Add(new datagriditem {Item_Name=inqueryItem.itemType.Type, Item_Discription=inqueryItem.item, Own_Description= inqueryItem.itemDescription,UOM = inqueryItem.UOM, Quantity= inqueryItem.Qty });
            //}
            loadInquiryStatus();
            foreach (cmbitem cmbitem in cmbInquiryStatus.Items)
            {
                if (cmbitem.name == inquiry.inquiryStatus.Status)
                {

                    cmbInquiryStatus.SelectedItem = cmbitem;
                }
            }
            

            dGitems.ItemsSource = datagriditems;

        }
        InquiryRepo inquiryRepo = new InquiryRepo();
        public void loadInquiryStatus()
        {

            List<InquiryStatus> inquiryStatuses = new List<InquiryStatus>();
            inquiryStatuses = inquiryRepo.getAllInquiryStatus();

            List<cmbitem> cmbitems = new List<cmbitem>();
           


            foreach (InquiryStatus status in inquiryStatuses)
            {

                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id });


            }
            cmbitems.Add(new cmbitem() { name = "<--Add New--> ", id = 0 });
            cmbInquiryStatus.ItemsSource = cmbitems;
        }
        public class cmbitem
        {
            public string name { get; set; }
            public int id { get; set; }

        }

        private void btnEditInquiry_Click(object sender, RoutedEventArgs e)
        {
            Editit = 1;
            if (grdinquiry.GetFocusedRowCellValue(grdinquiry.Columns.GetColumnByFieldName("Id")) != null)
            {

               inquiryid = (int)grdinquiry.GetFocusedRowCellValue(grdinquiry.Columns.GetColumnByFieldName("Id"));

                //MessageBox.Show(empid.ToString());
            }
            frmInquiryadd inquiryadd = new frmInquiryadd();
            inquiryadd.ShowDialog();
            loadgrid();

        }

        private void btnNewInquiry_Click(object sender, RoutedEventArgs e)
        {
            frmInquiryadd inquiryadd = new frmInquiryadd();
            inquiryadd.ShowDialog();
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
        InquiryStatus status = new InquiryStatus();
        private void btnUpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            if ((cmbInquiryStatus.SelectedItem as cmbitem) != null)
            {

                status = inquiryRepo.getstatus((cmbInquiryStatus.SelectedItem as cmbitem).id);
                inquiry.inquiryStatus = status;
            }
            inquiryRepo.updateStatus(inquiry,status);
            MessageBox.Show("Inquiry Status Updated Succesfully");
            loadgrid();

        }

        private void cmbInquiryStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbInquiryStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbInquiryStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                   Procurementss.Inquiriess.frmInquiryStatusAdd statusAdd = new Procurementss.Inquiriess.frmInquiryStatusAdd();
                    statusAdd.ShowDialog();
                    loadInquiryStatus();
                }



            }
        }
    }
}
