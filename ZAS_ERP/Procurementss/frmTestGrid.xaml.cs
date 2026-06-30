using DevExpress.Xpf.Grid;
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
using System.Windows.Shapes;

namespace ZAS_ERP.Procurementss
{
    /// <summary>
    /// Interaction logic for frmTestGrid.xaml
    /// </summary>
    public partial class frmTestGrid : Window
    {
        ProductRepo productrepo = new ProductRepo();
        public static InquiryProduct product = new InquiryProduct();
        List<Product> products = new List<Product>();
        List<InquiryProduct> inquiryProducts = new List<InquiryProduct>();
        //static InquiryProduct _inquiryProduct;
        List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>( );
        ProcurementProduct procurementProduct =new  ProcurementProduct();
        public frmTestGrid()
        {
            
            InitializeComponent();
            //griditem.ItemsSource = productrepo.getAllInquiryProducts();
            products = productrepo.getAll();
            griditem.ItemsSource = procurementProducts;
            lookupProductinGrid.ItemsSource = products;

            grdInquiryItems.ItemsSource = inquiryProducts;
            lookupProductsinGrid.ItemsSource = products;
        }

        private void PART_GridControl_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {

        }
        public List<InquiryProduct> getformdata()
        {
            List<InquiryProduct> inqueryItems = new List<InquiryProduct>();
            List<InquiryProduct> inqItems = new List<InquiryProduct>();
            InquiryProduct product = new InquiryProduct();
            inqueryItems = grdInquiryItems.ItemsSource as List<InquiryProduct>;
            //inqueryItems = grdInquiryItems.ItemsSource as List<InquiryProduct>;
            foreach (var inquiryProduct in inqueryItems)
            {
                product = inquiryProduct;
                product.product_Id = inquiryProduct.product.Id;
                product.UOM = inquiryProduct.product.unitOfMeasure.unitOfMeasure;
                inqItems.Add(product);

            }
                //    datagriditem item = dGitems.Items[i - 1] as datagriditem;
                //    {
                //        //if (item.inquiryitemId == 0) 
                //        //{ 
                //        //InquiryProduct inquiryit = new InquiryProduct();
                //        //    //Product product = new Product();
                //        //    inquiryit.Id = item.inquiryitemId;
                //        ////inquiryit.product = product;
                //        //inquiryit.product_Id = item.Id;
                //        ////inquiryit.product.Id = item.Id;
                //        ////inquiryit.product.item = item.Item_Name;
                //        ////inquiryit.product.itemDescription = item.Item_Discription;
                //        //inquiryit.UOM = item.UOM;
                //        //inquiryit.ownDiscription = item.Own_Description;
                //        //inquiryit.quantity = item.Quantity;
                //        //inqueryItems.Add(inquiryit);
                //        //}
                //        //else                     {
                //        //    InquiryProduct inquiryit = new InquiryProduct();
                //        //    //Product product = new Product();
                //        //    inquiryit.Id = item.inquiryitemId;
                //        //    //inquiryit.product = product;
                //        //    inquiryit.product_Id = item.Id;
                //        //    //inquiryit.product.Id = item.Id;
                //        //    //inquiryit.product.item = item.Item_Name;
                //        //    //inquiryit.product.itemDescription = item.Item_Discription;
                //        //    inquiryit.UOM = item.UOM;
                //        //    inquiryit.ownDiscription = item.Own_Description;
                //        //    inquiryit.quantity = item.Quantity;
                //        //    inqueryItems.Add(inquiryit);
                //        //}

                //        if (item.Id != 0)
                //        {
                //            InquiryProduct inquiryit = new InquiryProduct()
                //            {
                //                Id = item.inquiryitemId,

                //                product_Id = item.Id,
                //                ownDiscription = item.Own_Description,
                //                UOM = item.UOM,
                //                quantity = item.Quantity
                //            };
                //            inqueryItems.Add(inquiryit);
                //        }
                //        //else
                //        //{
                //        //    ProcurementProduct offerit = new ProcurementProduct()
                //        //    {
                //        //        Id = item.offeritemId,
                //        //        product_Id = item.inquiryitemId,
                //        //        value1 = item.value1,
                //        //        value2 = item.value2,
                //        //        caption1 = cmbcaption1.Text,
                //        //        caption2 = cmbcaption2.Text
                //        //    };
                //        //    offerItems.Add(offerit);
                //        //}

                //        //productrepo.Add(inqueryit);

                //    }

                //}

                return inqItems;
        }
        public List<ProcurementProduct> getformdata1()
        {
            List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
            List<ProcurementProduct> ProcItems = new List<ProcurementProduct>();
            ProcurementProduct product = new ProcurementProduct();
            procurementProducts = griditem.ItemsSource as List<ProcurementProduct>;
            foreach (var procurementProduct in procurementProducts)
            {
                product = procurementProduct;
                product.product_Id = procurementProduct.inquiryProduct.product.Id;
                product.inquiryProduct.UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure;
                ProcItems.Add(product);

            }
            //    datagriditem item = dGitems.Items[i - 1] as datagriditem;
            //    {
            //        //if (item.inquiryitemId == 0) 
            //        //{ 
            //        //InquiryProduct inquiryit = new InquiryProduct();
            //        //    //Product product = new Product();
            //        //    inquiryit.Id = item.inquiryitemId;
            //        ////inquiryit.product = product;
            //        //inquiryit.product_Id = item.Id;
            //        ////inquiryit.product.Id = item.Id;
            //        ////inquiryit.product.item = item.Item_Name;
            //        ////inquiryit.product.itemDescription = item.Item_Discription;
            //        //inquiryit.UOM = item.UOM;
            //        //inquiryit.ownDiscription = item.Own_Description;
            //        //inquiryit.quantity = item.Quantity;
            //        //procurementProducts.Add(inquiryit);
            //        //}
            //        //else                     {
            //        //    InquiryProduct inquiryit = new InquiryProduct();
            //        //    //Product product = new Product();
            //        //    inquiryit.Id = item.inquiryitemId;
            //        //    //inquiryit.product = product;
            //        //    inquiryit.product_Id = item.Id;
            //        //    //inquiryit.product.Id = item.Id;
            //        //    //inquiryit.product.item = item.Item_Name;
            //        //    //inquiryit.product.itemDescription = item.Item_Discription;
            //        //    inquiryit.UOM = item.UOM;
            //        //    inquiryit.ownDiscription = item.Own_Description;
            //        //    inquiryit.quantity = item.Quantity;
            //        //    procurementProducts.Add(inquiryit);
            //        //}

            //        if (item.Id != 0)
            //        {
            //            InquiryProduct inquiryit = new InquiryProduct()
            //            {
            //                Id = item.inquiryitemId,

            //                product_Id = item.Id,
            //                ownDiscription = item.Own_Description,
            //                UOM = item.UOM,
            //                quantity = item.Quantity
            //            };
            //            procurementProducts.Add(inquiryit);
            //        }
            //        //else
            //        //{
            //        //    ProcurementProduct offerit = new ProcurementProduct()
            //        //    {
            //        //        Id = item.offeritemId,
            //        //        product_Id = item.inquiryitemId,
            //        //        value1 = item.value1,
            //        //        value2 = item.value2,
            //        //        caption1 = cmbcaption1.Text,
            //        //        caption2 = cmbcaption2.Text
            //        //    };
            //        //    offerItems.Add(offerit);
            //        //}

            //        //productrepo.Add(inqueryit);

            //    }

            //}

            return ProcItems;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(getformdata().ToString());
            MessageBox.Show(getformdata1().ToString());

        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            Productss.frmItemadd frmItemadd = new Productss.frmItemadd();
            frmItemadd.ShowDialog();
            products = productrepo.getAll();
            lookupProductinGrid.ItemsSource = products;
            lookupProductsinGrid.ItemsSource = products;


        }

        private void deleteRowItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GridCellMenuInfo menuInfo = view.GridMenu.MenuInfo as GridCellMenuInfo;
            if (menuInfo != null && menuInfo.Row != null)
                view.DeleteRow(menuInfo.Row.RowHandle.Value);
        }
    }
}
