using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ToDoTasks;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Procurementss;

namespace ZAS_ERP.ToDoTasks.TargetRewardss
{
    /// <summary>
    /// Interaction logic for ucFrmTargetRewardAdd.xaml
    /// </summary>
    public partial class ucFrmTargetRewardAdd : UserControl
    {
        TargetRewards targetReward = new TargetRewards();

        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        public int rewardId = 0;
        UsersRepo UsersRepo = new UsersRepo();
        List<ViewInfo> views = new List<ViewInfo>();
        public TargetRewardStatus checkStatus = new TargetRewardStatus();
        public ucFrmTargetRewardAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //ERP_BL.Attach attachment = new ERP_BL.Attach();
            //attachment.GetChangeLogs();
            if (rewardId > 0)
            {
                targetReward = taskRepo.GetTargetReward(rewardId);

                List<TargetRewards> targetRewards = new List<TargetRewards>();
                targetRewards.Add(targetReward);
                grdRewards.ItemsSource = targetRewards;

                LoadStatuses();
                LoadStepStatuses();


                if (targetReward.isApplied == false)
                    datAppliedDate.DateTime = DateTime.Now;
                else
                {
                    if (targetReward.AppliedDate != null)
                        datAppliedDate.EditValue = targetReward.AppliedDate;
                    TargetRewardStages();
                }

                var creditJournalTransactions = targetReward.journalTransactions.Where(x => x.credit != 0).ToList();
                var debitJournalTransactions = targetReward.journalTransactions.Where(x => x.debit != 0).ToList();
                if (creditJournalTransactions.Count > 0)
                {
                    btnPushCredits.IsChecked = true;
                }
                if (debitJournalTransactions.Count > 0)
                {
                    btnPushDebits.IsChecked = true;
                }
                if (targetReward.GlPostingDate != null)
                {
                    datglPostingdate.EditValue = targetReward.GlPostingDate;
                }
                else
                {
                    datglPostingdate.EditValue = targetReward.CreationDate;
                }
                if(targetReward.financeRefNo!=null)
                {
                    txtFinanceRef.Text = targetReward.financeRefNo;
                }

                var statusList = (cmbStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbStatus.ItemsSource as List<cmbitem>;
                if (targetReward.Status != null)
                {
                    oldStatus = targetReward.Status;
                    int index = 0;
                    foreach (var _status in statusList)
                    {

                        if (_status != null && _status.id == targetReward.statusId)
                        {
                            cmbStatus.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                    checkStatus = targetReward.Status;
                }

                var stepStatusList = (cmbStepStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbStepStatus.ItemsSource as List<cmbitem>;
                if (targetReward.toDoTask != null && targetReward.toDoTask.Status != null)
                {
                    
                    int index = 0;
                    foreach (var _status in stepStatusList)
                    {

                        if (_status.id == targetReward.toDoTask.statusId)
                        {
                            cmbStepStatus.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }


                

                if (targetReward.creator != null)
                {
                    txtCreator.Text = targetReward.creator.employee.person.FName + " " + targetReward.creator.employee.person.LName;
                }

                views = UsersRepo.getViwerInfo(rewardId, (int)TransactionItemType.TargetReward);
                grdUsers.ItemsSource = views;

                if (targetReward.isVoid == true)
                    grdVoid.Visibility = Visibility.Visible;
                else
                    grdVoid.Visibility = Visibility.Collapsed;

                loadAttachments();

                if (targetReward.user != null)
                {
                    var byteImg = targetReward.user.employee.person.Photo;
                    if (byteImg != null)
                    {
                        var image = GetBitmapImageFromByteArray(byteImg);
                        imgUser.ImageSource = image;
                    }
                    txtUserName.Text = targetReward.user.userName;
                }

                txtCreationDate.DateTime = targetReward.CreationDate;
                datglPostingdate.EditValue = targetReward.GlPostingDate;

                if (targetReward.currency != null)
                    txtCurrency.Text = targetReward.currency.CurrencyName;

                txtRewardType.Text = targetReward.functionType.ToString();

                txtRewardAmount.Text = targetReward.RewardAmount.ToString();
                txtMER.Text = targetReward.MER.ToString();

                if (targetReward.toDoTask != null)
                {
                    txtStepName.Text = targetReward.toDoTask.TaskName;

                    if (targetReward.toDoTask.calculationType != null)
                    {
                        txtCalculationType.Text = targetReward.toDoTask.calculationType.TypeName;
                        if(targetReward.toDoTask.GetPropertyValue(targetReward.toDoTask.calculationType.AchievedField.SOFieldName) != null)
                            txtAchievedPoints.Text = targetReward.toDoTask.GetPropertyValue(targetReward.toDoTask.calculationType.AchievedField.SOFieldName).ToString();

                        if (targetReward.toDoTask.calculationType.AchievedField != null)
                        {
                            txtAchievedField.Text = targetReward.toDoTask.calculationType.AchievedField.DisplayName;
                        }
                        if (targetReward.toDoTask.calculationType.TotalField != null)
                        {
                            txtTargetField.Text = targetReward.toDoTask.calculationType.TotalField.DisplayName;
                        }
                    }
                    else if (targetReward.toDoTask.taskGroup!= null && targetReward.toDoTask.taskGroup.calculationType != null)
                    {
                        txtCalculationType.Text = targetReward.toDoTask.taskGroup.calculationType.TypeName;
                        if (targetReward.toDoTask.GetPropertyValue(targetReward.toDoTask.taskGroup.calculationType.AchievedField.SOFieldName) != null)
                            txtAchievedPoints.Text = targetReward.toDoTask.GetPropertyValue(targetReward.toDoTask.taskGroup.calculationType.AchievedField.SOFieldName).ToString();

                        if (targetReward.toDoTask.taskGroup.calculationType.AchievedField != null)
                        {
                            txtAchievedField.Text = targetReward.toDoTask.taskGroup.calculationType.AchievedField.DisplayName;
                        }
                        if (targetReward.toDoTask.taskGroup.calculationType.TotalField != null)
                        {
                            txtTargetField.Text = targetReward.toDoTask.taskGroup.calculationType.TotalField.DisplayName;
                        }
                    }

                    if (targetReward.toDoTask.targetGroup != null)
                        txtStepGroup.Text = targetReward.toDoTask.targetGroup.GroupName;
                    txtStepTargetPoints.Text = targetReward.toDoTask.TaskPoints.ToString();
                    txtTargetPointss.Text = targetReward.toDoTask.TaskPoints.ToString();
                    txtPercentageAchieved.Text = targetReward.toDoTask.TargetAchievedPercenatage.ToString();

                    txtTargetYear.Text = targetReward.toDoTask.TargetYear.Value.Year.ToString();
                    txtTargetMonth.Text = targetReward.toDoTask.TargetMonth.Value.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture);
                    txtStepDueDate.DateTime = targetReward.toDoTask.StepDueDate.Value;
                    if(targetReward.toDoTask.PointsUpdatedOn != null)
                        txtUpdatedOn.DateTime = targetReward.toDoTask.PointsUpdatedOn.Value;
                }

                if (targetReward.Payments.Where(x => x.isVoid != true) == null || targetReward.Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                {
                    txtPaidAmount.Text = "0";
                    txtAmountDue.Text = targetReward.RewardAmount.ToString();
                    pbStatus.Value = 0;
                }
                else
                {
                    txtPaidAmount.Text = targetReward.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount).ToString();
                    txtAmountDue.Text = (targetReward.RewardAmount - targetReward.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount)).ToString();
                    var paidAmount = targetReward.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount);
                    pbStatus.Value = Math.Round((paidAmount / targetReward.RewardAmount) * 100, 2);
                }

            }
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if(targetReward != null && rewardId > 0)
            {
                if(cmbStatus.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Status!");
                    cmbStatus.Focus();
                    return;
                }
                targetReward.statusId = (cmbStatus.SelectedItem as cmbitem).id;
                targetReward.AppliedDate = datAppliedDate.DateTime;

                if(targetReward.isApplied == false)
                {
                    targetReward.isApplied = true;
                    targetReward.isApproved = false;
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Target Reward") != null && targetReward.isApproved != true)
                {
                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Transaction is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        targetReward.stage = TransactionStage.Approved.ToString();
                        targetReward.isApproved = true;
                        targetReward.ApprovedDate = System.DateTime.Now;
                    }
                }
                if(!string.IsNullOrEmpty(txtFinanceRef.Text))
                {
                    targetReward.financeRefNo = txtFinanceRef.Text;
                }
                if (!string.IsNullOrEmpty(txtMER.Text))
                {
                    targetReward.MER =Math.Round(Convert.ToDouble( txtMER.Text),2);
                }
                if(datglPostingdate.EditValue!=null)
                {
                    targetReward.GlPostingDate = (DateTime)datglPostingdate.EditValue;
                }
                //targetReward.journalTransactions = GetJournalTransactions(targetReward);
                

                taskRepo.UpdateTargetReward(targetReward);

                if (oldStatus != null)
                {
                    if (oldStatus.Id != targetReward.Status.Id)
                    {
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Status of Reward has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (targetReward.toDoTask != null && targetReward.toDoTask.taskGroup != null && targetReward.toDoTask.taskGroup.users != null)
                            {
                                var userss = targetReward.toDoTask.taskGroup.users;
                                winTagUsers win = new winTagUsers(userss, targetReward.Id, TransactionItemType.TargetReward);
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
                        string oldStat = oldStatus.Status;
                        string newStat = targetReward.Status.Status;
                        string symbolCurr = "";

                        if (targetReward.currency != null)
                        {
                            symbolCurr = targetReward.currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Status of Target Reward having value: " + targetReward.RewardAmount.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                            Timestamp = DateTime.Now,
                            Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(targetReward.Id, TransactionItemType.TargetReward, comment, SYSTEM_STATIC.currentUser.employeeId);

                        //Creating notification
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward ", targetReward.Id, TransactionItemType.TargetReward, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward ", targetReward.Id, TransactionItemType.TargetReward, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }

                        AddNewComment();
                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + oldStatus.Status + ") to (" + targetReward.Status.Status + ")");
                        UsersRepo.Add(TransactionInfo.Status_Changed, targetReward.Id, (int)TransactionItemType.TargetReward, frmInputBox.comment);
                    }
                }

                DXMessageBox.Show("Updated Successfully!");
                Window myWin = Window.GetWindow(this);
                myWin.Close();
            }
        }
        public List<JournalTransaction> GetJournalTransactions(TargetRewards reward)
        {
            List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
            if (reward.journalTransactions.Count == 0)
            {
                winSelectLedgerCompDept winSelectLedgerCompDept = new winSelectLedgerCompDept();
                winSelectLedgerCompDept.ShowDialog();
                if (btnPushDebits.IsChecked == true)
                {
                    if (reward.toDoTask.taskGroup.CGSAccount != null)
                    {
                        journalTransactions.Add(new JournalTransaction()
                        {
                            accountId = reward.toDoTask.taskGroup.cgsAccount_Id,
                            coaTransactionsType = coaTransactionsType.TragetReward,
                            TargetRewardId = reward.Id,
                            creationDate = (DateTime)datglPostingdate.EditValue,
                            transactionRefno = txtFinanceRef.Text,
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            userId = SYSTEM_STATIC.currentUser.id,
                            debit = Convert.ToDouble(txtRewardAmount.Text),
                            credit = 0,
                            deptId = winSelectLedgerCompDept.department.Id,
                            total = Convert.ToDouble(txtRewardAmount.Text) - 0,
                            companyId = winSelectLedgerCompDept.company.Id,
                            currencyId = reward.toDoTask.taskGroup.currency_Id


                        });
                    }
                }
                if (btnPushCredits.IsChecked == true)
                {
                    if (reward.toDoTask.taskGroup.payableAccount_Id != null)
                    {
                        journalTransactions.Add(new JournalTransaction()
                        {
                            accountId = reward.toDoTask.taskGroup.payableAccount_Id,
                            coaTransactionsType = coaTransactionsType.TragetReward,
                            TargetRewardId = reward.Id,
                            creationDate = (DateTime)datglPostingdate.EditValue,
                            transactionRefno = txtFinanceRef.Text,
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            userId = SYSTEM_STATIC.currentUser.id,
                            debit = 0,
                            credit = Convert.ToDouble(txtRewardAmount.Text),
                            deptId = winSelectLedgerCompDept.department.Id,
                            total = 0 - Convert.ToDouble(txtRewardAmount.Text),
                            companyId = winSelectLedgerCompDept.company.Id,
                            currencyId = reward.toDoTask.taskGroup.currency_Id
                        });
                    }
                }
            }
            else
            {
                var jT = reward.journalTransactions.First();
                winSelectLedgerCompDept winSelectLedgerCompDept = new winSelectLedgerCompDept(jT.company, jT.department);
                winSelectLedgerCompDept.ShowDialog();
                if (btnPushDebits.IsChecked == true)
                {
                    if (reward.toDoTask.taskGroup.CGSAccount != null)
                    {
                        journalTransactions.Add(new JournalTransaction()
                        {
                            accountId = reward.toDoTask.taskGroup.cgsAccount_Id,
                            coaTransactionsType = coaTransactionsType.TragetReward,
                            TargetRewardId = reward.Id,
                            creationDate = (DateTime)datglPostingdate.EditValue,
                            transactionRefno = txtFinanceRef.Text,
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            userId = SYSTEM_STATIC.currentUser.id,
                            debit = Convert.ToDouble(txtRewardAmount.Text),
                            credit = 0,
                            deptId = winSelectLedgerCompDept.department.Id,
                            total = Convert.ToDouble(txtRewardAmount.Text) - 0,
                            companyId = winSelectLedgerCompDept.company.Id,
                            currencyId = reward.toDoTask.taskGroup.currency_Id


                        });
                    }
                }
                if (btnPushCredits.IsChecked == true)
                {
                    if (reward.toDoTask.taskGroup.payableAccount_Id != null)
                    {
                        journalTransactions.Add(new JournalTransaction()
                        {
                            accountId = reward.toDoTask.taskGroup.payableAccount_Id,
                            coaTransactionsType = coaTransactionsType.TragetReward,
                            TargetRewardId = reward.Id,
                            creationDate = (DateTime)datglPostingdate.EditValue,
                            transactionRefno = txtFinanceRef.Text,
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            userId = SYSTEM_STATIC.currentUser.id,
                            debit = 0,
                            credit = Convert.ToDouble(txtRewardAmount.Text),
                            deptId = winSelectLedgerCompDept.department.Id,
                            total = 0 - Convert.ToDouble(txtRewardAmount.Text),
                            companyId = winSelectLedgerCompDept.company.Id,
                            currencyId = reward.toDoTask.taskGroup.currency_Id
                        });
                    }
                }
            }
            return journalTransactions;
        }

        public List<JournalTransaction> GetGernalJournalTransactions(TargetRewards reward)
        {
            List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
            if (reward.journalTransactions.Count != 0)
            {
                if (btnPushDebits.IsChecked == true)
                {
                    if (reward.toDoTask.taskGroup.CGSAccount != null)
                    {
                        journalTransactions.Add(new JournalTransaction()
                        {
                            accountId = reward.toDoTask.taskGroup.cgsAccount_Id,
                            coaTransactionsType = coaTransactionsType.TragetReward,
                            TargetRewardId = reward.Id,
                            creationDate = (DateTime)datglPostingdate.EditValue,
                            transactionRefno = txtFinanceRef.Text,
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            userId = SYSTEM_STATIC.currentUser.id,
                            debit = Convert.ToDouble(txtRewardAmount.Text),
                            credit = 0,
                            deptId = winSelectLedgerCompDept.department.Id,
                            total = Convert.ToDouble(txtRewardAmount.Text) - 0,
                            companyId = winSelectLedgerCompDept.company.Id,
                            currencyId = reward.toDoTask.taskGroup.currency_Id


                        });
                    }
                }
                if (btnPushCredits.IsChecked == true)
                {
                    if (reward.toDoTask.taskGroup.payableAccount_Id != null)
                    {
                        journalTransactions.Add(new JournalTransaction()
                        {
                            accountId = reward.toDoTask.taskGroup.payableAccount_Id,
                            coaTransactionsType = coaTransactionsType.TragetReward,
                            TargetRewardId = reward.Id,
                            creationDate = (DateTime)datglPostingdate.EditValue,
                            transactionRefno = txtFinanceRef.Text,
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            userId = SYSTEM_STATIC.currentUser.id,
                            debit = 0,
                            credit = Convert.ToDouble(txtRewardAmount.Text),
                            deptId = winSelectLedgerCompDept.department.Id,
                            total = 0 - Convert.ToDouble(txtRewardAmount.Text),
                            companyId = winSelectLedgerCompDept.company.Id,
                            currencyId = reward.toDoTask.taskGroup.currency_Id
                        });
                    }
                }
            }
            return journalTransactions;
        }
        private void TargetRewardStages()
        {
            if (targetReward.isVoid == true)
            {
                grdVoid.Visibility = Visibility.Visible;
                txtVoid.RenderTransform = new RotateTransform(-45);
                //lblStage.Text = "Void";
            }
            else if (targetReward.isReApproved == false)
            {
                //lblStage.Text = "Under Re-Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (targetReward.isApproved == true && targetReward.stage == "Closed")
            {
                //lblStage.Text = "Approved and Closed";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (targetReward.isApproved == true && targetReward.Status.isActive == false && targetReward.PendingForClosing != true)
            {
                //lblStage.Text = "Approved and Closed";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (targetReward.isApproved == true && targetReward.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (targetReward.isApproved == true)
            {
                //lblStage.Text = "Approved";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (targetReward.isApproved == false)
            {
                //lblStage.Text = "Under Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (targetReward.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
        }

        private void LoadStepStatuses()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            //BillRepo billRepo = new BillRepo();
            List<ToDoTaskStatus> taskStatuses = new List<ToDoTaskStatus>();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Statuses in Targets") != null)
                taskStatuses = taskRepo.GetAllStepStatuses();
            else
                taskStatuses = taskRepo.GetAllStepStatuses().Where(x => x.isActive == true).ToList();

            Parallel.ForEach(taskStatuses, delegate (ToDoTaskStatus status) // foreach (BillStatus status in BillStatuses)
            {
                cmbitems.Add(new cmbitem()
                {
                    name = status.Status,
                    id = status.Id,
                    bcolor = status.backcolor,
                    fcolor = "#FF000000"
                });
            });
            cmbStepStatus.ItemsSource = cmbitems;
        }

        public void LoadStatuses()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            List<TargetRewardStatus> Statuses = new List<TargetRewardStatus>();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Loans and Advances Statuses") != null)
                Statuses = taskRepo.GetAllTargetRewardStatuses();
            else
                Statuses = taskRepo.GetAllTargetRewardStatuses().Where(x => x.isActive == true).ToList();

            Parallel.ForEach(Statuses, delegate (TargetRewardStatus status)
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

        private void AddNewComment()
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            if (targetReward != null)
            {
                var taskGroup = targetReward.toDoTask.taskGroup;
                if (taskGroup != null && taskGroup.usersBulk != null && taskGroup.usersBulk.Count != 0)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, taskGroup.usersBulk, TransactionItemType.TargetReward);
                    inputBox.ShowDialog();
                }
                else if (taskGroup != null && taskGroup.users != null && taskGroup.users.Count != 0)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, taskGroup.users, TransactionItemType.TargetReward);
                    inputBox.ShowDialog();
                }
                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                }


                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.comment != "" && targetReward.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        foreach (var user in frmInputBox.Comment.TaggedList)
                        {
                            if (frmInputBox.FlagForTag == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward with Reward Amount: " + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward with Target Type: " + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, frmInputBox.comment, user.id, "New Comment ", null);
                        }
                        foreach (var user in frmInputBox.Comment.CCUsersList)
                        {
                            if (frmInputBox.FlagForCC == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Target Reward with Target Type:" + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Target Reward with Target Type:" + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, frmInputBox.comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    procurementRepo.Add(targetReward.Id, TransactionItemType.TargetReward, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    //frmInputBox.taggedUsers = new List<User>();
                    loadcomments();
                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (targetReward.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Target Reward first to add a comment!");
                }

            }
            loadcomments();
        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            AddNewComment();
        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                grdComments.Visibility = Visibility.Visible;
                loadcomments();
            }
            else
                grdComments.Visibility = Visibility.Collapsed;
        }

        public void loadcomments()
        {
            try
            {
                if (targetReward != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(targetReward.Id, TransactionItemType.TargetReward);
                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }


        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            if (grdCommentss.SelectedItem != null)
            {
                var comment = grdCommentss.SelectedItem as CommentLog;
                if (targetReward != null)
                {
                    var taskGroup = targetReward.toDoTask.taskGroup;
                    if (taskGroup != null && taskGroup.users != null && taskGroup.users.Count != 0)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, taskGroup.users, comment, TransactionItemType.TargetReward);
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (targetReward != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && targetReward.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(targetReward.Id, TransactionItemType.TargetReward, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward with Reward Amount: " + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward with Reward Amount: " + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward with Reward Amount: " + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward with Reward Amount: " + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }


                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (targetReward.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Target Reward first to add a comment!");
                        }
                    }
                    loadcomments();
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);

                }
            }
        }

        private void Resend_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Resend_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            ProcurementRepo procurementRepo = new ProcurementRepo();

            var comment = grdCommentss.SelectedItem as CommentLog;


            if (comment != null)
            {
                comment = procurementRepo.GetComment(comment.Id);
                if (comment.employee.EmployeeUsers.FirstOrDefault(x => x.id == SYSTEM_STATIC.currentUser.id) == null)
                {
                    DXMessageBox.Show("Only sender of this comment can change Flag!");
                    return;
                }
                var taskGroup = targetReward.toDoTask.taskGroup;
                if (taskGroup != null && taskGroup.users != null && taskGroup.users.Count != 0)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, taskGroup.users, TransactionItemType.TargetReward);
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }
                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                }

