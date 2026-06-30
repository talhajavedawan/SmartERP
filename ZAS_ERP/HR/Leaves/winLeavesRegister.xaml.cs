
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
using System.Windows.Shapes;
using ZAS_ERP.HR.Leaves;
using DevExpress.XtraEditors;

namespace ZAS_ERP.HR
{
    /// <summary>
    /// Interaction logic for winLeavesRegister.xaml
    /// </summary>
    public partial class winLeavesRegister : Window
    {
        public List<ZAS_ERP.cmbitem> TreeItems = new List<cmbitem>();
        HrRepo hrRepo = new HrRepo();
        EmployeeRepo emprepo = new EmployeeRepo();

        public string transctions;
        public winLeavesRegister()
        {
            InitializeComponent();
        }

        private void MbtnLeaveRegister_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                grdLeavesRegister reg = new grdLeavesRegister();
                var leavesList = hrRepo.GetAllLeaveApplications();
                reg.grdLeaveRegisters.ItemsSource = leavesList;
                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(reg.grdLeaveRegisters);

                if (grdLeavesMain.Children != null)
                    grdLeavesMain.Children.Clear();

                grdLeavesMain.Children.Add(reg);
            }
            catch
            {
                DXMessageBox.Show("An error occured please click on refresh button.","Error",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
          
        }

        private void MbtnAddLeaves_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void CollapseGridBarItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            faLeftGrid.Visibility = Visibility.Collapsed;
            GridSplitter.Visibility = Visibility.Collapsed;
            faRightGrid.SetValue(Grid.ColumnProperty, 0);

            faRightGrid.SetValue(Grid.ColumnSpanProperty, 3);
           
        }


        //Expands the left Grid
        private void ExpandGridBarItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            faLeftGrid.Visibility = Visibility.Visible;
            GridSplitter.Visibility = Visibility.Visible;

