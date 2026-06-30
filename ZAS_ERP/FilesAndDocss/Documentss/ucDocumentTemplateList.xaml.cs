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
    /// Interaction logic for ucDocumentTemplateList.xaml
    /// </summary>
    public partial class ucDocumentTemplateList : UserControl
    {
        DocumentRepo documentRepo = new DocumentRepo();
        public ucDocumentTemplateList()
        {
            InitializeComponent();
        }

        //comment
        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucFrmDocumentTemplateAdd ucFrmAssetRental = new ucFrmDocumentTemplateAdd();
            Window win = new Window();
            ucFrmAssetRental.editFlag = false;
            win.Content = ucFrmAssetRental;
            win.Width = 400;
            win.Height = 550;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void MbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlDocumentTemplateList.SelectedItem != null)
            {
                var selectedRow = grdCntrlDocumentTemplateList.SelectedItem as DocumentTemplate;
                ucFrmDocumentTemplateAdd ucFrmDocument = new ucFrmDocumentTemplateAdd();
                Window win = new Window();
                ucFrmDocument.documentTemplateId = selectedRow.Id;
                ucFrmDocument.editFlag = true;
                win.Content = ucFrmDocument;
                win.Width = 400;
                win.Height = 550;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
        }

        private void mbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            documentRepo = new DocumentRepo();
            var listt = documentRepo.GetAllDocumentTemplate();
            grdCntrlDocumentTemplateList.ItemsSource = listt;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            documentRepo = new DocumentRepo();
            var listt = documentRepo.GetAllDocumentTemplate();
            grdCntrlDocumentTemplateList.ItemsSource = listt;
        }

        private void grdCntrlDocumentTemplateList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var row = grdCntrlDocumentTemplateList.GetRowByListIndex(e.ListSourceRowIndex) as DocumentTemplate;
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
        }
    }
}
