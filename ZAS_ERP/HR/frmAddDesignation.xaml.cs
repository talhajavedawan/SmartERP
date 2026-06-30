using DevExpress.Xpf.Core;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZAS_ERP.HR
{
    /// <summary>
    /// Interaction logic for frmAddDesignation.xaml
    /// </summary>
    public partial class frmAddDesignation : Window
    {
        public bool isEdit = false;
        EmployeeRepo empRepo = new EmployeeRepo();
        public List<Department> loginUserDepts = new List<Department>();
        public List<Company> loginUserCompanies = new List<Company>();
        public frmAddDesignation()
        {
            InitializeComponent();
        }

        private void BtnSaveDesignations_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Designation desig = new Designation();

                // getting Designation Title
                if (String.IsNullOrEmpty(desgnationTxtbx.Text))
                {
                    MessageBox.Show("Please enter designation first");
                }
                else
                {
                    desig.Title = desgnationTxtbx.Text;
                }

                //Getting Parent
                if (desigIsSubsidary.IsChecked == false)
                {
                    desig.ParentId = null;
                    desig.parentDesignation = null;
                }
                else
                {
                    if (cmbxDesignations.SelectedItem != null)
                    {
                        var parent = cmbxDesignations.SelectedItem as Designation;
                        desig.parentDesignation = parent;
                    }

                }

                //GEtting isActive
                desig.isActive = (bool)chckisActive.IsChecked;
                if (!String.IsNullOrEmpty(txtAnnuaLeaves.Text))
                {
                    desig.AnnualLeaveDays = Convert.ToDouble(txtAnnuaLeaves.Text);
                }

                if (!String.IsNullOrEmpty(txtCasualLeaves.Text))
                {
                    desig.CasualLeaveDays = Convert.ToDouble(txtCasualLeaves.Text);
                }
                if (isEdit == false)
                {
                    empRepo.addDesignation(desig);
                    DXMessageBox.Show("Designation Added Successfully", "Successfull", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                if (isEdit == true)
                {
                    var desId = designationId.Text;
                    if (!String.IsNullOrEmpty(desId))
                    {
                        var desIdInt = Convert.ToInt32(desId);
                        desig.DesigId = desIdInt;
                    }
                    else
                    {
                        MessageBox.Show("Error in updating Designation");
                        return;
                    }

                    empRepo.updateDesignation(desig);
                    DXMessageBox.Show("Designation Updated Successfully", "Successfull", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
         

        }

        public void PopulateFields()
        {
            var currentUserId = SYSTEM_STATIC.currentUser.id;
            UsersRepo repo = new UsersRepo();
            var loginUser = repo.getuser(currentUserId);
            loginUserDepts = loginUser.employee.departments;
            loginUserCompanies = loginUser.employee.Companies;

            if (loginUserDepts != null)
            {
                cmbxDepartments.ItemsSource = loginUserDepts;
            }
            if (loginUserCompanies != null)
            {
                cmbxCompanies.ItemsSource = loginUserCompanies;
            }
            EmployeeRepo empRepo = new EmployeeRepo();
            var desig = empRepo.getAllDesignation();
            cmbxDesignations.ItemsSource = desig;

        }

        private void WinDesignationAdd_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateFields();
        }

        private void DesigIsSubsidary_Checked(object sender, RoutedEventArgs e)
        {
            cmbxDesignations.IsEnabled = true;
        }

        private void DesigIsSubsidary_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbxDesignations.IsEnabled = false;

        }
    }
}
