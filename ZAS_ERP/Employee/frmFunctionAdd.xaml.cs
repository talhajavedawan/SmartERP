using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
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

namespace ZAS_ERP.Employee
{
    /// <summary>
    /// Interaction logic for frmFunctionAdd.xaml
    /// </summary>
    public partial class frmFunctionAdd : Window
    {
        EmployeeRepo empRepo = new EmployeeRepo();

        CompanyRepo cmpRepo = new CompanyRepo();
        public bool isEdit = false;

        public frmFunctionAdd()
        {
            InitializeComponent();
        }

        public void populateCmbx()
        {
            var cmp = cmpRepo.GetActiveCompanies();
            cmbxCompany.ItemsSource = cmp;



        }
        private void BtnFnxSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(cmbxFunctionType.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please Select Function Type!");
                    cmbxFunctionType.Focus();
                    return;
                }
                Function fn = new Function();

                if (isEdit == false)
                {

                    var title = txtTitle.Text;
                    var isActive = chckIsActive.IsChecked;

                    if (String.IsNullOrEmpty(title))
                    {
                        MessageBox.Show("Please Enter Function Title");
                        return;
                    }

                    fn.Title = title;
                    fn.IsActive = isActive;

                    fn.functionType = (FunctionType)cmbxFunctionType.SelectedIndex;

                    empRepo.AddFunction(fn);
                    MessageBox.Show("Function added successfuly ");
                    this.Close();
                }

                else if (isEdit == true)
                {
                    if (!String.IsNullOrEmpty(fnId.Text))
                    {
                        int FncId = Convert.ToInt32(fnId.Text);
                        fn.Id = FncId;
                        fn.Title = txtTitle.Text;
                        fn.IsActive = chckIsActive.IsChecked;

                        fn.functionType = (FunctionType)cmbxFunctionType.SelectedIndex;

                        empRepo.UpdateFunction(fn);
                        MessageBox.Show("Function updated successfuly ");
                        this.Close();

                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChckSubsidary_Checked(object sender, RoutedEventArgs e)
        {
            cmbxParent.IsEnabled = true;
        }

        private void ChckSubsidary_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbxParent.IsEnabled = false;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFunctionTypes();
        }

        private void LoadFunctionTypes()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.FunctionType.Other; i++)
            {
                cmbxFunctionType.Items.Add(((ERP_BL.Enums.FunctionType)i).ToString());
            }
        }

        private void CmbxCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //cmbxCompany.Text
        }
    }
}
