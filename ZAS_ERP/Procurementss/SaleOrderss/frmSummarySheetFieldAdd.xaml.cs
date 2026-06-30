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
using ERP_BL;
using ERP_BL.Databases;

namespace ZAS_ERP.Procurementss
{
    /// <summary>
    /// Interaction logic for frmSummarySheetFieldAdd.xaml
    /// </summary>
    public partial class frmSummarySheetFieldAdd : Window
    {
        public frmSummarySheetFieldAdd()
        {
            InitializeComponent();
        }
        public static int summarySheetFieldId;
        SaleOrderRepo repo = new SaleOrderRepo();
        SummarySheetField summarySheetField = new SummarySheetField();
        SummarySheetField parentCategory = new SummarySheetField();

        private void btnsummarySheetFieldSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtsummarySheetField.Text == "")
                {
                    MessageBox.Show("Please enter title", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtsummarySheetField.Focus();
                    return;
                }

                summarySheetField.Title = txtsummarySheetField.Text.Trim();
                
            if (chkisactive.IsChecked == true)
                summarySheetField.isActive = true;
            else
                summarySheetField.isActive = false;
            if (summarySheetField.Id == 0)
            {
                    if (MainWindow.currentUserid != 0)
                        summarySheetField.AddedbyUserId = MainWindow.currentUserid;
                    summarySheetField.Timestamp=System.DateTime.Now;
                    repo.AddSummarySheetField(summarySheetField);

                MessageBox.Show("New Field (" + txtsummarySheetField.Text + ") Added", "Congratulations");
            }
            else
            {
                repo.UpdateSummarySheetField(summarySheetField);

                MessageBox.Show(" (" + txtsummarySheetField.Text + ") updated", "Congratulations");
            }
            this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //public void loadCategoris()
        //{


        //    List<SummarySheetField> categories = new List<SummarySheetField>();
        //    categories = repo.getActiveProductCategories();
        //    lookupCategory.ItemsSource = categories;

        //}
        //private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void lookupCategory_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{
        //    parentCategory = lookupCategory.SelectedItem as SummarySheetField;
        //    if (parentCategory != null)
        //    {
        //        string selectedcust = parentCategory.category;
        //        lookupCategory.EditValue = selectedcust;


        //    }
        //}
        private void winSummarySheetFieldAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            summarySheetFieldId = 0;
        }

        private void winSummarySheetFieldAdd_Loaded(object sender, RoutedEventArgs e)
        {
            //loadCategoris();
            if (summarySheetFieldId != 0)
            {
                this.Title = "Edit Product Category";
                summarySheetField = repo.getSummarySheetField(summarySheetFieldId);
                txtsummarySheetField.Text = summarySheetField.Title;
                chkisactive.IsChecked = summarySheetField.isActive;
                //if (summarySheetField.parentCategory != null && summarySheetField.parentId != 0)
                //{
                //    parentCategory = summarySheetField.parentCategory;

                //    lookupCategory.SelectedItem = lookupCategory.GetItemByKeyValue(parentCategory);
                //}
            }
        }
    }
}
