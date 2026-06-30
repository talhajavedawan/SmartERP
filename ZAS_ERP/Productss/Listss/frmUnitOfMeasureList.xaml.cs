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

namespace ZAS_ERP.Productss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmUnitOfMeasureList : Window
    {


        List<UnitOfMeasure> unitOfMeasures= new List<UnitOfMeasure>();
        ProductRepo repo = new ProductRepo();

        //UnitOfMeasure unitOfMeasure = new UnitOfMeasure();
        public frmUnitOfMeasureList()
        {
            InitializeComponent();
            
            
        }

        private void winUnitOfMeasureList_Loaded(object sender, RoutedEventArgs e)
        {
            loadUnitOfMeasure();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdUnitOfMeasure);


        }



        private void loadUnitOfMeasure()
        {
           
            if (MainWindow.currentUserid == 0)
                unitOfMeasures = repo.getallUnitOfMeasure();
            else
                unitOfMeasures = repo.getActiveUnitOfMeasures();

            
            this.grdUnitOfMeasure.ItemsSource = unitOfMeasures;
            grdUnitOfMeasure.Columns.GetColumnByFieldName("Id").Visible = false;
            grdUnitOfMeasure.Columns.GetColumnByFieldName("user_Id").Visible = false;
            grdUnitOfMeasure.Columns.GetColumnByFieldName("user").Visible = false;

            

        }

        public void newUnitOfMeasure()
        {
            frmUnitOfMeasureAdd frmUnitOfMeasureadd = new frmUnitOfMeasureAdd();
            frmUnitOfMeasureadd.ShowDialog();
            
            loadUnitOfMeasure();
        }
        

        private void mbtnNewUnitOfMeasure_Click(object sender, RoutedEventArgs e)
        {
            newUnitOfMeasure();

        }

        private void mbtnEditUnitOfMeasure_Click(object sender, RoutedEventArgs e)
        {
            if (grdUnitOfMeasure.SelectedItem != null)
            {

                Productss.frmUnitOfMeasureAdd.unitOfMeasureId = (grdUnitOfMeasure.SelectedItem as UnitOfMeasure).Id;
                newUnitOfMeasure();
            }
            else
            {
                MessageBox.Show("Please select a Unit Of Measure to Edit");
            }
        }



        private void grdUnitOfMeasure_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Productss.frmUnitOfMeasureAdd.unitOfMeasureId = (grdUnitOfMeasure.SelectedItem as UnitOfMeasure).Id;
            newUnitOfMeasure();
        }

        private void WinUnitOfMeasureList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdUnitOfMeasure);
        }
        //int empid;



    }
}
