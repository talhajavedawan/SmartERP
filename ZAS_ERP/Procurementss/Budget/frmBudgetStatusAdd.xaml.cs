using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Procurements.Budget;
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

namespace ZAS_ERP.Procurementss.Budget
{
    /// <summary>
    /// Interaction logic for frmBudgetStatusAdd.xaml
    /// </summary>
    public partial class frmBudgetStatusAdd : DXWindow
    {
        public static int StatusId;

        BudgetCostSheetStatus status = new BudgetCostSheetStatus();
        BudgetCostCenterRepo repo = new BudgetCostCenterRepo();
        public frmBudgetStatusAdd()
        {
            InitializeComponent();
        } 
     

        private void ColorEdit_ColorChanged(object sender, RoutedEventArgs e)
        {
            if (cpStatus.Color.R <= 120 || cpStatus.Color.G <= 120 || cpStatus.Color.B <= 120)
            {
                var myColor = "#FFFFFFFF";
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(myColor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                txtStatus.Foreground = new SolidColorBrush(newColor);
                status.forecolor = myColor;

            }
            else
            {
                var myColor = "#FF000000";
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(myColor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                txtStatus.Foreground = new SolidColorBrush(newColor);
                status.forecolor = myColor;
            }
            txtStatus.Background = new SolidColorBrush(cpStatus.Color);
        }

        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {




                status.Status = txtStatus.Text.Trim();
                if (chkisactive.IsChecked == true)
                    status.isActive = true;
                else
                    status.isActive = false;
                status.backcolor = cpStatus.Text.Trim();
                if (status.Id == 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget Status") != null)
                    {
                        repo.addStatus(status);
                        SystemLog.LogInfo(this.GetType(), "New Budget Payment Status(" + txtStatus.Text + ") Added");
                        MessageBox.Show("New Budget Payment Status (" + txtStatus.Text + ") Added", "Congratulations");
                    }
                    else
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission Required!!! You are not Allowed to add new Budget status.");
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budget Status") != null)
                    {
                        repo.updateStatus(status);

                        MessageBox.Show("Budget Payment Status (" + txtStatus.Text + ") updated", "Congratulations");
                        SystemLog.LogInfo(this.GetType(), " Budget Payment Status(" + txtStatus.Text + ") updated");
                    }
                    else
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission Required!!! You are not Allowed to add new Budget status.");
                }
                this.Close();
            }
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), " Budget Status error" + ex.ToString());

                MessageBox.Show(ex.ToString());
            }
        }

        private void winBudgetStatussAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void winBudgetStatussAdd_Loaded(object sender, RoutedEventArgs e)
        {
            if (StatusId != 0)
            {
                status = repo.getstatus(StatusId);
                txtStatus.Text = status.Status;
                chkisactive.IsChecked = status.isActive;
            }
        }
    }
}
