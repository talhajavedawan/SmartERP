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

namespace ZAS_ERP.Termss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmIncoTermList : Window
    {


        List<Incoterm> incoTerms= new List<Incoterm>();
        IncotermRepo repo = new IncotermRepo();

        public frmIncoTermList()
        {
            InitializeComponent();
                       
        }

        private void winIncoTermList_Loaded(object sender, RoutedEventArgs e)
        {
            loadIncoTerm();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdIncoTerm);


        }



        private void loadIncoTerm()
        {
           
            if (MainWindow.currentUserid == 0)
                incoTerms = repo.getAll();
            else
                incoTerms = repo.getActiveIncoterm();
                       
            this.grdIncoTerm.ItemsSource = incoTerms;
            grdIncoTerm.Columns.GetColumnByFieldName("Id").Visible = false;
            grdIncoTerm.Columns.GetColumnByFieldName("user_Id").Visible = false;
            grdIncoTerm.Columns.GetColumnByFieldName("user").Visible = false;

        }

        public void newIncoTerm()
        {
            Termss.frmIncotermAdd frmIncoTermadd = new Termss.frmIncotermAdd();
            frmIncoTermadd.ShowDialog();
            loadIncoTerm();
        }
        

        private void mbtnNewIncoTerm_Click(object sender, RoutedEventArgs e)
        {
            newIncoTerm();

        }

        private void mbtnEditIncoTerm_Click(object sender, RoutedEventArgs e)
        {
            if (grdIncoTerm.SelectedItem != null)
            {
                Termss.frmIncotermAdd.incotermId = (grdIncoTerm.SelectedItem as Incoterm).Id;
                newIncoTerm();
            }
            else
            {
                MessageBox.Show("Please select a Incoterm to Edit");
            }
        }

        private void grdIncoTerm_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Termss.frmIncotermAdd.incotermId = (grdIncoTerm.SelectedItem as Incoterm).Id;
            newIncoTerm();
        }

        private void WinIncoTermList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdIncoTerm);
        }
    }
}
