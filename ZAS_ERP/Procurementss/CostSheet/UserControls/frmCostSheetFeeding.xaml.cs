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
using System.Windows.Shapes;
using ZAS_ERP.Procurementss.SaleOrderss;

namespace ZAS_ERP.Procurementss.CostSheet.UserControls
{
    /// <summary>
    /// Interaction logic for frmCostSheetFeeding.xaml
    /// </summary>
    public partial class frmCostSheetFeeding : DXWindow
    {
        SaleOrder saleOrder = new SaleOrder();
        Offer offer = new Offer();
        CostFieldValues costFieldValues = new CostFieldValues();
        public CostFieldValues row = new CostFieldValues();
        public frmCostSheetFeeding()
        {
            InitializeComponent();
        }
        public frmCostSheetFeeding(SaleOrder _saleOrder, CostFieldValues _costFieldValues)
        {
            InitializeComponent();
            saleOrder = _saleOrder;
            costFieldValues = _costFieldValues;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budgeted Costs") == null)
            {
                txtBudgetCost.IsReadOnly = true;

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Advance Costs") == null)
            {
                txtAdvanceCost.IsReadOnly = true;

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Vendor Nature") == null)
            {
                lookupVendorNature.IsReadOnly = true;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Vendor Rating") == null)
            {
                ratingControl.IsReadOnly = true;
            }
            //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Adjustment System Costs") == null)
            //{
            //    grdCostItems.Columns.GetColumnByFieldName("adjSCost").ReadOnly = true;

            //}
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Actual Costs") == null)
            {
                txtActualCost.IsReadOnly = true;

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs with out Approval") != null)
            {
               txtRevisedCost.IsReadOnly = false;
            }
            else
            {
                txtRevisedCost.IsReadOnly = true;

            }
            if (saleOrder.isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Cost Center after SO Approval") == null)
            {
                lookupVendors.IsReadOnly = true;
                txtAccountHead.IsReadOnly = true;
                txtAccountHead.IsReadOnly = true;
                txtMaker.IsReadOnly = true;
                txtBudgetCost.IsReadOnly = true;
                txtOrigin.IsReadOnly = true;
                chkPacking.IsEnabled = false;
                txtDeliveryDate.IsReadOnly = true;
                cmbPaymentTerm.IsReadOnly = true;
                cmbWarranty.IsReadOnly = true;
                cmbIncoterm.IsReadOnly = true;
                chkDgGood.IsEnabled = false;
                lookupPQ.IsReadOnly = true;
                txtLoadingPort.IsReadOnly = true;
                txtIntermediaryPort.IsReadOnly = true;
                txtDestinationPort.IsReadOnly = true;
                txtHSCode.IsReadOnly = true;
                chkDrawingRequired.IsEnabled = false;
                txtAttestedCOO.IsReadOnly = true;
                lookupST.IsReadOnly = true;
                chkExportLicense.IsEnabled = false;
                cmbCurrency.IsReadOnly = true;
                txtExchangeRate.IsReadOnly = true;
                txtOCAmount.IsReadOnly = true;
                cmbCurrency.IsEnabled = false;
                cmbPaymentTerm.IsEnabled = false;
                cmbIncoterm.IsEnabled = false;
                cmbWarranty.IsEnabled = false;
               
            }

        }
        public frmCostSheetFeeding(Offer _offer, CostFieldValues _costFieldValues)
        {
            InitializeComponent();
            offer = _offer;
            costFieldValues = _costFieldValues;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budgeted Costs") == null)
            {
                txtBudgetCost.IsReadOnly = true;

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Advance Costs") == null)
            {
                txtAdvanceCost.IsReadOnly = true;

            }
            //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Adjustment System Costs") == null)
            //{
            //    grdCostItems.Columns.GetColumnByFieldName("adjSCost").ReadOnly = true;

            //}
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Actual Costs") == null)
            {
                txtActualCost.IsReadOnly = true;

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs with out Approval") != null)
            {
               txtRevisedCost.IsReadOnly = false;
            }
            else
            {
                txtRevisedCost.IsReadOnly = true;

            }
            if (offer.isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Cost Center after Offer Approval") == null)
            {
                //lookupVendors.IsReadOnly = true;
                txtAccountHead.IsReadOnly = true;
                txtAccountHead.IsReadOnly = true;
                txtMaker.IsReadOnly = true;
                txtOrigin.IsReadOnly = true;
                chkPacking.IsEnabled = false;
                txtDeliveryDate.IsReadOnly = true;
                cmbPaymentTerm.IsReadOnly = true;
                cmbWarranty.IsReadOnly = true;
                cmbIncoterm.IsReadOnly = true;
                chkDgGood.IsEnabled = false;
                lookupPQ.IsReadOnly = true;
                txtLoadingPort.IsReadOnly = true;
                txtIntermediaryPort.IsReadOnly = true;
                txtDestinationPort.IsReadOnly = true;
                txtHSCode.IsReadOnly = true;
                chkDrawingRequired.IsEnabled = false;
                txtAttestedCOO.IsReadOnly = true;
                lookupST.IsReadOnly = true;
                chkExportLicense.IsEnabled = false;
                //cmbCurrency.IsReadOnly = true;
                txtExchangeRate.IsReadOnly = true;
                txtOCAmount.IsReadOnly = true;
                //cmbCurrency.IsEnabled = false;
                cmbPaymentTerm.IsEnabled = false;
                cmbIncoterm.IsEnabled = false;
                cmbWarranty.IsEnabled = false;
                txtRevisedCost.IsEnabled = false;
                txtActualCost.IsEnabled = false;

            }

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadPaymentTerms();
            loadIncoterms();
            loadWarrantys();
            loadVendors();
            loadShippingTerms();
            loadPQDocuments();
            loadVendorNatureManual();
            loadCurrencies();
            if (costFieldValues.Id!=0)
            {
                row.Id = costFieldValues.Id;
                if (costFieldValues.vendor != null)
                {

                    lookupVendors.Text = costFieldValues.vendor.company.CompanyName;
                    ratingControl.EditValue = costFieldValues.vendor.Rating;
                    txtVendorIndustryType.Text = costFieldValues.vendor.company.industryType.name;
                    if(costFieldValues.vendor.vendorNature!=null)
                        lookupVendorNature.Text = costFieldValues.vendor.vendorNature.name;

                    //txtVendorNatureS.Text = costFieldValues.vendor.vendorNature.name;
                }
                //if (costFieldValues.vendorNature!=null)
                //{
                //    lookupVendorNature.Text = costFieldValues.vendorNature.name;
                //}
                txtAccountHead.Text = costFieldValues.Title;
                txtBudgetCost.Text = costFieldValues.budgetedValue.ToString();
                txtRevisedCost.Text = costFieldValues.revisedValue.ToString();
                txtActualCost.Text = costFieldValues.actualValue.ToString();
                txtMaker.Text = costFieldValues.Maker;
                txtOrigin.Text = costFieldValues.Origin;
                txtDeliveryDays.Text = costFieldValues.deliveryDays;
                if(costFieldValues.Packing==true)
                {
                    chkPacking.IsChecked = true;
                }
                else
                {
                    chkPacking.IsChecked = false;
                }
                txtDeliveryDate.EditValue = costFieldValues.deliveryDate;

                if (costFieldValues.PaymentTerm != null )
                {
                    var paymentTermSource = (List<cmbitem>)cmbPaymentTerm.Items.SourceCollection;
                    var term = paymentTermSource.Find(x => x.id == costFieldValues.PaymentTerm.Id);
                    if (term == null)
                    {
                        paymentTermSource.Add(new cmbitem() { name = costFieldValues.PaymentTerm.term, id = costFieldValues.PaymentTerm.Id });
                        cmbPaymentTerm.ItemsSource = null;
                        cmbPaymentTerm.ItemsSource = paymentTermSource;
                    }
                    cmbPaymentTerm.SelectedItem = cmbPaymentTerm.Items[cmbPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == costFieldValues.PaymentTerm.Id))];
                }
                if (costFieldValues.IncotermName != null)
                {
                    var incotermSourceChange = (List<cmbitem>)cmbIncoterm.Items.SourceCollection;

                    cmbIncoterm.SelectedItem = cmbIncoterm.Items[cmbIncoterm.Items.IndexOf(incotermSourceChange.Find(x => x.id == costFieldValues.IncotermName.Id))];

                }
                if ( costFieldValues.Warranty != null)
                {
                    var incoSource = (List<cmbitem>)cmbWarranty.Items.SourceCollection;

                    cmbWarranty.SelectedItem = cmbWarranty.Items[cmbWarranty.Items.IndexOf(incoSource.Find(x => x.id == costFieldValues.Warranty.Id))];
                }
                if (costFieldValues.OC != null)
                {
                    var currencySource = (List<cmbitem>)cmbCurrency.Items.SourceCollection;
                    cmbCurrency.SelectedItem = cmbCurrency.Items[cmbCurrency.Items.IndexOf(currencySource.Find(x => x.id == costFieldValues.OC.Id))];
                }
                txtOCAmount.Text = costFieldValues.OCamount.ToString();
                txtExchangeRate.Text = costFieldValues.exchangeRate.ToString();
                if (costFieldValues.Dg_Goods == true)
                {
                    chkDgGood.IsChecked = true;
                }
                else
                {
                    chkDgGood.IsChecked = false;
                }
                if (costFieldValues.PQ != null)
                    lookupPQ.Text = costFieldValues.PQ.Name;
                txtLoadingPort.Text = costFieldValues.LoadingPort;
                txtDestinationPort.Text = costFieldValues.DestinationPort;
                txtIntermediaryPort.Text = costFieldValues.IntermediaryPort;
                txtHSCode.Text = costFieldValues.HScode;
                txtAdvanceCost.Text = costFieldValues.advanceValue.ToString();
                if (costFieldValues.DrawaingRequired == true)
                {
                    chkDrawingRequired.IsChecked = true;
                }
                else
                {
                    chkDrawingRequired.IsChecked = false;
                }
                txtAttestedCOO.Text = costFieldValues.AttestedCOO;
                if(costFieldValues.ST!=null)
                lookupST.Text = costFieldValues.ST.Name;


                if (costFieldValues.isExportLicense == true)
                {
                    chkExportLicense.IsChecked = true;
                }
                else
                {
                    chkExportLicense.IsChecked = false;
                }

            }

        }
        private void loadShippingTerms()
        {

            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            List<ShippingTerm> terms = saleOrderRepo.GetShippingTerms();

            lookupST.ItemsSource = terms;

        }
        private void loadPQDocuments()
        {

            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            List<PQDocument> documents = saleOrderRepo.GetPQDocuments();

            lookupPQ.ItemsSource = documents;

        }  
        private void loadVendorNatureManual()
        {
            CompanyRepo companyRepo = new CompanyRepo();
            List<VendorNature> vendorNatures = companyRepo.GetVendorNatures();
            lookupVendorNature.ItemsSource = vendorNatures;
        }
        public void loadVendors()
        {
            if (saleOrder.Id != 0 && saleOrder.department != null && saleOrder.vendors.Count != 0)
                lookupVendors.ItemsSource = saleOrder.department.Vendors;
            if (offer.Id != 0 && offer.department != null && offer.vendors.Count != 0)
                lookupVendors.ItemsSource = offer.department.Vendors;
        }

        public void loadIncoterms()
        {
            cmbIncoterm.ItemsSource = SYSTEM_STATIC.incoTermSource;
        }
        public void loadWarrantys()
        {
            cmbWarranty.ItemsSource = SYSTEM_STATIC.warrantySource;
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void loadCurrencies()
        {
            cmbCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
        }
        public void loadPaymentTerms()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            try
            {
                PaymentTermRepo TermRepo = new PaymentTermRepo();
                List<PaymentTerm> paymentTerms = new List<PaymentTerm>();
                paymentTerms = TermRepo.getAllForSO();
                foreach (var paymentTerm in paymentTerms)
                {
                    cmbitems.Add(new cmbitem() { name = paymentTerm.term, id = paymentTerm.Id });
                }
                cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

                cmbPaymentTerm.ItemsSource = cmbitems;
            }
            catch (Exception ex) { }
        }

        private void lookupVendors_PopupContentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vendor = lookupVendors.SelectedItem as Vendor;
            if (vendor != null)
            {
                if ((DateTime)saleOrder.CreationDate != null && (DateTime)saleOrder.CreationDate > new DateTime(2025, 2, 17) && vendor.isBlackList == true)
                {
                    return; // or whatever action you need to take
                }
                else
                if (vendor.isBlackList == true)
                {

                    lookupVendors.Background = Brushes.DarkRed;
                    lookupVendors.Foreground = Brushes.White;
                }
                else
                {
                    lookupVendors.Background = Brushes.Transparent;
                    lookupVendors.Foreground = Brushes.Black;
                }
                txtVendorIndustryType.Text = vendor.company.industryType.name;
                if(vendor.vendorNature!=null)
                lookupVendorNature.Text = vendor.vendorNature.name;
                ratingControl.EditValue = vendor.Rating;

            }

            
        }
        private void DXWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PaymentTermRepo paymentTermRepo = new PaymentTermRepo();
            IncotermRepo incotermRepo = new IncotermRepo();
            VendorRepo vendorRepo = new VendorRepo();
            if (lookupVendors.SelectedItem != null)
            {
                
                var vendor = vendorRepo.get((int)(lookupVendors.SelectedItem as Vendor).Id);
                row.vendor = lookupVendors.SelectedItem as Vendor;
                vendor.Rating = Convert.ToInt32(ratingControl.EditValue);
                if(lookupVendorNature.SelectedIndex>-1)
                vendor.vendorNatureId = (lookupVendorNature.SelectedItem as VendorNature).Id;
                vendorRepo.Update(vendor);
            }
            row.budgetedValue = Convert.ToDecimal(txtBudgetCost.Text);
            row.advanceValue = Convert.ToDecimal(txtAdvanceCost.Text);
            row.Title = txtAccountHead.Text;
            row.revisedValue = Convert.ToDecimal(txtRevisedCost.Text); 
            row.actualValue = Convert.ToDecimal(txtActualCost.Text);
            row.Maker = txtMaker.Text;
            row.Origin = txtOrigin.Text;
            row.LoadingPort = txtLoadingPort.Text;
            row.IntermediaryPort = txtIntermediaryPort.Text;
            row.DestinationPort = txtDestinationPort.Text;
            row.deliveryDays = txtDeliveryDays.Text;
            //if(lookupVendorNature.SelectedIndex>-1)
            //    row.vendorNature = lookupVendorNature.SelectedItem as VendorNature;
            if (chkPacking.IsChecked == true)
            {
                row.Packing = true;
            }
            else
            {
                row.Packing = false;
            }
            if (txtDeliveryDate.EditValue != null)
            {
                row.deliveryDate = (DateTime)txtDeliveryDate.EditValue;
            }

            if (cmbPaymentTerm.SelectedItem != null)
            {
                row.PaymentTerm = paymentTermRepo.get((cmbPaymentTerm.SelectedItem as cmbitem).id);
            }
            if (cmbWarranty.SelectedItem != null)
            {
                row.Warranty = paymentTermRepo.getWarrenty((cmbWarranty.SelectedItem as cmbitem).id);
            }
            if (cmbIncoterm.SelectedItem != null)
            {
                var incoterm = incotermRepo.get((cmbIncoterm.SelectedItem as cmbitem).id);
                IncoTermName incoTermName = new IncoTermName()
                {
                    Id = incoterm.Id,
                    termName = incoterm.term,
                    discription = incoterm.discription,
                    isActive = incoterm.isActive
                };
                row.IncotermName = incoTermName;
            }
            if (chkDgGood.IsChecked == true)
            {
                row.Dg_Goods = true;
            }
            else
            {
                row.Dg_Goods = false;
            }
            if (lookupPQ.SelectedItem != null)
            {
                row.PQ = lookupPQ.SelectedItem as PQDocument;
            }
            if (lookupST.SelectedItem != null)
            {
                row.ST = lookupST.SelectedItem as ShippingTerm;
            }
            row.HScode = txtHSCode.Text;
            row.AttestedCOO = txtAttestedCOO.Text;
            if (chkDrawingRequired.IsChecked == true)
            {
                row.DrawaingRequired = true;
            }
            else
            {
                row.DrawaingRequired = false;
            }
            if (chkExportLicense.IsChecked == true)
            {
                row.isExportLicense = true;
            }
            else
            {
                row.isExportLicense = false;
            }
            if (cmbCurrency.SelectedIndex != -1)
            {
                CurrencyRepo currencyRepo = new CurrencyRepo();
                row.OC = currencyRepo.get((cmbCurrency.SelectedItem as cmbitem).id);
            }
            row.exchangeRate = Convert.ToDecimal(txtExchangeRate.Text);
            row.OCamount = Convert.ToDecimal(txtOCAmount.Text);
        }
    }
}