                if (frmInputBox.commentAdded == true && targetReward.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);

                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (targetReward.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Sale Order first to add a comment!");
                }
            }
            loadcomments();
        }

        private void BtnCreatePayment_Click(object sender, RoutedEventArgs e)
        {
            bool isFullyPaid = true;

            if (rewardId > 0 && targetReward != null)
            {



                if (targetReward.Payments.Where(x => x.isVoid != true) == null || targetReward.Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                {
                    isFullyPaid = false;
                }
                else if ((targetReward.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount) - targetReward.RewardAmount) != 0)
                {
                    isFullyPaid = false;
                }


                if (isFullyPaid == true)
                {
                    DXMessageBox.Show("This Target Reward is fully Paid!");
                    return;
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Target Rewards Payment") != null)
                {
                    Window moduleWin = new Window();

                    ucFrmTargetRewardPayment frmPayments = new ucFrmTargetRewardPayment();

                    frmPayments.targetRewardId = targetReward.Id;
                    frmPayments.createdFromBill = true;
                    moduleWin.Content = frmPayments;
                    moduleWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    moduleWin.WindowState = WindowState.Maximized;
                    moduleWin.Show();

                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add new Payment!");
                }
            }
        }

        public void loadAttachments()
        {
            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(rewardId, TransactionItemType.TargetReward);
        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with Target Reward") != null)
            {
                if (grdAttachments.Visibility == Visibility.Visible)
                    grdAttachments.Visibility = Visibility.Collapsed;
                else
                {
                    grdAttachments.Visibility = Visibility.Visible;
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of Attached Files!");
            }
        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with Target Reward") != null)
            {
                if (grdAttach1.Visibility == Visibility.Visible)
                    grdAttach1.Visibility = Visibility.Collapsed;
                else
                {
                    cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveRewardAttachmentCategories();
                    grdAttach1.Visibility = Visibility.Visible;
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to Attach File!");
            }
        }

        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory1.SelectedItem != null)
            {
                if (rewardId != 0)
                {
                    try
                    {
                        int CategoryId = (cmbCategory1.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";

                        fileDialog.Multiselect = false;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\TargetReward\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew1.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew1.ToolTip = "Uploading";
                            btnAttachNew1.IsEnabled = true;

                            btnAttachment1.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += rewardId + "_" + TransactionItemType.TargetReward.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.TargetReward);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), rewardId, TransactionItemType.TargetReward, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, rewardId, 20, "Added a New attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(rewardId, TransactionItemType.TargetReward);
                                            imgAttachNew1.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew1.ToolTip = "Attach";
                                            btnAttachNew1.IsEnabled = true;
                                            btnAttachment1.Content = "Select";
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
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        imgAttachNew1.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                        btnAttachNew1.ToolTip = "Attach";
                        btnAttachNew1.IsEnabled = true;
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

        private void BtnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sSelectedPath = "";

                Button thisButton = (Button)sender;
                string str = thisButton.Tag.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    grdProgressBar.Visibility = Visibility.Visible;
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        //Button thisButton = (Button)sender;

                        ERP_BL.Attach attachment = new ERP_BL.Attach(str);

                        if (str.Contains("Sale_Order"))
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
                        if (str.Contains("Inquiry"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Inquiry);
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
                        if (str.Contains("TargetReward"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.TargetReward);
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
                MessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), ex.ToString());
            }
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (targetReward != null && targetReward.Id > 0)
            {
                var idd = targetReward.Id;
                if (idd != 0)
                {
                    frmTrackingWindow trackingWindow = new frmTrackingWindow(idd, TransactionItemType.TargetReward);
                    trackingWindow.ShowDialog();
                }
            }
            else
            {
                DXMessageBox.Show("Please save this Transaction first!");
            }
        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (targetReward.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Target Reward") != null))
            {
                if (DXMessageBox.Show("This Target Reward is currently in the list of Void Target Rewards! Do you want to remove it from Void?", "Remove Void Target Reward", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    targetReward.isVoid = false;
                    taskRepo.UpdateTargetReward(targetReward);

                    grdVoid.Visibility = Visibility.Collapsed;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Target Reward has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (targetReward.toDoTask.taskGroup != null && targetReward.toDoTask.taskGroup.users != null && targetReward.toDoTask.taskGroup.users.Count != 0)
                        {
                            winTagUsers win = new winTagUsers(targetReward.toDoTask.taskGroup.users, targetReward.Id, TransactionItemType.TargetReward);
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

                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Target Reward having Reward Amount: " + targetReward.RewardAmount.ToString() + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "Target Reward UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(targetReward.Id, TransactionItemType.TargetReward, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task with Reward Amount: " + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task with Reward Amount: " + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            else if (targetReward.isVoid == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Target Reward") != null)
            {
                if (DXMessageBox.Show("This Target Reward is not currently in the list of Void Target Rewards! Do you want to move it to Void Target Rewards?", "Add to Void Target Rewards", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    targetReward.isVoid = true;
                    taskRepo.UpdateTargetReward(targetReward);

                    grdVoid.Visibility = Visibility.Visible;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Target Reward has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (targetReward.toDoTask.taskGroup != null && targetReward.toDoTask.taskGroup.users != null && targetReward.toDoTask.taskGroup.users.Count != 0)
                        {
                            winTagUsers win = new winTagUsers(targetReward.toDoTask.taskGroup.users, targetReward.Id, TransactionItemType.TargetReward);
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

                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Target Reward having Amount: " + targetReward.RewardAmount.ToString() + " has been marked as void",
                        Timestamp = DateTime.Now,
                        Subject = "Target Reward Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(targetReward.Id, TransactionItemType.TargetReward, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Target Reward with Reward Amount: " + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Target Reward with Reward Amount: " + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }

            loadcomments();
            var thisWindow = Window.GetWindow(this);
            thisWindow.Close();
        }

        private void BtnApplyReward_Click(object sender, RoutedEventArgs e)
        {

        }

        TargetRewardStatus oldStatus = new TargetRewardStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        static TargetRewardStatus statusChanged = new TargetRewardStatus();
        public ucFrmTargetRewardAdd(TargetRewardStatus targetRewardStatus)
        {
            statusChanged = targetRewardStatus;
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (targetReward != null && rewardId > 0)
            {
                if (targetReward.isApplied == false)
                {
                    DXMessageBox.Show("This reward is not applied yet!");
                    return;
                }
                var previous_status = targetReward.Status.Status;
                if (targetReward != null)
                {
                    if (targetReward.isApproved == false)
                    {
                        DXMessageBox.Show("Target Reward is under approval!");
                        return;
                    }

                    statusChanged = null;
                    frmTargetRewardStatusChange ucFrmDirectClose = new frmTargetRewardStatusChange();
                    if (targetReward.Status != null)
                    {
                        ucFrmDirectClose.statusName.Text = targetReward.Status.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(targetReward.Status.backcolor);
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

                        var res = MessageBox.Show("Target Reward has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (targetReward.toDoTask != null && targetReward.toDoTask.taskGroup != null && targetReward.toDoTask.taskGroup.users != null)
                            {
                                var userss = targetReward.toDoTask.taskGroup.users;

                                //if (userss.Find(x => x.id == loansAdvance.creatorId) == null)
                                //    userss.Add(loansAdvance.Creator);


                                winTagUsers win = new winTagUsers(userss, targetReward.Id, TransactionItemType.TargetReward);
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


                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Target Reward") != null)
                        {

                            targetReward.PendingForClosing = false;
                            targetReward.stage = TransactionStage.Closed.ToString();
                            targetReward.statusId = statusChanged.Id;
                            targetReward.LastStatusChangeDate = System.DateTime.Now;
                            targetReward.ClosingDate = System.DateTime.Now;



                            taskRepo.UpdateTargetReward(targetReward);

                            AddNewComment();
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            UsersRepo.Add(TransactionInfo.Closed, targetReward.Id, (int)TransactionItemType.TargetReward, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Target Reward has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount:" + targetReward.RewardAmount,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (targetReward != null)
                                {
                                    procurementRepo.Add(targetReward.Id, TransactionItemType.TargetReward, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward", targetReward.Id, TransactionItemType.TargetReward, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward", targetReward.Id, TransactionItemType.TargetReward, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {

                            targetReward.PendingForClosing = true;
                            targetReward.stage = TransactionStage.AwaitingApproval.ToString();
                            targetReward.statusId = statusChanged.Id;
                            targetReward.LastStatusChangeDate = System.DateTime.Now;
                            targetReward.ClosingDate = System.DateTime.Now;



                            taskRepo.UpdateTargetReward(targetReward);

                            AddNewComment();
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            UsersRepo.Add(TransactionInfo.Closed, targetReward.Id, (int)TransactionItemType.TargetReward, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Target Reward has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount:" + targetReward.RewardAmount,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (targetReward != null)
                                {
                                    procurementRepo.Add(targetReward.Id, TransactionItemType.TargetReward, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward", targetReward.Id, TransactionItemType.TargetReward, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward", targetReward.Id, TransactionItemType.TargetReward, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (targetReward != null && rewardId > 0)
                {
                    if (targetReward.isApplied == false)
                    {
                        DXMessageBox.Show("This reward is not applied yet!");
                        return;
                    }

                    //SalesReceipt receipt = new SalesReceipt();
                    targetReward = taskRepo.GetTargetReward(rewardId);
                    UsersRepo usersRepo = new UsersRepo();

                    if (targetReward != null)
                    {
                        if (targetReward.isApproved == true)
                        {
                            if ( SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Target Reward") != null )
                            {
                                var res = MessageBox.Show("Target Reward is Approved, Do you want to UnApprove?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    targetReward.isApproved = false;
                                    targetReward.stage = TransactionStage.AwaitingApproval.ToString();

                                    AddNewComment();
                                    frmInputBox inputBox = new frmInputBox("Reward UnApproved");
                                    usersRepo.Add(TransactionInfo.Approved_Adding, targetReward.Id, (int)TransactionItemType.TargetReward, frmInputBox.comment);
                                    taskRepo.UpdateTargetReward(targetReward);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Target Reward has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (targetReward.toDoTask != null && targetReward.toDoTask.taskGroup != null && targetReward.toDoTask.taskGroup.users != null)
                                        {
                                            var userss = targetReward.toDoTask.taskGroup.users;

                                            //if (userss.Find(x => x.id == targetReward.creatorId) == null)
                                            //    userss.Add(loansAdvance.Creator);

                                            winTagUsers win = new winTagUsers(userss, targetReward.Id, TransactionItemType.TargetReward);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;

                                            //if (tagUsers.Find(x => x.id == loansAdvance.creatorId) == null)
                                            //    tagUsers.Add(loansAdvance.Creator);

                                            //if (ccUsers.Find(x => x.id == loansAdvance.creatorId) == null)
                                            //    ccUsers.Add(loansAdvance.Creator);
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    string symbolCurr = "";
                                    if (targetReward.currency != null)
                                    {
                                        symbolCurr = targetReward.currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Target Reward (Amount OC) having value: " + targetReward.RewardAmount.ToString() + "(" + symbolCurr + ") " + " has been UnApproved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Target Reward UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(targetReward.Id, TransactionItemType.TargetReward, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Target Reward" , targetReward.Id, TransactionItemType.TargetReward, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Target Reward", targetReward.Id, TransactionItemType.TargetReward, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Target Reward are UnApproved (" + targetReward.Id + ")");
                                    SystemLog.LogInfo(this.GetType(), "Target Reward is UnApproved (" + targetReward.Id + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Target Reward Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Target Reward Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (targetReward.isApproved == false)
                        {
                            if ( SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Target Reward") != null)
                            {
                                var res = MessageBox.Show("Target Reward are Pending for Approval, Do you want to Approve?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {

                                    targetReward.isApproved = true;
                                    targetReward.stage = TransactionStage.Approved.ToString();

                                    AddNewComment();
                                    frmInputBox inputBox = new frmInputBox("Reward Approved");
                                    usersRepo.Add(TransactionInfo.Approved_Adding, targetReward.Id, (int)TransactionItemType.TargetReward, frmInputBox.comment);

                                    taskRepo.UpdateTargetReward(targetReward);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Target Reward has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (targetReward.toDoTask != null && targetReward.toDoTask.taskGroup != null && targetReward.toDoTask.taskGroup.users != null)
                                        {
                                            var userss = targetReward.toDoTask.taskGroup.users;

                                            //if (userss.Find(x => x.id == loansAdvance.creatorId) == null)
                                            //    userss.Add(loansAdvance.Creator);

                                            winTagUsers win = new winTagUsers(userss, targetReward.Id, TransactionItemType.TargetReward);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;

                                            //if (tagUsers.Find(x => x.id == loansAdvance.creatorId) == null)
                                            //    tagUsers.Add(loansAdvance.Creator);

                                            //if (ccUsers.Find(x => x.id == loansAdvance.creatorId) == null)
                                            //    ccUsers.Add(loansAdvance.Creator);
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    string symbolCurr = "";
                                    if (targetReward.currency != null)
                                    {
                                        symbolCurr = targetReward.currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Target Reward (Amount OC) having value: " + targetReward.RewardAmount.ToString() + "(" + symbolCurr + ") " + " has been Approved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Target Reward Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(targetReward.Id, TransactionItemType.TargetReward, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Target Reward", targetReward.Id, TransactionItemType.TargetReward, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Target Reward", targetReward.Id, TransactionItemType.TargetReward, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Target Reward is Approved (" + targetReward.Id + ")");
                                    SystemLog.LogInfo(this.GetType(), "Target Reward is Approved (" + targetReward.Id + ")");
                                }


                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Target Reward Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Target Reward Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (targetReward.isReApproved == false)
                        {
                            if ( SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Target Reward") != null)
                            {
                                targetReward.isReApproved = true;
                                targetReward.stage = TransactionStage.Approved.ToString();

                                AddNewComment();
                                frmInputBox inputBox = new frmInputBox("Reward Approved");
                                usersRepo.Add(TransactionInfo.Approved_Adding, targetReward.Id, (int)TransactionItemType.TargetReward, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                taskRepo.UpdateTargetReward(targetReward);

                                MessageBox.Show("Target Reward are Approved (" + targetReward.Id + ")");
                                SystemLog.LogInfo(this.GetType(), "Target Reward is Approved (" + targetReward.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Target Reward Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Target Reward Directly user id=(" + MainWindow.currentUserid + ")");
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

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
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

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GrdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (rewardId != 0)
            {
                UsersRepo.Add(TransactionInfo.viewed, rewardId, (int)TransactionItemType.TargetReward, "Viewed details of Target Reward");
            }
        }

        private void btnGernalJournal_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View General Journal") != null)
            {
              
                if (targetReward.journalTransactions.Count > 0 && targetReward.journalTransactions != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Mouse.OverrideCursor = Cursors.Wait;
                    });

                    ZAS_ERP.ChartofAccounts.Windows.GernalJournal generalJournal = new ZAS_ERP.ChartofAccounts.Windows.GernalJournal(targetReward.journalTransactions.ToList());
                    generalJournal.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to View General Journal.");
            }
        }


        private void GrdRewards_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var reward = grdRewards.GetRowByListIndex(e.ListSourceRowIndex) as TargetRewards;
            var step = reward.toDoTask;
            if (e.IsGetData)
                switch (e.Column.FieldName)
                {
                    case "Stage":


                        if (reward.isVoid == true)
                        {
                            e.Value = "Void";
                            //lblStage.Text = "Void";
                        }
                        else if (reward.isReApproved == false)
                        {
                            e.Value = "Under Re-Approval";
                        }
                        else if (reward.isApproved == true && reward.stage == "Closed")
                        {
                            e.Value = "Approved and Closed";

                        }
                        else if (reward.isApproved == true && reward.Status.isActive == false && reward.PendingForClosing != true)
                        {
                            e.Value = "Approved and Closed";
                        }
                        else if (reward.isApproved == true && reward.PendingForClosing == true)
                        {
                            e.Value = "Under Closing Approval";
                        }
                        else if (reward.isApproved == true)
                        {
                            e.Value = "Approved";
                        }
                        else if (reward.isApproved == false)
                        {
                            e.Value = "Under Approval";
                        }
                        else if (reward.PendingForClosing == true)
                        {
                            e.Value = "Under Closing Approval";
                        }
                        break;
                    //case "AchievedPercentage":

                    //    if (step.TaskPoints > 0)
                    //    {
                    //        decimal percentage = 0;
                    //        if (step.taskGroup.Companies != null && step.taskGroup.Companies.Count > 0 && step.taskGroup.Departments != null && step.taskGroup.Departments.Count > 0)
                    //            percentage = Math.Round((Convert.ToDecimal(step.AchievedPoints) * 100 / Convert.ToDecimal(step.TaskPoints)), 2);
                    //        else
                    //            percentage = Math.Round((Convert.ToDecimal(step.AchievedPoints) * 100 / Convert.ToDecimal(step.TaskPoints)), 2);
                    //        e.Value = percentage;
                    //    }
                    //    break;

                    case "StepYear":
                        if (reward != null && reward.toDoTask != null && reward.toDoTask.TargetYear != null)
                        {
                            e.Value = reward.toDoTask.TargetYear.Value.Year;
                        }
                        break;

                    case "StepMonth":
                        if (reward != null && reward.toDoTask != null && reward.toDoTask.TargetMonth != null)
                        {
                            string fullMonthName = reward.toDoTask.TargetMonth.Value.ToString("MMMM", System.Globalization.CultureInfo.CreateSpecificCulture("en"));
                            e.Value = fullMonthName;
                        }
                        break;
                    case "TargettYear":
                        if (reward != null && reward.toDoTask != null && reward.toDoTask.ParentTask != null && reward.toDoTask.ParentTask.TargetYear != null)
                        {
                            e.Value = reward.toDoTask.ParentTask.TargetYear.Value.Year;
                        }
                        break;

                    case "TargettMonth":
                        if (reward != null && reward.toDoTask != null && reward.toDoTask.ParentTask != null && reward.toDoTask.ParentTask.TargetMonth != null)
                        {
                            string fullMonthName = reward.toDoTask.ParentTask.TargetMonth.Value.ToString("MMMM", System.Globalization.CultureInfo.CreateSpecificCulture("en"));
                            e.Value = fullMonthName;
                        }
                        break;
                    case "Closed":
                        if (reward != null)
                        {
                            if (reward.Payments.Count > 0 && reward.Payments.Where(x => x.Status != null && x.Status.isActive == true).Count() == 0)
                            {
                                e.Value = "Closed";
                            }
                            else
                            {
                                if (reward.RewardAmount - reward.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount) == 0)
                                    e.Value = "Paid";
                            }
                        }
                        break;

                    case "PaidAmount":
                        if (reward != null)
                        {
                            if (reward.Payments == null || reward.Payments.Where(x => x.isVoid != true) == null || reward.Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                            {
                                e.Value = Convert.ToDecimal(0);
                            }
                            else
                            {
                                e.Value = Convert.ToDecimal(reward.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount));
                            }
                        }
                        break;
                    case "UnPaidAmount":
                        if (reward != null)
                        {
                            if (reward.Payments == null || reward.Payments.Where(x => x.isVoid != true) == null || reward.Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                            {
                                e.Value = Convert.ToDecimal(reward.RewardAmount);
                            }
                            else
                            {
                                e.Value = Convert.ToDecimal(reward.RewardAmount - reward.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount));
                            }
                        }
                        break;

                    case "SalesBudgetedMarginPercent":
                        if (step.TaskPoints > 0)
                        {
                            decimal percentage = 0;
                            if (step.TaskPoints != 0)
                                percentage = Math.Round((Convert.ToDecimal(step.SalesBudgetedMargin) * 100 / Convert.ToDecimal(step.TaskPoints)), 2);

                            e.Value = percentage;
                        }
                        break;
                    case "SoAmountSERpercent":
                        if (step.TaskPoints > 0)
                        {
                            decimal percentage = 0;
                            if (step.SoAmountSER != 0)
                                percentage = Math.Round((Convert.ToDecimal(step.SalesBudgetedMargin) * 100 / Convert.ToDecimal(step.SoAmountSER)), 2);

                            e.Value = percentage;
                        }
                        break;

                    case "Companies":

                        if (step.taskGroup != null)
                        {
                            var companies = String.Join(" | ", step.taskGroup.Companies.Select(x => x.CompanyName));
                            e.Value = companies;
                        }
                        break;

                    case "Departments":

                        if (step.taskGroup != null)
                        {
                            var depts = String.Join(" | ", step.taskGroup.Departments.Select(x => x.DeptName));
                            e.Value = depts;
                        }
                        break;


                    case "StepLevel1":

                        if (step != null)
                        {
                            var groupList = new List<TargetGroup>();
                            var node = step.targetGroup;

                            while (node != null)
                            {
                                if (node.parentId != null)
                                {
                                    if (node.parentId != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        groupList.Add(node);
                                        node = node.targetGroup;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        groupList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    groupList.Add(node);
                                    break;
                                }

                            }
                            groupList.Reverse();
                            if (groupList.Count > 0)
                                e.Value = groupList[0].GroupName;
                        }

                        break;
                    case "StepLevel2":

                        if (step != null)
                        {
                            var groupList = new List<TargetGroup>();
                            var node = step.targetGroup;

                            while (node != null)
                            {
                                if (node.parentId != null)
                                {
                                    if (node.parentId != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        groupList.Add(node);
                                        node = node.targetGroup;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        groupList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    groupList.Add(node);
                                    break;
                                }

                            }

                            groupList.Reverse();

                            switch (groupList.Count)
                            {
                                case 0:

                                    break;
                                case 1:
                                    e.Value = groupList[0].GroupName;
                                    break;
                                case 2:
                                    e.Value = groupList[1].GroupName;
                                    break;
                                case 3:
                                    e.Value = groupList[1].GroupName;
                                    break;
                                case 4:
                                    e.Value = groupList[1].GroupName;
                                    break;
                                case 5:
                                    e.Value = groupList[1].GroupName;
                                    break;
                            }
                        }
                        break;

                    case "StepLevel3":

                        if (step != null)
                        {
                            var groupList = new List<TargetGroup>();
                            var node = step.targetGroup;

                            while (node != null)
                            {
                                if (node.parentId != null)
                                {
                                    if (node.parentId != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        groupList.Add(node);
                                        node = node.targetGroup;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        groupList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    groupList.Add(node);
                                    break;
                                }

                            }
                            groupList.Reverse();

                            switch (groupList.Count)
                            {
                                case 0:

                                    break;
                                case 1:
                                    e.Value = groupList[0].GroupName;
                                    break;
                                case 2:
                                    e.Value = groupList[1].GroupName;
                                    break;
                                case 3:
                                    e.Value = groupList[2].GroupName;
                                    break;
                                case 4:
                                    e.Value = groupList[2].GroupName;
                                    break;
                                case 5:
                                    e.Value = groupList[2].GroupName;
                                    break;
                            }
                        }

                        break;
                }
        }

        private void GrdRewards_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
