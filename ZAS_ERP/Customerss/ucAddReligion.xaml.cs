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

namespace ZAS_ERP.Customerss
{
    /// <summary>
    /// Interaction logic for ucAddReligion.xaml
    /// </summary>
    public partial class ucAddReligion : UserControl
    {
        public Window addCategoryWindow = new Window();
        CustomerCompRepo customerCompRepo = new CustomerCompRepo();
        Religion religion = new Religion();
        public bool editFlag = false;
        public int ReligionId = 0;
        public ucAddReligion()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if(editFlag == true)
                {
                    religion = customerCompRepo.GetReligion(ReligionId);
                    txtMethodName.Text = religion.ReligionName;

                    if (religion.IsActive == true)
                        chkIsActive.IsChecked = true;
                    else
                        chkIsActive.IsChecked = false;
                }
            }
            catch (Exception)
            {

                
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtMethodName.Text))
                {
                    DXMessageBox.Show("Please enter Category Name!");
                    txtMethodName.Focus();
                    return;
                }
                religion.ReligionName = txtMethodName.Text;
                if (chkIsActive.IsChecked == true)
                    religion.IsActive = true;
                else
                    religion.IsActive = false;

                if (editFlag == false && religion.Id == 0)
                {
                    customerCompRepo.AddReligionType(religion);
                    DXMessageBox.Show("Successfully Added!");
                    addCategoryWindow.Close();
                }
                else if (editFlag == true && religion.Id != 0)
                {
                    customerCompRepo.UpdateReligionType(religion); 
                    DXMessageBox.Show("Updated Successfully!");
                    addCategoryWindow.Close();
                }
            }
            catch (Exception)
            {

               
            }
        }
    }
}
