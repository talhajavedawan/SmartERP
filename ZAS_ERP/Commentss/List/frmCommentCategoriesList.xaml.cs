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
using ERP_BL;

using ERP_BL.Config;
using ERP_BL.Enums;

namespace ZAS_ERP.Commentss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmCategoriesList : Window
    {


        List<CommentCategory> commentCategories = new List<CommentCategory>();
        ProcurementRepo repo = new ProcurementRepo();
       
        public frmCategoriesList()
        {
            InitializeComponent();
            
            
        }

  
        public void loadCategories()
        {
            if(MainWindow.currentUserid==0)
            commentCategories = repo.getallCommentCategory();
            else
                commentCategories = repo.getActiveCommentCategories();

            this.grdCategories.ItemsSource = commentCategories;
            //grdCategories.Columns.GetColumnByFieldName("Id").Visible = false;
            //grdCategories.Columns.GetColumnByFieldName("parentId").Visible = false;

            grdCategories.Columns.GetColumnByFieldName("user_Id").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("user").Visible = false;

            //grdCategories.Columns.GetColumnByFieldName("parentCategory").Visible = false;

        }

     
        public void newCategory()
        {
            Commentss.frmCommentCategoryAdd commentCategoryAdd = new Commentss.frmCommentCategoryAdd();
            commentCategoryAdd.ShowDialog();
            loadCategories();
        }
        public void newItem()
        {
            frmCommentCategoryAdd frmItemadd = new frmCommentCategoryAdd();
            frmItemadd.ShowDialog();
            loadCategories();
            //loadItemgrid();
        }
        private void mbtnNewCategory_Click(object sender, RoutedEventArgs e)
        {
            newCategory();

        }

        private void mbtnEditCategory_Click(object sender, RoutedEventArgs e)
        {
            frmCommentCategoryAdd.commentCategoryId = (grdCategories.SelectedItem as CommentCategory).Id;
            newCategory();
        }

        private void mbtnNewItem_Click(object sender, RoutedEventArgs e)
        {
            newItem();

        }

        //private void mbtnEditItem_Click(object sender, RoutedEventArgs e)
        //{
        //    if (grdcomments.SelectedItem != null)
        //    {

        //        Commentss.frmItemadd.commentId = (grdcomments.SelectedItem as Comment).Id;
        //        newItem();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select an Item to Edit");
        //    }
        //}

        //private void grdCategories_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        //{
        //    if (grdCategories.SelectedItem != null)
        //        /*grdcomments.ItemsSource = (grdCategories.SelectedItem as CommentCategory).comments;*/
        //        loadItemgrid();
        //}

        private void grdCategories_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            frmCommentCategoryAdd.commentCategoryId = (grdCategories.SelectedItem as CommentCategory).Id;
            newCategory();
        }

        private void winCategoriesList_Loaded(object sender, RoutedEventArgs e)
        {
            loadCategories();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCategories);
        }

        private void WinCategoriesList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCategories);

        }

        //private void grdcomments_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    Commentss.frmItemadd.commentId = (grdcomments.SelectedItem as Comment).Id;
        //    newItem();
        //}
        //int empid;



    }
}
