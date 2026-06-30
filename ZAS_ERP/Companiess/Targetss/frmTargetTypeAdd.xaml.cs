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
using DevExpress.Xpf.Core;
using ERP_BL;
using ERP_BL.Databases;
using ERP_BL.Enums;

namespace ZAS_ERP.Targetss
{
    /// <summary>
    /// Interaction logic for frmTargetTypeAdd.xaml
    /// </summary>
    public partial class frmTargetTypeAdd : Window
    {
        public frmTargetTypeAdd()
        {
            InitializeComponent();
        }
        public static int targetId;
        DepartmentRepo repo = new DepartmentRepo();
        TargetType target = new TargetType();
        private void btnMeasureSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMeasure.Text))
                {
                    DXMessageBox.Show("Enter Name for Target Type");

                    return;
                }
                else if (cmbFrequency.SelectedIndex == -1)
                {
                    DXMessageBox.Show("Select a Target Frequency to continue");

                    return;
                }
                else if (string.IsNullOrEmpty( spnHierarchicalIndex.Text))
                {
                    DXMessageBox.Show("Select a Hierarchical Index to continue");

                    return;
                }

                target.Type = txtMeasure.Text.Trim();
                target.HierarchicalIndex = Convert.ToInt32(spnHierarchicalIndex.Text);
                if (cmbFrequency.SelectedItem.ToString() == TargetFrequency.Monthly.ToString())
                    target.Frequency = TargetFrequency.Monthly;
                else if (cmbFrequency.SelectedItem.ToString() == TargetFrequency.Yearly.ToString())
                    target.Frequency = (TargetFrequency.Yearly);

                if (chkisactive.IsChecked == true)
                    target.isActive = true;
                else
                    target.isActive = false;
                if (target.Id == 0)
                {
                    if (MainWindow.currentUserid != 0)
                        target.user_Id = MainWindow.currentUserid;
                    else
                        target.user_Id = null;
                    repo.AddTargetType(target);

                    MessageBox.Show("New Target Type (" + txtMeasure.Text + ") Added", "Congratulations");
                }
                else
                {
                    repo.UpdateTargetType(target);

                    MessageBox.Show("Target Type (" + txtMeasure.Text + ") updated", "Congratulations");
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void winTargetTypeAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            targetId = 0;
        }

        private void winTargetTypeAdd_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTargetFrequncies();
            if (targetId != 0)
            {
                target = repo.getTargetType(targetId);
                txtMeasure.Text = target.Type;
                if (target.Frequency == TargetFrequency.Monthly)
                    cmbFrequency.SelectedIndex = 0;
                else if (target.Frequency == TargetFrequency.Monthly)
                    cmbFrequency.SelectedIndex = 1;
                spnHierarchicalIndex.Value = target.HierarchicalIndex;

                chkisactive.IsChecked = target.isActive;
            }
        }

        private void LoadTargetFrequncies()
        {
            cmbFrequency.ItemsSource = repo.GetTargetFrequencies();
        }
    }
}
