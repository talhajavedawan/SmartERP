using DevExpress.Xpf.Core;
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
    /// Interaction logic for ucFrmTaskStatus.xaml
    /// </summary>
    public partial class ucFrmToDoTaskStatus : UserControl
    {
        ToDoTaskStatus status = new ToDoTaskStatus();
        public int statusId = 0;
        public bool editFlag = false;
        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        public ucFrmToDoTaskStatus()
        {
            InitializeComponent();
        }

        private void ColorEdit_ColorChanged(object sender, RoutedEventArgs e)
        {
            if (cpStatus.Color.R <= 120 || cpStatus.Color.G <= 120 || cpStatus.Color.B <= 120)
            {
                var myColor = "#FFFFFFFF";
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(myColor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                txtStatus.Foreground = new SolidColorBrush(newColor);
                status.forecolor = myColor;
            }
            else
            {
                var myColor = "#FF000000";
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(myColor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                txtStatus.Foreground = new SolidColorBrush(newColor);
                status.forecolor = myColor;
            }
            txtStatus.Background = new SolidColorBrush(cpStatus.Color);
        }

        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            status.Status = txtStatus.Text.Trim();
            if (chkisactive.IsChecked == true)
                status.isActive = true;
            else
                status.isActive = false;

            if (chkForTast.IsChecked == true)
                status.forTask = true;
            else
                status.forTask = false;

            if (chkForStep.IsChecked == true)
            {
                status.forStep = true;

                if(String.IsNullOrEmpty( txtMinPercentage.Text))
                {
                    DXMessageBox.Show("Please Enter Min Percentage!");
                    txtMinPercentage.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtMaxPercentage.Text) || Convert.ToDouble(txtMaxPercentage.Text) == 0)
                {
                    DXMessageBox.Show("Please Enter Max Percentage!");
                    txtMaxPercentage.Focus();
                    return;
                }
                var minPercent = Convert.ToDouble(txtMinPercentage.Text);
                var maxPercent = Convert.ToDouble(txtMaxPercentage.Text);

                if(minPercent > maxPercent)
                {
                    DXMessageBox.Show("Min %age cannot exceed Max %age!");
                    txtMinPercentage.Focus();
                    return;
                }

                int[] percentages = Enumerable.Range(Convert.ToInt32( minPercent), Convert.ToInt32((maxPercent- minPercent) + 1)).ToArray();

                if ( taskRepo.CheckStatusPercentage(percentages, status.Id) == true)
                {
                    DXMessageBox.Show("The percentage is incorrect!");
                    return;
                }

                status.MinPercentage = Convert.ToDouble(txtMinPercentage.Text);
                status.MaxPercentage = Convert.ToDouble(txtMaxPercentage.Text);
            }

            else
            {
                status.forStep = false;
            }


            status.backcolor = cpStatus.Text.Trim();

            if (editFlag == false)
            {
                taskRepo.AddTaskStatus(status);
                MessageBox.Show("New Status " + txtStatus.Text + " Added");
            }
            else if (editFlag == true)
            {
                taskRepo.UpdateTaskStatus(status);
                MessageBox.Show("Status Updated " + txtStatus.Text + " Added");
            }

            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true && statusId != 0)
            {
                status = taskRepo.GetTaskStatus(statusId);
                if (status.Status != null)
                {
                    txtStatus.Text = status.Status;
                }

                if (!String.IsNullOrEmpty( status.backcolor))
                {
                    object color = ColorConverter.ConvertFromString(status.backcolor);
                    cpStatus.Color = (Color)color;
                }

                if (status.isActive == true)
                    chkisactive.IsChecked = true;
                else
                    chkisactive.IsChecked = false;

                if (status.forTask == true)
                    chkForTast.IsChecked = true;
                else
                    chkForTast.IsChecked = false;

                if (status.forStep == true)
                    chkForStep.IsChecked = true;
                else
                    chkForStep.IsChecked = false;

                if(status.MinPercentage != null)
                {
                    txtMinPercentage.Text = status.MinPercentage.ToString();
                }
                if (status.MaxPercentage != null)
                {
                    txtMaxPercentage.Text = status.MaxPercentage.ToString();
                }
            }
        }

        private void ChkForStep_Checked(object sender, RoutedEventArgs e)
        {
            lblMinPercentage.Visibility = Visibility.Visible;
            grdPercentage.Visibility = Visibility.Visible;
        }

        private void ChkForStep_Unchecked(object sender, RoutedEventArgs e)
        {
            lblMinPercentage.Visibility = Visibility.Collapsed;
            grdPercentage.Visibility = Visibility.Collapsed;
        }
    }
}