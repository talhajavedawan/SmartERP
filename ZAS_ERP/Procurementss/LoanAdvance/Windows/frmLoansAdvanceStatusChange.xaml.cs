using DevExpress.Xpf.Core;
using ERP_BL.Enums;
using ERP_BL.Procurements.LoansAdvances;
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
using ZAS_ERP.Procurementss.Advances.UserControls;
using ZAS_ERP.Procurementss.Billss;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.AdminBillLoan;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoan;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoansAdvance;
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.Procurementss.LoanAdvance.Windows
{
    /// <summary>
    /// Interaction logic for frmLoansAdvanceStatusChange.xaml
    /// </summary>
    public partial class frmLoansAdvanceStatusChange : UserControl
    {
        AdvanceRepo loansAdvanceRepo = new AdvanceRepo();
        ucAdvancesList loansAdvancesList = new ucAdvancesList();
        public UcListWindow directCloseWin = new UcListWindow();
        public bool frmFlag = false;
        public LoansAdvanceTemplate loansAdvanceTemplate;
        public LoansAdvanceType loansAdvanceType;
        public frmLoansAdvanceStatusChange()
        {
            InitializeComponent();
           
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<cmbitem> billStatusLst = new List<cmbitem>();
                var allBillsStatus = loansAdvanceRepo.GetAllClosedStatus();
                if (allBillsStatus != null)
                {
                    Parallel.ForEach(allBillsStatus, delegate (LoansAdvanceStatus status) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
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
                if (loansAdvanceTemplate == LoansAdvanceTemplate.Advance)
                {
                    if(loansAdvanceType == LoansAdvanceType.Admin_Bill)
                    {
                        if ((cmbBillStatus.SelectedItem as cmbitem) != null)
                        {
                            var status = loansAdvanceRepo.GetLoansAdvanceStatus((cmbBillStatus.SelectedItem as cmbitem).id);
                            if (status != null)
                            {
                                //receiptStatus.statusChanged = status;
                                if (frmFlag == true)
                                {
                                    ucFrmLoansAdvances frmBillAdd = new ucFrmLoansAdvances(status);
                                }
                                else
                                {
                                    ucAdvancesList receiptStatus = new ucAdvancesList(status);

                                }
                                directCloseWin.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Select a status before saving!");
                        }
                    }
                    else if(loansAdvanceType == LoansAdvanceType.Vendor_Bill)
                    {
                        if ((cmbBillStatus.SelectedItem as cmbitem) != null)
                        {
                            var status = loansAdvanceRepo.GetLoansAdvanceStatus((cmbBillStatus.SelectedItem as cmbitem).id);
                            if (status != null)
                            {
                                //receiptStatus.statusChanged = status;
                                if (frmFlag == true)
                                {
                                    ucFrmBillLoansAdvance frmBillAdd = new ucFrmBillLoansAdvance(status);
                                }
                                else
                                {
                                    ucAdvancesList receiptStatus = new ucAdvancesList(status);

                                }
                                directCloseWin.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Select a status before saving!");
                        }
                    }
                   
                }
                else if(loansAdvanceTemplate== LoansAdvanceTemplate.Loan)
                {
                    if (loansAdvanceType == LoansAdvanceType.Admin_Bill)
                    {
                        if ((cmbBillStatus.SelectedItem as cmbitem) != null)
                        {
                            var status = loansAdvanceRepo.GetLoansAdvanceStatus((cmbBillStatus.SelectedItem as cmbitem).id);
                            if (status != null)
                            {
                                //receiptStatus.statusChanged = status;
                                if (frmFlag == true)
                                {
                                    ucFrmAdminBillLoan frmBillAdd = new ucFrmAdminBillLoan(status);
                                }
                                else
                                {
                                    ucAdvancesList receiptStatus = new ucAdvancesList(status);
                                }
                                directCloseWin.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Select a status before saving!");
                        }
                    }
                    else if(loansAdvanceType == LoansAdvanceType.Vendor_Bill)
                    {
                        if ((cmbBillStatus.SelectedItem as cmbitem) != null)
                        {
                            var status = loansAdvanceRepo.GetLoansAdvanceStatus((cmbBillStatus.SelectedItem as cmbitem).id);
                            if (status != null)
                            {
                                //receiptStatus.statusChanged = status;
                                if (frmFlag == true)
                                {
                                    ucFrmVendorBillLoan frmBillAdd = new ucFrmVendorBillLoan(status);
                                }
                                else
                                {
                                    ucAdvancesList receiptStatus = new ucAdvancesList(status);
                                }
                                directCloseWin.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Select a status before saving!");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }


        }
    }
}
