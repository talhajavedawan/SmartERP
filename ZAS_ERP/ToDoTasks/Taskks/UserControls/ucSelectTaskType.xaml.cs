using DevExpress.Xpf.Core;
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

namespace ZAS_ERP.ToDoTasks.Taskks.UserControls
{
    /// <summary>
    /// Interaction logic for ucSelectTaskType.xaml
    /// </summary>
    public partial class ucSelectTaskType : UserControl
    {
        public ucSelectTaskType()
        {
            InitializeComponent();
        }

        private void BtnContinue_Click(object sender, RoutedEventArgs e)
        {
            if (cmbxType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Type!");
                cmbxType.Focus();
                return;
            }
            if (cmbxType.SelectedIndex == 0)
            {
               
                    Window Win = new Window();
                    ucFrmTaskTypeAdd frmTaskTypeAdd = new ucFrmTaskTypeAdd();
                frmTaskTypeAdd.isProcurementType = true;
                frmTaskTypeAdd.editFlag = false;
                Win.Content = frmTaskTypeAdd;
                Win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                Win.WindowState = WindowState.Maximized;
                Win.Show();

                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
               
            }
            else if (cmbxType.SelectedIndex == 1)
            {
                
                    Window Win = new Window();
                    ucFrmTaskTypeAdd frmTaskTypeAdd = new ucFrmTaskTypeAdd();
                frmTaskTypeAdd.isProcurementType = false;
                frmTaskTypeAdd.editFlag = false;
                Win.Content = frmTaskTypeAdd;
                    Win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    Win.WindowState = WindowState.Maximized;
                    Win.Show();

                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
               

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbxType.Items.Add("Module Dependent");
            cmbxType.Items.Add("Independent");
        }
    }
}
