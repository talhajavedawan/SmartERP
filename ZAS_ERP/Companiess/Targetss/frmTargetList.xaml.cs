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
    public partial class frmTargetList : Window
    {


        List<Target> unitOfMeasures = new List<Target>();
        DepartmentRepo repo = new DepartmentRepo();

        //Target unitOfMeasure = new Target();
        public frmTargetList()
        {
            InitializeComponent();


        }

        private void winTargetList_Loaded(object sender, RoutedEventArgs e)
        {
            loadTarget();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdTarget);


        }



        private void loadTarget()
        {

            if (MainWindow.currentUserid == 0)
                unitOfMeasures = repo.getallTarget();
            else
                unitOfMeasures = repo.getActiveTargetsForDepartment(MainWindow.currentUserid);


            this.grdTarget.ItemsSource = unitOfMeasures;
            //grdTarget.Columns.GetColumnByFieldName("Id").Visible = false;
            //grdTarget.Columns.GetColumnByFieldName("user_Id").Visible = false;
            //grdTarget.Columns.GetColumnByFieldName("user").Visible = false;



        }

        public void newTarget()
        {

            frmTargetAdd frmTargetadd = new frmTargetAdd();
            frmTargetadd.ShowDialog();

            loadTarget();


        }


        private void mbtnNewTarget_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Target") != null)
            {
                newTarget();
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add new Target", "Permission Denied!");

            }

        }

        private void mbtnEditTarget_Click(object sender, RoutedEventArgs e)
        {
            if (grdTarget.SelectedItem != null)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Target") != null)
                {
                    Targetss.frmTargetAdd.targetId = (grdTarget.SelectedItem as Target).Id;
                    newTarget();
                }
                else
                {
                    DXMessageBox.Show("Permission Required to Edit Target", "Permission Denied!");

                }

            }
            else
            {
                DXMessageBox.Show("Please select a Target Type to Edit");
            }
        }



        private void grdTarget_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Target") != null)
            {
                Targetss.frmTargetAdd.targetId = (grdTarget.SelectedItem as Target).Id;
                newTarget();
            }
            else
            {
                DXMessageBox.Show("Permission Required to Edit Target", "Permission Denied!");

            }

        }

        private void WinTargetList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdTarget);
        }

        private void GrdTarget_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            var row = grdTarget.GetRowByListIndex( e.ListSourceRowIndex) as Target;
            //if(row== null)
            //{
            //    int rowHandle = grdTarget.View.GetValue(e.ListSourceRowIndex);
            //    bool isNewItemRow = GridView.IsNewItemRow(rowHandle);
            //}
            TargetAward award = new TargetAward();
            double avgMonthly=0;
            double avgIndviualAward = 0;
            double avgTotalAward = 0;
            double avgIndviuals = 0;


            if (row != null)
            {
                //var products = (e.GetListSourceFieldValue("TargetAwards")) as List<TargetAward>;
                award = row.TargetAwards.Find(x => x.name == "Total");
                avgMonthly = row.TargetAwards.Where(x => x.name != "Total" && x.name != "Extra").Average(x => x.target);
                avgIndviualAward = row.TargetAwards.Where(x => x.name != "Total" && x.name != "Extra").Average(x => x.IndviualAward);
                avgTotalAward = row.TargetAwards.Where(x => x.name != "Total" && x.name != "Extra").Average(x => x.TotalAward);
                avgIndviuals = row.TargetAwards.Where(x=> x.name != "Total" && x.name != "Extra").Average(x => x.NoOfEmployees );


            }
            var aa = e.GetListSourceFieldValue("TargetAwards");

            if (e.Column.FieldName == "AverageIndviuals" && e.IsGetData)

            {
                



                    e.Value = avgIndviuals;

                
            }
            //if (e.Column.FieldName == "AverageTotalAward" && e.IsGetData)

            //{
            //    e.Value = avgTotalAward;

            //}
            //if (e.Column.FieldName == "AverageMonthly" && e.IsGetData)

            //{
            //    e.Value = avgMonthly;

            //}
            //if (e.Column.FieldName == "AverageIndviualAward" && e.IsGetData)

            //{
            //    e.Value = avgIndviualAward;

            //}
            if (e.Column.FieldName == "TotalTarget" && e.IsGetData)

            {
                if (award != null)
                {



                    e.Value = award.target;

                }
            }
            //else if (e.Column.FieldName == "IndviualAward" && e.IsGetData)

            //{
            //    if (award != null)
            //    {



            //        e.Value = award.IndviualAward;

            //    }
            //}
            else if (e.Column.FieldName == "TotalAward" && e.IsGetData)

            {
                
                if (award != null)
                {



                    e.Value = award.TotalAward;

                }
                //int empid;



            }
        }

    }
}
