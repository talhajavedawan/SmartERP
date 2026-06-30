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

namespace ZAS_ERP.BackgroundUpload
{
    /// <summary>
    /// Interaction logic for ucFrmTemplate.xaml
    /// </summary>
    public partial class ucFrmTemplate : UserControl
    {
        public ucFrmTemplate()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.TaskGroupTemplate.Optional; i++)
            {
                cmbxTemplates.Items.Add(((ERP_BL.Enums.TaskGroupTemplate)i).ToString());
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbxTemplates.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Module!");
                cmbxTemplates.Focus();
                return;
            }

            if (cmbxTemplates.SelectedIndex == 0)
            {
                DXWindow win = new DXWindow();
                ucFrmGroupListItem itemTitle = new ucFrmGroupListItem();
               // itemTitle.template = ERP_BL.Enums.TaskGroupTemplate.Standard;
                win.Title = "New Group";

                win.Content = itemTitle;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //win.Height = 450;
                //win.Width = 250;
                win.ShowDialog();

                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else if (cmbxTemplates.SelectedIndex == 1)
            {
                DXWindow win = new DXWindow();
                ucFrmGroupListItem itemTitle = new ucFrmGroupListItem();
                //itemTitle.template = ERP_BL.Enums.TaskGroupTemplate.Optional;
                win.Title = "New Group";

                win.Content = itemTitle;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //win.Height = 450;
                //win.Width = 250;
                win.ShowDialog();

                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
        }
    }
}
