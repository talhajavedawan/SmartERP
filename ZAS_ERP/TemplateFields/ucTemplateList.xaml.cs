using ERP_BL.Fields;
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

namespace ZAS_ERP.TemplateFields
{
    /// <summary>
    /// Interaction logic for ucTemplateList.xaml
    /// </summary>
    public partial class ucTemplateList : UserControl
    {
        FieldsRepo fieldsRepo = new FieldsRepo();
        public ucTemplateList()
        {
            InitializeComponent();
        }

        private void MbtnAddTemplate_Click(object sender, RoutedEventArgs e)
        {
            ucFrmAddTemplate addTemplate = new ucFrmAddTemplate();

            addTemplate.addTemplateWindow.Content = addTemplate;
            addTemplate.addTemplateWindow.Height = 320;
            addTemplate.addTemplateWindow.Width = 350;
            addTemplate.addTemplateWindow.ResizeMode = ResizeMode.CanMinimize;
            addTemplate.addTemplateWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            addTemplate.addTemplateWindow.ShowDialog();
        }

        private void MbtnEditTemplate_Click(object sender, RoutedEventArgs e)
        {
            ucFrmAddTemplate addTemplate = new ucFrmAddTemplate();
            var selectedRow = grdCntrlTemplateList.SelectedItem as Template;

            addTemplate.template = fieldsRepo.GetTemplate(selectedRow.Id);
            addTemplate.editFlag = true;
            addTemplate.addTemplateWindow.Content = addTemplate;
            addTemplate.addTemplateWindow.Height = 320;
            addTemplate.addTemplateWindow.Width = 350;
            addTemplate.addTemplateWindow.ResizeMode = ResizeMode.CanMinimize;
            addTemplate.addTemplateWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            addTemplate.addTemplateWindow.ShowDialog();
        }

        private void OpenTemplateForm()
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            fieldsRepo = new FieldsRepo();
            grdCntrlTemplateList.ItemsSource = fieldsRepo.GetAllTemplates();
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            fieldsRepo = new FieldsRepo();
            grdCntrlTemplateList.ItemsSource = fieldsRepo.GetAllTemplates();
        }
    }
}
