using ERP_BL.Documents;
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

namespace ZAS_ERP.FilesAndDocss.Documentss
{
    /// <summary>
    /// Interaction logic for ucDocumentList.xaml
    /// </summary>
    public partial class ucDocumentTypeList : UserControl
    {
        DocumentRepo documentRepo = new DocumentRepo();
        public ucDocumentTypeList()
        {
            InitializeComponent();
        }

        //comment
        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucFrmDocumentTypeAdd ucFrmAssetRental = new ucFrmDocumentTypeAdd();
            Window win = new Window();
            ucFrmAssetRental.editFlag = false;
            win.Content = ucFrmAssetRental;
            win.Width = 400;
            win.Height = 480;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void MbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlDocumentTypeList.SelectedItem != null)
            {
                var selectedRow = grdCntrlDocumentTypeList.SelectedItem as DocumentType;
                ucFrmDocumentTypeAdd ucFrmAssetRental = new ucFrmDocumentTypeAdd();
                Window win = new Window();
                ucFrmAssetRental.documentTypeId = selectedRow.Id;
                ucFrmAssetRental.editFlag = true;
                win.Content = ucFrmAssetRental;
                win.Width = 400;
                win.Height = 480;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
        }

        private void mbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            documentRepo = new DocumentRepo();
            var listt = documentRepo.GetAllDocumentType();
            grdCntrlDocumentTypeList.ItemsSource = listt;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            documentRepo = new DocumentRepo();
            var listt = documentRepo.GetAllDocumentType();
            grdCntrlDocumentTypeList.ItemsSource = listt;
        }

        private void grdCntrlDocumentTypeList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var row = grdCntrlDocumentTypeList.GetRowByListIndex(e.ListSourceRowIndex) as DocumentType;
            if (e.Column.FieldName == "Companiess" && e.IsGetData)
            {
                if (row.companies != null && row.companies.Count > 0)
                    e.Value = String.Join(" | ", row.companies.Select(x => x.CompanyName));
            }
            if (e.Column.FieldName == "Departmentss" && e.IsGetData)
            {
                if (row.departments != null && row.departments.Count > 0)
                    e.Value = String.Join(" | ", row.departments.Select(x => x.DeptName));
            }
            if (e.Column.FieldName == "Templatess" && e.IsGetData)
            {
                if (row.DocumentTemplates != null && row.DocumentTemplates.Count > 0)
                    e.Value = String.Join(" | ", row.DocumentTemplates.Select(x => x.TemplateName));
            }
        }
    }
}
