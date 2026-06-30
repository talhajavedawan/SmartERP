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

namespace ZAS_ERP.Termss
{
    /// <summary>
    /// Interaction logic for frmIncotermAdd.xaml
    /// </summary>
    public partial class frmIncotermAdd : Window
    {
        public frmIncotermAdd()
        {
            InitializeComponent();
        }
        public static int incotermId;
        IncotermRepo repo = new IncotermRepo();
        Incoterm incoterm = new Incoterm();
        private void btnIncotermSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            
            incoterm.term = txtIncoterm.Text.Trim();
            if (chkisactive.IsChecked == true)
                incoterm.isActive = true;
            else
                incoterm.isActive = false;
            if (incoterm.Id == 0)
            {

                    incoterm.user_Id = MainWindow.currentUserid;
                repo.Add(incoterm);

                MessageBox.Show("New Incoterm (" + txtIncoterm.Text + ") Added", "Congratulations");
            }
            else
            {
                repo.Update(incoterm);

                MessageBox.Show("Incoterm (" + txtIncoterm.Text + ") updated", "Congratulations");
            }
            this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void winIncotermAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            incotermId = 0;
        }

        private void winIncotermAdd_Loaded(object sender, RoutedEventArgs e)
        {
            if (incotermId != 0)
            {
                incoterm = repo.get(incotermId);
                txtIncoterm.Text = incoterm.term;
                chkisactive.IsChecked = incoterm.isActive;
            }
        }
    }
}
