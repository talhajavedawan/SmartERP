using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using DevExpress.Xpf.Grid;
using ERP_BL;
using ERP_BL.Databases;
using ERP_BL.Enums;

namespace ZAS_ERP.Commentss
{
    /// <summary>
    /// Interaction logic for frmCommentCategoryAdd.xaml
    /// </summary>
    public partial class frmCommentCategoryAdd : Window
    {
        public frmCommentCategoryAdd()
        {
            InitializeComponent();
            griddeptview.NodeCheckStateChanged += OndeptgirdNodeCheckStateChanged;

            gridTransactionTypes.SelectionChanged += OndeptGridSelectionChanged;
        }
        public static int commentCategoryId;
        ProcurementRepo repo = new ProcurementRepo();
        CommentCategory commentCategory = new CommentCategory();

        private void btncommentCategorySave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtcommentCategory.Text == "")
                {
                    MessageBox.Show("Please enter category name", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtcommentCategory.Focus();
                    return;
                }
                List<ERP_BL.Enums.TransactionItemType> transactions= new List<ERP_BL.Enums.TransactionItemType>();
                commentCategory.TransactionTypes = new List<TransactionItem>();
                foreach (var trans in gridTransactionTypes.SelectedItems)
                {
                    var Enumtrans = SYSTEM_STATIC.ParseEnum<ERP_BL.Enums.TransactionItemType>(trans.ToString());
                    var transItem = new TransactionItem();
                    transItem.TransactionType = Enumtrans;
                    //transactions.Add(SystemLogic.ParseEnum<ERP_BL.Enums.TransactionItemType>(trans.ToString()));
                    commentCategory.TransactionTypes.Add(transItem );

                }
                //var transactions= gridTransactionTypes.SelectedItems as List<ERP_BL.Enums.TransactionItemType>;
                commentCategory.discription = txtDiscription.Text.Trim();
            commentCategory.category = txtcommentCategory.Text.Trim();
            if (chkisactive.IsChecked == true)
                commentCategory.isActive = true;
            else
                commentCategory.isActive = false;
            if (commentCategory.Id == 0)
            {
                    if (MainWindow.currentUserid != 0)
                        commentCategory.user_Id = MainWindow.currentUserid;
                    else
                        commentCategory.user_Id = null;
                    repo.AddCommentCategory(commentCategory);

                MessageBox.Show("New Comment Category (" + txtcommentCategory.Text + ") Added", "Congratulations");
            }
            else
            {
                repo.UpdateCommentCategory(commentCategory);

                MessageBox.Show("Comment Category (" + txtcommentCategory.Text + ") updated", "Congratulations");
            }
            this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void loadCategoris()
        {


            //List<CommentCategory> categories = new List<CommentCategory>();
            //categories = repo.getActiveCommentCategories();
            

        }

        private void loadgrid()
        {
            gridTransactionTypes.ItemsSource= Enum.GetNames(typeof(ERP_BL.Enums.TransactionItemType));
        }

        private void OndeptgirdNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                gridTransactionTypes.SelectItem(e.Node.RowHandle);
            else
                gridTransactionTypes.UnselectItem(e.Node.RowHandle);
        }
        private void OndeptGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = griddeptview;
            var node = view.GetNodeByRowHandle(e.ControllerRow);
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    if (node != null)
                        node.IsChecked = true;
                    break;
                case CollectionChangeAction.Remove:
                    if (node != null)
                        node.IsChecked = false;
                    break;
                case CollectionChangeAction.Refresh:
                    var selectedRows = gridTransactionTypes.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }
        }
        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        
        private void winCommentCategoryAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            commentCategoryId = 0;
        }

        private void winCommentCategoryAdd_Loaded(object sender, RoutedEventArgs e)
        {
            loadgrid();
            if (commentCategoryId != 0)
            {

                this.Title = "Edit Comment Category";
                commentCategory = repo.getCommentCategory(commentCategoryId);
                txtcommentCategory.Text = commentCategory.category;
                txtDiscription.Text = commentCategory.discription;
                chkisactive.IsChecked = commentCategory.isActive;
                foreach(var item in commentCategory.TransactionTypes)
                {
                    gridTransactionTypes.SelectItem(gridTransactionTypes.FindRow(item.TransactionType));
                }
                
            }
        }
    }
}
