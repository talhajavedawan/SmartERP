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
    /// Interaction logic for ucAddEfficiencyPointsHead.xaml
    /// </summary>
    public partial class ucAddEfficiencyPointsHead : UserControl
    {
        TaskRepo taskRepo = new TaskRepo();
        EfficiencyPoints efficiencyPoints = new EfficiencyPoints();
        public int efficiencyPointId = 0;
        public bool editFlag = false;
        public ucAddEfficiencyPointsHead()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(editFlag == true && efficiencyPointId != 0)
            {
                efficiencyPoints = taskRepo.GetEfficiencyPointsById(efficiencyPointId);
                txtEfficiencyPointTitle.Text = efficiencyPoints.title;
                chkEdtIsActive.IsChecked = efficiencyPoints.isActive;
                txtTotalPoints.Text = efficiencyPoints.Points.ToString();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty( txtEfficiencyPointTitle.Text))
            {
                DXMessageBox.Show("Please Enter Title!");
                txtEfficiencyPointTitle.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtTotalPoints.Text))
            {
                DXMessageBox.Show("Please Enter Points!");
                txtTotalPoints.Focus();
                return;
            }

            efficiencyPoints.title = txtEfficiencyPointTitle.Text;
            efficiencyPoints.isActive = chkEdtIsActive.IsChecked.Value;
            efficiencyPoints.Points = Convert.ToDouble(txtTotalPoints.Text);

            if (editFlag == true)
            {
                taskRepo.UpdateEfficiencyPoints(efficiencyPoints);
                DXMessageBox.Show("Updated Successfully!");
            }
            else
            {
                taskRepo.AddEfficiencyPoints(efficiencyPoints);
                DXMessageBox.Show("Added Successfully!");
            }
            var myWin = Window.GetWindow(this);
            myWin.Close();
        }
    }
}
