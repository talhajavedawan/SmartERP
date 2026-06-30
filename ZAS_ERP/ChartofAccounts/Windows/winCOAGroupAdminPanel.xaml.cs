using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
using ERP_BL.Enums;
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

namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for winCOAGroupAdminPanel.xaml
    /// </summary>
    public partial class winCOAGroupAdminPanel : DXWindow
    {
        List<cmbitem> allAccountTypes = new List<cmbitem>();
        List<cmbitem> selectedAccountTypes = new List<cmbitem>();
        List<ChartofAccount> allChartofAccounts = new List<ChartofAccount>();
        List<ChartofAccount> selectedChartofAccounts = new List<ChartofAccount>();
        ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();
        ChartofAccountGroup group = new ChartofAccountGroup();

        public winCOAGroupAdminPanel()
        {
            InitializeComponent();
        }
        public winCOAGroupAdminPanel( ChartofAccountGroup _group)
        {
            InitializeComponent();
            group = _group;
            cmbxCOAGroups.Text = group.Title;


        }

        private void imgLeftToRightType_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightType.Width = 30;
            if (gridAccountTypes.SelectedItem != null)
            {
                var type = gridAccountTypes.SelectedItem as cmbitem;
                selectedAccountTypes.Add(type);
                allAccountTypes.Remove(type);
                gridAccountTypes.ItemsSource = null;
                gridSelectedAccountTypes.ItemsSource = null;
                gridAccountTypes.ItemsSource = allAccountTypes;
                gridSelectedAccountTypes.ItemsSource = selectedAccountTypes;
                allChartofAccounts.AddRange(coaRepo.getAccountsByType(SYSTEM_STATIC.currentUser.employeeId, (COA_AccountType)Enum.Parse(typeof(COA_AccountType), type.name)));


                //allDepartments.AddRange(company.departments.Except(selectedDepartments));

                allChartofAccounts = allChartofAccounts.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                gridAllChartofAccounts.ItemsSource = allChartofAccounts;

            }
            else
            {
                DXMessageBox.Show("Please select Account Type which you want to Insert to Selected Account Types!");
            }
        }

        private void imgLeftToRightType_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightType.Width = 28;

        }

        private void imgRightToLeftType_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftType.Width = 28;

        }

        private void imgRightToLeftType_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftType.Width = 30;
            if (gridSelectedAccountTypes.SelectedItem != null)
            {
                var type = gridSelectedAccountTypes.SelectedItem as cmbitem;
                selectedAccountTypes.Remove(type);
                allAccountTypes.Add(type);
                gridAccountTypes.ItemsSource = null;
                gridSelectedAccountTypes.ItemsSource = null;

                gridAccountTypes.ItemsSource = allAccountTypes;
                gridSelectedAccountTypes.ItemsSource = selectedAccountTypes;

                var COAs = selectedChartofAccounts.Except(allChartofAccounts.Intersect(selectedChartofAccounts));
                allChartofAccounts = new List<ChartofAccount>();



                foreach (var _type in selectedAccountTypes)
                {
                    allChartofAccounts.AddRange(coaRepo.getAccountsByType(SYSTEM_STATIC.currentUser.employeeId, (COA_AccountType)Enum.Parse(typeof(COA_AccountType), _type.name)));
                }


                allChartofAccounts = allChartofAccounts.Except(COAs).ToList();

                allChartofAccounts = allChartofAccounts.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                gridAllChartofAccounts.ItemsSource = null;
                gridAllChartofAccounts.ItemsSource = allChartofAccounts;
                gridSelectedChartofAccounts.ItemsSource = null;
                gridSelectedChartofAccounts.ItemsSource = selectedChartofAccounts;
            }
            else
            {
                DXMessageBox.Show("Please select Account type which you want to Insert to All Account Types!");
            }
        }

        private void imgLeftToRightCOA_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightCOA.Width = 28;

        }

        private void imgLeftToRightCOA_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightCOA.Width = 30;

            if (gridAllChartofAccounts.SelectedItem != null)
            {
                var coa = gridAllChartofAccounts.SelectedItem as ChartofAccount;
                if (allChartofAccounts.Find(x => x.parentId == coa.Id) == null)
                {

                    allChartofAccounts.Remove(coa);
                    if (!selectedChartofAccounts.Contains(coa))
                        selectedChartofAccounts.Add(coa);

                    var parent = coa.parent;
                    while (parent != null)
                    {
                        if (allChartofAccounts.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!selectedChartofAccounts.Contains(parent))
                            {
                                selectedChartofAccounts.Add(parent);
                            }
                            var findParet = allChartofAccounts.Find(x => x.parent == parent);
                            if (findParet == null)
                            {
                                allChartofAccounts.Remove(parent);
                            }
                        }
                        parent = parent.parent;
                    }
                    gridAllChartofAccounts.ItemsSource = null;
                    gridSelectedChartofAccounts.ItemsSource = null;
                    gridAllChartofAccounts.ItemsSource = allChartofAccounts;
                    gridSelectedChartofAccounts.ItemsSource = selectedChartofAccounts;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Chart of Account which you want to Insert to Selected Chart of Account!");
            }
        }

        private void imgRightToLeftCOA_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftCOA.Width = 30;

        }

        private void imgRightToLeftCOA_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftCOA.Width = 28;

            if (gridSelectedChartofAccounts.SelectedItem != null)
            {
                var coa = gridSelectedChartofAccounts.SelectedItem as ChartofAccount;

                if (selectedChartofAccounts.Find(x => x.parentId == coa.Id) == null)
                {
                    selectedChartofAccounts.Remove(coa);
                    if (!allChartofAccounts.Contains(coa))
                        allChartofAccounts.Add(coa);
                    var parent = coa.parent;
                    while (parent != null)
                    {
                        if (selectedChartofAccounts.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!allChartofAccounts.Contains(parent))
                                allChartofAccounts.Add(parent);
                            var findParet = selectedChartofAccounts.Find(x => x.parent == parent);
                            if (findParet == null)
                            {
                                selectedChartofAccounts.Remove(parent);
                            }
                        }
                        parent = parent.parent;
                    }
                    gridAllChartofAccounts.ItemsSource = null;
                    gridSelectedChartofAccounts.ItemsSource = null;
                    gridAllChartofAccounts.ItemsSource = allChartofAccounts;
                    gridSelectedChartofAccounts.ItemsSource = selectedChartofAccounts;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Chart of Account which you want to Remove from Selected Chart of Account !");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(group!=null)
            {
                List<ChartofAccount> coas = new List<ChartofAccount>();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                group.ChartofAccounts = new List<ChartofAccount>();
                foreach (var chartofAccount in selectedChartofAccounts)
                {
                    var dbCOA = coaRepo.get(chartofAccount.Id);
                    group.ChartofAccounts.Add(dbCOA);
                }
                    //foreach (var chartofAccount in selectedChartofAccounts)
                    //{
                    //    chartofAccount.Groups.Add(group);
                    //    chartofAccount.Groups.GroupBy(x => x).Select(d => d.First()).ToList();
                    //    if (chartofAccount.Id != 0 && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Chart of Account") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Chart of Account") != null))
                    //    {
                    //        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account without Approval") != null && chartofAccount.isApproved != true)
                    //        {
                    //            if (DevExpress.Xpf.Core.DXMessageBox.Show("This Chart of Account is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    //            {
                    //                chartofAccount.stage = TransactionStage.Approved.ToString();
                    //                chartofAccount.isApproved = true;
                    //                chartofAccount.ApprovedDate = System.DateTime.Now;
                    //            }
                    //        }
                    //        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account without ReApproval") != null && chartofAccount.isReApproved != true)
                    //        {
                    //            if (DevExpress.Xpf.Core.DXMessageBox.Show("This Chart of Account is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    //            {
                    //                chartofAccount.stage = TransactionStage.Approved.ToString();
                    //                chartofAccount.isReApproved = true;
                    //                chartofAccount.ReApprovalDate = System.DateTime.Now;
                    //            }
                    //        }
                    //        coas.Add(chartofAccount);
                    //    }
                    //}
                    coaRepo.UpdateWithGroup(group);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                });
                DXMessageBox.Show("Chart of Account Group has been updated successfully!");

                this.Close();
            }
            else
            {
                DXMessageBox.Show("Please select Chart of Account Group!");
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (COA_AccountType name in Enum.GetValues(typeof(COA_AccountType)))
            {
                cmbitem item = new cmbitem();
                item.id = Convert.ToInt32(name);
                item.name = name.ToString();
                allAccountTypes.Add(item);
            }
            LoadCOAGroups();
            //gridAccountTypes.ItemsSource = allAccountTypes;
            selectedChartofAccounts = group.ChartofAccounts;
            foreach (ChartofAccount _ChartofAccount in selectedChartofAccounts)
            {
                cmbitem item = new cmbitem();
                item.id = Convert.ToInt32(_ChartofAccount.accountType);
                item.name = _ChartofAccount.accountType.ToString();
                selectedAccountTypes.Add(item);
                selectedAccountTypes = selectedAccountTypes.GroupBy(x => x.id).Select(x => x.First()).ToList();
            }

            foreach (cmbitem _accountType in selectedAccountTypes)
            {
                allAccountTypes.Remove(allAccountTypes.Find( x=>x.id==_accountType.id));
                allChartofAccounts.AddRange(coaRepo.getChartofAccountsByType(SYSTEM_STATIC.currentUser.id, (COA_AccountType)Enum.ToObject(typeof(COA_AccountType), _accountType.id)));
                allChartofAccounts = allChartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            }
            foreach (ChartofAccount _coa in selectedChartofAccounts)
            {
                allChartofAccounts.Remove(allChartofAccounts.Find(x => x.Id == _coa.Id));
                allChartofAccounts = allChartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            }
            gridAccountTypes.ItemsSource = allAccountTypes;
            gridSelectedAccountTypes.ItemsSource = selectedAccountTypes;
            gridAllChartofAccounts.ItemsSource = allChartofAccounts;
            gridSelectedChartofAccounts.ItemsSource = selectedChartofAccounts;
        }
        public void LoadCOAGroups()
        {
            cmbxCOAGroups.ItemsSource= coaRepo.GetAllChartofAccountGroups();
        }
      
    }
}
