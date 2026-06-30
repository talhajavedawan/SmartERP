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

namespace ZAS_ERP.Procurementss.SaleOrderss.UserControls
{
    /// <summary>
    /// Interaction logic for ucGenerateSoKey.xaml
    /// </summary>
    public partial class ucGenerateSoKey : DXWindow
    {
        int saleOrderId = 0;
        int keyId = 0;
        public SaleOrdeRrefKey key = new SaleOrdeRrefKey();
        bool editFlag = false;
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        public ucGenerateSoKey()
        {
            InitializeComponent();
        }
        DepartmentRepo departmentRepo = new DepartmentRepo();
        CompanyRepo companyRepo = new CompanyRepo();
        public ucGenerateSoKey(int _saleOrderId)
        {
            InitializeComponent();
            saleOrderId = _saleOrderId;
        }
        public ucGenerateSoKey(int _KeyId,bool _editFlag)
        {
            InitializeComponent();
            keyId = _KeyId;
            editFlag = _editFlag;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(editFlag==true)
            {
                key.key = txtSOKey.Text;
                key.Creator = txtCreator.Text;
                key.keyDate = (DateTime)datKeyGen.EditValue;
                key.SaleOrderNumber = Convert.ToInt32(txtNoOfSaleOrder.Text);
                if (mbtnAllCompanies.SelectedIndex == -1)
                {
                    DXMessageBox.Show("Please select company");
                    return;
                }
                else
                {
                    key.comp_Id = (mbtnAllCompanies.SelectedItem as Company).Id;

                }
                if (mbtnAllDepartments.SelectedIndex == -1)
                {
                    DXMessageBox.Show("Please select department");
                    return;
                }
                else
                {
                    key.dept_Id = (mbtnAllDepartments.SelectedItem as Department).Id;
                }
                saleOrderRepo.UpdateKey(key);
                DXMessageBox.Show("Key updated successfully","Congratulations");
            }
            else
            {
                var saleOrder = saleOrderRepo.get(saleOrderId);
                key.key = txtSOKey.Text;
                key.Creator = txtCreator.Text;
                key.keyDate = (DateTime)datKeyGen.EditValue;
                key.amountOC = saleOrder.totalCFRValue;
                key.salesRefNo = saleOrder.SalesReferenceNo;
                key.SaleOrderNumber = Convert.ToInt32(txtNoOfSaleOrder.Text);
                if (mbtnAllCompanies.SelectedIndex == -1)
                {
                    DXMessageBox.Show("Please select company");
                    return;
                }
                else
                {
                    key.comp_Id = (mbtnAllCompanies.SelectedItem as Company).Id;

                }
                if (mbtnAllDepartments.SelectedIndex == -1)
                {
                    DXMessageBox.Show("Please select department");
                    return;
                }
                else
                {
                    key.dept_Id = (mbtnAllDepartments.SelectedItem as Department).Id;

                }
            }
            this.Close();
        }
        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            loadCompanies();
            loadDepartments();
            if (editFlag == true)
            {
                key = saleOrderRepo.GetSaleOrderRefKey(keyId);
                txtSOKey.Text = key.key;
                datKeyGen.EditValue = key.keyDate;
                mbtnAllCompanies.Text = key.Company.CompanyName;
                mbtnAllDepartments.Text = key.department.DeptName;
                txtCreator.Text = key.Creator;
                txtNoOfSaleOrder.Text = key.SaleOrderNumber.ToString();
            }
            else
            {
                txtCreator.Text = SYSTEM_STATIC.currentUser.userName;
                datKeyGen.EditValue = DateTime.Now;
                txtSOKey.Text = saleOrderId.ToString();
              
            }

        }
        public void loadCompanies()
        {
            mbtnAllCompanies.ItemsSource= companyRepo.GetAllActiveLinkAble();
        }
        public void loadDepartments()
        {
            mbtnAllDepartments.ItemsSource= departmentRepo.GetAllLinkAble();
        }
       
    }
}
