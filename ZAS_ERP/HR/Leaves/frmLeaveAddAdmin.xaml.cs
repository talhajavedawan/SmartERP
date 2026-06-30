using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.HR;
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

namespace ZAS_ERP.HR.Leaves
{
    /// <summary>
    /// Interaction logic for frmLeaveAddAdmin.xaml
    /// </summary>
    public partial class frmLeaveAddAdmin : UserControl
    {
        public List<ERP_BL.Enums.LeaveAdjustmentType> adjType = new List<ERP_BL.Enums.LeaveAdjustmentType>();

        public List<ERP_BL.Enums.LeaveType> leaveType = new List<ERP_BL.Enums.LeaveType>();

        public HrRepo hrRepo = new HrRepo();
        public double annualBalance = 0;
        public double casualBalance = 0;
        public double totalBalance = 0;
        public double appliedDays = 0;

        public frmLeaveAddAdmin()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFormData();
        }
        public void LoadFormData()
        {
            dateApplied.DateTime = DateTime.Now;

            //load adjustment type enums in combobox
            foreach (int i in Enum.GetValues(typeof(ERP_BL.Enums.LeaveAdjustmentType)))
            {
                adjType.Add(((ERP_BL.Enums.LeaveAdjustmentType)i));

            }
            cmbAdjustmentType.ItemsSource = adjType;

            //Load LEaveType enums in combobox
            foreach (int i in Enum.GetValues(typeof(ERP_BL.Enums.LeaveType)))
            {
                leaveType.Add(((ERP_BL.Enums.LeaveType)i));

            }
            cmbLeaveType.ItemsSource = leaveType;

            //Load employees in lookupedit
            EmployeeRepo repo = new EmployeeRepo();
            //_usersRepo users = new _usersRepo();
            //var allUsers = users.getAllActiveUsers();
            var empList = repo.GetAllActiveEmployees();
            if (empList != null)
            {
                cmbEmployee.ItemsSource = empList;
            }

            
        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnApplyLeaves_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GrdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void RadCasual_Checked(object sender, RoutedEventArgs e)
        {
            if (grdDateFrom != null || grdDateTo != null || grdTotalDays != null)
            {
                grdDateFrom.Visibility = Visibility.Visible;
                grdDateTo.Visibility = Visibility.Visible;
                grdTotalDays.Visibility = Visibility.Visible;
            }

            if (grdTimeFrom != null || grdTimeTo != null || grdTotalTime != null)
            {
                grdTimeFrom.Visibility = Visibility.Collapsed;
                grdTimeTo.Visibility = Visibility.Collapsed;
                grdTotalTime.Visibility = Visibility.Collapsed;
            }
        }

        private void RadAnnual_Checked(object sender, RoutedEventArgs e)
        {
            grdDateFrom.Visibility = Visibility.Visible;
            grdDateTo.Visibility = Visibility.Visible;
            grdTotalDays.Visibility = Visibility.Visible;

            grdTimeFrom.Visibility = Visibility.Collapsed;
            grdTimeTo.Visibility = Visibility.Collapsed;
            grdTotalTime.Visibility = Visibility.Collapsed;
        }

        private void RadHalf_Checked(object sender, RoutedEventArgs e)
        {
            grdDateFrom.Visibility = Visibility.Collapsed;
            grdDateTo.Visibility = Visibility.Collapsed;
            grdTotalDays.Visibility = Visibility.Collapsed;

            grdTimeFrom.Visibility = Visibility.Visible;
            grdTimeTo.Visibility = Visibility.Visible;
            grdTotalTime.Visibility = Visibility.Visible;
        }

        private void TimeTo_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                TimeSpan span = TimeTo.DateTime.Subtract(TimeFrom.DateTime);
                var hours = (int)span.Hours;
                if (hours < 0)
                {
                    TimeTo.Clear();
                    TimeTo.DateTime = TimeFrom.DateTime;
                    txtTotalTime.Text = "";
                    DXMessageBox.Show("Time To cannot be lesser than Time From", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    //hours = hours + 1;
                    txtTotalTime.Text = span.ToString();
                }
            }
            catch (Exception ex)
            {

            }

        }



