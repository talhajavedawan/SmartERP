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

namespace ZAS_ERP.Employee
{
    /// <summary>
    /// Interaction logic for winAddFunctionList.xaml
    /// </summary>
    public partial class winAddFunctionList : Window
    {
        EmployeeRepo empRepo = new EmployeeRepo();
        public winAddFunctionList()
        {
            InitializeComponent();

            
        }

        public void addNewFnx()
        {
            frmFunctionAdd frm = new frmFunctionAdd();
            frm.isEdit = false;
            frm.ShowDialog();
            loadFunctions();
        }

        public void editFnx()
        {
           

            var focRow = (Function)grdFunctionsLst.GetFocusedRow();
            if (focRow != null)
            {
                frmFunctionAdd frm = new frmFunctionAdd();
                frm.isEdit = true;

                frm.fnId.Text = focRow.Id.ToString();
                frm.txtTitle.Text = focRow.Title;
                frm.chckIsActive.IsChecked = (bool)focRow.IsActive;

                frm.ShowDialog();
                loadFunctions();
            }


          
        }


        public void loadFunctions()
        {
             empRepo = new EmployeeRepo();

            var fnx = empRepo.GetAllFunctions();
            grdFunctionsLst.ItemsSource = fnx;
            grdFunctionsLst.Columns["Id"].Visible = false;
     
            grdFunctionsLst.Columns["company"].Visible = false;


        }
        private void MbtnNewAssetStatus_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnNewFunction_Click(object sender, RoutedEventArgs e)
        {
            addNewFnx();
        }

        private void BtnNewFunction_Click(object sender, RoutedEventArgs e)
        {
            addNewFnx();

        }

        private void BtnEditFunction_Click(object sender, RoutedEventArgs e)
        {
            editFnx();
        }
       

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //var fnx = empRepo.GetAllFunctions();
            //if(fnx != null)
            //grdFunctionsLst.ItemsSource = fnx;
            loadFunctions();

        }
    }
}
