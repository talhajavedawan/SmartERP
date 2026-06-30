using DevExpress.Xpf.Grid;
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
using ERP_BL;

using ERP_BL.Config;
using ERP_BL.Enums;
using DevExpress.Xpf.Core;

namespace ZAS_ERP.Targetss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmTargetTypeList : Window
    {


        List<TargetType> unitOfMeasures= new List<TargetType>();
        DepartmentRepo repo = new DepartmentRepo();

        //TargetType unitOfMeasure = new TargetType();
        public frmTargetTypeList()
        {
            InitializeComponent();
            
            
        }

        private void winTargetTypeList_Loaded(object sender, RoutedEventArgs e)
        {
            loadTargetType();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdTargetType);


        }



        private void loadTargetType()
        {
           
            if (MainWindow.currentUserid == 0)
                unitOfMeasures = repo.getallTargetType();
            else
                unitOfMeasures = repo.getActiveTargetTypes();

            
            this.grdTargetType.ItemsSource = unitOfMeasures;
            //grdTargetType.Columns.GetColumnByFieldName("Id").Visible = false;
            //grdTargetType.Columns.GetColumnByFieldName("user_Id").Visible = false;
            //grdTargetType.Columns.GetColumnByFieldName("user").Visible = false;

            

        }

        public void newTargetType()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Target Type") != null)
            {
                frmTargetTypeAdd frmTargetTypeadd = new frmTargetTypeAdd();
                frmTargetTypeadd.ShowDialog();

                loadTargetType();
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add new Target Type", "Permission Denied!");

            }
            
        }
        

        private void mbtnNewTargetType_Click(object sender, RoutedEventArgs e)
        {
            newTargetType();

        }

        private void mbtnEditTargetType_Click(object sender, RoutedEventArgs e)
        {
            if (grdTargetType.SelectedItem != null)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Target Type") != null)
                {
                    Targetss.frmTargetTypeAdd.targetId = (grdTargetType.SelectedItem as TargetType).Id;
                    newTargetType();
                }
                else
                {
                    DXMessageBox.Show("Permission Required to Edit Target Type","Permission Denied!");

                }

            }
            else
            {
                DXMessageBox.Show("Please select a Target Type to Edit");
            }
        }



        private void grdTargetType_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Target Type") != null)
            {
                Targetss.frmTargetTypeAdd.targetId = (grdTargetType.SelectedItem as TargetType).Id;
                newTargetType();
            }
            else
            {
                DXMessageBox.Show("Permission Required to Edit Target Type", "Permission Denied!");

            }
            
        }

        private void WinTargetTypeList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdTargetType);
        }
        //int empid;



    }
}
