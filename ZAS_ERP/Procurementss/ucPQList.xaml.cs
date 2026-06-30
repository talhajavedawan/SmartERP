using DevExpress.Xpf.Core;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZAS_ERP.Procurementss.CostSheet
{
    /// <summary>
    /// Interaction logic for ucPQList.xaml
    /// </summary>
    public partial class ucPQList : UserControl
    {
        public SaleOrderRepo repo = new SaleOrderRepo();

        public ucPQList()
        {
            InitializeComponent();
        }
        private void AddPQ_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add PQ Documents") != null)
            {
                frmPQAdd frmBillReferenceNo = new frmPQAdd();
                frmBillReferenceNo.editFlag = false;
                Window win = new Window();
                win.Content = frmBillReferenceNo;
                win.Height = 350;
                win.Width = 400;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add PQ Documents!");
            }
        }

        private void UpdatePQ_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit PQ Documents") != null)
            {
                var selectedRow = grdPQ.SelectedItem as PQDocument;

                if (selectedRow != null)
                {
                    frmPQAdd frmPQ = new frmPQAdd();
                    frmPQ.editFlag = true;
                    frmPQ.document = repo.GetPQ(selectedRow.Id);
                    Window win = new Window();
                    win.Content = frmPQ;
                    win.Height = 350;
                    win.Width = 400;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.ResizeMode = ResizeMode.CanMinimize;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to Edit PQ Documents!");
            }

        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            repo = new SaleOrderRepo();
            grdPQ.ItemsSource = repo.GetPQDocuments();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            repo = new SaleOrderRepo();

            var documents = repo.GetPQDocuments();
            grdPQ.ItemsSource = documents;
        }
    }
}
