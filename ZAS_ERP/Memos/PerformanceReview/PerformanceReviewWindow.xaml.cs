using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.Memos;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace ZAS_ERP.Memos.PerformanceReview
{
    /// <summary>
    /// Interaction logic for PerformanceReviewWindow.xaml
    /// </summary>
    public partial class PerformanceReviewWindow : Window
    {
        List<PerformanceIndicatorDefinition> definitions = new List<PerformanceIndicatorDefinition>();
        private List<PerformanceIndicatorRating> ratings;
        EmployeePerformanceReview review = new EmployeePerformanceReview();
        MemoRepo memoRepo = new MemoRepo();
        public int reviewIdToEdit;
        public bool editFlag = false;
        public int memoId = 0;
        private bool isLoading = false;

        List<User> AllAllowedUser = new List<User>();
        List<User> SelectedAllowedUser = new List<User>();

        PerformanceReviewerStage? performanceReviewerStage = null;
        public PerformanceReviewWindow()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            isLoading = true;

            //LoadFormOptions();
            LoadIndicators();

            var memo = memoRepo.GetMemo(memoId);

            List<ERP_BL.Databases.User> users = new List<User>();
            users.Add(memo.createdFor);
            lookupEmployee.ItemsSource = users;

            lookupEmployee.Text = memo.createdFor.userName;

            users = new List<User>();
            users = memo.CCUsersList;

            lookupSupervisor.ItemsSource = users;
            lookupSupervisorLevel2.ItemsSource = users;
            lookupAdmin.ItemsSource = users;
            lookupManagement.ItemsSource = users;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Create Performance Review from Memo") != null)
            {
                datYear.IsEnabled = true;
            }

            if (editFlag == false)
            {
                datCreationDate.EditValue = DateTime.Now;

                gridRatings.Columns["SelfRating"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                gridRatings.Columns["SupervisorLevelOneRating"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                gridRatings.Columns["SupervisorLevelTwoRating"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                gridRatings.Columns["AdminRating"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                gridRatings.Columns["ManagementRating"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;

                chkLock.Visibility = Visibility.Collapsed;
                radioLevelTwo.IsChecked = true;

                datYear.DateTime = DateTime.Now;

                tabUsers.IsEnabled = false;
            }
            else if(editFlag == true)
            {
                review = memoRepo.GetPerformanceReview(reviewIdToEdit);

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Create Performance Review from Memo") == null)
                {
                    tabUsers.IsEnabled = false;
                }

                if(review.HiddenChatUsers != null && review.HiddenChatUsers.Count > 0 && review.HiddenChatUsers.FirstOrDefault(x=>x.id == SYSTEM_STATIC.currentUser.id) != null)
                {
                    btnAddHiddenComment.Visibility = Visibility.Visible;
                    btnHiddenCommentLog.Visibility = Visibility.Visible;
                }

                txtStage.Text = review.performanceReviewerStage.ToString();

                var userIds = review.Memo?.CCUsersList.Select(x=>x.id);

                if(userIds != null && userIds.Count() > 0)
                {
                    UsersRepo usersRepo = new UsersRepo();
                    foreach (var id in userIds)
                    {
                        AllAllowedUser.Add(usersRepo.GetUserforPerformanceReview(id));
                    }
                    //AllAllowedUser = review.Memo?.CCUsersList;

                    if (review.HiddenChatUsers != null)
                        foreach (var _selectedUser in review.HiddenChatUsers)
                        {
                            SelectedAllowedUser.Add(_selectedUser);
                        }
                    grdCntrlUsersSelected.ItemsSource = null;
                    grdCntrlUsersSelected.ItemsSource = SelectedAllowedUser;

                    foreach (var rrr in SelectedAllowedUser)
                    {
                        if (AllAllowedUser.Contains(AllAllowedUser.FirstOrDefault(x=>x.id == rrr.id)))
                        {
                            AllAllowedUser.Remove(AllAllowedUser.FirstOrDefault(x => x.id == rrr.id));
                        }
                    }
                    grdCntrlUsers.ItemsSource = null;
                    grdCntrlUsers.ItemsSource = AllAllowedUser;
                }

                if(review.HiddenChatUsers != null && review.HiddenChatUsers.Count > 0 && review.HiddenChatUsers.FirstOrDefault(x=>x.id == SYSTEM_STATIC.currentUser.id) != null)
                {
                    btnHiddenCommentLog.Visibility = Visibility.Visible;
                    btnAddHiddenComment.Visibility = Visibility.Visible;
                }


                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Create Performance Review from Memo") == null)
                {
                    radioSelf.IsEnabled = false;
                    radioLevelOne.IsEnabled = false;
                    radioLevelTwo.IsEnabled = false;

                    lookupEmployee.IsEnabled = false;
                    lookupSupervisor.IsEnabled = false;
                    lookupSupervisorLevel2.IsEnabled = false;
                    lookupAdmin.IsEnabled = false;
                    lookupManagement.IsEnabled = false;
                }

                if (review == null)
                {
                    MessageBox.Show("Review not found.");
                    this.Close();
                    return;
                }


                var totalScore = ratings.Sum(x => x.IndicatorDefinition.Weightage);

                double gainedScore = review.Ratings.Sum(x => x.SelfRating);
                txtSelfPercentage.Text = Math.Round(((gainedScore / totalScore) * 100), 2) + " %";

                gainedScore = review.Ratings.Sum(x => x.SupervisorLevelOneRating);
                txtSupervisorPercentage.Text = Math.Round(((gainedScore / totalScore) * 100), 2) + " %";

                gainedScore = review.Ratings.Sum(x => x.SupervisorLevelTwoRating);
                txtSupervisorLevel2Percentage.Text = Math.Round(((gainedScore / totalScore) * 100), 2) + " %";

                gainedScore = review.Ratings.Sum(x => x.AdminRating);
                txtAdminPercentage.Text = Math.Round(((gainedScore / totalScore) * 100), 2) + " %";

                gainedScore = review.Ratings.Sum(x => x.ManagementRating);
                txtManagementPercentage.Text = Math.Round(((gainedScore / totalScore) * 100), 2) + " %";

                switch (review.performanceReviewType)
                {
                    case PerformanceReviewType.Self:
                        radioSelf.IsChecked = true;

                        txtSupervisorPercentage.Text = "N/A";
                        txtSupervisorLevel2Percentage.Text = "N/A";
                        break;
                    case PerformanceReviewType.LevelOne:
                        radioLevelOne.IsChecked = true;

                        if (review.SupervisorLevelOne != null)
                            lookupSupervisor.Text = review.SupervisorLevelOne.userName;

                        txtSupervisorLevel2Percentage.Text = "N/A";
                        break;
                    case PerformanceReviewType.LevelTwo:
                        radioLevelTwo.IsChecked = true;

                        if (review.SupervisorLevelOne != null)
                            lookupSupervisor.Text = review.SupervisorLevelOne.userName;

                        if (review.SupervisorLevelTwo != null)
                            lookupSupervisorLevel2.Text = review.SupervisorLevelTwo.userName;
                        break;
                }

                if (review.CreationDate != null)
                    datCreationDate.EditValue = review.CreationDate;

                if(review.Employee != null)
                    lookupEmployee.Text = review.Employee.userName;

                if (review.Admin != null)
                    lookupAdmin.Text = review.Admin.userName;

                if (review.Management != null)
                    lookupManagement.Text = review.Management.userName;

                // Load ratings
                // Load all active indicator definitions
                definitions = memoRepo.GetAllActivePerformanceIndicators();

                // Load existing ratings
                var existingRatings = review.Ratings.ToList();

                // Merge: match existing ratings or create new empty ones
                ratings = definitions.Select(def =>
                {
                    var existing = existingRatings.FirstOrDefault(r => r.IndicatorDefinitionId == def.Id);
                    if (existing != null)
                    {
                        return existing;
                    }
                    else
                    {
                        return new PerformanceIndicatorRating
                        {
                            ReviewId = review.Id,
                            IndicatorDefinitionId = def.Id,
                            IndicatorDefinition = def,
                            SelfRating = 0,
                            SupervisorLevelOneRating = 0,
                            SupervisorLevelTwoRating = 0,
                            AdminRating = 0,
                            ManagementRating = 0
                        };
                    }
                }).OrderBy(r => r.IndicatorDefinition.DisplayOrder).ToList();

                gridRatings.ItemsSource = ratings;


                datYear.DateTime = review.PerformanceYear.Value;

                SetStages();

                if (review.Employee != null)
                {
                    var byteImg = review.Employee.employee?.person?.Photo;
                    if (byteImg != null)
                    {
                        var image = GetBitmapImageFromByteArray(byteImg);
                        UserImage.ImageSource = image;
                    }
                }

                

            }
            isLoading = false;
        }

        private void SetStages()
        {
            // Stage variable for easier comparison
            var stage = review.performanceReviewerStage;

            if(review.performanceReviewType == PerformanceReviewType.LevelTwo)
            {
                // Cache for user role
                bool isEmployee = review.Employee?.id == SYSTEM_STATIC.currentUser.id;
                bool isSupervisorLevel1 = review.SupervisorLevelOne?.id == SYSTEM_STATIC.currentUser.id;
                bool isSupervisorLevel2 = review.SupervisorLevelTwo?.id == SYSTEM_STATIC.currentUser.id;
                bool isAdmin = review.Admin?.id == SYSTEM_STATIC.currentUser.id;
                bool isManagement = review.Management?.id == SYSTEM_STATIC.currentUser.id;

                // Employee
                if (isEmployee)
                {
                    chkLock.Visibility = Visibility.Collapsed;

                    if (stage == ERP_BL.Enums.PerformanceReviewerStage.Self)
                        SetAllowEditing(true, false, false, false, false);
                    else
                    {
                        SetAllowEditing(false, false, false, false, false);
                        //MainFormGrid.IsEnabled = false;
                    }
                }

                // Supervisor Level 1
                else if (isSupervisorLevel1)
                {
                    if (stage == ERP_BL.Enums.PerformanceReviewerStage.Self)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Content = "Lock for Employee";
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel1)
                    {
                        SetAllowEditing(false, true, false, false, false);
                        chkLock.Content = "Locked for Employee";
                        chkLock.IsChecked = true;
                        chkLock.Visibility = Visibility.Visible;

                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel2)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;

                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Admin)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;

                    }
                    else if(stage == ERP_BL.Enums.PerformanceReviewerStage.Management)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                        //MainFormGrid.IsEnabled = false;
                    }
                }

                // Supervisor Level 2
                else if (isSupervisorLevel2)
                {
                    if (stage == ERP_BL.Enums.PerformanceReviewerStage.Self)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel1)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Content = "Lock for Supervisor (L1)";
                        chkLock.Visibility = Visibility.Visible;

                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel2)
                    {
                        SetAllowEditing(false, false, true, false, false);
                        chkLock.Content = "Locked for Supervisor (L1)";
                        chkLock.IsChecked = true;
                        chkLock.Visibility = Visibility.Visible;

                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Admin)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Management)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                        //MainFormGrid.IsEnabled = false;
                    }
                }

                // Admin
                else if (isAdmin)
                {
                    if (stage == ERP_BL.Enums.PerformanceReviewerStage.Self)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel1)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel2)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Content = "Lock for Supervisor (L2)";
                        chkLock.Visibility = Visibility.Visible;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Admin)
                    {
                        SetAllowEditing(false, false, false, true, false);
                        chkLock.Content = "Locked for Supervisor (L2)";
                        chkLock.IsChecked = true;
                        chkLock.Visibility = Visibility.Visible;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Management)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                        //MainFormGrid.IsEnabled = false;
                    }
                }

                // Management
                else if (isManagement)
                {
                    if (stage == ERP_BL.Enums.PerformanceReviewerStage.Self)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel1)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel2)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Admin)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Content = "Lock for Admin";
                        chkLock.Visibility = Visibility.Visible;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Management)
                    {
                        SetAllowEditing(false, false, false, false, true);
                        chkLock.Content = "Locked for Admin";
                        chkLock.IsChecked = true;
                        chkLock.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    //MainFormGrid.IsEnabled = false;
                }
            }
            else if(review.performanceReviewType == PerformanceReviewType.LevelOne)
            {
                // Cache for user role
                bool isEmployee = review.Employee?.id == SYSTEM_STATIC.currentUser.id;
                bool isSupervisorLevel1 = review.SupervisorLevelOne?.id == SYSTEM_STATIC.currentUser.id;
                bool isAdmin = review.Admin?.id == SYSTEM_STATIC.currentUser.id;
                bool isManagement = review.Management?.id == SYSTEM_STATIC.currentUser.id;

                // Employee
                if (isEmployee)
                {
                    chkLock.Visibility = Visibility.Collapsed;

                    if (stage == ERP_BL.Enums.PerformanceReviewerStage.Self)
                        SetAllowEditing(true, false, false, false, false);
                    else
                    {
                        SetAllowEditing(false, false, false, false, false);
                        //MainFormGrid.IsEnabled = false;
                    }
                }

                // Supervisor Level 1
                else if (isSupervisorLevel1)
                {
                    if (stage == ERP_BL.Enums.PerformanceReviewerStage.Self)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Content = "Lock for Employee";
                        chkLock.Visibility = Visibility.Visible;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel1)
                    {
                        SetAllowEditing(false, true, false, false, false);
                        chkLock.Content = "Locked for Employee";
                        chkLock.IsChecked = true;
                        chkLock.Visibility = Visibility.Visible;

                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Admin)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Management)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                        //MainFormGrid.IsEnabled = false;
                    }
                }

                // Admin
                else if (isAdmin)
                {
                    if (stage == ERP_BL.Enums.PerformanceReviewerStage.Self)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel1)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Content = "Lock for Supervisor";
                        chkLock.Visibility = Visibility.Visible;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Admin)
                    {
                        SetAllowEditing(false, false, false, true, false);
                        chkLock.Content = "Locked for Supervisor";
                        chkLock.IsChecked = true;
                        chkLock.Visibility = Visibility.Visible;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Management)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                    }
                }

                // Management
                else if (isManagement)
                {
                    if (stage == ERP_BL.Enums.PerformanceReviewerStage.Self)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel1)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Admin)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Content = "Lock for Admin";
                        chkLock.Visibility = Visibility.Visible;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Management)
                    {
                        SetAllowEditing(false, false, false, false, true);
                        chkLock.Content = "Locked for Admin";
                        chkLock.IsChecked = true;
                        chkLock.Visibility = Visibility.Visible;
                    }
                }
            }
            else if (review.performanceReviewType == PerformanceReviewType.Self)
            {
                // Cache for user role
                bool isEmployee = review.Employee?.id == SYSTEM_STATIC.currentUser.id;
                bool isAdmin = review.Admin?.id == SYSTEM_STATIC.currentUser.id;
                bool isManagement = review.Management?.id == SYSTEM_STATIC.currentUser.id;

                // Employee
                if (isEmployee)
                {
                    chkLock.Visibility = Visibility.Collapsed;

                    if (stage == ERP_BL.Enums.PerformanceReviewerStage.Self)
                        SetAllowEditing(true, false, false, false, false);
                    else
                    {
                        SetAllowEditing(false, false, false, false, false);
                        //MainFormGrid.IsEnabled = false;
                    }
                }              

                // Admin
                else if (isAdmin)
                {
                    if (stage == ERP_BL.Enums.PerformanceReviewerStage.Self)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Content = "Lock for Employee";
                        chkLock.Visibility = Visibility.Visible;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Admin)
                    {
                        SetAllowEditing(false, false, false, true, false);
                        chkLock.Content = "Locked for Employee";
                        chkLock.IsChecked = true;
                        chkLock.Visibility = Visibility.Visible;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Management)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Visibility = Visibility.Collapsed;
                        //MainFormGrid.IsEnabled = false;
                    }
                }

                // Management
                else if (isManagement)
                {
                    if (stage == ERP_BL.Enums.PerformanceReviewerStage.Self)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Content = "Lock for Employee";
                        chkLock.Visibility = Visibility.Collapsed;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Admin)
                    {
                        SetAllowEditing(false, false, false, false, false);
                        chkLock.Content = "Lock for Admin";
                        chkLock.Visibility = Visibility.Visible;
                    }
                    else if (stage == ERP_BL.Enums.PerformanceReviewerStage.Management)
                    {
                        SetAllowEditing(false, false, false, false, true);
                        chkLock.Content = "Locked for Admin";
                        chkLock.IsChecked = true;
                        chkLock.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        public BitmapImage GetBitmapImageFromByteArray(byte[] bytesArr)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                stream.Write(bytesArr, 0, bytesArr.Length);
                stream.Position = 0;
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage returnImage = new BitmapImage();
                returnImage.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                returnImage.StreamSource = ms;
                returnImage.EndInit();

                return returnImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        // Helper function to set all ratings' editing permissions
        void SetAllowEditing(bool self, bool supervisorLevel1, bool supervisorLevel2, bool admin, bool management)
        {
            gridRatings.Columns["SelfRating"].AllowEditing = self ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
            gridRatings.Columns["SupervisorLevelOneRating"].AllowEditing = supervisorLevel1 ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
            gridRatings.Columns["SupervisorLevelTwoRating"].AllowEditing = supervisorLevel2 ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
            gridRatings.Columns["AdminRating"].AllowEditing = admin ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
            gridRatings.Columns["ManagementRating"].AllowEditing = management ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
        }

        private void LoadFormOptions()
        {
            lookupEmployee.ItemsSource = memoRepo.getAllusers();
            lookupSupervisor.ItemsSource = memoRepo.getAllusers();
            lookupAdmin.ItemsSource = memoRepo.getAllusers();
        }

        private void LoadIndicators()
        {
            definitions = memoRepo.GetAllActivePerformanceIndicators();
            ratings = definitions.Select(def => new PerformanceIndicatorRating
            {
                IndicatorDefinitionId = def.Id,
                IndicatorDefinition = def
            }).OrderBy(r => r.IndicatorDefinition.DisplayOrder).ToList();

            gridRatings.ItemsSource = ratings;
            var total = definitions.Sum(x=>x.Weightage);
        }

        private void LoadReviewForEditing(int reviewId)
        {
           
        }


        private void BtnSaveReview_Click(object sender, RoutedEventArgs e)
        {
            if (lookupEmployee.SelectedItem == null)
            {
                MessageBox.Show("Please select Employee!");
                lookupEmployee.Focus();
                return;
            }

            if (radioLevelTwo.IsChecked == true)
            {
                if (lookupSupervisor.SelectedItem == null)
                {
                    MessageBox.Show("Please select Supervisor!");
                    lookupSupervisor.Focus();
                    return;
                }
                if (lookupSupervisorLevel2.SelectedItem == null)
                {
                    MessageBox.Show("Please select Supervisor Level 2!");
                    lookupSupervisorLevel2.Focus();
                    return;
                }
            }
            else if (radioLevelOne.IsChecked == true)
            {
                if (lookupSupervisor.SelectedItem == null)
                {
                    MessageBox.Show("Please select Supervisor!");
                    lookupSupervisor.Focus();
                    return;
                }
            }

            if (lookupAdmin.SelectedItem == null)
            {
                MessageBox.Show("Please select Admin!");
                lookupAdmin.Focus();
                return;
            }
            if (lookupManagement.SelectedItem == null)
            {
                MessageBox.Show("Please select Management!");
                lookupManagement.Focus();
                return;
            }
            if (lookupEmployee.SelectedItem == null)
            {
                MessageBox.Show("Please select Employee!");
                lookupEmployee.Focus();
                return;
            }
            if (datYear.EditValue == null)
            {
                MessageBox.Show("Please select Year!");
                datYear.Focus();
                return;
            }

            if (reviewIdToEdit > 0 && editFlag == true)
            {
                // UPDATE MODE
                var updatedReview = memoRepo.GetPerformanceReview(reviewIdToEdit);

                if(SelectedAllowedUser == null || SelectedAllowedUser.Count == 0)
                {
                    updatedReview.HiddenChatUsers = null;
                }
                else if (SelectedAllowedUser.Count != 0)
                {
                    updatedReview.HiddenChatUsers = new List<User>();
                    foreach (var _user in SelectedAllowedUser)
                    {
                        if (!updatedReview.HiddenChatUsers.Contains(_user))
                        {
                            updatedReview.HiddenChatUsers.Add(_user);
                        }
                    }
                }

                if (performanceReviewerStage != null)
                    updatedReview.performanceReviewerStage = performanceReviewerStage.Value;

                updatedReview.PerformanceYear = datYear.DateTime;
                updatedReview.EmployeeId = (lookupEmployee.SelectedItem as User)?.id;

                if (radioLevelTwo.IsChecked == true)
                {
                    updatedReview.performanceReviewType = PerformanceReviewType.LevelTwo;
                    updatedReview.SupervisorLevelOneId = (lookupSupervisor.SelectedItem as User)?.id;
                    updatedReview.SupervisorLevelTwoId = (lookupSupervisorLevel2.SelectedItem as User)?.id;
                }
                else if (radioLevelOne.IsChecked == true)
                {
                    updatedReview.performanceReviewType = PerformanceReviewType.LevelOne;
                    updatedReview.SupervisorLevelOneId = (lookupSupervisor.SelectedItem as User)?.id;
                    updatedReview.SupervisorLevelTwoId = null;
                }
                else if (radioSelf.IsChecked == true)
                {
                    updatedReview.performanceReviewType = PerformanceReviewType.Self;
                    updatedReview.SupervisorLevelOneId = null;
                    updatedReview.SupervisorLevelTwoId = null;
                }
                updatedReview.AdminId = (lookupAdmin.SelectedItem as User)?.id;
                updatedReview.ManagementId = (lookupManagement.SelectedItem as User)?.id;
                updatedReview.Ratings = ratings;

                memoRepo.UpdatePerformanceReview(updatedReview); // You implement this
                MessageBox.Show("Review updated successfully!");
            }
            else
            {
                // Create MODE
                EmployeePerformanceReview performanceReview = new EmployeePerformanceReview();

                performanceReview.MemoId = memoId;
                performanceReview.CreationDate = DateTime.Now;

                if (performanceReviewerStage != null)
                    performanceReview.performanceReviewerStage = performanceReviewerStage.Value;

                performanceReview.PerformanceYear = datYear.DateTime;
                performanceReview.EmployeeId = (lookupEmployee.SelectedItem as User)?.id;

                if (radioLevelTwo.IsChecked == true)
                {
                    performanceReview.performanceReviewType = PerformanceReviewType.LevelTwo;
                    performanceReview.SupervisorLevelOneId = (lookupSupervisor.SelectedItem as User)?.id;
                    performanceReview.SupervisorLevelTwoId = (lookupSupervisorLevel2.SelectedItem as User)?.id;
                }
                else if (radioLevelOne.IsChecked == true)
                {
                    performanceReview.performanceReviewType = PerformanceReviewType.LevelOne;
                    performanceReview.SupervisorLevelOneId = (lookupSupervisor.SelectedItem as User)?.id;
                    performanceReview.SupervisorLevelTwoId = null;
                }
                else if (radioSelf.IsChecked == true)
                {
                    performanceReview.performanceReviewType = PerformanceReviewType.Self;
                    performanceReview.SupervisorLevelOneId = null;
                    performanceReview.SupervisorLevelTwoId = null;
                }
                performanceReview.AdminId = (lookupAdmin.SelectedItem as User)?.id;
                performanceReview.ManagementId = (lookupManagement.SelectedItem as User)?.id;
                performanceReview.Ratings = ratings;

                memoRepo.AddPerformanceReview(performanceReview);
                MessageBox.Show("Review saved successfully!");
            }

            this.Close();
        }

        private void tableViewRatings_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var row = e.Row as PerformanceIndicatorRating;
            if (row == null)
                return;

            string fieldName = e.Column.FieldName;
            int? oldValue = e.OldValue as int?;
            int? newValue = e.Value as int?;

            bool isAdminType = row.IndicatorDefinition.IsAdminType;
            var selectedAdmin = lookupAdmin.SelectedItem as ERP_BL.Databases.User;
            bool isCurrentUserAdmin = selectedAdmin != null && selectedAdmin.id == SYSTEM_STATIC.currentUser.id;

            // Restrict editing for Admin indicators by non-admins
            if (fieldName != "ManagementRating" && fieldName != "SelfRating" && isAdminType && !isCurrentUserAdmin)
            {
                DXMessageBox.Show("This part is for Admin, you aren't allowed to update its value!");

                // Revert value to previous one
                SetRatingValue(row, fieldName, oldValue);
                return;
            }
            else if(!isAdminType && isCurrentUserAdmin)
            {
                DXMessageBox.Show("This part is not for Admin, you aren't allowed to update its value!");

                // Revert value to previous one
                SetRatingValue(row, fieldName, oldValue);
                return;
            }

            // Validate range
            if (newValue.HasValue)
            {
                int maxAllowed = row.IndicatorDefinition.Weightage;
                if (newValue < 0 || newValue > maxAllowed)
                {
                    MessageBox.Show(
                        $"The allowed value for '{row.IndicatorDefinition.Name}' is between 0 and {maxAllowed}.",
                        "Invalid Input",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    // Revert value to previous one
                    SetRatingValue(row, fieldName, oldValue);
                }

                if (isAdminType)
                {
                    if (isCurrentUserAdmin)
                    {
                        switch (review.performanceReviewType)
                        {
                            case PerformanceReviewType.LevelTwo:
                                row.SupervisorLevelOneRating = newValue ?? 0;
                                row.SupervisorLevelTwoRating = newValue ?? 0;
                                if (review.performanceReviewerStage != PerformanceReviewerStage.Management)
                                {
                                    row.ManagementRating = newValue ?? 0;
                                }
                                break;
                            case PerformanceReviewType.LevelOne:
                                row.SupervisorLevelOneRating = newValue ?? 0;
                                if (review.performanceReviewerStage != PerformanceReviewerStage.Management)
                                {
                                    row.ManagementRating = newValue ?? 0;
                                }
                                break;
                            case PerformanceReviewType.Self:
                                if (review.performanceReviewerStage != PerformanceReviewerStage.Management)
                                {
                                    row.ManagementRating = newValue ?? 0;
                                }
                                break;
                        }
                    }                        
                }
                else if (!isAdminType)
                {
                    if(SYSTEM_STATIC.currentUser.id == review.Management?.id)
                    {
                        row.AdminRating = newValue ?? 0;
                    }
                }
            }
        }

        private void SetRatingValue(PerformanceIndicatorRating row, string fieldName, int? value)
        {
            switch (fieldName)
            {
                case "SelfRating":
                    row.SelfRating = value ?? 0;
                    break;
                case "SupervisorLevelOneRating":
                    row.SupervisorLevelOneRating = value ?? 0;
                    break;
                case "SupervisorLevelTwoRating":
                    row.SupervisorLevelTwoRating = value ?? 0;
                    break;
                case "AdminRating":
                    row.AdminRating = value ?? 0;
                    break;
                case "ManagementRating":
                    row.ManagementRating = value ?? 0;
                    break;
            }
        }


        private void lookupEmployee_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var employee = lookupEmployee.SelectedItem as ERP_BL.Databases.User;

            txtDepartment.Text = employee.employee?.CoreDepartment?.DeptName;
            txtPosition.Text = employee.employee?.empFunction?.Title;

            txtName.Text = employee.employee?.person?.FName + " "+ employee.employee?.person.LName;
            txtDesignation.Text = employee.employee?.empFunction?.Title;

            datJoiningDate.EditValue = employee.employee?.JoinDate;
            //datJoiningDate.EditValue = employee.employee?.JoinDate;

            if (employee.employee?.JoinDate != null)
            {


                var joiningDate = employee.employee?.JoinDate; // example joining date
                DateTime now = DateTime.Now;

                int years = now.Year - joiningDate.Value.Year;
                int months = now.Month - joiningDate.Value.Month;

                // Adjust if the current month is earlier than the joining month
                if (months < 0)
                {
                    years--;
                    months += 12;
                }

                txtTenure.Text = years + " Years and " + months + " Months";
            }
        }

        private void lookupSupervisor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var employee = lookupSupervisor.SelectedItem as ERP_BL.Databases.User;

            txtSupervisorDepartment.Text = employee.employee?.CoreDepartment?.DeptName;
            txtSupervisorPosition.Text = employee.employee?.empFunction?.Title;
        }

        private void lookupSupervisorLevel2_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var employee = lookupSupervisorLevel2.SelectedItem as ERP_BL.Databases.User;

            txtSupervisorLevel2Dept.Text = employee.employee?.CoreDepartment?.DeptName;
            txtSupervisorLevel2Position.Text = employee.employee?.empFunction?.Title;
        }

        private void chkLock_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading) return;

            if (editFlag == true)
            {


                if (review == null) return;

                if (review.performanceReviewType == PerformanceReviewType.LevelTwo)
                {
                    bool isSupervisorLevel1 = review.SupervisorLevelOne?.id == SYSTEM_STATIC.currentUser.id;
                    bool isSupervisorLevel2 = review.SupervisorLevelTwo?.id == SYSTEM_STATIC.currentUser.id;
                    bool isAdmin = review.Admin?.id == SYSTEM_STATIC.currentUser.id;
                    bool isManagement = review.Management?.id == SYSTEM_STATIC.currentUser.id;

                    // Supervisor
                    if (isSupervisorLevel1)
                    {
                        if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.Self)
                            performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel1;
                    }

                    // Supervisor
                    else if (isSupervisorLevel2)
                    {
                        if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel1)
                            performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel2;
                    }

                    // Admin
                    else if (isAdmin)
                    {
                        if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel2)
                            performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.Admin;
                    }

                    // Management
                    else if (isManagement)
                    {
                        if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.Admin)
                            performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.Management;
                    }

                    MessageBox.Show("Review locked and moved to the next stage.");
                }
                else if (review.performanceReviewType == PerformanceReviewType.LevelOne)
                {
                    bool isSupervisorLevel1 = review.SupervisorLevelOne?.id == SYSTEM_STATIC.currentUser.id;
                    bool isAdmin = review.Admin?.id == SYSTEM_STATIC.currentUser.id;
                    bool isManagement = review.Management?.id == SYSTEM_STATIC.currentUser.id;

                    // Supervisor
                    if (isSupervisorLevel1)
                    {
                        if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.Self)
                            performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel1;
                    }

                    // Admin
                    else if (isAdmin)
                    {
                        if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel1)
                            performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.Admin;
                    }

                    // Management
                    else if (isManagement)
                    {
                        if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.Admin)
                            performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.Management;
                    }

                    MessageBox.Show("Review locked and moved to the next stage.");
                } 
                else if (review.performanceReviewType == PerformanceReviewType.Self)
                {
                    bool isAdmin = review.Admin?.id == SYSTEM_STATIC.currentUser.id;
                    bool isManagement = review.Management?.id == SYSTEM_STATIC.currentUser.id;

                    // Admin
                    if (isAdmin)
                    {
                        if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.Self)
                            performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.Admin;
                    }

                    // Management
                    else if (isManagement)
                    {
                        if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.Admin)
                            performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.Management;
                    }

                    MessageBox.Show("Review locked and moved to the next stage.");
                }
            }

        }

        private void chkLock_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!editFlag || review == null) return;

            if (review.performanceReviewType == PerformanceReviewType.LevelTwo)
            {
                bool isSupervisorLevel1 = review.SupervisorLevelOne?.id == SYSTEM_STATIC.currentUser.id;
                bool isSupervisorLevel2 = review.SupervisorLevelTwo?.id == SYSTEM_STATIC.currentUser.id;
                bool isAdmin = review.Admin?.id == SYSTEM_STATIC.currentUser.id;
                bool isManagement = review.Management?.id == SYSTEM_STATIC.currentUser.id;

                if (isManagement && review.performanceReviewerStage == PerformanceReviewerStage.Management)
                    performanceReviewerStage = PerformanceReviewerStage.Admin;

                else if (isAdmin && review.performanceReviewerStage == PerformanceReviewerStage.Admin)
                    performanceReviewerStage = PerformanceReviewerStage.SupervisorLevel2;

                else if (isSupervisorLevel2 && review.performanceReviewerStage == PerformanceReviewerStage.SupervisorLevel2)
                    performanceReviewerStage = PerformanceReviewerStage.SupervisorLevel1;

                else if (isSupervisorLevel1 && review.performanceReviewerStage == PerformanceReviewerStage.SupervisorLevel1)
                    performanceReviewerStage = PerformanceReviewerStage.Self;

                MessageBox.Show("Review unlocked and moved to the previous stage.");
            }
            else if (review.performanceReviewType == PerformanceReviewType.LevelOne)
            {
                bool isSupervisorLevel1 = review.SupervisorLevelOne?.id == SYSTEM_STATIC.currentUser.id;
                bool isAdmin = review.Admin?.id == SYSTEM_STATIC.currentUser.id;
                bool isManagement = review.Management?.id == SYSTEM_STATIC.currentUser.id;

                if (isManagement && review.performanceReviewerStage == PerformanceReviewerStage.Management)
                    performanceReviewerStage = PerformanceReviewerStage.Admin;

                else if (isAdmin && review.performanceReviewerStage == PerformanceReviewerStage.Admin)
                    performanceReviewerStage = PerformanceReviewerStage.SupervisorLevel1;

                else if (isSupervisorLevel1 && review.performanceReviewerStage == PerformanceReviewerStage.SupervisorLevel1)
                    performanceReviewerStage = PerformanceReviewerStage.Self;

                MessageBox.Show("Review unlocked and moved to the previous stage.");
            }
            else if (review.performanceReviewType == PerformanceReviewType.Self)
            {
                bool isAdmin = review.Admin?.id == SYSTEM_STATIC.currentUser.id;
                bool isManagement = review.Management?.id == SYSTEM_STATIC.currentUser.id;

                if (isManagement && review.performanceReviewerStage == PerformanceReviewerStage.Management)
                    performanceReviewerStage = PerformanceReviewerStage.Admin;

                else if (isAdmin && review.performanceReviewerStage == PerformanceReviewerStage.Admin)
                    performanceReviewerStage = PerformanceReviewerStage.Self;

                MessageBox.Show("Review unlocked and moved to the previous stage.");
            }
        }



        //private void chkLock_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    if (review == null) return;

        //    if (review.performanceReviewType == PerformanceReviewType.LevelTwo)
        //    {
        //        //bool isEmployee = review.Employee?.id == SYSTEM_STATIC.currentUser.id;
        //        bool isSupervisorLevel1 = review.SupervisorLevelOne?.id == SYSTEM_STATIC.currentUser.id;
        //        bool isSupervisorLevel2 = review.SupervisorLevelTwo?.id == SYSTEM_STATIC.currentUser.id;
        //        bool isAdmin = review.Admin?.id == SYSTEM_STATIC.currentUser.id;
        //        bool isManagement = review.Management?.id == SYSTEM_STATIC.currentUser.id; ;

        //        // Employee - revert from Supervisor back to Self
        //        if (isSupervisorLevel1)
        //        {
        //            if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel1)
        //            {
        //                review.performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.Self;
        //                MessageBox.Show("Review unlocked and moved back to Employee.");
        //            }
        //            else
        //            {
        //                PreventUnlock();
        //            }
        //        }

        //        // Supervisor - revert from Supervisor Level 2 back to Supervisor Level 1
        //        else if (isSupervisorLevel2)
        //        {
        //            if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel2)
        //            {
        //                review.performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel1;
        //                MessageBox.Show("Review unlocked and moved back to Supervisor Level 1.");
        //            }
        //            else
        //            {
        //                PreventUnlock();
        //            }
        //        }

        //        // Admin - revert from Admin back to Supervisor Level 2
        //        else if (isAdmin)
        //        {
        //            if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.Admin)
        //            {
        //                review.performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel2;
        //                MessageBox.Show("Review unlocked and moved back to Supervisor Level 2.");
        //            }
        //            else
        //            {
        //                PreventUnlock();
        //            }
        //        }

        //        // Management - cannot unlock (final stage)
        //        else if (isManagement)
        //        {
        //            if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.Management)
        //            {
        //                review.performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.Admin;
        //                MessageBox.Show("Review unlocked and moved back to Admin.");
        //            }
        //            else
        //            {
        //                PreventUnlock();
        //            }
        //        }
        //    }
        //    else if (review.performanceReviewType == PerformanceReviewType.LevelOne)
        //    {
        //        //bool isEmployee = review.Employee?.id == SYSTEM_STATIC.currentUser.id;
        //        bool isSupervisorLevel1 = review.SupervisorLevelOne?.id == SYSTEM_STATIC.currentUser.id;
        //        bool isAdmin = review.Admin?.id == SYSTEM_STATIC.currentUser.id;
        //        bool isManagement = review.Management?.id == SYSTEM_STATIC.currentUser.id; ;

        //        // Employee - revert from Supervisor back to Self
        //        if (isSupervisorLevel1)
        //        {
        //            if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel1)
        //            {
        //                review.performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.Self;
        //                MessageBox.Show("Review unlocked and moved back to Employee.");
        //            }
        //            else
        //            {
        //                PreventUnlock();
        //            }
        //        }

        //        // Admin - revert from Admin back to Supervisor Level 2
        //        else if (isAdmin)
        //        {
        //            if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.Admin)
        //            {
        //                review.performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.SupervisorLevel1;
        //                MessageBox.Show("Review unlocked and moved back to Supervisor Level 1.");
        //            }
        //            else
        //            {
        //                PreventUnlock();
        //            }
        //        }

        //        // Management - cannot unlock (final stage)
        //        else if (isManagement)
        //        {
        //            if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.Management)
        //            {
        //                review.performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.Admin;
        //                MessageBox.Show("Review unlocked and moved back to Admin.");
        //            }
        //            else
        //            {
        //                PreventUnlock();
        //            }
        //        }
        //    }
        //    else if (review.performanceReviewType == PerformanceReviewType.Self)
        //    {
        //        //bool isEmployee = review.Employee?.id == SYSTEM_STATIC.currentUser.id;
        //        bool isAdmin = review.Admin?.id == SYSTEM_STATIC.currentUser.id;
        //        bool isManagement = review.Management?.id == SYSTEM_STATIC.currentUser.id;

        //        // Admin - revert from Admin back to Supervisor Level 2
        //        if (isAdmin)
        //        {
        //            if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.Admin)
        //            {
        //                review.performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.Self;
        //                MessageBox.Show("Review unlocked and moved back to Employee.");
        //            }
        //            else
        //            {
        //                PreventUnlock();
        //            }
        //        }

        //        // Management - cannot unlock (final stage)
        //        else if (isManagement)
        //        {
        //            if (review.performanceReviewerStage == ERP_BL.Enums.PerformanceReviewerStage.Management)
        //            {
        //                review.performanceReviewerStage = ERP_BL.Enums.PerformanceReviewerStage.Admin;
        //                MessageBox.Show("Review unlocked and moved back to Admin.");
        //            }
        //            else
        //            {
        //                PreventUnlock();
        //            }
        //        }
        //    }
        //}

        private void PreventUnlock()
        {
            MessageBox.Show("Unlocking is not allowed at this stage.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            chkLock.Checked -= chkLock_Checked;
            chkLock.Unchecked -= chkLock_Unchecked;
            chkLock.IsChecked = true;
            chkLock.Checked += chkLock_Checked;
            chkLock.Unchecked += chkLock_Unchecked;
        }

        private void loadcomments()
        {
            try
            {
                if (memoId > 0)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(memoId, TransactionItemType.Memo);

                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void btnAddComment_Click(object sender, RoutedEventArgs e)
        {
            MemoNotificationsRepo notificationsRepo = new MemoNotificationsRepo();
            if (editFlag == true && memoId > 0)
            {
                try
                {
                    var memo = memoRepo.GetMemo(memoId);

                    if (memo != null)
                    {
                        if (memo.memoType == MemoType.Group && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Comment in Group Memos") == null)
                        {
                            DXMessageBox.Show("Permission required to Add Comment in Group Memos!");
                            return;
                        }

                        if (memo.memoType == MemoType.Non_Linked)
                        {
                            UsersRepo usersRepo = new UsersRepo();

                            List<ERP_BL.Databases.User> users = new List<ERP_BL.Databases.User>();
                            users.Add(memo.createdBy);
                            users.Add(memo.createdFor);
                            var CCusers = memo.CCUsersList;
                            if (CCusers != null && CCusers.Count > 0)
                                users.AddRange(CCusers);

                            users = users.GroupBy(x => x).Select(d => d.First()).ToList();

                            frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, users, TransactionItemType.Memo);
                            inputBox.txtSubject.Text = memo.Subject;
                            inputBox.txtSubject.IsReadOnly = true;

                            inputBox.cmbNotificationFlag.Visibility = Visibility.Collapsed;
                            inputBox.lblFlag.Visibility = Visibility.Collapsed;

                            inputBox.cmbEmployee.Visibility = Visibility.Collapsed;
                            inputBox.lblAsignee.Visibility = Visibility.Collapsed;

                            inputBox.cmbCategories.Visibility = Visibility.Collapsed;
                            inputBox.lblCategory.Visibility = Visibility.Collapsed;

                            inputBox.cmbCCRecomendUsers.Visibility = Visibility.Collapsed;
                            inputBox.lblCCRecomendation.Visibility = Visibility.Collapsed;

                            inputBox.cmbTagRecomendUsers.Visibility = Visibility.Collapsed;
                            inputBox.lblTagRecomendation.Visibility = Visibility.Collapsed;

                            inputBox.lblRecommendationHeader.Visibility = Visibility.Collapsed;

                            inputBox.ShowDialog();

                        }
                        else
                        if (memo.memoType == MemoType.Group && memo.taskGroup != null && memo.taskGroup.users != null && memo.taskGroup.users.Count > 0)
                        {
                            List<ERP_BL.Databases.User> users = new List<ERP_BL.Databases.User>();
                            users.Add(memo.createdBy);
                            users.AddRange(memo.taskGroup.users);
                            var CCusers = memo.CCUsersList;
                            if (CCusers != null && CCusers.Count > 0)
                                users.AddRange(CCusers);

                            users = users.GroupBy(x => x).Select(d => d.First()).ToList();
                            frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, memo.taskGroup.users, TransactionItemType.Memo);
                            inputBox.txtSubject.Text = memo.Subject;
                            inputBox.txtSubject.IsReadOnly = true;

                            inputBox.cmbNotificationFlag.Visibility = Visibility.Collapsed;
                            inputBox.lblFlag.Visibility = Visibility.Collapsed;

                            inputBox.cmbEmployee.Visibility = Visibility.Collapsed;
                            inputBox.lblAsignee.Visibility = Visibility.Collapsed;

                            inputBox.cmbCategories.Visibility = Visibility.Collapsed;
                            inputBox.lblCategory.Visibility = Visibility.Collapsed;

                            inputBox.cmbCCRecomendUsers.Visibility = Visibility.Collapsed;
                            inputBox.lblCCRecomendation.Visibility = Visibility.Collapsed;

                            inputBox.cmbTagRecomendUsers.Visibility = Visibility.Collapsed;
                            inputBox.lblTagRecomendation.Visibility = Visibility.Collapsed;

                            inputBox.cmbTagUsers.Visibility = Visibility.Collapsed;
                            inputBox.lblTagUser.Visibility = Visibility.Collapsed;

                            inputBox.cmbCCUsers.Visibility = Visibility.Collapsed;
                            inputBox.lblCCUser.Visibility = Visibility.Collapsed;

                            inputBox.btnClearTagUser.Visibility = Visibility.Collapsed;

                            inputBox.lblRecommendationHeader.Visibility = Visibility.Collapsed;
                            inputBox.ShowDialog();

                        }
                        else if (memo.memoType == MemoType.Linked && memo.createdBy != null && memo.createdFor != null)
                        {
                            List<ERP_BL.Databases.User> users = new List<ERP_BL.Databases.User>();
                            users.Add(memo.createdBy);
                            users.Add(memo.createdFor);
                            var CCusers = memo.CCUsersList;
                            if (CCusers != null && CCusers.Count > 0)
                                users.AddRange(CCusers);
                            users = users.GroupBy(x => x).Select(d => d.First()).ToList();
                            frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, users, TransactionItemType.Memo);
                            inputBox.txtSubject.Text = memo.Subject;
                            inputBox.txtSubject.IsReadOnly = true;

                            inputBox.cmbNotificationFlag.Visibility = Visibility.Collapsed;
                            inputBox.lblFlag.Visibility = Visibility.Collapsed;

                            inputBox.cmbEmployee.Visibility = Visibility.Collapsed;
                            inputBox.lblAsignee.Visibility = Visibility.Collapsed;

                            inputBox.cmbCategories.Visibility = Visibility.Collapsed;
                            inputBox.lblCategory.Visibility = Visibility.Collapsed;

                            inputBox.cmbCCRecomendUsers.Visibility = Visibility.Collapsed;
                            inputBox.lblCCRecomendation.Visibility = Visibility.Collapsed;

                            inputBox.cmbTagRecomendUsers.Visibility = Visibility.Collapsed;
                            inputBox.lblTagRecomendation.Visibility = Visibility.Collapsed;

                            inputBox.lblRecommendationHeader.Visibility = Visibility.Collapsed;
                            inputBox.ShowDialog();
                        }
                        else
                        {
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                        }

                        if (memo != null)
                        {
                            ProcurementRepo procurementRepo = new ProcurementRepo();

                            if (frmInputBox.comment != "" && memo.Id != 0)
                            {
                                if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                                {
                                    var commentId = procurementRepo.AddCommentLinkNotification(memo.Id, TransactionItemType.Memo, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                                    if (commentId != null)
                                    {
                                        foreach (var user in frmInputBox.Comment.TaggedList)
                                        {
                                            if (frmInputBox.FlagForTag == true)
                                                notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Memo with Subject:" + memo.Subject, memo.Id, TransactionItemType.Memo, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                            else
                                                notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Memo Subject:" + memo.Subject, memo.Id, TransactionItemType.Memo, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                        }

                                        foreach (var user in frmInputBox.Comment.CCUsersList)
                                        {
                                            if (frmInputBox.FlagForCC == true)
                                                notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Memo Subject:" + memo.Subject, memo.Id, TransactionItemType.Memo, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                            else
                                                notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Memo Subject:" + memo.Subject, memo.Id, TransactionItemType.Memo, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                        }
                                    }
                                }

                                //procurementRepo.Add(task.Id, TransactionItemType.Tasks, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                                MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                                if (window1 != null) { window1.UrgentNotificationGlow(); }
                            }
                            else if (memo.Id == 0)
                            {
                                DXMessageBox.Show("Kindly save Memo first to add a comment!");
                            }

                        }

                        loadcomments();
                    }
                    else
                    {
                        DXMessageBox.Show("Select a Memo to Add new comment!");
                    }
                }
                catch (Exception ex)
                {
                    DXMessageBox.Show(ex.Message);
                }
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

        private void btnSetVoid_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (editFlag == true && isLoading == false)
            {
                var dialog = new StageSelectionDialog
                {
                    Owner = this
                };

                dialog.Width = 400;
                dialog.Height = 250;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                if (dialog.ShowDialog() == true)
                {
                    performanceReviewerStage = dialog.SelectedStage;
                    MessageBox.Show($"Stage set to: {performanceReviewerStage}", "Stage Updated");
                    // You can now use this `stage` variable to control logic/visibility
                }
            }

            if (radioSelf.IsChecked == true)
            {
                gridSupervisorLevel1.Visibility = Visibility.Collapsed;
                gridSupervisorLevel2.Visibility = Visibility.Collapsed;

                gridRatings.Columns["SupervisorLevelOneRating"].Visible = false;
                gridRatings.Columns["SupervisorLevelOneRating"].ShowInColumnChooser = false;

                gridRatings.Columns["SupervisorLevelTwoRating"].Visible = false;
                gridRatings.Columns["SupervisorLevelTwoRating"].ShowInColumnChooser = false;
            }
            else if (radioLevelOne.IsChecked == true)
            {
                gridSupervisorLevel1.Visibility = Visibility.Visible;
                gridSupervisorLevel2.Visibility = Visibility.Collapsed;

                gridRatings.Columns["SupervisorLevelOneRating"].Visible = true;
                gridRatings.Columns["SupervisorLevelOneRating"].ShowInColumnChooser = true;

                gridRatings.Columns["SupervisorLevelTwoRating"].Visible = false;
                gridRatings.Columns["SupervisorLevelTwoRating"].ShowInColumnChooser = false;
            }
            else if (radioLevelTwo.IsChecked == true)
            {
                gridSupervisorLevel1.Visibility = Visibility.Visible;
                gridSupervisorLevel2.Visibility = Visibility.Visible;

                gridRatings.Columns["SupervisorLevelOneRating"].Visible = true;
                gridRatings.Columns["SupervisorLevelOneRating"].ShowInColumnChooser = true;

                gridRatings.Columns["SupervisorLevelTwoRating"].Visible = true;
                gridRatings.Columns["SupervisorLevelTwoRating"].ShowInColumnChooser = true;
            }
        }

        private void imgLeftToRight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRight.Height = 25;
            imgLeftToRight.Width = 25;
        }

        private void imgLeftToRight_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRight.Height = 32;
            imgLeftToRight.Width = 32;
            try
            {
                var selectedItem = grdCntrlUsers.SelectedItem as User;

                if (selectedItem != null)
                {

                    AllAllowedUser.Remove(selectedItem);
                    if (!SelectedAllowedUser.Contains(selectedItem))
                        SelectedAllowedUser.Add(selectedItem);

                }
                else
                {
                    DXMessageBox.Show("Please Select First From Employee List Register!");
                }
                grdCntrlUsersSelected.RefreshData();
                grdCntrlUsers.RefreshData();
                grdCntrlUsers.SelectedItem = null;
                grdCntrlUsersSelected.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void btnLeftMoveCustomer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeft.Width = 32;
            imgRightToLeft.Height = 32;

            try
            {
                var selectedItem = grdCntrlUsersSelected.SelectedItem as User;

                if (selectedItem != null)
                {
                    if (!AllAllowedUser.Contains(selectedItem))
                        AllAllowedUser.Add(selectedItem);
                    SelectedAllowedUser.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First From Selected Employee Register!");
                }
                grdCntrlUsersSelected.RefreshData();
                grdCntrlUsers.RefreshData();
                grdCntrlUsers.SelectedItem = null;
                grdCntrlUsersSelected.SelectedItem = null;
            }
            catch (Exception)
            {


            }
        }

        private void btnLeftMoveCustomer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeft.Width = 25;
            imgRightToLeft.Height = 25;
        }

        private void btnHiddenAddComment_Click(object sender, RoutedEventArgs e)
        {
            MemoNotificationsRepo notificationsRepo = new MemoNotificationsRepo();
            if (editFlag == true && memoId > 0)
            {
                try
                {
                    if (review != null && review.Id > 0)
                    {
                        if (review.HiddenChatUsers != null && review.HiddenChatUsers.Count > 0)
                        {
                            List<ERP_BL.Databases.User> users = new List<ERP_BL.Databases.User>();
                            users = review.HiddenChatUsers;

                            users = users.GroupBy(x => x).Select(d => d.First()).ToList();
                            frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, users, TransactionItemType.PerformanceReview);
                            //inputBox.txtSubject.Text = memo.Subject;
                            //inputBox.txtSubject.IsReadOnly = true;

                            //inputBox.cmbNotificationFlag.Visibility = Visibility.Collapsed;
                            //inputBox.lblFlag.Visibility = Visibility.Collapsed;

                            //inputBox.cmbEmployee.Visibility = Visibility.Collapsed;
                            //inputBox.lblAsignee.Visibility = Visibility.Collapsed;

                            //inputBox.cmbCategories.Visibility = Visibility.Collapsed;
                            //inputBox.lblCategory.Visibility = Visibility.Collapsed;

                            //inputBox.cmbCCRecomendUsers.Visibility = Visibility.Collapsed;
                            //inputBox.lblCCRecomendation.Visibility = Visibility.Collapsed;

                            //inputBox.cmbTagRecomendUsers.Visibility = Visibility.Collapsed;
                            //inputBox.lblTagRecomendation.Visibility = Visibility.Collapsed;

                            //inputBox.cmbTagUsers.Visibility = Visibility.Collapsed;
                            //inputBox.lblTagUser.Visibility = Visibility.Collapsed;

                            //inputBox.cmbCCUsers.Visibility = Visibility.Collapsed;
                            //inputBox.lblCCUser.Visibility = Visibility.Collapsed;

                            //inputBox.btnClearTagUser.Visibility = Visibility.Collapsed;

                            //inputBox.lblRecommendationHeader.Visibility = Visibility.Collapsed;
                            inputBox.ShowDialog();

                        }
                        
                        else
                        {
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                        }

                        if (review != null)
                        {
                            ProcurementRepo procurementRepo = new ProcurementRepo();

                            if (frmInputBox.comment != "" && review.Id != 0)
                            {
                                if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                                {
                                    var commentId = procurementRepo.AddCommentLinkNotification(review.Id, TransactionItemType.PerformanceReview, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                                    if (commentId != null)
                                    {
                                        foreach (var user in frmInputBox.Comment.TaggedList)
                                        {
                                            if (frmInputBox.FlagForTag == true)
                                                notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Performance Review having year " + review.PerformanceYear?.Year, review.Id, TransactionItemType.PerformanceReview, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                            else
                                                notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Performance Review having year " + review.PerformanceYear?.Year, review.Id, TransactionItemType.PerformanceReview, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                        }

                                        foreach (var user in frmInputBox.Comment.CCUsersList)
                                        {
                                            if (frmInputBox.FlagForCC == true)
                                                notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Performance Review having year " + review.PerformanceYear?.Year, review.Id, TransactionItemType.PerformanceReview, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                            else
                                                notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Performance Review having year " + review.PerformanceYear?.Year, review.Id, TransactionItemType.PerformanceReview, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                        }
                                    }
                                }

                                //procurementRepo.Add(task.Id, TransactionItemType.Tasks, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                                MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                                if (window1 != null) { window1.UrgentNotificationGlow(); }
                            }
                            else if (review.Id == 0)
                            {
                                DXMessageBox.Show("Kindly save Performance Review first to add a comment!");
                            }

                        }

                        loadHiddencomments();
                    }
                    else
                    {
                        DXMessageBox.Show("Please save Performance Review first to Add comment!");
                    }
                }
                catch (Exception ex)
                {
                    DXMessageBox.Show(ex.Message);
                }
            }
        }

        private void btnHiddenCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (grdHiddenComments.Visibility == Visibility.Collapsed)
            {
                grdHiddenComments.Visibility = Visibility.Visible;
                loadHiddencomments();
            }
            else
                grdHiddenComments.Visibility = Visibility.Collapsed;
        }

        private void loadHiddencomments()
        {
            try
            {
                if (reviewIdToEdit > 0)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(reviewIdToEdit, TransactionItemType.PerformanceReview);

                    grdHiddenCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }
    }

}
