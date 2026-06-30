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

namespace ZAS_ERP.Procurementss.Offerss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmOfferStatusList : Window
    {


        List<OfferStatus> OfferStatuss= new List<OfferStatus>();
        OfferRepo repo = new OfferRepo();

        //OfferStatus OfferStatus = new OfferStatus();
        public frmOfferStatusList()
        {
            InitializeComponent();
            
            
        }

        private void winOfferStatusList_Loaded(object sender, RoutedEventArgs e)
        {
            loadOfferStatus();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdOfferStatus);


        }



        private void loadOfferStatus()
        {
           
            if (MainWindow.currentUserid == 0)
                OfferStatuss = repo.getAllOfferStatus();
            
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Offer Statuses") != null)
            {
                OfferStatuss = repo.getAllOfferStatus();
            }
            else
            {
                OfferStatuss = repo.getAllActiveStatus();
            }
            //OfferStatuss = repo.getAllActiveOfferStatus();

            
            this.grdOfferStatus.ItemsSource = OfferStatuss;
            grdOfferStatus.Columns.GetColumnByFieldName("Id").Visible = false;
            //grdOfferStatus.Columns.GetColumnByFieldName("user_Id").Visible = false;
            //grdOfferStatus.Columns.GetColumnByFieldName("user").Visible = false;

            

        }

        public void newOfferStatus()
        {
            frmOfferStatussAdd frmOfferStatussadd = new frmOfferStatussAdd();
            frmOfferStatussadd.ShowDialog();
            
            loadOfferStatus();
        }
        

        private void mbtnNewOfferStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Offer Status") != null)
            {
                newOfferStatus();
            }
            else
            {
                DXMessageBox.Show("Permission required to add new Status!");
            } 

        }

        private void mbtnEditOfferStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Offer Status") != null)
            {
                if (grdOfferStatus.SelectedItem != null)
                {

                    Offerss.frmOfferStatussAdd.StatusId = (grdOfferStatus.SelectedItem as OfferStatus).Id;

                    newOfferStatus();
                }
                else
                {
                    MessageBox.Show("Please select a Offer Payment Status to Edit");
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to add new Status!");
            }

            
        }



        private void grdOfferStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Offer Status") != null)
            {
                Offerss.frmOfferStatussAdd.StatusId = (grdOfferStatus.SelectedItem as OfferStatus).Id;
                newOfferStatus();
            }
            else
            {
                DXMessageBox.Show("Permission required to add new Status!");
            }
            
        }

        private void WinOfferStatusList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdOfferStatus);
        }
        //int empid;



    }
}
