using DevExpress.Xpf.Core;
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
    /// Interaction logic for ucFrmTaskTracking.xaml
    /// </summary>
    public partial class ucFrmTaskTracking : UserControl
    {
        public ucFrmTaskTracking()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtDetails.Text))
            {
                DXMessageBox.Show("Please Enter Details!");
                txtDetails.Focus();
                return;
            }
            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void TxtDetails_TextChanged(object sender, TextChangedEventArgs e)
        {
            var count = txtDetails.Text.Length;
            txtCounter.Text = count+"/200";
            if (count == 200)
                txtCounter.Foreground = Brushes.Red;
            else
                txtCounter.Foreground = Brushes.Green;
        }

        private void TxtDetails_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
    }
}
