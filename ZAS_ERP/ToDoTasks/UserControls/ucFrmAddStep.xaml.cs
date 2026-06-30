using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.ToDoTasks;
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

namespace ZAS_ERP.ToDoTasks.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmAddStep.xaml
    /// </summary>
    public partial class ucFrmAddStep : UserControl
    {
        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        ToDoTask task = new ToDoTask();
        List<TargetRewards> targetSalesRewards = new List<TargetRewards>();
        List<TargetRewards> targetFinanceRewards = new List<TargetRewards>();
        List<TargetRewards> targetOtherRewards = new List<TargetRewards>();

        public bool editFlag = false;
        public int stepId = 0;
        public int taskId = 0;
        public int taskGroupId = 0;
        public ucFrmAddStep()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //if (cmbStepStatus.SelectedIndex < 0)
            //{
            //    DXMessageBox.Show("Please select Status!");
            //    cmbStepStatus.Focus();
            //    return;
            //}
            if (String.IsNullOrEmpty(txtStepName.Text))
            {
                DXMessageBox.Show("Please enter Step Name!");
                txtStepName.Focus();
                return;
            }
            if (Convert.ToDouble( txtStepPoints.Text) == 0)
            {
                DXMessageBox.Show("Please enter Step Points!");
                txtStepPoints.Focus();
                return;
            }
            if (String.IsNullOrEmpty(datStepDueDate.Text))
            {
                DXMessageBox.Show("Please enter Step due Date!");
                datStepDueDate.Focus();
                return;
            }

            if (datTargetYear.EditValue == null)
            {
                DXMessageBox.Show("Please select Target Year!");
                datTargetYear.Focus();
                return;
            }
            

            //if (datTargetMonth.EditValue == null)
            //{
            //    DXMessageBox.Show("Please select Target Year!");
            //    datTargetMonth.Focus();
            //    return;
            //}

            if (cmbStepStatus.SelectedIndex > -1)
                task.statusId = (cmbStepStatus.SelectedItem as cmbitem).id;
            task.TaskName = txtStepName.Text;
            task.TaskPoints = Convert.ToDouble(txtStepPoints.Text);

            if (lookUpTargetGroup.SelectedIndex > -1)
                task.targetGroupId = (lookUpTargetGroup.SelectedItem as TargetGroup).Id;
            else
                task.targetGroupId = null;

            if (lookUpCalculationType.SelectedIndex > -1)
                task.CalculationType_Id = (lookUpCalculationType.SelectedItem as StatusCalculationType).Id;
            else
                task.CalculationType_Id = null;

            if (Convert.ToInt32(txtParentId.Text) > 0)
                task.parentTaskId = Convert.ToInt32(txtParentId.Text);

            task.StepDueDate = datStepDueDate.DateTime;
            task.TargetYear = datTargetYear.DateTime;

            if (datTargetMonth.EditValue != null)
                task.TargetMonth = new DateTime(2022, datTargetMonth.DateTime.Month, 1);
            else
                task.TargetMonth = null;

            task.creationDate = DateTime.Now;

            if(taskGroupId > 0)
                task.taskGroupId = taskGroupId;

            if (chkCompleted.IsChecked == true)
                task.isCompleted = true;
            else
                task.isCompleted = false;

            if (chkClosed.IsChecked == true)
                task.isClosed = true;
            else
                task.isClosed = false;

            if (editFlag == true)
            {

                if(targetSalesRewards != null)
                {
                    foreach(var _reward in targetSalesRewards)
                    {
                        if (!task.targetRewards.Contains(_reward))
                        {
                            _reward.CreationDate = DateTime.Now;
                            _reward.functionType = ERP_BL.Enums.FunctionType.Sales;
                            _reward.creator_Id = SYSTEM_STATIC.currentUser.id;
                            task.targetRewards.Add(_reward);

                        }
                            
                    }
                }

                if (targetFinanceRewards != null)
                {
                    foreach (var _reward in targetFinanceRewards)
                    {
                        if (!task.targetRewards.Contains(_reward))
                        {
                            _reward.CreationDate = DateTime.Now;
                            _reward.functionType = ERP_BL.Enums.FunctionType.Finance;
                            _reward.creator_Id = SYSTEM_STATIC.currentUser.id;
                            task.targetRewards.Add(_reward);
                        }
                    }
                }

                if (targetOtherRewards != null)
                {
                    foreach (var _reward in targetOtherRewards)
                    {
                        if (!task.targetRewards.Contains(_reward))
                        {
                            _reward.CreationDate = DateTime.Now;
                            _reward.functionType = ERP_BL.Enums.FunctionType.Other;
                            _reward.creator_Id = SYSTEM_STATIC.currentUser.id;
                            _reward.GlPostingDate = _reward.CreationDate;
                            task.targetRewards.Add(_reward);
                        }
                    }
                }
                //if (task.AchievedPoints != Convert.ToDouble(txtAchievedPoints.Text))
                //{
                //    task.AchievedPoints = Convert.ToDouble(txtAchievedPoints.Text);
                //    task.PointsUpdatedOn = DateTime.Now;
                //}
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Steps Without ReApproval") == null)
                {
                    task.isReApproved = false;
                }

                taskRepo.UpdateStep(task);
                DXMessageBox.Show("Updated Successfully!");
            }
            else
            {
                task.AchievedPoints = 0;
                task.PointsUpdatedOn = null;
                task.isApproved = false;
                taskRepo.AddTask(task);
                DXMessageBox.Show("Added Successfully!");
            }

            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStatuses();
            LoadTargetGroups();
            LoadStatusCalculationTypes();

            if(SYSTEM_STATIC.AllowedPermissions.Find(x=>x.Name == "Finance Target Rewards") == null)
                tabFinanceRewards.IsEnabled = false;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sales Target Rewards") == null)
                tabSalesRewards.IsEnabled = false;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Other Target Rewards") == null)
                tabOtherRewards.IsEnabled = false;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Sales Target Rewards") == null)
                tblViewSalesRewards.NewItemRowPosition = DevExpress.Xpf.Grid.NewItemRowPosition.None;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Finance Target Rewards") == null)
                tblViewFinanceRewards.NewItemRowPosition = DevExpress.Xpf.Grid.NewItemRowPosition.None;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Other Target Rewards") == null)
                tblViewOtherRewards.NewItemRowPosition = DevExpress.Xpf.Grid.NewItemRowPosition.None;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Over Rule Calculation Type") == null)
                lookUpCalculationType.IsEnabled = false;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Over Rule Group Level") == null)
                lookUpTargetGroup.IsEnabled = false;


            if (editFlag == false)
            {
                txtAchievedPoints.IsEnabled = false;
                if (taskGroupId > 0)
                {
                    var taskGroup = taskRepo.GetTaskGroup(taskGroupId);
                    lblGroupName.Text = taskGroup.GroupName;
                    if (taskGroup.calculationType != null)
                        lookUpCalculationType.Text = taskGroup.calculationType.TypeName;
                }
                if (taskId > 0)
                {
                    var parentTask = taskRepo.GetTask(stepId);
                    if (parentTask.targetGroup != null)
                        lookUpTargetGroup.Text = parentTask.targetGroup.GroupName;
                }                
            }

            if(editFlag == true && stepId > 0)
            {
                btnAutoCalculatePoints.IsEnabled = false;

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Step") == null)
                    btnSave.IsEnabled = false;

                task = taskRepo.GetTask(stepId);

                LoadRewardGrid();

                if (task.targetRewards != null)
                {
                    targetSalesRewards = task.targetRewards.Where(x=>x.functionType == ERP_BL.Enums.FunctionType.Sales && x.isVoid != true).ToList();
                    grdSalesRewards.ItemsSource = targetSalesRewards;

                    targetFinanceRewards = task.targetRewards.Where(x => x.functionType == ERP_BL.Enums.FunctionType.Finance && x.isVoid != true).ToList();
                    grdFinanceRewards.ItemsSource = targetFinanceRewards;

                    targetOtherRewards = task.targetRewards.Where(x => x.functionType == ERP_BL.Enums.FunctionType.Other && x.isVoid != true).ToList();
                    grdOtherRewards.ItemsSource = targetOtherRewards;
                }
                else
                {
                    targetSalesRewards = new List<TargetRewards>();
                    grdSalesRewards.ItemsSource = targetSalesRewards;

                    targetFinanceRewards = new List<TargetRewards>();
                    grdFinanceRewards.ItemsSource = targetFinanceRewards;

                    targetOtherRewards = new List<TargetRewards>();
                    grdOtherRewards.ItemsSource = targetOtherRewards;
                    //targetSalesRewards = task.targetSalesRewards;
                }

                //Select Status
                var statusList = (cmbStepStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbStepStatus.ItemsSource as List<cmbitem>;
                if (task.Status != null)
                {
                    int index = 0;
                    foreach (var _status in statusList)
                    {
                        if (_status.id == task.statusId)
                        {
                            cmbStepStatus.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                if (task.TaskName != null)
                    txtStepName.Text = task.TaskName;

                if (task.targetGroup != null)
                    lookUpTargetGroup.Text = task.targetGroup.GroupName;
                else if (task.taskGroup != null && task.taskGroup.targetGroup != null)
                    lookUpTargetGroup.Text = task.taskGroup.targetGroup.GroupName;

                if (task.calculationType != null)
                    lookUpCalculationType.Text = task.calculationType.TypeName;
                else if(task.taskGroup.calculationType != null)
                    lookUpCalculationType.Text = task.taskGroup.calculationType.TypeName;

                txtStepPoints.Text = task.TaskPoints.ToString();
                txtAchievedPoints.Text = task.AchievedPoints.ToString();

                if (task.StepDueDate != null)
                    datStepDueDate.EditValue = task.StepDueDate;

                if (task.TargetYear != null)
                    datTargetYear.EditValue = task.TargetYear.Value;
                else
                    datTargetYear.EditValue = null;

                if (task.TargetMonth != null)
                    datTargetMonth.EditValue = task.TargetMonth.Value;
                else
                    datTargetMonth.EditValue = null;

                if (task.PointsUpdatedOn != null)
                    datUpdatedOn.EditValue = task.PointsUpdatedOn;

                if (task.isCompleted == true)
                    chkCompleted.IsChecked = true;
                else
                    chkCompleted.IsChecked = false;

                if (task.isClosed == true)
                    chkClosed.IsChecked = true;
                else
                    chkClosed.IsChecked = false;
            }
        }

        private void BtnAutoCalculate_Click(object sender, RoutedEventArgs e)
        {
            if(Convert.ToInt32(txtParentId.Text) > 0)
            {
                txtStepPoints.Text = taskRepo.CalculateStepPoints(Convert.ToInt32(txtParentId.Text)).ToString();
            }
        }


        private void LoadTargetGroups()
        {
            lookUpTargetGroup.ItemsSource = taskRepo.GetAllTargetGroups();
        }

        private void LoadStatusCalculationTypes()
        {
            lookUpCalculationType.ItemsSource = taskRepo.GetAllStatusCalculationTypes();
        }

        private void LoadRewardGrid()
        {
            var allEmployees = taskRepo.getAllusers();
            if(editFlag == true)
            {
                if (task.ParentTask != null && task.ParentTask.taskGroup != null && task.ParentTask.taskGroup.usersBulk != null && task.ParentTask.taskGroup.usersBulk.Count > 0)
                {
                    lookupEmployees.ItemsSource = task.ParentTask.taskGroup.usersBulk.Where(x => x.employee.empFunction != null && x.employee.empFunction.functionType == ERP_BL.Enums.FunctionType.Sales).ToList();
                    lookupFinanceUsers.ItemsSource = task.ParentTask.taskGroup.usersBulk.Where(x => x.employee.empFunction != null && x.employee.empFunction.functionType == ERP_BL.Enums.FunctionType.Finance).ToList();

                    lookupOtherUsers.ItemsSource = allEmployees;
                    //lookupOtherUsers.ItemsSource = task.ParentTask.taskGroup.users.Where(x => x.employee.empFunction != null && x.employee.empFunction.functionType == ERP_BL.Enums.FunctionType.Other).ToList();
                }
                else if (task.ParentTask != null && task.ParentTask.taskGroup != null && task.ParentTask.taskGroup.users != null && task.ParentTask.taskGroup.users.Count > 0)
                {
                    lookupEmployees.ItemsSource = task.ParentTask.taskGroup.users.Where(x=>x.employee.empFunction != null && x.employee.empFunction.functionType == ERP_BL.Enums.FunctionType.Sales).ToList();
                    lookupFinanceUsers.ItemsSource = task.ParentTask.taskGroup.users.Where(x => x.employee.empFunction != null && x.employee.empFunction.functionType == ERP_BL.Enums.FunctionType.Finance).ToList();

                    lookupOtherUsers.ItemsSource = allEmployees;
                    //lookupOtherUsers.ItemsSource = task.ParentTask.taskGroup.users.Where(x => x.employee.empFunction != null && x.employee.empFunction.functionType == ERP_BL.Enums.FunctionType.Other).ToList();
                }

                
                var currencies = taskRepo.getAllCurrencies(); ;
                lookupCurrency.ItemsSource = currencies;
                lookupCurrencyForFinance.ItemsSource = currencies;
                lookupCurrencyForOthers.ItemsSource = currencies;
            }
        }

        private void LoadStatuses()
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

        private void GrdSalesRewards_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void TblViewSalesRewards_ShowGridMenu(object sender, DevExpress.Xpf.Grid.GridMenuEventArgs e)
        {

        }

        private void View_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
        }
    }
}
