using DevExpress.Xpf.Core;
using ERP_BL.FilesAndDocs;
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

namespace ZAS_ERP.FilesAndDocss.TravellingRecords.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmTraveler.xaml
    /// </summary>
    public partial class ucFrmTraveler : UserControl
    {
       
        public Window addCategoryWindow = new Window();
        VisitingRecordRepo recordRepo = new VisitingRecordRepo();
        public Traveler traveler = new Traveler();
        public int travelerId = 0;
        public bool editFlag = false;
        List<ResidentCountry> residentCountries = new List<ResidentCountry>();
        public ucFrmTraveler()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            populatefIelds();

            grdResidentCountries.ItemsSource = residentCountries;
        }

        private void LoadCountries()
        {
            lookupCountry.ItemsSource = recordRepo.GetAllCountries();
        }

        public void populatefIelds()
        {
            LoadCountries();
            residentCountries = new List<ResidentCountry>();
            if (editFlag == true)
            {
                traveler = recordRepo.GetTraveler(travelerId);
                txtTravelerName.Text = traveler.Name;

                if (traveler.ResidentCountries != null)
                    residentCountries = traveler.ResidentCountries;

                //if (traveler.residentCountry != null)
                //    lookupCountry.Text = traveler.residentCountry.CountryName;

                if (traveler.isActive == true)
                    chkIsActive.IsChecked = true;
                else
                    chkIsActive.IsChecked = false;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtTravelerName.Text))
            {
                DXMessageBox.Show("Please enter Traveler Name!");
                txtTravelerName.Focus();
                return;
            }

            if (residentCountries == null && residentCountries.Count == 0)
            {
                DXMessageBox.Show("Please Add at least one visit!");
                return;
            }

            traveler.Name = txtTravelerName.Text;
            //traveler.residentCountryId = (lookupCountry.SelectedItem as ERP_BL.Countryy.Country).Id;
            if (chkIsActive.IsChecked == true)
                traveler.isActive = true;
            else
                traveler.isActive = false;

            if (residentCountries != null)
            {
                if (traveler.ResidentCountries == null)
                    traveler.ResidentCountries = new List<ResidentCountry>();
                foreach (var _visit in residentCountries)
                {
                    if (!traveler.ResidentCountries.Contains(_visit))
                    {
                        traveler.ResidentCountries.Add(_visit);
                    }

                }
            }


            if (editFlag == false && traveler.Id == 0)
            {
                recordRepo.AddTraveler(traveler);
                DXMessageBox.Show("Successfully Added!");
                addCategoryWindow.Close();
            }
            else if (editFlag == true && traveler.Id != 0)
            {
                recordRepo.UpdateTraveler(traveler);
                DXMessageBox.Show("Updated Successfully!");
                addCategoryWindow.Close();
            }
        }
        

        private void GrdResidentCountries_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void View_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {

        }

        private void TblViewResidentCountries_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "fromDate" || e.Column.FieldName == "toDate")
            {
                var row = e.Row as ResidentCountry;
                if (row != null)
                {
                    var fromDate = row.fromDate;
                    var toDate = row.toDate;

                    if (fromDate != null && toDate != null)
                    {
                        if (fromDate > toDate)
                        {
                            DXMessageBox.Show("From Date cannot be less than To Date!");
                            switch (e.Column.FieldName)
                            {
                                case "fromDate":
                                    row.fromDate = null;
                                    break;
                                case "toDate":
                                    row.toDate = null;
                                    break;
                            }
                            return;
                        }
                        else
                        {
                            foreach(var _country in residentCountries)
                            {
                                if (row != _country)
                                {


                                    var range1 = new { start = DateTime.Parse(row.fromDate.Value.ToString()), end = DateTime.Parse(row.toDate.Value.ToString()) };
                                    var range2 = new { start = DateTime.Parse(_country.fromDate.Value.ToString()), end = DateTime.Parse(_country.toDate.Value.ToString()) };
                                    var iStart = range1.start < range2.start ? range2.start : range1.start;
                                    var iEnd = range1.end < range2.end ? range1.end : range2.end;
                                    var newRange = iStart < iEnd ? new { start = iStart, end = iEnd } : null;

                                    if (newRange != null)
                                    {
                                        DXMessageBox.Show("Please select different range of Dates as this range is intersecting with previous records!");
                                        switch (e.Column.FieldName)
                                        {
                                            case "fromDate":
                                                row.fromDate = null;
                                                break;
                                            case "toDate":
                                                row.toDate = null;
                                                break;
                                        }
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


    }
}
