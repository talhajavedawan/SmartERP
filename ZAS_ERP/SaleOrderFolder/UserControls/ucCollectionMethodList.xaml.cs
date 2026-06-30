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

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucCollectionMethodList.xaml
    /// </summary>
    public partial class ucCollectionMethodList : UserControl
    {
        ucFrmAddCollectionMethod addCollectionMethodObj = new ucFrmAddCollectionMethod();
        ucFrmAddCollectionMethod updateCollectionMethodObj = new ucFrmAddCollectionMethod();
        public ucCollectionMethodList()
        {
            InitializeComponent();
            try
            {
                GetAllCollectionMethods obj = new GetAllCollectionMethods();
                grdCntrlCollectionMethodList.ItemsSource = obj.CollectionMethodList;
                grdCntrlCollectionMethodList.Columns["ID"].Visible = false;
            }
            catch
            {

            }

            //Add New Collection Method menu Item permission
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Collection Method") != null)
            {
                mbtnAddMethod.IsEnabled = true;
            }
            else
            {
                mbtnAddMethod.IsEnabled = false;
            }

            //Update Existing Collection Method menu Item permission
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Update Existing Collection Method") != null)
            {
                mbtnUpdateMethod.IsEnabled = true;
            }
            else
            {
                mbtnUpdateMethod.IsEnabled = false;
            }

        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Load_collectionMethods()
        {
            try
            {
                GetAllCollectionMethods methods = new GetAllCollectionMethods();
                grdCntrlCollectionMethodList.ItemsSource = methods.CollectionMethodList;
                grdCntrlCollectionMethodList.Columns["ID"].Visible = false;
            }
            catch
            {

            }
            
        }

        private void Edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            
        }

        private void Add_New_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            
        }

        private void MbtnUpdateMethod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Update Existing Collection Method") != null)
                {
                    var selectedRow = (CollectionMethods)grdCntrlCollectionMethodList.SelectedItem;

                    if (selectedRow != null)
                    {

                        updateCollectionMethodObj = new ucFrmAddCollectionMethod();
                        updateCollectionMethodObj.flagEditAdd = 1;
                        updateCollectionMethodObj.Id = selectedRow.ID;
                        updateCollectionMethodObj.txtCollectionMethod.Text = selectedRow.CollectionMethod;
                        updateCollectionMethodObj.chkEdtIsActive.IsChecked = selectedRow.IsActive;

                        updateCollectionMethodObj.updateCollectionMethodWin.Height = 250;
                        updateCollectionMethodObj.updateCollectionMethodWin.Width = 550;
                        updateCollectionMethodObj.updateCollectionMethodWin.ResizeMode = ResizeMode.CanMinimize;
                        updateCollectionMethodObj.updateCollectionMethodWin.Content = updateCollectionMethodObj;
                        updateCollectionMethodObj.updateCollectionMethodWin.Title = "Update Collection Method";
                        updateCollectionMethodObj.updateCollectionMethodWin.ShowDialog();
                        Load_collectionMethods();
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to update existing Collection method!");
                    return;
                }
            }
            catch
            {

            }
        }

        private void MbtnAddMethod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Collection Method") != null)
                {
                    addCollectionMethodObj = new ucFrmAddCollectionMethod();
                    addCollectionMethodObj.flagEditAdd = 0;

                    addCollectionMethodObj.addCollectionMethodWin.Height = 250;
                    addCollectionMethodObj.addCollectionMethodWin.Width = 550;
                    addCollectionMethodObj.addCollectionMethodWin.ResizeMode = ResizeMode.CanMinimize;
                    addCollectionMethodObj.addCollectionMethodWin.Content = addCollectionMethodObj;
                    addCollectionMethodObj.addCollectionMethodWin.Title = "Add Collection Method";
                    addCollectionMethodObj.addCollectionMethodWin.ShowDialog();
                    Load_collectionMethods();
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add new Collection method!");
                    return;
                }
            }
            catch
            {

            }
        }
    }

    public class CollectionMethods
    {
        public int ID { get; set; }
        public string CollectionMethod { get; set; }
        public bool IsActive { get; set; }
    }

    public class GetAllCollectionMethods
    {
        public List<CollectionMethods> CollectionMethodList = new List<CollectionMethods>();
        List<CollectionMethods> methodList = new List<CollectionMethods>();

        public GetAllCollectionMethods()
        {
            try
            {
                SalesReceiptRepo repo = new SalesReceiptRepo();
                var allCollectionMethods = repo.GetAllCollectionMethods();
                if (allCollectionMethods.Count > 0)
                {
                    foreach (var _collectionMethod in allCollectionMethods)
                    {
                        CollectionMethods method = new CollectionMethods();

                        method.ID = _collectionMethod.Id;
                        method.CollectionMethod = _collectionMethod.MethodName;
                        method.IsActive = _collectionMethod.isActive;
                        //if(_collectionMethod.isActive == true)
                        //{
                        //    method.IsActive.IsChecked =true;
                        //}
                        //else
                        //{
                        //    method.IsActive.IsChecked = false;
                        //}

                        methodList.Add(method);
                    }
                    CollectionMethodList = methodList;
                }
            }
            catch
            {

            }
        }
    }
}