using DevExpress.Xpf.Core;
using ERP_BL.ToDoTasks.Taskss;
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

namespace ZAS_ERP.ToDoTasks.Taskks.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmEfficiencyPoints.xaml
    /// </summary>
    public partial class ucFrmEfficiencyPoints : Window
    {
        public List<TaskEfficiency> taskEfficiencies = new List<TaskEfficiency>();
        public List<TaskEfficiency> finalTaskEfficiencies = new List<TaskEfficiency>();

        public double TotalPoints = 0;
        public double AchievedPoints = 0;
        int taskId = 0;
        TaskRepo taskRepo = new TaskRepo();
        public ucFrmEfficiencyPoints()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdEfficiencyPoints.ItemsSource = taskEfficiencies;
            
            var efficiencyPoints = taskRepo.GetAllEfficiencyPoints();
            lookupEfficiencyPointsGrid.ItemsSource = efficiencyPoints;
            
            LoadonReceiptDeductions();
        }


        public void LoadonReceiptDeductions()
        {
            try
            {
                if (taskId != 0)
                {
                    var task = taskRepo.GetTask(taskId);

                       
                        foreach (var _efficiency in task.TaskEfficiencies)
                        {
                            grdEfficiencyPoints.Columns["efficiencyPoints"].Header = "Efficiency Points";
                            lblHeading.Text = "- Add Efficiency Points -";
                            taskEfficiencies.Add(new TaskEfficiency()
                            {
                                Id = _efficiency.Id,
                                efficiencyPoints_Id = _efficiency.efficiencyPoints_Id,
                                TotalPoints = _efficiency.TotalPoints,
                                efficiencyPoints = _efficiency.efficiencyPoints,
                                AchievedPoints = _efficiency.AchievedPoints
                            });
                        }

                    grdEfficiencyPoints.ItemsSource = taskEfficiencies;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }

        public ucFrmEfficiencyPoints(int _taskId)
        {
            InitializeComponent();
            taskId = _taskId;
        }

        private void btnAddEfficiencyPoints_Click(object sender, RoutedEventArgs e)
        {

        }

        private void View_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var _efficiency in grdEfficiencyPoints.ItemsSource as List<TaskEfficiency>)
            {
                finalTaskEfficiencies.Add(new TaskEfficiency()
                {
                    Id = _efficiency.Id,
                    efficiencyPoints_Id = _efficiency.efficiencyPoints.Id,
                    TotalPoints = _efficiency.efficiencyPoints.Points,
                    AchievedPoints = _efficiency.AchievedPoints,
                    tasksId = taskId
                });
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ViewEfficiencyPoints_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var row = e.Row as TaskEfficiency;
            if (row.efficiencyPoints.Points < 0)
            {
                if(row.AchievedPoints > 0)
                {
                    DXMessageBox.Show("Achieved Points cannot be positive for this Row!");
                    row.AchievedPoints = 0;
                    return;
                }
                if(row.AchievedPoints < row.efficiencyPoints.Points)
                {
                    DXMessageBox.Show("Achieved Points cannot exceed the Total Points!");
                    row.AchievedPoints = 0;
                    return;
                }
            }
            else
            {
                if (row.AchievedPoints > row.efficiencyPoints.Points)
                {
                    DXMessageBox.Show("Achieved Points cannot exceed the Total Points!");
                    row.AchievedPoints = 0;
                    return;
                }
            }
        }
    }
}
