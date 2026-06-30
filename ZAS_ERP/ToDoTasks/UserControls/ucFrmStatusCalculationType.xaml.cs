using DevExpress.Xpf.Core;
using ERP_BL.Enums;
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
    /// Interaction logic for ucFrmStatusCalculationType.xaml
    /// </summary>
    public partial class ucFrmStatusCalculationType : UserControl
    {
        StatusCalculationType calculationType = new StatusCalculationType();
        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        public bool editFlag = false;
        public int typeId = 0;
        List<cmbitem> cmbitems = new List<cmbitem>();
        public ucFrmStatusCalculationType()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbxAchievedPoints.ItemsSource = SYSTEM_STATIC.GetSoFields();
            cmbxTotalPoints.ItemsSource = SYSTEM_STATIC.GetTargetFields();
            
            if (editFlag == true && typeId > 0)
            {
                calculationType = taskRepo.GetStatusCalculationType(typeId);
                txtCalculationType.Text = calculationType.TypeName;

                if (calculationType.AchievedField != null)
                {
                    var incoSourceChange = (List<cmbitem>)cmbxAchievedPoints.Items.SourceCollection;

                    cmbxAchievedPoints.SelectedItem = cmbxAchievedPoints.Items[cmbxAchievedPoints.Items.IndexOf(incoSourceChange.Find(x => x.name == calculationType.AchievedField.DisplayName))];

                }

                if (/*calculationType.POWarrantyId != 0 ||*/ calculationType.TotalField != null)
                {
                    var incoSourceChange = (List<cmbitem>)cmbxTotalPoints.Items.SourceCollection;

                    cmbxTotalPoints.SelectedItem = cmbxTotalPoints.Items[cmbxTotalPoints.Items.IndexOf(incoSourceChange.Find(x => x.name == calculationType.TotalField.DisplayName))];

                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
           
            if(String.IsNullOrEmpty( txtCalculationType.Text))
            {
                DXMessageBox.Show("Please enter Calculation Type");
                txtCalculationType.Focus();
                return;
            }
            if (cmbxAchievedPoints.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Achieved Points Property");
                cmbxAchievedPoints.Focus();
                return;
            }
            if (cmbxTotalPoints.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Total Points Property");
                cmbxTotalPoints.Focus();
                return;
            }
            
            calculationType.TypeName = txtCalculationType.Text;
            
            //taskRepo = new ToDoTaskRepo();
            if (editFlag == false)
            {
                calculationType.AchievedField = new SoCalculationFields();
                calculationType.TotalField = new SoCalculationFields();
                calculationType.AchievedField.DisplayName = (cmbxAchievedPoints.SelectedItem as cmbitem).name;
                calculationType.AchievedField.SOFieldName = (cmbxAchievedPoints.SelectedItem as cmbitem).description;

                calculationType.TotalField.DisplayName = (cmbxTotalPoints.SelectedItem as cmbitem).name;
                calculationType.TotalField.SOFieldName = (cmbxTotalPoints.SelectedItem as cmbitem).description;

                taskRepo.AddStatusCalculationType(calculationType);
                DXMessageBox.Show("Added Succesfully!");
            }

            else
            {
                calculationType.AchievedField.DisplayName = (cmbxAchievedPoints.SelectedItem as cmbitem).name;
                calculationType.AchievedField.SOFieldName = (cmbxAchievedPoints.SelectedItem as cmbitem).description;

                calculationType.TotalField.DisplayName = (cmbxTotalPoints.SelectedItem as cmbitem).name;
                calculationType.TotalField.SOFieldName = (cmbxTotalPoints.SelectedItem as cmbitem).description;

                taskRepo.UpdateStatusCalculationType(calculationType);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

    }
}
