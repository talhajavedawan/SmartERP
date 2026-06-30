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
    /// Interaction logic for frmEmployeeStatusAdd.xaml
    /// </summary>
    public partial class frmEmployeeStatusAdd : Window
    {
        public bool isEdit = false;
        EmployeeRepo repo = new EmployeeRepo();

        public frmEmployeeStatusAdd()
        {
            InitializeComponent();
        }

        private void BtnEmpStatusSave_Click(object sender, RoutedEventArgs e)
        {
            EmployeeWorkingStatus status = new EmployeeWorkingStatus();

            if (isEdit == false)
            {
                if (empStatusTxtbx.Text == null)
                {
                    MessageBox.Show("Please enter Employee Status");
                    return;
                }
                if (empStatusColorEdit.Text == null)
                {
                    MessageBox.Show("Please enter Employee Status Color");
                    return;

                }
                if (empStatusTxtbx.Text != null || empStatusColorEdit.Text != null)
                {
                    status.Status = empStatusTxtbx.Text;
                    status.backcolor = empStatusColorEdit.Text;
                    status.isActive = empStatusIsActive.IsChecked;
                }


                repo.AddEmployeeStatus(status);
                MessageBox.Show("Employee Status added successfuly ");
                this.Close();

            }
            if (isEdit == true)
            {
                if (empStatusTxtbx.Text == null)
                {
                    MessageBox.Show("Please enter Asset Status");
                    return;
                }
                if (empStatusColorEdit.Text == null)
                {
                    MessageBox.Show("Please enter Asset Status Color");
                    return;

                }
                if (empStatusTxtbx.Text != null || empStatusColorEdit.Text != null)
                {
                    status.Status = empStatusTxtbx.Text;
                    status.backcolor = empStatusColorEdit.Text;
                    status.isActive = empStatusIsActive.IsChecked;
                }
                if (empStatusId.Text != null)
                {
                    status.Id = Convert.ToInt32(empStatusId.Text);
                }

                repo.UpdateEmployeeStatus(status);
                MessageBox.Show("Employee Status updated successfuly ");
                this.Close();

            }

        }
    }
}
