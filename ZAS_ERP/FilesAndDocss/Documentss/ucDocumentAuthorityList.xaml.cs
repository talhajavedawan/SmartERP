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
    /// Interaction logic for ucDocumentAuthorityList.xaml
    /// </summary>
    public partial class ucDocumentAuthorityList : UserControl
    {
        DocumentRepo documentRepo = new DocumentRepo();
        public ucDocumentAuthorityList()
        {
            InitializeComponent();
        }


        //comment
        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucFrmDocumentAuthorityAdd ucFrmAssetRental = new ucFrmDocumentAuthorityAdd();
            Window win = new Window();
            ucFrmAssetRental.editFlag = false;
            win.Content = ucFrmAssetRental;
            win.Width = 400;
            win.Height = 250;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlDocumentTypeList.SelectedItem != null)
            {
                var selectedRow = grdCntrlDocumentTypeList.SelectedItem as DocumentType;
                ucFrmDocumentAuthorityAdd ucFrmAssetRental = new ucFrmDocumentAuthorityAdd();
                Window win = new Window();
                ucFrmAssetRental.documentAuthorityId = selectedRow.Id;
                ucFrmAssetRental.editFlag = true;
                win.Content = ucFrmAssetRental;
                win.Width = 400;
                win.Height = 250;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();
            }
        }

        private void mbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            documentRepo = new DocumentRepo();
            var listt = documentRepo.GetAllDocumentAuthority();
            grdCntrlDocumentTypeList.ItemsSource = listt;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            documentRepo = new DocumentRepo();
            var listt = documentRepo.GetAllDocumentAuthority();
            grdCntrlDocumentTypeList.ItemsSource = listt;
        }

    }
}
