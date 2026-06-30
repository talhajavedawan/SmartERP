using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using ERP_BL.ExchangeRates;
using ERP_BL.FilesAndDocs;
using System;
using System.Collections;
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

namespace ZAS_ERP.FilesAndDocss.TravellingRecords.UserControls
{
    /// <summary>
    /// Interaction logic for ucVisitingRecord.xaml
    /// </summary>
    public partial class ucVisitingRecord : UserControl
    {
        public List<VisitingCountry> visitingCountries = new List<VisitingCountry>();
        VisitingRecordRepo recordRepo = new VisitingRecordRepo();
        public ResidentCountry residentCountry = new ResidentCountry();
        public int groupId = 0;
        public bool? editFlag = false;
        public int recordId = 0;
        public DateTime? lastArrivalDate;

        TravelingRecords travelingRecord = new TravelingRecords();
        public ucVisitingRecord()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.ItineraryStatus.Planned; i++)
            {

                cmbItineraryStatus.Items.Add(((ERP_BL.Enums.ItineraryStatus)i).ToString());
            }
            LoadCurrencies();
            LoadCountries();
            lookupAirline.ItemsSource = recordRepo.GetAllAirlines();

            if (recordId > 0)
            {
                travelingRecord = recordRepo.GetTravelingRecord(recordId);
            }
            if (groupId > 0 && recordId > 0 && editFlag == true)
            {
                visitingCountries = recordRepo.GetAllVisitingCountriesByGroupId(groupId, recordId);
            }
            grdVisitingRecords.ItemsSource = visitingCountries;
        }

        private void LoadCurrencies()
        {
            var currencies = recordRepo.getAllCurrencies();
            lookupCurrency.ItemsSource = currencies;
        }

        private void LoadCountries()
        {
            lookupDepartingCountry.ItemsSource = recordRepo.GetAllCountries();
            lookupVisitingCountry.ItemsSource = recordRepo.GetAllCountries();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
           

            Window myWindow = Window.GetWindow(this);

            if(recordId > 0)
                visitingCountries.ForEach(x => x.TravelingRecordsId = recordId);
           

            if (editFlag == true)
            {
                recordRepo.UpdateVisitingCountries(visitingCountries);
                DXMessageBox.Show("Updated Successfully!");
            }
            else
            {
                recordRepo.AddVisitingCountries(visitingCountries);
                DXMessageBox.Show("Added Successfully!");
            }
            myWindow.Close();
        }

        private void GrdVisitingRecords_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "NumberOfDays" && e.IsGetData)
            {
                var visit = grdVisitingRecords.GetRowByListIndex(e.ListSourceRowIndex) as VisitingCountry;

                //if (visit.End == true)
                //{
                //    if (visit != null && visit.DepartureDate != null && visit.ArrivalDate != null)
                //        e.Value = (visit.ArrivalDate.Value - visit.DepartureDate.Value).TotalDays;
                //}
                //else
                //{
                    if (visit != null && visit.DepartureDate != null && visit.LeavingDate != null)
                        e.Value = (visit.LeavingDate.Value - visit.DepartureDate.Value).TotalDays;
                //}
            }
        }

        private void TblViewGrdVisitingRecords_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            
        }

        private void TblViewGrdVisitingRecords_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var row = e.Row as VisitingCountry;

            if (e.Column.FieldName != "MER")
            {

                row.TicketCostMER = row.MER * row.TicketCost;
            }

            if (e.Column.FieldName != "TicketCost")
            {

                row.TicketCostMER = row.MER * row.TicketCost;
            }

            if (e.Column.FieldName != "sequence")
            {
                if(row.sequence < 1)
                {
                    DXMessageBox.Show("Please enter the sequence before proceeding!");
                    return;
                }
            }

            if (e.Column.FieldName == "End")
            {
                List<VisitingCountry> items = new List<VisitingCountry>();
                items.Add(row);
                items = visitingCountries.Except(items).ToList();

                if (row.End == true)
                {
                    if(visitingCountries != null && items.FirstOrDefault(x=>x.End == true) != null)
                    {
                        DXMessageBox.Show("Current Itinerary already ended at sequence # "+ items.FirstOrDefault(x => x.End == true).sequence);
                        row.End = false;
                    }
                }
                if(row.visitingCountry == null && row.End == true)
                {
                    DXMessageBox.Show("Please select Staying country before ending Itinerary!");
                    row.End = false;
                }
                //if (row.End == true && row.visitingCountry.Id != residentCountry.countryId)
                //{
                //    DXMessageBox.Show("Itinerary should be ended at Resident country!");
                //    row.visitingCountry = null;
                //    return;
                //}
            }


            if (e.Column.FieldName == "sequence")
            {
                List<VisitingCountry> items = new List<VisitingCountry>();
                items.Add(row);
                items = visitingCountries.Except(items).ToList();
                if (visitingCountries!= null && items.FirstOrDefault(x=>x.sequence == row.sequence) != null)
                {
                    DXMessageBox.Show("Sequence cannot be repeated!");
                    row.sequence = 0;
                    return;
                }
                if(row.sequence > 1)
                {
                    var country = visitingCountries.FirstOrDefault(x => x.sequence == (row.sequence - 1));
                    if(country == null)
                    {
                        DXMessageBox.Show("Previous sequence of "+row.sequence+" is missing!");
                        return;
                    }
                    if(country != null && country.visitingCountry != null)
                    {
                        row.departingCountry = country.visitingCountry;
                    }
                    if(country != null && country.LeavingDate != null)
                    {
                        row.DepartureDate = country.LeavingDate;
                    }
                }
            }

            if (e.Column.FieldName == "departingCountry")
            {
                if(row.sequence == 1)
                {
                    //var depCountry = row.departingCountry;
                    //if(residentCountry.countryId != depCountry.Id)
                    //{
                    //    DXMessageBox.Show("Departing country should be same as Resident Country because it's the start of Itinerary!");
                    //    row.departingCountry = null;
                    //    return;
                    //}
                }
                else if(row.sequence > 1)
                {
                    var depCountry = row.departingCountry;
                    var country = visitingCountries.FirstOrDefault(x => x.sequence == (row.sequence - 1));
                    if(country != null)
                    {
                        if(depCountry != null && country.visitingCountry != null && depCountry.Id != country.visitingCountry.Id)
                        {
                            DXMessageBox.Show("Departing country cannot be changed!");
                            row.departingCountry = country.visitingCountry;
                            return;
                        }
                    }
                    else
                    {
                        DXMessageBox.Show("Previous sequence of " + row.sequence + " is missing!");
                        return;
                    }
                }
            }

            if (e.Column.FieldName == "DepartureDate")
            {

                if(travelingRecord.residentFromDate > row.DepartureDate)
                {
                    DXMessageBox.Show("Departure date is out of range!");
                    row.DepartureDate = null;
                    return;
                }
                if(row.sequence == 1)
                {
                    if(row.DepartureDate < lastArrivalDate)
                    {
                        DXMessageBox.Show("Departure date cannot be less than "+lastArrivalDate);
                        row.DepartureDate = null;
                        return;
                    }
                }
                if (row.sequence > 1)
                {
                    var depDate = row.DepartureDate;
                    var country = visitingCountries.FirstOrDefault(x => x.sequence == (row.sequence - 1));
                    if (country != null)
                    {
                        if (depDate != country.LeavingDate)
                        {
                            DXMessageBox.Show("Departure date cannot be changed!");
                            row.DepartureDate = country.LeavingDate;
                            return;
                        }
                    }
                    else
                    {
                        DXMessageBox.Show("Previous sequence of " + row.sequence + " is missing!");
                        return;
                    }
                }
            }

            if (e.Column.FieldName == "visitingCountry")
            {
                var visitingCountry = row.visitingCountry;
                var depCountry = row.departingCountry;

                if(depCountry != null && visitingCountry != null && depCountry.Id == visitingCountry.Id)
                {
                    DXMessageBox.Show("Staying country should be different from Departing Country!");
                    row.visitingCountry = null;
                    return;
                }
                
                //if (row.End == true)
                //{  
                //    if (visitingCountry != null && visitingCountry.Id != residentCountry.country.Id)
                //    {
                //        DXMessageBox.Show("Staying country should be same as Resident Country of the Traveler because it's the End of Itinerary!");
                //        row.visitingCountry = null;
                //        return;
                //    }
                //}
            }
            
            if (e.Column.FieldName == "LeavingDate")
            {
                if (travelingRecord.residentToDate < row.LeavingDate)
                {
                    DXMessageBox.Show("Leaving date is out of range!");
                    row.LeavingDate = null;
                    return;
                }
            }

            if (e.Column.FieldName == "ArrivalDate")
            {
                if (travelingRecord.residentToDate < row.ArrivalDate)
                {
                    DXMessageBox.Show("Arrival date is out of range!");
                    row.LeavingDate = null;
                    return;
                }
            }

            if (e.Column.FieldName == "DepartureDate" || e.Column.FieldName == "ArrivalDate" || e.Column.FieldName == "LeavingDate")
            {
                if (row != null)
                {
                    var depDate = row.DepartureDate;
                    var arrDate = row.ArrivalDate;
                    var leavingDate = row.LeavingDate;

                    if (depDate != null && arrDate != null)
                    {
                        if ((arrDate < depDate) || (leavingDate < depDate))
                        {
                            DXMessageBox.Show("Arrival Date or Leaving Date cannot be less than Departing Date");
                            switch (e.Column.FieldName)
                            {
                                case "DepartureDate":
                                    row.DepartureDate = null;
                                    break;
                                case "ArrivalDate":
                                    row.ArrivalDate = null;
                                    break;
                                case "LeavingDate":
                                    row.LeavingDate = null;
                                    break;
                            }
                            return;
                        }
                    }
                }
            }
        }

        private void MbtnDelete_Click(object sender, RoutedEventArgs e)
        {


            var visitRecord = grdVisitingRecords.SelectedItem as VisitingCountry;

            if(visitRecord != null && visitRecord.Id > 0)
            {
                if (DevExpress.Xpf.Core.DXMessageBox.Show("Are you sure want to Delete this record?", "Delete Record", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    recordRepo.DeleteVisitingCountries(visitRecord);


                    visitingCountries = recordRepo.GetAllVisitingCountriesByGroupId(groupId, recordId);
                    grdVisitingRecords.ItemsSource = visitingCountries;
                }
                    
            }
        }

        

        private void LookupCurrency_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //CurrencyRepo currencyRepo = new CurrencyRepo();
            //DateTime creationDate = (DateTime)datCreationDate.EditValue;
            //DateTime d1 = new DateTime(2016, 01, 01);
            //ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
            //ExchangeRate exchangeRate = null;
            //if (creationDate > d1)
            //{
            //    if (cmbxCurrency.SelectedIndex != -1 && cmbxCompany.SelectedIndex != -1)
            //    {
            //        var exchangeRateGroupMER = exchangeRateGroupRepo.GetGroupByCurrenciesMER((cmbxCurrency.SelectedItem as Currency).Id, (cmbxCompany.SelectedItem as Company).currency.Id, creationDate.Year);
            //        if ((cmbxCompany.SelectedItem as Company).currency.Id == (cmbxCurrency.SelectedItem as Currency).Id)
            //        {
            //            txtMER.Text = 1.ToString();
            //        }
            //        else
            //        {

            //            if (exchangeRateGroupMER != null)
            //            {
            //                exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == (cmbxCompany.SelectedItem as Company).Id);
            //                switch (creationDate.Month)
            //                {
            //                    case 1:
            //                        if (exchangeRate != null)
            //                            txtMER.Text = exchangeRate.rateJan.ToString();
            //                        break;
            //                    case 2:
            //                        if (exchangeRate != null)
            //                            txtMER.Text = exchangeRate.rateFeb.ToString();
            //                        break;
            //                    case 3:
            //                        if (exchangeRate != null)
            //                            txtMER.Text = exchangeRate.rateMar.ToString();
            //                        break;
            //                    case 4:
            //                        if (exchangeRate != null)
            //                            txtMER.Text = exchangeRate.rateApr.ToString();
            //                        break;
            //                    case 5:
            //                        if (exchangeRate != null)
            //                            txtMER.Text = exchangeRate.rateMay.ToString();
            //                        break;
            //                    case 6:
            //                        if (exchangeRate != null)
            //                            txtMER.Text = exchangeRate.rateJun.ToString();
            //                        break;
            //                    case 7:
            //                        if (exchangeRate != null)
            //                            txtMER.Text = exchangeRate.rateJul.ToString();
            //                        break;
            //                    case 8:
            //                        if (exchangeRate != null)
            //                            txtMER.Text = exchangeRate.rateAug.ToString();
            //                        break;
            //                    case 9:
            //                        if (exchangeRate != null)
            //                            txtMER.Text = exchangeRate.rateSep.ToString();
            //                        break;
            //                    case 10:
            //                        if (exchangeRate != null)
            //                            txtMER.Text = exchangeRate.rateOct.ToString();
            //                        break;
            //                    case 11:
            //                        if (exchangeRate != null)
            //                            txtMER.Text = exchangeRate.rateNov.ToString();
            //                        break;
            //                    case 12:
            //                        if (exchangeRate != null)
            //                            txtMER.Text = exchangeRate.rateDec.ToString();
            //                        break;
            //                    default:
            //                        if (exchangeRate != null)
            //                            txtMER.Text = 0.ToString();
            //                        break;
            //                }
            //            }
            //        }
            //    }

            //}
        }

        
    }
}
