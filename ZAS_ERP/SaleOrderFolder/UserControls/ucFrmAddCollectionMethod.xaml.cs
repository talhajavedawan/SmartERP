using ERP_BL.Databases;
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
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmAddCollectionMethod.xaml
    /// </summary>
    ///
    public partial class ucFrmAddCollectionMethod : UserControl
    {
        public UcListWindow addCollectionMethodWin = new UcListWindow();
        public UcListWindow updateCollectionMethodWin = new UcListWindow();


        public int flagEditAdd;
        public int Id;

        SalesReceiptRepo repo = new SalesReceiptRepo();
        bool flag = false;
        public ucFrmAddCollectionMethod()
        {
            InitializeComponent();
            addCollectionMethodWin.Closing += AddCollectionMethod_Window_Closing;
            updateCollectionMethodWin.Closing += UpdateCollectionMethod_Window_Closing;
        }

        private void ChkEdtIsActive_Checked(object sender, RoutedEventArgs e)
        {
            flag = true;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (flagEditAdd == 0)
                {
                    if (String.IsNullOrEmpty(txtCollectionMethod.Text) || String.IsNullOrWhiteSpace(txtCollectionMethod.Text))
                    {
                        MessageBox.Show("Collection Method cannot be empty..");
                    }
                    else
                    {
                        CollectionMethod method = new CollectionMethod();
                        method.MethodName = txtCollectionMethod.Text;
                        method.isActive = flag;

                        repo.addCollectionMethod(method);

                        MessageBox.Show("Collection Method added successfully.");
                        GetAllCollectionMethods collectMthd = new GetAllCollectionMethods();
                        ucCollectionMethodList obj = new ucCollectionMethodList();
                        obj.grdCntrlCollectionMethodList.ItemsSource = collectMthd.CollectionMethodList;
                        addCollectionMethodWin.Close();
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(txtCollectionMethod.Text) || String.IsNullOrWhiteSpace(txtCollectionMethod.Text))
                    {
                        MessageBox.Show("Collection Method cannot be empty..");
                    }
                    else
                    {
                        CollectionMethod method = new CollectionMethod();
                        method.Id = Id;
                        method.MethodName = txtCollectionMethod.Text;
                        method.isActive = flag;

                        repo.updateCollectionMethod(method);

                        MessageBox.Show("Collection Method updated successfully.");
                        GetAllCollectionMethods collectMthd = new GetAllCollectionMethods();
                        ucCollectionMethodList obj = new ucCollectionMethodList();
                        obj.grdCntrlCollectionMethodList.ItemsSource = collectMthd.CollectionMethodList;
                        updateCollectionMethodWin.Close();

                    }
                }

            }
            catch
            {

            }
            
            
        }

        private void AddCollectionMethod_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //change the event to avoid close form
            e.Cancel = false;
        }

        private void UpdateCollectionMethod_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //change the event to avoid close form
            e.Cancel = false;
        }

        private void ChkEdtIsActive_Unchecked(object sender, RoutedEventArgs e)
        {
            flag = false;

        }
    }
}
