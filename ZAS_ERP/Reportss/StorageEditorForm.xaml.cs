using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZAS_ERP.Reportss
{
    /// <summary>
    /// Interaction logic for StorageEditorForm.xaml
    /// </summary>
    public partial class StorageEditorForm : Window
    {
        public static int GroupId;
        public StorageEditorForm()
        {
            InitializeComponent();
        }
        ReportGroup reportGroup = new ReportGroup();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void lookupGroup_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            reportGroup = lookupGroup.SelectedItem as ReportGroup;
            GroupId = reportGroup.Id;
            if (reportGroup != null)
            {
                string selectedcust = reportGroup.group;
                lookupGroup.EditValue = selectedcust;
                listBox1.ItemsSource = reportGroup.reports;

            }
        }

        private void btnAddGroup_Click(object sender, RoutedEventArgs e)
        {
            frmReportGroup frmReportGroup = new frmReportGroup();
            frmReportGroup.ShowDialog();
            ReportRepo reportRepo = new ReportRepo();
            lookupGroup.ItemsSource = reportRepo.GetALLReportGroups();
        }
    }
}
