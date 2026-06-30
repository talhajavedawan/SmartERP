using DevExpress.Mvvm;
using DevExpress.Xpf.Controls;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ERP_BL;
using ERP_BL.Config;
using ERP_BL.Databases;
using ERP_BL.Enums;

namespace ZAS_ERP.start
{
    /// <summary>
    /// Interaction logic for frmstartWizard.xaml
    /// </summary>
    /// 

        
    
    public partial class frmstartWizard
    {
        public string onlychar { get; set; }
        public string onlynum { get; set; }
        public string required { get; set; }
        public string nospecchar { get; set; }
        Config config = new Config();
        public string image1 = "heellllooooo";
        public bool abortExit;
        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
        pUser poweruser = new pUser();
        CompanyRepo companyRepo = new CompanyRepo();
        public DataTemplate FooterTemplate { get; set; }
        public frmstartWizard()
        {
            InitializeComponent();
            progWizardbar.Width = 0;

            List<string> INDList = config.getIndustryType();
            List<string> Biztype = config.getBizType();
            //if (INDList.Count > 0)
            //    foreach (string IND in INDList)
            //    {
            //        cmbIndustry.Items.Add(IND);
            //    }
            if (Biztype.Count > 0)
                foreach (string Biz in Biztype)
                {
                    cmbBussinesType.Items.Add(Biz);
                }
        }

        public void loadIndustryTypes()
        {



            List<IndustryType> industryTypes = new List<IndustryType>();
            //  employees = cont1.GetEmployees();
            industryTypes = companyRepo.GetIndustryTypes();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (IndustryType industry in industryTypes)
            {

                cmbitems.Add(new cmbitem() { name = industry.name , id = industry.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            cmbIndustry.ItemsSource = cmbitems;

        }
        void wizard_Cancel(object sender, DevExpress.Xpf.Core.CancelRoutedEventArgs e)
        {
            if (abortExit==false) e.Cancel = true;
        }

        private void WizardButton_Cancel(object sender, RoutedEventArgs e)
        {
            if (DXMessageBox.Show("Your current progress will be lost! Would you like to continue?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No) { e.Handled = false; return; }
            else { abortExit = true; this.Close(); }

        }
        private void WizardButton_next(object sender, RoutedEventArgs e)
        {  progWizardbar.Width=800;
            progWizardbar.Value += 25;
        }
        private void WizardButton_back(object sender, RoutedEventArgs e)
        {
            progWizardbar.Value -= 25;
        }

        private void WizardButton_NewCompany(object sender, RoutedEventArgs e)
        {
            try
            {


                company.address = new Address()
                {
                    Line1 = txtAdressLine1.Text.Trim(),
                    Line2 = txtAdressLine2.Text.Trim(),
                    State = txtstate.Text.Trim(),
                    Country = txtCountry.Text.Trim(),
                    Zip = string.IsNullOrEmpty(txtZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim())
                };

                company.contact = new Contact()
                {
                    ContactNo = txtPhonenum.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Fax = txtFaxnum.Text.Trim(),
                    SMLink1 = txtPage1.Text.Trim(),
                    SMLink2 = txtPage2.Text.Trim(),
                    SMLink3 = txtPage3.Text.Trim(),
                    Website = txtWebsite.Text.Trim()

                };
                company.CompanyName = txtBussinesName.Text.Trim();
                company.EmployeerNo = txtEIN.Text.Trim();
                company.industryTypeId = (cmbIndustry.SelectedItem as cmbitem).id;
                company.BizType = (BizTypes)cmbBussinesType.SelectedIndex;
                company.openingDate = System.DateTime.Today;
                company.closingDate = null;
                DBContextERP companycontext = new DBContextERP();
                company.compnayType = CompnayTypes.Group;
                
                companyRepo.addCompany(company);


                poweruser.userName = txtUsername.Text.Trim();
                poweruser.Password = SYSTEM_STATIC.GenerateSHA512String(txtPassword.Password.Trim());

                companycontext.pUsers.Add(poweruser);

                companycontext.SaveChanges();
                MessageBox.Show(company.CompanyName + " is Created Successfully!", "Congratulations");
                
                Properties.Settings.Default["serverip"] = SYSTEM_STATIC.server.ToString();
                Properties.Settings.Default.Save();
                Properties.Settings.Default["dbname"] = start.frmDbConnectt.dbname.ToString();
                Properties.Settings.Default.Save();
                //DBContextERP powerusercontext = new DBContextERP();
                //FIXIT
                //powerusercontext.pUsers.Add();
                start.frmDbConnectt dbWindow = new start.frmDbConnectt(/*new frmserverConnect()*/);
                dbWindow.Show();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtBussinesName.Focus();
            loadIndustryTypes();
        }

        private void txtEIN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                txtAdressLine1.Focus();
            }
            if (e.Key == Key.Enter)
            {
                
                //wizardgroup.ne();
            }
        }

        private void cmbIndustry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbIndustry.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbIndustry.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Employeess.frmEmployeeAdd employeeadd = new Employeess.frmEmployeeAdd();
                    employeeadd.ShowDialog();
                    loadIndustryTypes();

                }



            }
        }
    }

}