        private void DateFrom_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                dateTo.IsEnabled = true;
                if (dateTo.EditValue == null)
                    dateTo.DateTime = dateFrom.DateTime;
            }
            catch (Exception ex)
            {

            }



        }

        private void TimeFrom_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                TimeTo.IsEnabled = true;
                if (TimeTo.EditValue == null)
                    TimeTo.DateTime = TimeFrom.DateTime;
            }
            catch (Exception ex)
            {

            }


        }

        private void DateTo_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                TimeSpan span = dateTo.DateTime.Subtract(dateFrom.DateTime);
                var days = (int)span.Days;
                if (days < 0)
                {
                    dateTo.Clear();
                    txtTotalDays.Text = "";
                    DXMessageBox.Show("Date To cannot be lesser than Date From", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    days = days + 1;
                    txtTotalDays.Text = days.ToString();
                    if ((ERP_BL.Enums.LeaveType)cmbLeaveType.SelectedItem == ERP_BL.Enums.LeaveType.AnnualLeave)
                    {
                        newAnnual.Text = days.ToString();
                        newSick.Text = "0";
                    }
                    if ((ERP_BL.Enums.LeaveType)cmbLeaveType.SelectedItem == ERP_BL.Enums.LeaveType.CasualLeave)
                    {
                        newAnnual.Text = "0";
                        newSick.Text = days.ToString();
                    }
                    newTotal.Text = days.ToString();

                    int openingSickInt = 0;
                    int newSickInt = 0;
                    int openingAnnualInt = 0;
                    int newAnnualInt = 0;
                    int openingTotalInt = 0;
                    int newTotalInt = 0;
                       


                    if (!String.IsNullOrEmpty(openingSick.Text))
                    {  openingSickInt = Convert.ToInt32(openingSick.Text); }

                    if (!String.IsNullOrEmpty(newSick.Text))
                    {  newSickInt = Convert.ToInt32(newSick.Text); }

                    if (!String.IsNullOrEmpty(openingAnnual.Text))
                    {  openingAnnualInt = Convert.ToInt32(openingAnnual.Text); }

                    if (!String.IsNullOrEmpty(newAnnual.Text))
                    {  newAnnualInt = Convert.ToInt32(newAnnual.Text); }

                    if (!String.IsNullOrEmpty(openingTotal.Text))
                    {  openingTotalInt = Convert.ToInt32(openingTotal.Text); }

                    if (!String.IsNullOrEmpty(openingTotal.Text))
                    {  newTotalInt = Convert.ToInt32(newTotal.Text); }


                    var closingSickInt = openingSickInt - newSickInt;
                    var closingAnnualInt = openingAnnualInt - newAnnualInt;
                    var closingTotalInt = openingTotalInt - newTotalInt;

                    closingSick.Text = closingSickInt.ToString();
                    closingAnnual.Text = closingAnnualInt.ToString();
                    closingTotal.Text = closingTotalInt.ToString();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void CmbEmployee_PopupContentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            
        }

        private void CmbEmployee_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                if (cmbEmployee.SelectedItem != null)
                {
                    dateFrom.EditValue = null;
                    dateTo.EditValue = null;
                    //dateApplied.EditValue = null;
                    txtTotalDays.EditValue = null;
                    newSick.Text = null;
                    newAnnual.Text = null;
                    newTotal.Text = null;
                    closingSick.Text = null;
                    closingAnnual.Text = null;
                    closingTotal.Text = null;

                    var emp = cmbEmployee.SelectedItem as ERP_BL.Databases.Employee;

                    if (emp == null)
                    {
                        return;
                    }
                    txtFname.Text = emp.person.FName;
                    txtLname.Text = emp.person.LName;
                   
                        if (emp.Desig != null)
                        {
                            txtPosition.Text = emp.Desig.Title;
                            var allowedAnnual = emp.Desig.AnnualLeaveDays;
                            var allowedCasual = emp.Desig.CasualLeaveDays;
                            var totalLeaves = allowedAnnual + allowedCasual;

                            totalSick.Text = allowedCasual.ToString();
                            totalAnnual.Text = allowedAnnual.ToString();
                            total.Text = totalLeaves.ToString();
                        }

                        //Load Annual Balance
                        var annualLeaves = hrRepo.GetEmployeeAnnualLeaves(emp.EmpId);

                        if (annualLeaves != null)
                        {
                            annualBalance = annualLeaves.Sum(x => x.LeaveDays);
                        }
                        else
                        { annualBalance = 0; }

                        var casualLeaves = hrRepo.GetEmployeeCasualLeaves(emp.EmpId);
                        if (casualLeaves != null)
                        {
                            casualBalance = casualLeaves.Sum(x => x.LeaveDays);
                        }
                        else
                        {
                            casualBalance = 0;
                        }
                        totalBalance = annualBalance + casualBalance;

                        openingSick.Text = casualBalance.ToString();
                        openingAnnual.Text = annualBalance.ToString();
                        openingTotal.Text = totalBalance.ToString();
                    
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void CmbLeaveType_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (cmbLeaveType.SelectedItem != null)
            {
                dateFrom.EditValue = null;
                dateTo.EditValue = null;
                dateApplied.EditValue = null;
                txtTotalDays.EditValue = null;
                newSick.Text = null;
                newAnnual.Text = null;
                newTotal.Text = null;
                closingSick.Text = null;
                closingAnnual.Text = null;
                closingTotal.Text = null;


                var selectedItem = (ERP_BL.Enums.LeaveType)cmbLeaveType.SelectedItem;

                if (selectedItem == ERP_BL.Enums.LeaveType.Absent)
                {
                    grdDateFrom.Visibility = Visibility.Collapsed;
                    grdDateTo.Visibility = Visibility.Collapsed;
                    grdTotalDays.Visibility = Visibility.Collapsed;

                    grdTimeFrom.Visibility = Visibility.Visible;
                    grdTimeTo.Visibility = Visibility.Visible;
                    grdTotalTime.Visibility = Visibility.Visible;
                }

                if (selectedItem == ERP_BL.Enums.LeaveType.AnnualLeave)
                {
                    grdDateFrom.Visibility = Visibility.Visible;
                    grdDateTo.Visibility = Visibility.Visible;
                    grdTotalDays.Visibility = Visibility.Visible;

                    grdTimeFrom.Visibility = Visibility.Collapsed;
                    grdTimeTo.Visibility = Visibility.Collapsed;
                    grdTotalTime.Visibility = Visibility.Collapsed;              
                                 
                }
                if (selectedItem == ERP_BL.Enums.LeaveType.CasualLeave)
                {
                    if (grdDateFrom != null || grdDateTo != null || grdTotalDays != null)
                    {
                        grdDateFrom.Visibility = Visibility.Visible;
                        grdDateTo.Visibility = Visibility.Visible;
                        grdTotalDays.Visibility = Visibility.Visible;
                    }

                    if (grdTimeFrom != null || grdTimeTo != null || grdTotalTime != null)
                    {
                        grdTimeFrom.Visibility = Visibility.Collapsed;
                        grdTimeTo.Visibility = Visibility.Collapsed;
                        grdTotalTime.Visibility = Visibility.Collapsed;
                    }
                }

                //foreach (int i in Enum.GetValues(typeof(ERP_BL.Enums.LeaveType)))
                //{
                //    if((ERP_BL.Enums.LeaveType)selectedItem == ((ERP_BL.Enums.LeaveType)i))
                //    {

                //    }
                //    //leaveType.Add(((ERP_BL.Enums.LeaveType)i));
                //}
            }
        }

        private void CmbAdjustmentType_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var selectedItem = (ERP_BL.Enums.LeaveAdjustmentType)cmbAdjustmentType.SelectedItem;
           
            if (selectedItem == ERP_BL.Enums.LeaveAdjustmentType.Allocation)
            {
            }
            if (selectedItem == ERP_BL.Enums.LeaveAdjustmentType.ExtraDay)
            {
            }
            if (selectedItem == ERP_BL.Enums.LeaveAdjustmentType.Leave)
            {
            }
        }
    }
}
