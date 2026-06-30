using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.ExchangeRates;
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

namespace ZAS_ERP.Procurementss.Offerss
{
    /// <summary>
    /// Interaction logic for frmComparativeStatement.xaml
    /// </summary>
    public partial class frmComparativeStatement : DXWindow
    {
        VendorRepo vendorRepo = new VendorRepo();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        IncotermRepo incotermRepo = new IncotermRepo();
        public static int offerId;
        OfferRepo offerRepo = new OfferRepo();
        CurrencyRepo currencyRepo = new CurrencyRepo();
        public virtual List<ComparativeStatementItem> items { get; set; }
        public static ComparativeStatement  comparativeStatement = new ComparativeStatement();
        public static List<ComparativeStatement> comparativeStatements = new List<ComparativeStatement>();

        Offer offer = new Offer();
        ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
        static List<ExchangeRateGroup> exchangeRateGroupsMER = new List<ExchangeRateGroup>();
        ExchangeRate exchangeRateMER = null;
        public int compId=0;
        public frmComparativeStatement()
        {
            InitializeComponent();
        }
        public frmComparativeStatement(int _offerId)
        {
            InitializeComponent();
            offerId = _offerId;
        }
        public void LoadMERGroups()
        {
            exchangeRateGroupsMER = exchangeRateGroupRepo.GetAllMER();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                

                LoadUsers();
               
                LoadMERGroups();
                if (offerId != 0)
                {
                    offer = offerRepo.GetForComparativeStatement(offerId);
                    LoadLists();

                    if (compId != 0)
                    {
                        comparativeStatement = offerRepo.GetCompatativeStatementById(compId);

                        txtOfferValue.Text = comparativeStatement.offerValue.ToString();
                        //foreach (cmbitem cmbitem in lookupOfferCurrency.Items)
                        //{
                        //    if (cmbitem.id == comparativeStatement.offerCurrencyId)
                        //    {
                        //        lookupOfferCurrency.SelectedItem = cmbitem;
                        //    }
                        //}
                        lookupOfferCurrency.Text = comparativeStatement.offerCurrency.CurrencyName;
                        if(comparativeStatement.incoTerm!=null)
                        lookupOfferIncoTerm.Text = comparativeStatement.incoTerm.term;
                        if (comparativeStatement.itemIncoTerm != null)
                            lookupVendorIncoTerm.Text = comparativeStatement.itemIncoTerm.term;
                        txtVendorName.Text = comparativeStatement.vendorName;
                        if (comparativeStatement.vendor != null)
                            lookupVendor.Text = comparativeStatement.vendor.company.CompanyName;
                        if (comparativeStatement.creator != null)
                            lookupCreator.Text = comparativeStatement.creator.userName;
                        txtVendorName.Text = comparativeStatement.vendorName;
                       
                        if (comparativeStatement.product != null)
                            lookupProduct.Text = comparativeStatement.product.inquiryProduct.ownDiscription;
                        if (comparativeStatement.comparativeStatementItems != null)
                        {
                            grdComparativeItems.ItemsSource = comparativeStatement.comparativeStatementItems;
                        }
                        if (comparativeStatement.isSelect == true)
                        {
                            checkSelctedVendor.IsChecked = true;
                        }
                        else
                        {
                            checkSelctedVendor.IsChecked = false;
                        }
                    }
                    else
                    {
                        comparativeStatement = new ComparativeStatement();
                        txtOfferValue.Text = offer.totalCFRValue.ToString();
                        //foreach (cmbitem cmbitem in lookupOfferCurrency.Items)
                        //{
                        //    if (cmbitem.id == offer.currency_Id)
                        //    {
                        //        lookupOfferCurrency.SelectedItem = cmbitem;
                        //    }
                        //}
                        lookupOfferCurrency.Text = offer.currency.CurrencyName;

                        lookupOfferIncoTerm.Text = offer.incoterm.term;
                        lookupCreator.Text = SYSTEM_STATIC.currentUser.userName;



                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
        public void LoadUsers()
        {
            UsersRepo usersRepo = new UsersRepo();
            lookupCreator.ItemsSource = usersRepo.getAllActiveUsersForComparativeStatements();
        }
        public void LoadLists()
        {
            CurrencyRepo repo = new CurrencyRepo();
            ProcurementRepo procurementRepo = new ProcurementRepo();
            items = new List<ComparativeStatementItem>();
            grdComparativeItems.ItemsSource = items;
            lookupVendor.ItemsSource = vendorRepo.getCurrentUserVendors(SYSTEM_STATIC.currentUser.id);
            lookupOfferIncoTerm.ItemsSource = incotermRepo.getAll();
            lookupVendorIncoTerm.ItemsSource= incotermRepo.getAll();
            lookupOfferCurrency.ItemsSource = repo.getAll();
            lookupConvertedCurrency.ItemsSource = currencyRepo.getAll();
            lookupItemCurrency.ItemsSource = currencyRepo.getAll();
            lookupCostSheetField.ItemsSource = procurementRepo.GetAllCostSheetFields();
            var dbOffer = procurementRepo.GetComparativeOffer(offerId);
            lookupProduct.ItemsSource = offer.products;
        }

        private void view_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                //if (lookupVendor.SelectedIndex == -1)
                //{
                //    DXMessageBox.Show("Please select vendor", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;
                //}
                //else
          if (lookupOfferIncoTerm.SelectedIndex == -1)
                {
                    DXMessageBox.Show("Please select offer incoterm", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
          if (lookupOfferCurrency.SelectedIndex == -1)
                {
                    DXMessageBox.Show("Please select offer currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                if (grdComparativeItems.ItemsSource == null)
                {
                    DXMessageBox.Show("Please select offer currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                if (lookupCreator.SelectedIndex==-1)
                {
                    DXMessageBox.Show("Please select creator", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }    
                if (lookupProduct.SelectedIndex==-1)
                {
                    DXMessageBox.Show("Please select product", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (lookupVendorIncoTerm.SelectedIndex == -1)
                {
                    DXMessageBox.Show("Please select vendor incoterm", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    if (comparativeStatement.Id != 0)
                    {
                        comparativeStatement.incoTerm_Id = (lookupOfferIncoTerm.SelectedItem as Incoterm).Id;
                        comparativeStatement.vendorIncoTermId = (lookupVendorIncoTerm.SelectedItem as Incoterm).Id;
                        comparativeStatement.offerCurrencyId = (lookupOfferCurrency.SelectedItem as Currency).Id;
                        if (lookupVendor.SelectedItem != null)
                        {
                            var vendor = vendorRepo.Get((lookupVendor.SelectedItem as Vendor).Id);
                            comparativeStatement.VendorId = vendor.Id;

                        }
                        comparativeStatement.offerValue = Convert.ToDouble(txtOfferValue.Text);
                        comparativeStatement.creatorId = (lookupCreator.SelectedItem as User).id;
                        comparativeStatement.productId = (lookupProduct.SelectedItem as ProcurementProduct).Id;
                        if(!string.IsNullOrEmpty(txtVendorName.Text))
                        {
                            comparativeStatement.vendorName = txtVendorName.Text;
                        }
                        if(checkSelctedVendor.IsChecked==true)
                        {
                            comparativeStatement.isSelect = true;
                        }
                        else
                        {
                            comparativeStatement.isSelect = false;
                        }
                        foreach (var item in comparativeStatement.comparativeStatementItems)
                        {
                            if (item.Id == 0)
                            {
                                if(item.itemCurrency != null)
                                item.itemCurrencyId = item.itemCurrency.Id;
                                if (item.costSheetField != null)
                                    item.costSheetFieldId = item.costSheetField.Id;
                                //if (item.itemIncoTerm != null)
                                //    item.itemIncoTermId = item.itemIncoTerm.Id;

                                item.MER = item.MER;
                                if (item.convertedCurrency != null)
                                    item.convertedCurrencyId = item.convertedCurrency.Id;
                                item.amountOC = item.amountOC;
                                item.amountMER = item.amountMER;
                                item.comparativeStatementId = comparativeStatement.Id;
                                item.ownDescription = item.ownDescription;
                                item.itemCurrency = null;
                                item.costSheetField = null;
                                //item.itemIncoTerm = null;
                                //item.MER = item.MER;
                                item.convertedCurrency = null;

                            }
                        }
                        offerRepo.UpdateCompatativeStatement(comparativeStatement);



                    }
                    else
                    {
                        comparativeStatement = new ComparativeStatement();
                        if (lookupVendor.SelectedItem != null)
                        {
                            var vendor = vendorRepo.Get((lookupVendor.SelectedItem as Vendor).Id);
                            comparativeStatement.VendorId = vendor.Id;

                        }
                        comparativeStatement.incoTerm_Id = (lookupOfferIncoTerm.SelectedItem as Incoterm).Id;
                        comparativeStatement.vendorIncoTermId = (lookupVendorIncoTerm.SelectedItem as Incoterm).Id;

                        comparativeStatement.offerCurrencyId = (lookupOfferCurrency.SelectedItem as Currency).Id;
                        comparativeStatement.offerValue = Convert.ToDouble(txtOfferValue.Text);
                        comparativeStatement.creatorId = (lookupCreator.SelectedItem as User).id;
                        comparativeStatement.productId = (lookupProduct.SelectedItem as ProcurementProduct).Id;
                        if (checkSelctedVendor.IsChecked == true)
                        {
                            comparativeStatement.isSelect = true;
                        }
                        else
                        {
                            comparativeStatement.isSelect = false;
                        }
                        if (!string.IsNullOrEmpty(txtVendorName.Text))
                        {
                            comparativeStatement.vendorName = txtVendorName.Text;
                        }
                        if (items.Count > 0)
                        {
                            comparativeStatement.comparativeStatementItems = new List<ComparativeStatementItem>();

                            foreach (var item in items)
                            {

                                ComparativeStatementItem item1 = new ComparativeStatementItem();
                                item1.comparativeStatementId = item.Id;
                                if (item.itemCurrency != null)
                                {
                                    var convCurrency = currencyRepo.get((int)item.convertedCurrency.Id);
                                    item1.convertedCurrencyId = convCurrency.Id;

                                }
                                if (item.itemCurrency != null)
                                {
                                    var itemCurrency = currencyRepo.get((int)item.itemCurrency.Id);
                                    item1.itemCurrencyId = itemCurrency.Id;

                                }
                                if (item.costSheetField != null)
                                {
                                    var costSheetfield = procurementRepo.GetSheetField((int)item.costSheetField.Id);
                                    item1.costSheetFieldId = costSheetfield.Id;


                                }
                                //if (item.itemIncoTerm != null)
                                //{
                                //    var itemIncoTerm = incotermRepo.get((int)item.itemIncoTerm.Id);
                                //    item1.itemIncoTermId = itemIncoTerm.Id;

                                //}
                                item1.MER = item.MER;
                                item1.amountOC = item.amountOC;
                                item1.amountMER = item.amountMER;
                                item.ownDescription = item.ownDescription;
                                comparativeStatement.comparativeStatementItems.Add(item1);
                            }
                        }
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void view_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            try
            {
                if (grdComparativeItems.GetFocusedRow() != null)
                {
                    var offerMER = offer;
                    double todayRate = 0;
                    double totalItemCurrencyValue = 0;
                    int totalConvertedRate = 0;

                    if ((e.Cell.Row as ComparativeStatementItem).itemCurrency != null && (e.Cell.Row as ComparativeStatementItem).convertedCurrency != null)
                    {


                        int transactionCurrency = Convert.ToInt32((e.Cell.Row as ComparativeStatementItem).itemCurrency.Id);
                        int convertdCurrency = Convert.ToInt32((e.Cell.Row as ComparativeStatementItem).convertedCurrency.Id);
                        var itemCurrencyGroup = exchangeRateGroupsMER.FirstOrDefault(x => x.transaction_currency_Id == transactionCurrency && x.base_currency_Id == offer.company.CurrencyId && x.TargetYear == offerMER.CreationDate.Value.Year);
                        var convertedCurrencyGroup = exchangeRateGroupsMER.FirstOrDefault(x => x.transaction_currency_Id == convertdCurrency && x.base_currency_Id == offer.company.CurrencyId && x.TargetYear == offerMER.CreationDate.Value.Year);
                        if (convertdCurrency != null && transactionCurrency != null)
                        {
                            if (itemCurrencyGroup != null)
                            {
                                exchangeRateMER = itemCurrencyGroup.exchangeRates.FirstOrDefault(x => x.company_Id == offerMER.company_Id);
                                switch (offerMER.CreationDate.Value.Month)
                                {
                                    case 1:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateJan;
                                        break;
                                    case 2:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateFeb;
                                        break;
                                    case 3:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateMar;
                                        break;
                                    case 4:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateApr;
                                        break;
                                    case 5:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateMay;
                                        break;
                                    case 6:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateJun;
                                        break;
                                    case 7:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateJul;
                                        break;
                                    case 8:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateAug;
                                        break;
                                    case 9:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateSep;
                                        break;
                                    case 10:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateOct;
                                        break;
                                    case 11:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateNov;
                                        break;
                                    case 12:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateDec;
                                        break;
                                    default:
                                        if (exchangeRateMER != null)
                                            todayRate = 0;
                                        break;
                                }

                                var total = todayRate;
                                //(e.Row as ComparativeStatementItem).MER = total;
                                 totalItemCurrencyValue = (e.Row as ComparativeStatementItem).amountOC * total;
                                
                                //(e.Row as ComparativeStatementItem).amountMER = total * (e.Row as ComparativeStatementItem).amountOC;
                            }
                            if (convertedCurrencyGroup != null)
                            {
                                exchangeRateMER = convertedCurrencyGroup.exchangeRates.FirstOrDefault(x => x.company_Id == offerMER.company_Id);
                                switch (offerMER.CreationDate.Value.Month)
                                {
                                    case 1:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateJan;
                                        break;
                                    case 2:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateFeb;
                                        break;
                                    case 3:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateMar;
                                        break;
                                    case 4:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateApr;
                                        break;
                                    case 5:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateMay;
                                        break;
                                    case 6:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateJun;
                                        break;
                                    case 7:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateJul;
                                        break;
                                    case 8:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateAug;
                                        break;
                                    case 9:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateSep;
                                        break;
                                    case 10:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateOct;
                                        break;
                                    case 11:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateNov;
                                        break;
                                    case 12:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateDec;
                                        break;
                                    default:
                                        if (exchangeRateMER != null)
                                            todayRate = 0;
                                        break;
                                }

                                var rate = todayRate;
                                //(e.Row as ComparativeStatementItem).MER = total;
                                //var totalPKR = (e.Row as ComparativeStatementItem).amountOC * total;
                                var amountMER= totalItemCurrencyValue / rate;

                                (e.Row as ComparativeStatementItem).amountMER = amountMER;
                                (e.Row as ComparativeStatementItem).MER = amountMER/(e.Row as ComparativeStatementItem).amountOC;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void grdComparativeItems_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                switch (e.Column.FieldName)
                {
                    case "MER":
                        var row = grdComparativeItems.GetRowByListIndex(e.ListSourceRowIndex) as ComparativeStatementItem;
                        var offerMER = offer;
                        double todayRate = 0;
                        var exchangeRateGroupMER = exchangeRateGroupsMER.FirstOrDefault(x => x.transaction_currency_Id == row.itemCurrencyId && x.base_currency_Id == row.convertedCurrencyId && x.TargetYear == offerMER.CreationDate.Value.Year);

                        if (exchangeRateGroupMER != null)
                        {
                            exchangeRateMER = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == offerMER.company_Id);
                            switch (offerMER.CreationDate.Value.Month)
                            {
                                case 1:
                                    if (exchangeRateMER != null)
                                        todayRate = exchangeRateMER.rateJan;
                                    break;
                                case 2:
                                    if (exchangeRateMER != null)
                                        todayRate = exchangeRateMER.rateFeb;
                                    break;
                                case 3:
                                    if (exchangeRateMER != null)
                                        todayRate = exchangeRateMER.rateMar;
                                    break;
                                case 4:
                                    if (exchangeRateMER != null)
                                        todayRate = exchangeRateMER.rateApr;
                                    break;
                                case 5:
                                    if (exchangeRateMER != null)
                                        todayRate = exchangeRateMER.rateMay;
                                    break;
                                case 6:
                                    if (exchangeRateMER != null)
                                        todayRate = exchangeRateMER.rateJun;
                                    break;
                                case 7:
                                    if (exchangeRateMER != null)
                                        todayRate = exchangeRateMER.rateJul;
                                    break;
                                case 8:
                                    if (exchangeRateMER != null)
                                        todayRate = exchangeRateMER.rateAug;
                                    break;
                                case 9:
                                    if (exchangeRateMER != null)
                                        todayRate = exchangeRateMER.rateSep;
                                    break;
                                case 10:
                                    if (exchangeRateMER != null)
                                        todayRate = exchangeRateMER.rateOct;
                                    break;
                                case 11:
                                    if (exchangeRateMER != null)
                                        todayRate = exchangeRateMER.rateNov;
                                    break;
                                case 12:
                                    if (exchangeRateMER != null)
                                        todayRate = exchangeRateMER.rateDec;
                                    break;
                                default:
                                    if (exchangeRateMER != null)
                                        todayRate = 0;
                                    break;
                            }

                            var total = todayRate;
                            e.Value = total;
                        }
                        break;
                }
            }
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {

        }

       

        private void lookupProduct_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if(lookupProduct.SelectedIndex>-1)
            {
                ProcurementProduct offerProd=  offer.products.FirstOrDefault(x=>x.Id== (lookupProduct.SelectedItem as ProcurementProduct).Id);
                txtOfferValue.Text = offerProd.value1.ToString();
            }
        }
    }
}
