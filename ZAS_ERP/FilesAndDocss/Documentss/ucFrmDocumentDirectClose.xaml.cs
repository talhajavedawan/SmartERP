using ERP_BL.Documents;
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
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.FilesAndDocss.Documentss
{
    /// <summary>
    /// Interaction logic for ucFrmDocumentDirectClose.xaml
    /// </summary>
    public partial class ucFrmDocumentDirectClose : UserControl
    {
        DocumentRepo documentRepo = new DocumentRepo();
        ucDocumentList documentStatus = new ucDocumentList();
        public UcListWindow directCloseWin = new UcListWindow();
        public bool frmFlag = false;
        public ucFrmDocumentDirectClose()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<cmbitem> billStatusLst = new List<cmbitem>();
                var allBillsStatus = documentRepo.GetAllCloseDocumentStatus();
                if (allBillsStatus != null)
                {
                    Parallel.ForEach(allBillsStatus, delegate (DocumentStatus status) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
                    {
                        billStatusLst.Add
                        (new cmbitem()
                        {
                            name = status.Status,
                            id = status.Id,
                            bcolor = status.backcolor,
                            fcolor = "#FF000000"
                        });


                    });
                    cmbBillStatus.ItemsSource = billStatusLst;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Sale Receipt Status 
                if ((cmbBillStatus.SelectedItem as cmbitem) != null)
                {
                    var status = documentRepo.GetDocumentStatus((cmbBillStatus.SelectedItem as cmbitem).id);
                    if (status != null)
                    {
                        //receiptStatus.statusChanged = status;
                        if (frmFlag == true)
                        {
                            ucFrmDocumentAdd frmDocumentAdd = new ucFrmDocumentAdd(status);
                        }
                        else
                        {
                            ucDocumentList documentStatus = new ucDocumentList(status);

                        }
                        directCloseWin.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Select a status before saving!");
                }
            }
            catch
            {

            }


        }


    }
}