            faRightGrid.SetValue(Grid.ColumnProperty, 2);
            faRightGrid.SetValue(Grid.ColumnSpanProperty, 1);

          
        }

        private void LeaveTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LeaveTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }

        private void ManageRoles_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void ManageRoles_ItemClick_1(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void ManageLists_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            grdLeavesRegister reg = new grdLeavesRegister();
            var leavesList = hrRepo.GetAllLeaveApplications();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Leave Register") != null)
            {
                reg.grdLeaveRegisters.ItemsSource = leavesList;
                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(reg.grdLeaveRegisters);
            }
            else
            {
                reg.grdLeaveRegisters.ItemsSource = null;
                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(reg.grdLeaveRegisters);
            }
            

            if (grdLeavesMain.Children != null)
                grdLeavesMain.Children.Clear();

            grdLeavesMain.Children.Add(reg);
            LoadEmployees();
           

            GenerateTreeView();


            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Allocate leave Button") != null)
            {
                renewLeaves.IsEnabled = true;
            }
            else
            {
                renewLeaves.IsEnabled = false;
            }
        }

        public void GenerateTreeView()
        {
            HrRepo repo = new HrRepo();

            cmbitem treeItemLeave = new cmbitem() { name = "Leaves", description = "Leaves" };
            cmbitem treeItemLeaveOpen = new cmbitem() { name = "Leaves(Open)", description = "Leaves" };
            ICollection<cmbitem> cmbItemsLeaveOpen = new List<cmbitem>();
            foreach (LeaveStatus status in repo.GetAllActiveLeaveStatus().OrderBy(x => x.Status).ToList())
            {
                cmbItemsLeaveOpen.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Leaves" });
            }
            treeItemLeaveOpen.Items = cmbItemsLeaveOpen;
            treeItemLeave.Items.Add(treeItemLeaveOpen);

            cmbitem treeItemLeaveClose = new cmbitem() { name = "Leaves(Closed)", description = "Leaves" };

            //if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "View InActive Leaves") != null))
            //{
                ICollection<cmbitem> cmbItemsLeaveClose = new List<cmbitem>();
                foreach (LeaveStatus status in repo.GetAllInActiveLeaveStatus().OrderBy(x => x.Status).ToList())
                {
                    cmbItemsLeaveClose.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Leaves" });
                }
                treeItemLeaveClose.Items = cmbItemsLeaveClose;
                treeItemLeave.Items.Add(treeItemLeaveClose);

            //}


            TreeItems.Add(treeItemLeave);

            //Add Tree Items List as Item Source of TreeVIew
            leaveTreeView.ItemsSource = TreeItems;

        }

        private void MbtnAdjustLeaves_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void MbtnAllocateLeaves_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void MbtnLeaves_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            frmLeaveAddAdmin frm = new frmLeaveAddAdmin();
            Window win = new Window();

            win.Content = frm;
            win.ShowDialog();
        }

        private void RenewLeaves_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            HrRepo repo = new HrRepo();
            EmployeeRepo empRepo = new EmployeeRepo();

            var lstEmp = empRepo.GetAllActiveEmployees();
           
          
            var res =  DXMessageBox.Show("Are you sure to want to allocate leaves to all eployees","Confirmation",MessageBoxButton.YesNoCancel,MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    winProgressBar win = new winProgressBar();
                    win.ShowDialog();
                    //foreach (var _emp in lstEmp)
                    //{

                    //    Leave leave = new Leave();
                    //    leave.LeaveType = ERP_BL.Enums.LeaveType.AnnualLeave;
                    //    if (_emp.Desig != null)
                    //    {
                    //        if (_emp.Desig.AnnualLeaveDays != 0)
                    //        {
                    //            leave.LeaveDays = _emp.Desig.AnnualLeaveDays;
                    //            leave.LeaveDes = "Allocation";
                    //            //leave.employeeId = _emp.EmpId;
                    //            leave.employee = _emp;
                    //            leave.DateFrom = DateTime.Now;
                    //            leave.DateTo = DateTime.Now;
                    //            leave.ApplyDate = DateTime.Now;

                    //            repo.AddEmployeeLeave(leave);
                    //        }
                    //    }
                    //    Leave leave2 = new Leave();
                    //    leave2.LeaveType = ERP_BL.Enums.LeaveType.CasualLeave;
                    //    if (_emp.Desig != null)
                    //    {
                    //        if (_emp.Desig.CasualLeaveDays != 0)
                    //        {
                    //            leave2.LeaveDays = _emp.Desig.CasualLeaveDays;
                    //            leave2.LeaveDes = "Allocation";
                    //            //leave.employeeId = _emp.EmpId;
                    //            leave2.employee = _emp;
                    //            leave2.DateFrom = DateTime.Now;
                    //            leave2.DateTo = DateTime.Now;
                    //            leave2.ApplyDate = DateTime.Now;

                    //            repo.AddEmployeeLeave(leave2);
                    //        }
                    //    }

                    //}
                    //DXMessageBox.Show("Leave Allocation completed for all employees","Successfull",MessageBoxButton.OK,MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    DXMessageBox.Show(ex.Message);
                }

            }

        }
        private void LoadEmployees()
        {
            lookUpEmployee.ItemsSource = emprepo.GetAllEmployees();
        }
        private void ClearDateFilter_Click(object sender, RoutedEventArgs e)
        {
            dateDay.Text = null;
            dateMonth.Text = null;
            dateEditYear.Text = null;
            lookUpEmployee.Text = "";
            lookUpEmployee.SelectedItem = null;
        }

        private void LookUpEmployee_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            lookUpEmployee.Text = "";
            lookUpEmployee.SelectedItem = null;
        }

        private void ApplyDateFilter_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Leave Register") != null)
            {
                try
                {
                    //wHEN eMPLOYEE iS sELECTED
                    if (lookUpEmployee.SelectedItem != null)
                    {
                        var selectedEmp = lookUpEmployee.SelectedItem as ERP_BL.Databases.Employee;
                        var empId = selectedEmp.EmpId;

                        //all are not null
                        if (!String.IsNullOrEmpty(dateDay.Text) && !String.IsNullOrEmpty(dateMonth.Text) && !String.IsNullOrEmpty(dateEditYear.Text))
                        {
                            if (grdLeavesMain.Children != null)
                                grdLeavesMain.Children.Clear();

                            var day = dateDay.DateTime.Day;
                            var month = dateMonth.DateTime.Month;
                            var year = dateEditYear.DateTime.Year;

                            DateTime date = new DateTime(year, month, day, 00, 00, 00);

                            grdLeavesRegister grd = new grdLeavesRegister();

                            var leaveAppLst = hrRepo.GetAllLeaveApplicationsDate(date, empId);

                            grd.grdLeaveRegisters.ItemsSource = leaveAppLst;
                            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grd.grdLeaveRegisters);
                            grd.lblEmpLeaves.Text = "Leaves" + " (" + date.Day.ToString() + "/" + date.Month.ToString() + "/" + date.Year.ToString() + " " + selectedEmp.person.FName + " " + selectedEmp.person.LName + " )";
                            grdLeavesMain.Children.Add(grd);


                        }


                        //only day is null
                        else if (String.IsNullOrEmpty(dateDay.Text) && !String.IsNullOrEmpty(dateMonth.Text) && !String.IsNullOrEmpty(dateEditYear.Text))
                        {
                            if (grdLeavesMain.Children != null)
                                grdLeavesMain.Children.Clear();

                            //var day = dateDay.DateTime.Day;
                            var month = dateMonth.DateTime.Month;
                            var year = dateEditYear.DateTime.Year;

                            DateTime date = new DateTime(year, month, 01, 00, 00, 00);

                            grdLeavesRegister grd = new grdLeavesRegister();

                            var leaveAppLst = hrRepo.GetAllLeaveApplicationsMonth(date, empId);

                            grd.grdLeaveRegisters.ItemsSource = leaveAppLst;
                            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grd.grdLeaveRegisters);

                            grd.lblEmpLeaves.Text = "Leaves" + " (" + date.Month.ToString() + "/" + date.Year.ToString() + " " + selectedEmp.person.FName + " " + selectedEmp.person.LName + ")";
                            grdLeavesMain.Children.Add(grd);
                        }
                        //date and month are null
                        else if (String.IsNullOrEmpty(dateDay.Text) && String.IsNullOrEmpty(dateMonth.Text) && !String.IsNullOrEmpty(dateEditYear.Text))
                        {
                            if (grdLeavesMain.Children != null)
                                grdLeavesMain.Children.Clear();

                            //var day = dateDay.DateTime.Day;
                            var month = dateMonth.DateTime.Month;
                            var year = dateEditYear.DateTime.Year;

                            DateTime date = new DateTime(year, month, 01, 00, 00, 00);

                            grdLeavesRegister grd = new grdLeavesRegister();

                            var leaveAppLst = hrRepo.GetAllLeaveApplicationsYear(date, empId);

                            grd.grdLeaveRegisters.ItemsSource = leaveAppLst;
                            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grd.grdLeaveRegisters);

                            grd.lblEmpLeaves.Text = "Leaves" + " (" + date.Year.ToString() + " " + selectedEmp.person.FName + " " + selectedEmp.person.LName + ")";
                            grdLeavesMain.Children.Add(grd);
                        }
                        //All are null
                        else if (String.IsNullOrEmpty(dateDay.Text) && String.IsNullOrEmpty(dateMonth.Text) && String.IsNullOrEmpty(dateEditYear.Text))
                        {
                            if (grdLeavesMain.Children != null)
                                grdLeavesMain.Children.Clear();

                            //var day = dateDay.DateTime.Day;
                            //  var month = dateMonth.DateTime.Month;
                            //  var year = dateEditYear.DateTime.Year;

                            //  DateTime date = new DateTime(year, month, 01, 00, 00, 00);

                            grdLeavesRegister grd = new grdLeavesRegister();

                            var leaveAppLst = hrRepo.GetAllLeaveApplications(empId);

                            grd.grdLeaveRegisters.ItemsSource = leaveAppLst;
                            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grd.grdLeaveRegisters);

                            grd.lblEmpLeaves.Text = "Leaves" + " (" + selectedEmp.person.FName + " " + selectedEmp.person.LName + ")";
                            grdLeavesMain.Children.Add(grd);

                        }
                        else
                        {
                            DXMessageBox.Show("Your Selection is wrong, Please apply right filter", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                    }




                    //WHen Employee is Not selected

                    else
                    {
                        //all are not null
                        if (!String.IsNullOrEmpty(dateDay.Text) && !String.IsNullOrEmpty(dateMonth.Text) && !String.IsNullOrEmpty(dateEditYear.Text))
                        {
                            //MessageBox.Show("/all are not null - emp slected");
                            if (grdLeavesMain.Children != null)
                                grdLeavesMain.Children.Clear();

                            var day = dateDay.DateTime.Day;
                            var month = dateMonth.DateTime.Month;
                            var year = dateEditYear.DateTime.Year;

                            DateTime date = new DateTime(year, month, day, 00, 00, 00);

                            grdLeavesRegister grd = new grdLeavesRegister();

                            var leaveAppLst = hrRepo.GetAllLeaveApplicationsDate(date);

                            grd.grdLeaveRegisters.ItemsSource = leaveAppLst;
                            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grd.grdLeaveRegisters);

                            grd.lblEmpLeaves.Text = "Leaves" + " (" + date.Day.ToString() + "/" + date.Month.ToString() + "/" + date.Year.ToString() + " )";
                            grdLeavesMain.Children.Add(grd);

                        }
                        //only date is null
                        else if (String.IsNullOrEmpty(dateDay.Text) && !String.IsNullOrEmpty(dateMonth.Text) && !String.IsNullOrEmpty(dateEditYear.Text))
                        {
                            if (grdLeavesMain.Children != null)
                                grdLeavesMain.Children.Clear();

                            //var day = dateDay.DateTime.Day;
                            var month = dateMonth.DateTime.Month;
                            var year = dateEditYear.DateTime.Year;

                            DateTime date = new DateTime(year, month, 01, 00, 00, 00);

                            grdLeavesRegister grd = new grdLeavesRegister();

                            var leaveAppLst = hrRepo.GetAllLeaveApplicationsMonth(date);

                            grd.grdLeaveRegisters.ItemsSource = leaveAppLst;
                            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grd.grdLeaveRegisters);

                            grd.lblEmpLeaves.Text = "Leaves" + " (" + /*date.Day.ToString() + "/" +*/ date.Month.ToString() + "/" + date.Year.ToString() + ")";
                            grdLeavesMain.Children.Add(grd);
                        }
                        //date and month are null
                        else if (String.IsNullOrEmpty(dateDay.Text) && String.IsNullOrEmpty(dateMonth.Text) && !String.IsNullOrEmpty(dateEditYear.Text))
                        {
                            if (grdLeavesMain.Children != null)
                                grdLeavesMain.Children.Clear();

                            //var day = dateDay.DateTime.Day;
                            var month = dateMonth.DateTime.Month;
                            var year = dateEditYear.DateTime.Year;

                            DateTime date = new DateTime(year, month, 01, 00, 00, 00);

                            grdLeavesRegister grd = new grdLeavesRegister();

                            var leaveAppLst = hrRepo.GetAllLeaveApplicationsYear(date);

                            grd.grdLeaveRegisters.ItemsSource = leaveAppLst;
                            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grd.grdLeaveRegisters);

                            grd.lblEmpLeaves.Text = "Leaves" + " (" + /*date.Day.ToString() + "/" +*//* date.Month.ToString() + "/" +*/ date.Year.ToString() + ")";
                            grdLeavesMain.Children.Add(grd);

                        }
                        //All are null
                        else if (String.IsNullOrEmpty(dateDay.Text) && String.IsNullOrEmpty(dateMonth.Text) && String.IsNullOrEmpty(dateEditYear.Text))
                        {
                            //MessageBox.Show("/All are null - emp not slected");
                            DXMessageBox.Show("Your Selection is wrong, Please apply right filter", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Warning);

                        }
                        else
                        {
                            DXMessageBox.Show("Your Selection is wrong, Please apply right filter", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                DXMessageBox.Show("You are not allowed to View Leave Register","Permission Denied",MessageBoxButton.OK,MessageBoxImage.Stop); 
            }
           
            

        }
    }
}
