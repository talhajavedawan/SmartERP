using ERP_BL.HR;
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

namespace ZAS_ERP.HR.Leaves
{
    /// <summary>
    /// Interaction logic for frmLeaveStatusAdd.xaml
    /// </summary>
    public partial class frmLeaveStatusAdd : Window
    {
        public bool isEdit = false;
        HrRepo repo = new HrRepo();

        public frmLeaveStatusAdd()
        {
            InitializeComponent();
        }

        private void BtnLeaveStatusSave_Click(object sender, RoutedEventArgs e)
        {
            LeaveStatus status = new LeaveStatus();

            if (isEdit == false)
            {
                if (leaveStatusTxtbx.Text == null)
                {
                    MessageBox.Show("Please enter Leave Status");
                    return;
                }
                if (leaveStatusColorEdit.Text == null)
                {
                    MessageBox.Show("Please enter Leave Status Color");
                    return;

                }
                if (!String.IsNullOrEmpty(leaveStatusTxtbx.Text)  || !String.IsNullOrEmpty(leaveStatusColorEdit.Text))
                {
                    status.Status = leaveStatusTxtbx.Text;
                    status.backcolor = leaveStatusColorEdit.Text;
                    status.isActive = leaveStatusIsActive.IsChecked;
                }


                repo.AddLeaveStatus(status);
                MessageBox.Show("Leave Status added successfuly ");
                this.Close();

            }
            if (isEdit == true)
            {
                if (leaveStatusTxtbx.Text == null)
                {
                    MessageBox.Show("Please enter Leave Status");
                    return;
                }
                if (leaveStatusColorEdit.Text == null)
                {
                    MessageBox.Show("Please enter Leave Status Color");
                    return;

                }
                if (leaveStatusTxtbx.Text != null || leaveStatusColorEdit.Text != null)
                {
                    status.Status = leaveStatusTxtbx.Text;
                    status.backcolor = leaveStatusColorEdit.Text;
                    status.isActive = leaveStatusIsActive.IsChecked;
                }
                if (leaveStatusId.Text != null)
                {
                    status.Id = Convert.ToInt32(leaveStatusId.Text);
                }

                repo.UpdateLeaveStatus(status);
                MessageBox.Show("Leave Status updated successfuly ");
                this.Close();

            }
        }
    }
}
