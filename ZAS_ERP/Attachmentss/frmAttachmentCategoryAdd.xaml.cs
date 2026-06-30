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
using ERP_BL;
using ERP_BL.Databases;

namespace ZAS_ERP.Attachmentss
{
    /// <summary>
    /// Interaction logic for frmAttachmentCategoryAdd.xaml
    /// </summary>
    public partial class frmAttachmentCategoryAdd : Window
    {
        public frmAttachmentCategoryAdd()
        {
            InitializeComponent();
        }
        public static int attachmentCategoryId;
        AttachmentsRepo repo = new AttachmentsRepo();
        AttachmentCategory attachmentCategory = new AttachmentCategory();
        AttachmentCategory parentCategory = new AttachmentCategory();

        private void btnattachmentCategorySave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtattachmentCategory.Text == "")
                {
                    MessageBox.Show("Please enter category name", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtattachmentCategory.Focus();
                    return;
                }

                attachmentCategory.description = txtDiscription.Text.Trim();
                attachmentCategory.Name = txtattachmentCategory.Text.Trim();
                if(parentCategory.Id!=0)
                attachmentCategory.ParentId = parentCategory.Id;
            if (chkisactive.IsChecked == true)
                attachmentCategory.isActive = true;
            else
                attachmentCategory.isActive = false;
                if (attachmentCategory.Id == 0)
                {
                    if (MainWindow.currentUserid != 0)
                        attachmentCategory.userId = MainWindow.currentUserid;
                    else
                    {
                    }
                    attachmentCategory.additionDate = System.DateTime.Now;
                    attachmentCategory.lastChangedDate = System.DateTime.Now;

                    repo.AddAttachmentCategory(attachmentCategory);

                    MessageBox.Show("New Attachment Category (" + txtattachmentCategory.Text + ") Added", "Congratulations");
                }
                else
                {
                    repo.UpdateAttachmentCategory(attachmentCategory);

                    MessageBox.Show("Attachment Category (" + txtattachmentCategory.Text + ") updated", "Congratulations");
                }
            this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void loadCategoris()
        {


            List<AttachmentCategory> categories = new List<AttachmentCategory>();
            categories = repo.getActiveAttachmentCategories();
            lookupCategory.ItemsSource = categories;

        }
        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lookupCategory_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            parentCategory = lookupCategory.SelectedItem as AttachmentCategory;
            if (parentCategory != null)
            {
                string selectedcust = parentCategory.Name;
                lookupCategory.EditValue = selectedcust;


            }
        }
        private void winAttachmentCategoryAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            attachmentCategoryId = 0;
        }

        private void winAttachmentCategoryAdd_Loaded(object sender, RoutedEventArgs e)
        {
            loadCategoris();
            if (attachmentCategoryId != 0)
            {
                this.Title = "Edit Attachment Category";
                attachmentCategory = repo.getAttachmentCategory(attachmentCategoryId);
                txtattachmentCategory.Text = attachmentCategory.Name;
                txtDiscription.Text = attachmentCategory.description;
                chkisactive.IsChecked = attachmentCategory.isActive;
                if (attachmentCategory.Inquiry==ERP_BL.Enums.TransactionItemType.Inquiry)
                {
                    radioInquiry.IsChecked = true;
                }
                if (attachmentCategory.Offer == ERP_BL.Enums.TransactionItemType.Offer)
                {
                    radioOffer.IsChecked = true;
                }
                if (attachmentCategory.SO == ERP_BL.Enums.TransactionItemType.Sale_Order)
                {
                    radioSO.IsChecked = true;
                }
                if (attachmentCategory.SI == ERP_BL.Enums.TransactionItemType.Sale_Invoice)
                {
                    radioSI.IsChecked = true;
                }
                if (attachmentCategory.SR == ERP_BL.Enums.TransactionItemType.Sale_Receipt)
                {
                   radioSR.IsChecked = true;
                }
                if (attachmentCategory.PO == ERP_BL.Enums.TransactionItemType.Purchase_Order)
                {
                    radioPO.IsChecked = true;
                }
                if (attachmentCategory.PI == ERP_BL.Enums.TransactionItemType.Purchase_Invoice)
                {
                    radioPI.IsChecked = true;
                }
                if (attachmentCategory.Payment == ERP_BL.Enums.TransactionItemType.Payments)
                {
                    radioPayment.IsChecked = true;
                }
                if (attachmentCategory.ABill == ERP_BL.Enums.TransactionItemType.Admin_Bill)
                {
                    radioABill.IsChecked = true;
                }
                if (attachmentCategory.VBill == ERP_BL.Enums.TransactionItemType.Bill)
                {
                    radioVBill.IsChecked = true;
                }
                if (attachmentCategory.IBT == ERP_BL.Enums.TransactionItemType.InterBank_Transfer)
                {
                    radioIbt.IsChecked = true;
                }
                if (attachmentCategory.ICBT == ERP_BL.Enums.TransactionItemType.InterCompanyBank_Transfer)
                {
                    radioIcbt.IsChecked = true;
                }
                if (attachmentCategory.MS == ERP_BL.Enums.TransactionItemType.Memorandum_Sale)
                {
                    radioMSales.IsChecked = true;
                }
                if (attachmentCategory.Tasks == ERP_BL.Enums.TransactionItemType.Tasks)
                {
                    radioTasks.IsChecked = true;
                }
                if (attachmentCategory.TargetReward == ERP_BL.Enums.TransactionItemType.TargetReward)
                {
                    radioTargetRewards.IsChecked = true;
                }
                if (attachmentCategory.ProcurementProducts == ERP_BL.Enums.TransactionItemType.ProcurementProducts)
                {
                    radioProcurementProd.IsChecked = true;
                }
                if (attachmentCategory.VehicleExpenses == ERP_BL.Enums.TransactionItemType.VehicleExpenses)
                {
                    radioVehicleExpenses.IsChecked = true;
                }
                if (attachmentCategory.RentalContract == ERP_BL.Enums.TransactionItemType.RentalContract)
                {
                    radioRentalContract.IsChecked = true;
                }
                if (attachmentCategory.RentalOrder == ERP_BL.Enums.TransactionItemType.RentalOrder)
                {
                    radioRentalOrder.IsChecked = true;
                }
                if (attachmentCategory.RentalInvoice == ERP_BL.Enums.TransactionItemType.RentalInvoice)
                {
                    radioRentalInvoice.IsChecked = true;
                }
                if (attachmentCategory.Memo == ERP_BL.Enums.TransactionItemType.Memo)
                {
                    radioMemo.IsChecked = true;
                }
                if (attachmentCategory.Document == ERP_BL.Enums.TransactionItemType.Document)
                {
                    radioDocument.IsChecked = true;
                }
                if (attachmentCategory.TravelingRecord == ERP_BL.Enums.TransactionItemType.TravelingRecord)
                {
                    radioTravelingRecord.IsChecked = true;
                }
                if (attachmentCategory.AssetRental == ERP_BL.Enums.TransactionItemType.AssetRental)
                {
                    radioAsset.IsChecked = true;
                }
                if (attachmentCategory.TenantRental == ERP_BL.Enums.TransactionItemType.TenantRental)
                {
                    radioTenant.IsChecked = true;
                }

                if (attachmentCategory.parentCategory != null && attachmentCategory.ParentId != 0)
                {
                    parentCategory = attachmentCategory.parentCategory;
                    lookupCategory.SelectedItem = lookupCategory.GetItemByKeyValue(parentCategory);
                }
            }
        }

        private void RadioInquiry_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.Inquiry = ERP_BL.Enums.TransactionItemType.Inquiry;

        }

        private void RadioOffer_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.Offer = ERP_BL.Enums.TransactionItemType.Offer;

        }

        private void RadioSO_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.SO = ERP_BL.Enums.TransactionItemType.Sale_Order;

        }

        private void RadioSI_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.SI = ERP_BL.Enums.TransactionItemType.Sale_Invoice;

        }

        private void RadioPO_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.PO = ERP_BL.Enums.TransactionItemType.Purchase_Order;

        }

        private void RadioSR_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.SR = ERP_BL.Enums.TransactionItemType.Sale_Receipt;

        }

        private void RadioPI_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.PI = ERP_BL.Enums.TransactionItemType.Purchase_Invoice;

        }

        private void RadioPayment_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.Payment = ERP_BL.Enums.TransactionItemType.Payments;

        }

        private void RadioMSales_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.MS = ERP_BL.Enums.TransactionItemType.Memorandum_Sale;
        }
        private void RadioVBill_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.VBill = ERP_BL.Enums.TransactionItemType.Bill;
        }

        private void RadioABill_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.ABill = ERP_BL.Enums.TransactionItemType.Admin_Bill;

        }

        private void RadioIbt_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.IBT = ERP_BL.Enums.TransactionItemType.InterBank_Transfer;

        }

        private void RadioIcbt_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.ICBT = ERP_BL.Enums.TransactionItemType.InterCompanyBank_Transfer;

        }

        private void RadioInquiry_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.Inquiry = null;
        }

        private void RadioOffer_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.Offer = null;
        }

        private void RadioSO_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.SO = null;
        }

        private void RadioSI_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.SI = null;
        }

        private void RadioPO_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.PO = null;
        }

        private void RadioSR_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.SR = null;
        }

        private void RadioPI_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.PI = null;
        }

        private void RadioPayment_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.Payment = null;

        }

        private void RadioMSales_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.MS = null;
        }

        private void RadioVBill_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.VBill = null;
        }

        private void RadioABill_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.ABill = null;
        }

        private void RadioIbt_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.IBT = null;
        }

        private void RadioIcbt_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.ICBT = null;
        }

        private void RadioLoansAdvances_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.LoansAdvances = ERP_BL.Enums.TransactionItemType.LoansAdvances;
        }

        private void RadioLoansAdvances_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.LoansAdvances = null;
        }

        private void RadioTasks_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.Tasks = ERP_BL.Enums.TransactionItemType.Tasks;
        }

        private void RadioTasks_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.Tasks = null;
        }

        private void RadioTargetRewards_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.TargetReward = ERP_BL.Enums.TransactionItemType.TargetReward;
        }

        private void RadioTargetRewards_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.TargetReward = null;
        }

        private void RadioTravelingRecord_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.TravelingRecord = ERP_BL.Enums.TransactionItemType.TravelingRecord;
        }

        private void RadioTravelingRecord_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.TravelingRecord = null;
        }

        private void radioSTL_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.STL = ERP_BL.Enums.TransactionItemType.STL;
        }
        private void radioSTL_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.STL = null;
        }

        private void RadioProcurementProd_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.ProcurementProducts = ERP_BL.Enums.TransactionItemType.ProcurementProducts;
        }

        private void RadioProcurementProd_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.ProcurementProducts = null;
        }

        private void RadioVehicleExpenses_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.VehicleExpenses = ERP_BL.Enums.TransactionItemType.VehicleExpenses;
        }

        private void RadioVehicleExpenses_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.VehicleExpenses = null;
        }

        private void radioRentalContract_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.RentalContract = null;
        }

        private void radioRentalContract_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.RentalContract = ERP_BL.Enums.TransactionItemType.RentalContract;
        }

        private void radioRentalOrder_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.RentalOrder = ERP_BL.Enums.TransactionItemType.RentalOrder;
        }

        private void radioRentalOrder_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.RentalOrder = null;
        }

        private void radioRentalInvoice_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.RentalInvoice = ERP_BL.Enums.TransactionItemType.RentalInvoice;
        }

        private void radioRentalInvoice_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.RentalInvoice = null;
        }

        private void radioMemo_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.Memo = ERP_BL.Enums.TransactionItemType.Memo;
        }

        private void radioMemo_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.Memo = null;
        }

        private void radioModuleContract_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.ModuleContract = null;

        }

        private void radioModuleContract_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.ModuleContract = ERP_BL.Enums.TransactionItemType.ModuleContract;

        }

        private void radioDocumentContract_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.Document = ERP_BL.Enums.TransactionItemType.Document;
        }

        private void radioDocumentContract_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.Document = null;
        }

        private void radioAsset_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.AssetRental = ERP_BL.Enums.TransactionItemType.AssetRental;
        }

        private void radioAsset_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.Document = null;
        }

        private void radioTenant_Checked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.TenantRental = ERP_BL.Enums.TransactionItemType.TenantRental;
        }

        private void radioTenant_Unchecked(object sender, RoutedEventArgs e)
        {
            attachmentCategory.TenantRental = null;
        }
    }
}
