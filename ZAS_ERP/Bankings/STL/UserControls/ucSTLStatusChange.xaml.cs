using ERP_BL.Procurements.InterBankTransfers;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ZAS_ERP.Bankings.STL.Windows;

namespace ZAS_ERP.Bankings.STL.UserControls
{
    /// <summary>
    /// Interaction logic for ucSTLStatusChange.xaml
    /// </summary>
    public partial class ucSTLStatusChange : UserControl
    {
     
        public static int inActiveStatuses;
        public static int stlid;
        public int internalSTLid;
        public static ERP_BL.Procurements.InterBankTransfers.STL stl= new ERP_BL.Procurements.InterBankTransfers.STL();
        static STLRepo stlRepo { get; set; }
        public ucSTLStatusChange()
        {
            InitializeComponent();
            internalSTLid = stlid;
            stl = new ERP_BL.Procurements.InterBankTransfers.STL();
            stlRepo = null;
        }
        public ucSTLStatusChange(STLRepo _stlRepo)
        {
            InitializeComponent();
            internalSTLid = stlid;
            stl = new ERP_BL.Procurements.InterBankTransfers.STL();
            stlRepo = _stlRepo;
        }
       





        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            stl = stlRepo.Get(stlid);
            loadSTLStatus();
            if (stl != null && stl.Id != 0)
            {
                txtStatus.Text = stl.stlStatus.Status.ToString();

                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(stl.stlStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                lblstatuscolor.Background = new SolidColorBrush(newColor);

                foreach (cmbitem cmbitem in cmbSTLStatus.Items)
                {
                    if (cmbitem.name == stl.stlStatus.Status)
                    {
                        cmbSTLStatus.SelectedItem = cmbitem;
                    }
                }
            }
            else
            {
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
                //((Panel)this.Parent).Children.Remove(this);
                MessageBox.Show("Couldn't load STL Data! Try again...!");
            }

        }
        private void cmbSTLStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbSTLStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbSTLStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmSTLStatusAdd statusAdd = new frmSTLStatusAdd();
                    statusAdd.ShowDialog();
                    loadSTLStatus();
                }
            }
        }
        public void loadSTLStatus()
        {

            List<STLStatus> stlStatuses = new List<STLStatus>();
            if (inActiveStatuses == 0)
                stlStatuses = stlRepo.getAllActiveSTLStatus();
            else
                stlStatuses = stlRepo.getAllInActiveSTLStatus();

            List<cmbitem> cmbitems = new List<cmbitem>();

            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });

            foreach (STLStatus status in stlStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }

            cmbSTLStatus.ItemsSource = cmbitems;
        }
        private void BtnStatusAvoid_Click(object sender, RoutedEventArgs e)
        {
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }
        public static void UpdateSTL()
        {
            stlRepo = new STLRepo();
            stlRepo.updateStatusById(stl.Id, stl.stlStatus);
            stlRepo = null;
          
        }

        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            if ((cmbSTLStatus.SelectedItem as cmbitem) != null)
            {

                STLStatus status = stlRepo.getstatus((cmbSTLStatus.SelectedItem as cmbitem).id);
                stl.stlStatus = new STLStatus();
                stl.stlStatus = status;
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else
            {

                MessageBox.Show("Please select New Status First");

            }
        }
    }
}
