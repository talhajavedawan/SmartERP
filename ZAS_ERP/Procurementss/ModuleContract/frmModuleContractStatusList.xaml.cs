using DevExpress.Xpf.Core;
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
    /// Interaction logic for frmModuleContractStatusList.xaml
    /// </summary>
    public partial class frmModuleContractStatusList : Window
    {
        List<ModuleContractStatus> ModuleContractStatuss = new List<ModuleContractStatus>();
        ModuleContractRepo repo = new ModuleContractRepo();
        public frmModuleContractStatusList()
        {
            InitializeComponent();
        }
        private void winModuleContractStatusList_Loaded(object sender, RoutedEventArgs e)
        {
            loadModuleContractStatus();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdModuleContractStatus);


        }



        private void loadModuleContractStatus()
        {

            if (MainWindow.currentUserid == 0)
                ModuleContractStatuss = repo.getAllModuleContractStatus();

            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive ModuleContract Statuses") != null)
            {
                ModuleContractStatuss = repo.getAllModuleContractStatus();
            }
            else
            {
                ModuleContractStatuss = repo.getAllActiveStatus();
            }
            //ModuleContractStatuss = repo.getAllActiveModuleContractStatus();


            this.grdModuleContractStatus.ItemsSource = ModuleContractStatuss;
            grdModuleContractStatus.Columns.GetColumnByFieldName("Id").Visible = false;
            //grdModuleContractStatus.Columns.GetColumnByFieldName("user_Id").Visible = false;
            //grdModuleContractStatus.Columns.GetColumnByFieldName("user").Visible = false;



        }

        public void newModuleContractStatus()
        {
            ZAS_ERP.Procurementss.ModuleContract.frmModuleContractStatussAdd frmModuleContractStatussadd = new frmModuleContractStatussAdd();
            frmModuleContractStatussadd.ShowDialog();

            loadModuleContractStatus();
        }


        private void mbtnNewModuleContractStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add ModuleContract Status") != null)
            {
                newModuleContractStatus();
            }
            else
            {
                DXMessageBox.Show("Permission required to add new Status!");
            }

        }

        private void mbtnEditModuleContractStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit ModuleContract Status") != null)
            {
                if (grdModuleContractStatus.SelectedItem != null)
                {

                    ZAS_ERP.Procurementss.ModuleContract.frmModuleContractStatussAdd.StatusId = (grdModuleContractStatus.SelectedItem as ModuleContractStatus).Id;

                    newModuleContractStatus();
                }
                else
                {
                    MessageBox.Show("Please select a ModuleContract Payment Status to Edit");
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to add new Status!");
            }


        }



        private void grdModuleContractStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit ModuleContract Status") != null)
            {
                ZAS_ERP.Procurementss.ModuleContract.frmModuleContractStatussAdd.StatusId = (grdModuleContractStatus.SelectedItem as ModuleContractStatus).Id;
                newModuleContractStatus();
            }
            else
            {
                DXMessageBox.Show("Permission required to add new Status!");
            }

        }

        private void WinModuleContractStatusList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdModuleContractStatus);
        }
    }
}
