using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.AssetsRentals;
using ERP_BL.Countryy;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ZAS_ERP.Bankings;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;

namespace ZAS_ERP.AssetRentalss.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmAddAssetRental.xaml
    /// </summary>
    public partial class ucFrmAddAssetRental : UserControl
    {
        AssetRentalRepo rentalRepo = new AssetRentalRepo();
        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
        AssetRental asset = new AssetRental();
        AssetRental trackingOrder = new AssetRental();
        public int assetId = 0;
        public bool editFlag = false;

        AssetRentalStatus checkStatus = new AssetRentalStatus();

        UsersRepo UsersRepo = new UsersRepo();
        List<ViewInfo> views = new List<ViewInfo>();
        public ucFrmAddAssetRental()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadCompanies();
            loadStatuses();
            loadAssetType();
            loadAssetNumber();
            loadAssetNature();
            loadCountries();
            LoadEmployees();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Creation Date of Rental Contracts") == null)
                datCreationDate.IsEnabled = false;
            else
                datCreationDate.IsEnabled = true;

            if (editFlag == false)
            {
                datCreationDate.DateTime = DateTime.Now;
                txtCreator.Text = SYSTEM_STATIC.currentUser.employee.person.FName + ' ' + SYSTEM_STATIC.currentUser.employee.person.LName;
            }

            if (editFlag == true)
            {
                asset = rentalRepo.GetAssetRental(assetId);

                if (asset.CreationDate != null)
                    datCreationDate.EditValue = asset.CreationDate;

                txtAssetName.Text = asset.AssetName;

                if (asset.assetType != null)
                    lookupAssetType.Text = asset.assetType.TypeName;

                if (asset.AssetNature != null)
                    lookupAssetNature.Text = asset.AssetNature.NatureName;

                if (asset.assetSubNature != null)
                    lookupAssetSubNature.Text = asset.assetSubNature.NatureName;

                if (asset.assetBrand != null)
                    lookupAssetBrand.Text = asset.assetBrand.BrandName;

                if (asset.assetModel != null)
                    lookupAssetModel.Text = asset.assetModel.ModelNumber;

                if (asset.assetNumber != null)
                    lookupAssetNumber.Text = asset.assetNumber.Number;
                txtAssetNumber.Text = asset.AssetNumber;


                


                if (asset.company != null)
                    lookupCompany.Text = asset.company.CompanyName;

                if (asset.department != null)
                    lookupDepartment.Text = asset.department.DeptName;

                

                if (asset.assetHolderEmployee != null)
                {
                    lookupAssetHolder.Text = asset.assetHolderEmployee.person.FullName;
                    chkHolderFromSystem.IsChecked = true;
                }
                else
                {
                    txtHolder.Text = asset.AssetHolder;
                    chkHolderFromSystem.IsChecked = false;
                }


                if (asset.Creator != null)
                    txtCreator.Text = asset.Creator.employee.person.FName + " " + asset.Creator.employee.person.LName;

                chkRentable.IsChecked = asset.isRentable;
                chkSubsidary.IsChecked = asset.isSubsidary;
                chkLeased.IsChecked = asset.isLeased;
                chkVendorFromSystem.IsChecked = asset.VendorFromSystem;

                if (asset.vendor != null)
                    lookupVendor.Text = asset.vendor.company.CompanyName;
                else
                    txtVendor.Text = asset.Vendor;

                if (asset.parentAsset != null)
                    lookupParentAsset.Text = asset.parentAsset.AssetName;

                if (asset.country != null)
                    lookupCountry.Text = asset.country.CountryName;

                if (asset.city != null)
                    lookupAssetCity.Text = asset.city.CityName;

                if (asset.assetRentalLocation != null)
                    lookupLocation.Text = asset.assetRentalLocation.LocationTitle;

                if (asset.assetRentalUnit != null)
                    lookupAssetUnit.Text = asset.assetRentalUnit.UnitNo;


                //if (asset.address != null)
                //{
                //    txtAddress.Text = asset.address.Line1;
                //    txtCity.Text = asset.address.City;
                //    txtCountry.Text = asset.address.Country;
                //}

                txtSystemRef.Text = asset.SystemRef;
                
                txtDescription.Text = asset.Description;
                
                txtSerialNo.Text = asset.SerialNumber;
                lblRefNo.Text = asset.transactionGroupId.ToString();
                AssetRentalStages();

                //Select Status
                var statusList = (cmbStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbStatus.ItemsSource as List<cmbitem>;
                if (asset.Status != null)
                {
                    checkStatus = asset.Status;
                    int index = 0;
                    foreach (var _status in statusList)
                    {
                        if (_status.id == asset.statusId)
                        {
                            cmbStatus.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                datPurchaseDate.EditValue = asset.PurchasingDate;
                txtPurchaseCostManual.Text = asset.PurchasingCostManual.ToString();
                txtProgressiveCostManual.Text = asset.ProgressiveCostManual.ToString();
                txtPurchaseCost.Text = asset.adminBills.Where(x=>x.isProgressiveCost == false).Sum(x=>x.AmountOC).ToString();
                txtProgressiveCost.Text = asset.adminBills.Where(x => x.isProgressiveCost == true).Sum(x => x.AmountOC).ToString();
            }
        }


        private void AssetRentalStages()
        {
            if (asset.isVoid == true)
            {
                grdVoid.Visibility = Visibility.Visible;
                txtVoid.RenderTransform = new RotateTransform(-45);
                //lblStage.Text = "Void";
            }
            else if (asset.isReApproved == false)
            {
                //lblStage.Text = "Under Re-Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (asset.isApproved == true && asset.stage == "Closed")
            {
                //lblStage.Text = "Approved and Closed";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (asset.isApproved == true && asset.Status.isActive == false && asset.PendingForClosing != true)
            {
                //lblStage.Text = "Approved and Closed";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (asset.isApproved == true && asset.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (asset.isApproved == true)
            {
                //lblStage.Text = "Approved";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (asset.isApproved == false)
            {
                //lblStage.Text = "Under Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (asset.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
        }

        private void loadCompanies()
        {
            empUser = rentalRepo.GetEmployee(SYSTEM_STATIC.currentUser.employeeId);
            lookupCompany.ItemsSource = empUser.Companies;
        }

        private void loadAssetType()
        {
            var assetTypes = rentalRepo.GetAllAssetType();
            lookupAssetType.ItemsSource = assetTypes;
        }

        private void loadAssetNumber()
        {
            var assetNumbers = rentalRepo.GetAllAssetNumber();
            lookupAssetNumber.ItemsSource = assetNumbers;
        }

        private void loadAssetNature()
        {
            var assetNatures = rentalRepo.GetAllAssetNature();
            lookupAssetNature.ItemsSource = assetNatures;
        }

        private void loadCountries()
        {
            CountryRepo countryRepo = new CountryRepo();
            var countries = countryRepo.GetAllCountries();
            lookupCountry.ItemsSource = countries;
        }

        private void loadStatuses()
        {
            List<AssetRentalStatus> assetRentalStatuses = new List<AssetRentalStatus>();

            List<cmbitem> cmbitems = new List<cmbitem>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment Statuses") != null)
                assetRentalStatuses = rentalRepo.GetAllAssetRentalStatuses();
            else
                assetRentalStatuses = rentalRepo.GetAllAssetRentalStatuses().Where(x => x.isActive == true).ToList();
            assetRentalStatuses = assetRentalStatuses.Where(x => x.isDisable != true).ToList();

            Parallel.ForEach(assetRentalStatuses, delegate (AssetRentalStatus status) // foreach (BillStatus status in BillStatuses)
            {
                cmbitems.Add(new cmbitem()
                {
                    name = status.Status,
                    id = status.Id,
                    bcolor = status.backcolor,
                    fcolor = "#FF000000"
                });
            });
            cmbStatus.ItemsSource = cmbitems;
        }
        int intGroupId;
        private void GroupIdCalculation()
        {
            string groupId;
            string year;
            string month;
            string id;

            var lastPaymentId = rentalRepo.GetLastTransactionId();
            if (lastPaymentId == 0)
            {
                year = DateTime.Now.Year.ToString();
                month = DateTime.Now.Month.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                id = "1";
                groupId = year + month + id;
            }
            else
            {
                // var last = history.Last();
                var lastId = lastPaymentId/*.transactionGroupId*/;
                string fullId = lastId.ToString();
                int length = fullId.Length;
                //length = length - 1;

                year = fullId.Substring(0, 4);
                if (year != DateTime.Now.Year.ToString())
                {
                    year = DateTime.Now.Year.ToString();
                }
                month = fullId.Substring(4, 2);

                var currentMonth = DateTime.Now.Month.ToString();
                if (currentMonth.Length == 1)
                {
                    currentMonth = "0" + currentMonth;
                }

                int idLen = length - 6;
                id = fullId.Substring(6, idLen);

                if (month != currentMonth)
                {
                    month = currentMonth;

                    id = "1";
                }
                else
                {
                    int intId = Convert.ToInt32(id);
                    intId = intId + 1;

                    id = intId.ToString();
                }
                groupId = year + month + id;
            }

            intGroupId = Convert.ToInt32(groupId);
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == false)
            {
                //datCreationDate.DateTime = DateTime.Now;
                GroupIdCalculation();
                txtSystemRef.Text = "Asset-" + intGroupId;
                lblRefNo.Text = " (Asset-" + intGroupId + ")";
                asset.transactionGroupId = intGroupId;
            }
            if (datCreationDate.DateTime == null)
            {
                DXMessageBox.Show("Please enter Creation Date!");
                datCreationDate.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtAssetName.Text))
            {
                DXMessageBox.Show("Please enter Asset Name!");
                txtAssetName.Focus();
                return;
            }
            if (lookupAssetType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Asset Type!");
                lookupAssetType.Focus();
                return;
            }
            if (lookupAssetNature.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Asset Nature!");
                lookupAssetNature.Focus();
                return;
            }
            if (lookupAssetSubNature.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Asset Sub Nature!");
                lookupAssetSubNature.Focus();
                return;
            }
            if (lookupAssetBrand.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Asset Brand!");
                lookupAssetBrand.Focus();
                return;
            }

            if (lookupAssetModel.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Asset Model!");
                lookupAssetModel.Focus();
                return;
            }

            if (lookupAssetNumber.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Asset Number!");
                lookupAssetNumber.Focus();
                return;
            }

            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company!");
                lookupCompany.Focus();
                return;
            }
            if (lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department!");
                lookupDepartment.Focus();
                return;
            }
            


            if (chkVendorFromSystem.IsChecked == true)
            {
                if (lookupVendor.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Vendor!");
                    lookupVendor.Focus();
                    return;
                }

            }
            else
            {
                if (String.IsNullOrEmpty(txtVendor.Text))
                {
                    DXMessageBox.Show("Please enter Vendor!");
                    txtVendor.Focus();
                    return;
                }
            }
            if (cmbStatus.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Status!");
                cmbStatus.Focus();
                return;
            }

            if (chkHolderFromSystem.IsChecked == true)
            {
                if (lookupAssetHolder.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Asset Holder!");
                    lookupAssetHolder.Focus();
                    return;
                }

            }
            else
            {
                if (String.IsNullOrEmpty(txtHolder.Text))
                {
                    DXMessageBox.Show("Please enter Asset Holder!");
                    txtHolder.Focus();
                    return;
                }
            }
           

            if (String.IsNullOrEmpty(txtDescription.Text))
            {
                DXMessageBox.Show("Please enter Description!");
                txtDescription.Focus();
                return;
            }
            if (lookupCountry.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Country!");
                lookupCountry.Focus();
                return;
            }
            if (lookupAssetCity.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select City!");
                lookupAssetCity.Focus();
                return;
            }
            if (lookupLocation.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Location!");
                lookupLocation.Focus();
                return;
            }
            if (lookupAssetUnit.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Unit!");
                lookupAssetUnit.Focus();
                return;
            }
            //if (String.IsNullOrEmpty(txtAddress.Text))
            //{
            //    DXMessageBox.Show("Please enter Address!");
            //    txtAddress.Focus();
            //    return;
            //}
            //if (String.IsNullOrEmpty(txtCity.Text))
            //{
            //    DXMessageBox.Show("Please enter City!");
            //    txtCity.Focus();
            //    return;
            //}
            //if (String.IsNullOrEmpty(txtCountry.Text))
            //{
            //    DXMessageBox.Show("Please enter Country!");
            //    txtCountry.Focus();
            //    return;
            //}

            if (chkSubsidary.IsChecked == true)
            {
                asset.isSubsidary = true;
                if (lookupParentAsset.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Parent Asset!");
                    lookupParentAsset.Focus();
                    return;
                }
                asset.parentId = (lookupParentAsset.SelectedItem as AssetRental).Id;

            }
            else
            {
                asset.isSubsidary = false;
                asset.parentId = null;
            }

            if (chkRentable.IsChecked == true)
                asset.isRentable = true;
            else
                asset.isRentable = false;

            if (chkLeased.IsChecked == true)
                asset.isLeased = true;
            else
                asset.isLeased = false;

            asset.CreationDate = datCreationDate.DateTime;
            asset.AssetName = txtAssetName.Text;
            asset.assetTypeId = (lookupAssetType.SelectedItem as AssetType).Id;
            asset.AssetNatureId = (lookupAssetNature.SelectedItem as RentalAssetNature).Id;
            asset.assetSubNatureId = (lookupAssetSubNature.SelectedItem as RentalAssetSubNature).Id;
            asset.assetBrandId = (lookupAssetBrand.SelectedItem as AssetBrand).Id;
            asset.assetModelId = (lookupAssetModel.SelectedItem as AssetModel).Id;
            asset.assetNumberId = (lookupAssetNumber.SelectedItem as AssetNumber).Id;
            asset.companyId = (lookupCompany.SelectedItem as Company).Id;
            asset.deptId = (lookupDepartment.SelectedItem as Department).Id;
            
            asset.statusId = (cmbStatus.SelectedItem as cmbitem).id;
            asset.SystemRef = txtSystemRef.Text;
            asset.AssetNumber = txtAssetNumber.Text;
            if (chkVendorFromSystem.IsChecked == true)
            {
                asset.VendorFromSystem = true;
                asset.vendorId = (lookupVendor.SelectedItem as Vendor).Id;
                asset.Vendor = null;
            }
            else
            {
                asset.VendorFromSystem = false;
                asset.vendorId = null;
                asset.Vendor = txtVendor.Text;
            }

            if (chkHolderFromSystem.IsChecked == true)
            {
                asset.assetHolderEmployeeId = (lookupAssetHolder.SelectedItem as ERP_BL.Databases.Employee).EmpId;
                asset.AssetHolder = null;
            }
            else
            {
                asset.assetHolderEmployeeId = null;
                asset.AssetHolder = txtHolder.Text;
            }

            asset.SerialNumber = txtSerialNo.Text;

            asset.Description = txtDescription.Text;
            asset.countryId = (lookupCountry.SelectedItem as ERP_BL.Countryy.Country).Id;
            asset.cityId = (lookupAssetCity.SelectedItem as City).Id;
            asset.assetRentalLocationId = (lookupLocation.SelectedItem as AssetRentalLocation).Id;
            asset.assetRentalUnitId = (lookupAssetUnit.SelectedItem as AssetRentalUnit).Id;

            //if (asset.address == null)
            //{
            //    asset.address = new Address();
            //}

            //asset.address.Line1 = txtAddress.Text;
            //asset.address.City = txtCity.Text;
            //asset.address.Country = txtCountry.Text;

            asset.PurchasingDate = datPurchaseDate.DateTime;
            asset.PurchasingCostManual = Convert.ToDouble(txtPurchaseCostManual.Text);

            if (!String.IsNullOrEmpty(txtProgressiveCostManual.Text))
                asset.ProgressiveCostManual = Convert.ToDouble(txtProgressiveCostManual.Text);
            else
                asset.ProgressiveCostManual = 0;

            asset.PurchasingCost = Convert.ToDouble(txtPurchaseCost.Text);
            asset.ProgressiveCost = Convert.ToDouble(txtProgressiveCost.Text);


            if (editFlag == false)
            {
                asset.creatorId = SYSTEM_STATIC.currentUser.id;
                asset.isApproved = false;
                rentalRepo.AddAssetRental(asset);
                UsersRepo.Add(TransactionInfo.Initialized, asset.Id, (int)TransactionItemType.AssetRental, "Asset Added");
                DXMessageBox.Show("Successfully Added!");
            }
            else
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Assets without Approval") != null && asset.isApproved != true)
                {
                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Asset is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        asset.stage = TransactionStage.Approved.ToString();
                        asset.isApproved = true;
                        asset.ApprovedDate = System.DateTime.Now;
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Assets without ReApproval") != null && asset.isReApproved == false)
                {
                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Asset is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        asset.stage = TransactionStage.Approved.ToString();
                        asset.isReApproved = true;
                        asset.ReApprovalDate = System.DateTime.Now;
                    }
                }
                if (checkStatus != null && checkStatus.Id != 0)
                {
                    if (checkStatus.Id != asset.Status.Id)
                    {
                        asset.LastStatusChangeDate = System.DateTime.Now;
                        if (asset.Status.isActive != true)
                        {
                            asset.ClosingDate = System.DateTime.Now;
                        }
                    }
                }

                rentalRepo.UpdateAssetRental(asset);

                if (checkStatus.Id != asset.Status.Id)
                {
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Status of Asset has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (lookupDepartment.SelectedItem as Department != null && (lookupDepartment.SelectedItem as Department).Id != 0 && (lookupCompany.SelectedItem as Company)?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            var userss = UsersRepo.getusersByCompanyDepartment(asset.department.Id, asset.company.Id);

                            if (userss.Find(x => x.id == asset.creatorId) == null)
                                userss.Add(asset.Creator);

                            winTagUsers win = new winTagUsers(userss, asset.Id, TransactionItemType.AssetRental);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;

                            if (tagUsers.Find(x => x.id == asset.creatorId) == null)
                                tagUsers.Add(asset.Creator);

                            if (ccUsers.Find(x => x.id == asset.creatorId) == null)
                                ccUsers.Add(asset.Creator);
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    string oldStat = checkStatus.Status;
                    string newStat = asset.Status.Status;
                    string symbolCurr = "";
                    //if (asset.currency != null)
                    //{
                    //    symbolCurr = asset.currency.Abbrivation.ToString();
                    //}
                    CommentLog comment = new CommentLog();

                    comment.Comment = "Status of Asset having System Ref: " + asset.SystemRef + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat;
                    comment.Timestamp = DateTime.Now;
                    comment.Subject = "Status Changed";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;


                    procurementRepo.Add(asset.Id, TransactionItemType.AssetRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating notification
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Asset with System Ref #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, comment.Comment, user.id, "New Comment ", null);
                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Asset with System Ref #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }



                }
                if (checkStatus != null && checkStatus.Id != 0)
                {
                    //saleOrderRepo.Add(saleOrderid, 3, "Status Changed from (" + checkStatus.Status + ") to (" + saleOrder.saleOrderStatus.Status + ")");
                    UsersRepo.Add(TransactionInfo.Status_Changed, asset.Id, 3, "Status Changed from (" + checkStatus.Status + ") to (" + asset.Status.Status + ")");
                }
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
                UsersRepo.Add(TransactionInfo.Edited, asset.Id, (int)TransactionItemType.AssetRental, frmInputBox.comment);

                DXMessageBox.Show("Successfully Updated!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void LookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            CompanyRepo companyRepo = new CompanyRepo();

            Company company = companyRepo.GetCompany((lookupCompany.SelectedItem as Company).Id);

            List<Department> departments = new List<Department>();

            if (company != null)
            {
                if (company.departments != null)
                {
                    foreach (var _dept in empUser.departments.Where(x => x.isActive == true && x.IsAssetType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    if (asset != null && editFlag == true)
                        if (asset.department != null)
                                if (departments.FirstOrDefault(x => x.Id == asset.department.Id) == null)
                                    departments.Add(asset.department);
                }

                lookupDepartment.ItemsSource = departments;
            }
        }

        private void LookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if(lookupCompany.SelectedIndex > -1 && lookupDepartment.SelectedIndex > -1)
            {
                var comp = lookupCompany.SelectedItem as Company;
                var dept = lookupDepartment.SelectedItem as Department;

                lookupParentAsset.ItemsSource = rentalRepo.GetActiveAssetRentalsByCompDept(comp.Id, dept.Id);

                //var employees = comp.employees.Where(x => x.departments.FirstOrDefault(y => y.Id == dept.Id) != null).ToList();
                //lookupAssetHolder.ItemsSource = employees;

                
                var vendors = rentalRepo.GetActiveVendorsByCompDept(comp.Id, dept.Id);
                lookupVendor.ItemsSource = vendors;
            }

            
        }

        private void LoadEmployees()
        {
            EmployeeRepo empRepo = new EmployeeRepo();
            var employees = empRepo.GetAllEmployees();
            lookupAssetHolder.ItemsSource = employees;
        }

        private void LookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void chkSubsidary_Checked(object sender, RoutedEventArgs e)
        {
            lookupParentAsset.IsEnabled = true;
        }

        private void chkSubsidary_Unchecked(object sender, RoutedEventArgs e)
        {
            lookupParentAsset.IsEnabled = false;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (assetId != 0)
            {
                UsersRepo.Add(TransactionInfo.viewed, assetId, (int)TransactionItemType.AssetRental, "Viewed details of Assets");
            }
        }

        private void btnCollapse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnexpand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void grdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void btnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                Button thisButton = (Button)sender;
                string str = thisButton.Tag.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    grdProgressBar.Visibility = Visibility.Visible;
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        //Button thisButton = (Button)sender;

                        ERP_BL.Attach attachment = new ERP_BL.Attach(str);

                        if (str.Contains("AssetRental"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.AssetRental);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else if (str.Contains("Rentals"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.AssetRental);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else if (str.Contains("Sale_Order"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Order);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Offer"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Offer);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Sale_Invoice"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Invoice);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Purchase_Order"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Purchase_Order);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Sale_Receipt"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Receipt);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Payments"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Payments);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Bill"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Bill);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("LoansAdvances"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.LoansAdvances);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }


                    });
                    thread.Start();
                    //grdProgressBar.Visibility = Visibility.Collapsed;
                }
                else
                    return;



            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), ex.ToString());


            }

        }

        private void btnAttachment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory1.SelectedItem != null)
            {
                if (assetId != 0)
                {
                    try
                    {

                        int CategoryId = (cmbCategory1.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Multiselect = false;
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\AssetRental\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += assetId + "_" + TransactionItemType.AssetRental.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.AssetRental);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), assetId, TransactionItemType.AssetRental, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, asset.Id, (int)TransactionItemType.AssetRental, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            //treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(OrderId, TransactionItemType.LoansAdvances);
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
                                        });



                                    }
                                    else
                                    {
                                        this.Dispatcher.Invoke(() =>
                                        {
                                            System.IO.File.Move(destination, sourceFile);
                                            DXMessageBox.Show("Error while Uploading Attachment, Try Again", "Try again");
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
                                        });
                                    }
                                });
                                thread.Start();
                            }
                            else
                            {
                                MessageBox.Show("Invalid File name size");
                                return;

                            }


                            //DXMessageBox.Show("Attachment Uploaded");


                        }


                    }
                    catch (Exception ex)
                    {
                        DXMessageBox.Show(ex.ToString());
                        imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                        btnAttachNew.ToolTip = "Attach";
                        btnAttachNew.IsEnabled = true;
                    }
                    finally
                    {



                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("Select a Attachment Category in which you want to upload an attachment.", "Select Category", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            if (gridTracker.Visibility == Visibility.Collapsed)
            {
                gridTracker.Visibility = Visibility.Visible;
            }
            else
            {
                gridTracker.Visibility = Visibility.Collapsed;
            }
        }

        private void btnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (editFlag == true && asset != null)
                {

                    //SalesReceipt receipt = new SalesReceipt();
                    asset = rentalRepo.GetAssetRental(asset.Id);
                    UsersRepo usersRepo = new UsersRepo();

                    if (asset != null)
                    {
                        if (asset.isApproved == true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Assets without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Assets") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Asset is Approved, Do you want to UnApprove?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    asset.isApproved = false;
                                    asset.stage = TransactionStage.AwaitingApproval.ToString();
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, asset.Id, (int)TransactionItemType.AssetRental, frmInputBox.comment);

                                    rentalRepo.UpdateAssetRental(asset);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Asset has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (asset.department != null && asset.department.Id != 0 && asset.company?.Id != 0)
                                        {
                                            var userss = usersRepo.getusersByCompanyDepartment(asset.department.Id, asset.company.Id);

                                            if (userss.Find(x => x.id == asset.creatorId) == null)
                                                userss.Add(asset.Creator);

                                            winTagUsers win = new winTagUsers(userss, asset.Id, TransactionItemType.AssetRental);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;

                                            if (tagUsers.Find(x => x.id == asset.creatorId) == null)
                                                tagUsers.Add(asset.Creator);

                                            if (ccUsers.Find(x => x.id == asset.creatorId) == null)
                                                ccUsers.Add(asset.Creator);
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    //string symbolCurr = "";
                                    //if (asset.currency != null)
                                    //{
                                    //    symbolCurr = asset.currency.Abbrivation.ToString();
                                    //}
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Asset having System Ref: " + asset.SystemRef.ToString() + " has been UnApproved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Asset UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(asset.Id, TransactionItemType.AssetRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Asset #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Asset #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Asset is UnApproved (" + asset.Id + ")");
                                    SystemLog.LogInfo(this.GetType(), "Asset is UnApproved (" + asset.Id + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Asset Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Asset Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (asset.isApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Assets without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Assets") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Asset is Pending for Approval, Do you want to Approve?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {

                                    asset.isApproved = true;
                                    asset.stage = TransactionStage.Approved.ToString();

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, asset.Id, (int)TransactionItemType.AssetRental, frmInputBox.comment);

                                    rentalRepo.UpdateAssetRental(asset);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Asset has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (asset.department != null && asset.department.Id != 0 && asset.company?.Id != 0)
                                        {
                                            var userss = usersRepo.getusersByCompanyDepartment(asset.department.Id, asset.company.Id);

                                            if (userss.Find(x => x.id == asset.creatorId) == null)
                                                userss.Add(asset.Creator);

                                            winTagUsers win = new winTagUsers(userss, asset.Id, TransactionItemType.AssetRental);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;

                                            if (tagUsers.Find(x => x.id == asset.creatorId) == null)
                                                tagUsers.Add(asset.Creator);

                                            if (ccUsers.Find(x => x.id == asset.creatorId) == null)
                                                ccUsers.Add(asset.Creator);
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    //string symbolCurr = "";
                                    //if (loansAdvance.currency != null)
                                    //{
                                    //    symbolCurr = loansAdvance.currency.Abbrivation.ToString();
                                    //}
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Asset having System Ref: " + asset.SystemRef.ToString() + " has been Approved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Asset Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(asset.Id, TransactionItemType.AssetRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Asset #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Asset #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Asset is Approved (" + asset.Id + ")");
                                    SystemLog.LogInfo(this.GetType(), "Asset is Approved (" + asset.Id + ")");
                                }


                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Asset Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Asset Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (asset.isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Assets without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Assets") != null) ? true : false)
                            {
                                asset.isReApproved = true;
                                asset.stage = TransactionStage.Approved.ToString();

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, asset.Id, (int)TransactionItemType.AssetRental, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                rentalRepo.UpdateAssetRental(asset);

                                MessageBox.Show("Asset is Approved (" + asset.Id + ")");
                                SystemLog.LogInfo(this.GetType(), "Asset is Approved (" + asset.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Assets Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Assets Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }

                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        AssetRentalStatus oldStatus = new AssetRentalStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        static AssetRentalStatus statusChanged = new AssetRentalStatus();

        public ucFrmAddAssetRental(AssetRentalStatus assetRentalStatus)
        {
            statusChanged = assetRentalStatus;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            //loansAdvanceRepo = new LoansAdvanceRepo();

            if (asset != null && assetId > 0)
            {
                var previous_status = asset.Status.Status;
                if (asset != null)
                {
                    if (asset.isApproved == false)
                    {
                        DXMessageBox.Show("Transaction is under approval!");
                        return;
                    }

                    statusChanged = null;
                    ucAssetRentalStatusChange ucFrmDirectClose = new ucAssetRentalStatusChange();
                    if (asset.Status != null)
                    {
                        ucFrmDirectClose.statusName.Text = asset.Status.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(asset.Status.backcolor);
                    }

                    ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;
                    ucFrmDirectClose.frmFlag = true;
                    ucFrmDirectClose.directCloseWin.Width = 450;
                    ucFrmDirectClose.directCloseWin.Height = 650;
                    ucFrmDirectClose.directCloseWin.ResizeMode = ResizeMode.NoResize;
                    ucFrmDirectClose.directCloseWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucFrmDirectClose.directCloseWin.ShowDialog();

                    if (statusChanged != null)
                    {
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Asset has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (asset.department != null && asset.department.Id != 0 && asset.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                var userss = usersRepo.getusersByCompanyDepartment(asset.department.Id, asset.company.Id);

                                if (userss.Find(x => x.id == asset.creatorId) == null)
                                    userss.Add(asset.Creator);


                                winTagUsers win = new winTagUsers(userss, asset.Id, TransactionItemType.AssetRental);
                                //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;



                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();

                            }
                        }


                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Assets") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Assets without Approval") != null)
                        {

                            asset.PendingForClosing = false;
                            asset.stage = TransactionStage.Closed.ToString();
                            asset.statusId = statusChanged.Id;
                            asset.LastStatusChangeDate = System.DateTime.Now;
                            asset.ClosingDate = System.DateTime.Now;



                            rentalRepo.UpdateAssetRental(asset);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.AssetRental, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Asset having system ref #: " + asset.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (asset != null)
                                {
                                    procurementRepo.Add(asset.Id, TransactionItemType.AssetRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Asset #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Asset #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {
                            asset.PendingForClosing = true;
                            asset.stage = TransactionStage.AwaitingApproval.ToString();
                            asset.statusId = statusChanged.Id;
                            asset.LastStatusChangeDate = System.DateTime.Now;
                            asset.ClosingDate = System.DateTime.Now;

                            rentalRepo.UpdateAssetRental(asset);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.AssetRental, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Asset having system ref #: " + asset.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (asset != null)
                                {
                                    procurementRepo.Add(asset.Id, TransactionItemType.AssetRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Asset #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Asset #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnAddComment_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true && asset != null)
            {

                //if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)

                //{

                //    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Bill);
                //    inputBox.ShowDialog();

                //}
                //                else 

                if (asset.department != null && asset.department.Id != 0 && asset.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*/)
                {

                    var userss = UsersRepo.getusersByCompanyDepartment(asset.department.Id, asset.company.Id);

                    if (asset.Creator != null && userss.Find(x => x.id == asset.creatorId) == null)
                        userss.Add(asset.Creator);

                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, userss, TransactionItemType.AssetRental);
                    inputBox.ShowDialog();

                }

                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                }

                if (asset != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    if (frmInputBox.comment != "" && asset.Id != 0)
                    {
                        if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                        {
                            var commentId = procurementRepo.AddCommentLinkNotification(asset.Id, TransactionItemType.AssetRental, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Asset with System Ref #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Asset with System Ref #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Asset with System Ref #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Asset with System Ref #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }


                            //foreach (var user in frmInputBox.Comment.TaggedList)
                            //{
                            //    if (frmInputBox.FlagForTag == true)
                            //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + loansAdvance.SalesReferenceNo, loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            //    else
                            //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + loansAdvance.SalesReferenceNo, loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.comment, user.id, "New Comment ", null);

                            //}
                            //foreach (var user in frmInputBox.Comment.CCUsersList)
                            //{
                            //    if (frmInputBox.FlagForCC == true)
                            //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Purchase Invoice #" + loansAdvance.SalesReferenceNo, loansAdvance.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                            //    else
                            //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Purchase Invoice #" + loansAdvance.SalesReferenceNo, loansAdvance.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, 0, user.id, "New Comment ", null);
                            //}
                        }

                        //procurementRepo.Add(loansAdvance.Id, TransactionItemType.Purchase_Invoice, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                        MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                        if (window1 != null) { window1.UrgentNotificationGlow(); }
                    }
                    else if (asset.Id == 0)
                    {
                        DXMessageBox.Show("Kindly save this Transaction first to add a comment!");
                    }

                }
            }
        }

        public void loadcomments()
        {
            try
            {
                if (asset != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(asset.Id, TransactionItemType.AssetRental);

                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void btnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                grdComments.Visibility = Visibility.Visible;
                loadcomments();
            }
            else
                grdComments.Visibility = Visibility.Collapsed;
        }

        private void btnAttachNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
            else
            {
                treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(asset.Id, TransactionItemType.AssetRental);
                grdAttachments.Visibility = Visibility.Visible;
            }
        }

        private void btnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (asset != null && asset.Id > 0)
            {
                var idd = asset.Id;
                if (idd != 0)
                {
                    frmTrackingWindow trackingWindow = new frmTrackingWindow(idd, TransactionItemType.AssetRental);
                    trackingWindow.ShowDialog();
                }
            }
            else
            {
                DXMessageBox.Show("Please save this Transaction first!");
            }
        }

        private void btnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (asset.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Assets") != null))
            {
                if (DXMessageBox.Show("This is currently in the list of Void Assets! Do you want to remove it from Void?", "Remove Void Asset", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    asset.isVoid = false;
                    rentalRepo.UpdateAssetRental(asset);

                    grdVoid.Visibility = Visibility.Collapsed;

                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Asset has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (asset.department != null && asset.department.Id != 0 && asset.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(asset.department.Id, asset.company.Id), asset.Id, TransactionItemType.AssetRental);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    //string symbolCurr = "";
                    //if (asset.currency != null)
                    //{
                    //    symbolCurr = asset.currency.Abbrivation.ToString();
                    //}
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Asset having System Ref: " + asset.SystemRef + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "Asset UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(asset.Id, TransactionItemType.AssetRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Asset # " + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Asset #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Assets") != null)
            {
                if (DXMessageBox.Show("This is not currently in the list of Void Assets! Do you want to move it to Void assets?", "Add to Void Assets", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    asset.isVoid = true;
                    rentalRepo.UpdateAssetRental(asset);

                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);

                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Asset has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (asset.department != null && asset.department.Id != 0 && asset.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*//*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(asset.department.Id, asset.company.Id), asset.Id, TransactionItemType.AssetRental);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    //string symbolCurr = "";
                    //if (loansAdvance.currency != null)
                    //{
                    //    symbolCurr = loansAdvance.currency.Abbrivation.ToString();
                    //}
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Asset having System Ref: " + asset.SystemRef + " has been marked as void",
                        Timestamp = DateTime.Now,
                        Subject = "Asset Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(asset.Id, TransactionItemType.AssetRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Asset #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Asset #" + asset.SystemRef, asset.Id, TransactionItemType.AssetRental, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }

            loadcomments();
            var thisWindow = Window.GetWindow(this);
            thisWindow.Close();
        }

        private void btnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
            else
            {
                cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveAssetAttachmentCategories();
                grdAttach1.Visibility = Visibility.Visible;
            }
        }

        private void btnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (grdAttachments1.Visibility == Visibility.Visible)
                grdAttachments1.Visibility = Visibility.Collapsed;
            else
            {
                if (asset.Id != 0)
                {
                    List<TreeItem> otherAttachments = new List<TreeItem>();
                    List<TreeItem> atachments = SYSTEM_STATIC.GetAssetAttachmentsListByCategory((int)asset.Id, TransactionItemType.AssetRental);

                    foreach (var cat in atachments)
                    {
                        foreach (var otherCat in otherAttachments)
                        {
                            if (otherCat.name == cat.name)
                            {
                                foreach (var file in otherCat.Items)
                                {
                                    cat.Items.Add(file);
                                }
                            }
                        }
                    }
                    treeViewAttachments1.ItemsSource = atachments;
                }
                grdAttachments1.Visibility = Visibility.Visible;

            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void btnStageTracking_Click(object sender, RoutedEventArgs e)
        {
            if (gridOrderStageTrack.Visibility == Visibility.Collapsed)
            {
                ucTrackingGrid trackingGrid = new ucTrackingGrid();
                trackingGrid.itemId = asset.Id;
                trackingGrid.itemType = TransactionItemType.AssetRental;
                grdTracking.Children.Add(trackingGrid);
                gridOrderStageTrack.Visibility = Visibility.Visible;
            }
            else
            {
                grdTracking.Children.Clear();
                gridOrderStageTrack.Visibility = Visibility.Collapsed;
            }
        }

        private void btnCreateRentalInvoice_Click(object sender, RoutedEventArgs e)
        {

        }

        public void GellAllOrdersTracking()
        {

            //saleOrderRepo = new SaleOrderRepo();
            trackingOrder = rentalRepo.GetAssetRental(asset.Id);
            if (trackingOrder != null)
            {
                if (grdTracking.Children != null && grdTracking.Children.Count > 0)
                {
                    ucTrackingGrid uc = grdTracking.Children[0] as ucTrackingGrid;


                    OrderTracking tracking = new OrderTracking();
                    uc.grdOrdersTracking.ItemsSource = tracking.getTransactions(trackingOrder.Id, TransactionItemType.AssetRental);
                }
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GellAllOrdersTracking();
        }

        private void btnExpand_Click_1(object sender, EventArgs e)
        {
            if (grdTracking.Children != null && grdTracking.Children.Count > 0)
            {
                ucTrackingGrid uc = grdTracking.Children[0] as ucTrackingGrid;
                uc.grdOrdersTracking.ShowLoadingPanel = true;
                (uc.grdOrdersTracking.View as TreeListView).ExpandAllNodes();
                uc.grdOrdersTracking.ShowLoadingPanel = false;
            }
        }

        private void btnCollapsed_Click(object sender, EventArgs e)
        {
            if (grdTracking.Children != null && grdTracking.Children.Count > 0)
            {
                ucTrackingGrid uc = grdTracking.Children[0] as ucTrackingGrid;
                uc.grdOrdersTracking.ShowLoadingPanel = true;

                (uc.grdOrdersTracking.View as TreeListView).CollapseAllNodes();
                uc.grdOrdersTracking.ShowLoadingPanel = false;
            }

        }

        private void lookupCountry_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var country = lookupCountry.SelectedItem as ERP_BL.Countryy.Country;

            CountryRepo countryRepo = new CountryRepo();

            var cities = countryRepo.GetAllCitiesByCountry(country);
            lookupAssetCity.ItemsSource = cities;
        }

        private void lookupAssetCity_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var country = lookupCountry.SelectedItem as ERP_BL.Countryy.Country;
            var city = lookupAssetCity.SelectedItem as ERP_BL.Countryy.City;

            if(country != null && city != null)
            {
                var locations = rentalRepo.GetAllAssetRentalLocationByCountryCity(country, city);

                lookupLocation.ItemsSource = locations;
            }
            
        }

        private void btnCreateBillForPurchasingCost_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                Window win = new Window();

                ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();
                frmBillAdd.assetRentalId = asset.Id;
                frmBillAdd.billTypes = AdminBillTypes.Asset;
                frmBillAdd.isProgressiveCost = false;
                frmBillAdd.editFlag = false;

                win.Content = frmBillAdd;
                win.Show();
            }
        }

        private void btnCreateAdminBill_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                Window win = new Window();

                ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();
                frmBillAdd.assetRentalId = asset.Id;
                frmBillAdd.billTypes = AdminBillTypes.Asset;
                frmBillAdd.isProgressiveCost = true;
                frmBillAdd.editFlag = false;

                win.Content = frmBillAdd;
                win.Show();
            }
        }

        private void LookupAssetHolder_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void chkVendorFromSystem_Checked(object sender, RoutedEventArgs e)
        {
            txtVendor.Visibility = Visibility.Collapsed;
            lookupVendor.Visibility = Visibility.Visible;
        }

        private void chkVendorFromSystem_Unchecked(object sender, RoutedEventArgs e)
        {
            txtVendor.Visibility = Visibility.Visible;
            lookupVendor.Visibility = Visibility.Collapsed;
        }

        private void lookupAssetUnit_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void lookupLocation_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var country = lookupCountry.SelectedItem as ERP_BL.Countryy.Country;
            var city = lookupAssetCity.SelectedItem as ERP_BL.Countryy.City;
            var location = lookupLocation.SelectedItem as AssetRentalLocation;

            if (country != null && city != null && location != null)
            {
                var units = rentalRepo.GetAllAssetRentalUnitByCountryCityLocation(country, city, location);

                lookupAssetUnit.ItemsSource = units;
            }
        }

        private void lookupAssetNature_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var nature = lookupAssetNature.SelectedItem as RentalAssetNature;

            if (nature != null)
            {
                var natureList = rentalRepo.GetAllRentalAssetSubNature().Where(x => x.assetNatureId == nature.Id);
                lookupAssetSubNature.ItemsSource = natureList;
            }
        }

        private void lookupAssetSubNature_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var nature = lookupAssetNature.SelectedItem as RentalAssetNature;
            var subNature = lookupAssetSubNature.SelectedItem as RentalAssetSubNature;

            if (nature != null && subNature != null)
                lookupAssetBrand.ItemsSource = rentalRepo.GetAllAssetBrand().Where(x => x.assetNatureId == nature.Id && x.AssetSubNatures.FirstOrDefault(y=>y.Id  == subNature.Id) != null).ToList();
        }

        private void lookupAssetBrand_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var nature = lookupAssetNature.SelectedItem as RentalAssetNature;
            var subNature = lookupAssetSubNature.SelectedItem as RentalAssetSubNature;
            var brand = lookupAssetBrand.SelectedItem as AssetBrand;

            if(nature != null && subNature != null && brand != null)
            {
                var models = rentalRepo.GetAllAssetModel().Where(x => x.assetNatureId == nature.Id && x.AssetSubNatures.FirstOrDefault(y => y.Id == subNature.Id) != null && x.assetBrands.FirstOrDefault(y => y.Id == brand.Id) != null).ToList();
                lookupAssetModel.ItemsSource = models;
            }
        }

        private void chkHolderFromSystem_Checked(object sender, RoutedEventArgs e)
        {
            txtHolder.Visibility = Visibility.Collapsed;
            lookupAssetHolder.Visibility = Visibility.Visible;
        }

        private void chkHolderFromSystem_Unchecked(object sender, RoutedEventArgs e)
        {
            txtHolder.Visibility = Visibility.Visible;
            lookupAssetHolder.Visibility = Visibility.Collapsed;
        }

    }
}
