using ERP_BL.Databases;
using ERP_BL.Procurements;
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

namespace ZAS_ERP.Procurementss.ModuleContract
{
    /// <summary>
    /// Interaction logic for frmModuleContractStatussAdd.xaml
    /// </summary>
    public partial class frmModuleContractStatussAdd : Window
    {
        public frmModuleContractStatussAdd()
        {
            InitializeComponent();
        }
        public static int StatusId;
        ModuleContractRepo repo = new ModuleContractRepo();
        ModuleContractStatus status = new ModuleContractStatus();
        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                status.Status = txtStatus.Text.Trim();
                if (chkisactive.IsChecked == true)
                    status.isActive = true;
                else
                    status.isActive = false;
                if (chkisDisable.IsChecked == true)
                    status.isDisable = true;
                else
                    status.isDisable = false;
                status.backcolor = cpStatus.Text.Trim();
                //repo.addStatus(status);
                if (status.Id == 0)
                {
                    //if (MainWindow.currentUserid != 0)
                    //    ModuleContractStatus.user_Id = MainWindow.currentUserid;
                    //else
                    //    ModuleContractStatus.user_Id = null;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add ModuleContract Status") != null)
                    {
                        repo.addStatus(status);
                        SystemLog.LogInfo(this.GetType(), "New ModuleContract Payment Status(" + txtStatus.Text + ") Added");
                        MessageBox.Show("New ModuleContract Payment Status (" + txtStatus.Text + ") Added", "Congratulations");
                    }
                    else
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission Required!!! You are not Allowed to add new ModuleContract status.");
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit ModuleContract Status") != null)
                    {
                        repo.updateStatus(status);

                        MessageBox.Show("ModuleContract Payment Status (" + txtStatus.Text + ") updated", "Congratulations");
                        SystemLog.LogInfo(this.GetType(), " ModuleContract Payment Status(" + txtStatus.Text + ") updated");
                    }
                    else
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission Required!!! You are not Allowed to add new ModuleContract status.");
                    //    repo.updateStatus(ModuleContractStatus);

                    //MessageBox.Show("ModuleContract Payment Status (" + txtStatus.Text + ") updated", "Congratulations");
                }
                this.Close();
            }
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), " ModuleContract Status error" + ex.ToString());

                MessageBox.Show(ex.ToString());
            }
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
        private void winModuleContractStatusAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StatusId = 0;
        }

        private void winModuleContractStatusAdd_Loaded(object sender, RoutedEventArgs e)
        {
            if (StatusId != 0)
            {
                status = repo.getstatus(StatusId);
                txtStatus.Text = status.Status;
                chkisactive.IsChecked = status.isActive;
                chkisDisable.IsChecked = status.isDisable;
            }
        }
    }
}
