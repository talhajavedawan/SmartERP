
using ERP_BL.Databases;
using ERP_BL.Procurements;
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

namespace ZAS_ERP.Procurementss.ModuleContract.UserControls
{
    /// <summary>
    /// Interaction logic for ucStatuschange.xaml
    /// </summary>
    public partial class ucStatuschange : UserControl
    {
        public static int ModuleContractid;
        public static ERP_BL.Procurements.ModuleContract ModuleContract = new ERP_BL.Procurements.ModuleContract();
        public static ModuleContractRepo ModuleContractRepo = new ModuleContractRepo();
        public ucStatuschange()
        {
            InitializeComponent();
        }
        private void cmbModuleContractStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbModuleContractStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbModuleContractStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmModuleContractStatusAdd statusAdd = new frmModuleContractStatusAdd();
                    statusAdd.ShowDialog();
                    loadModuleContractStatus();
                }



            }

        }
        public void loadModuleContractStatus()
        {

            List<ModuleContractStatus> ModuleContractStatuses = new List<ModuleContractStatus>();
            ModuleContractStatuses = ModuleContractRepo.getAllInactiveStatus();

            List<cmbitem> cmbitems = new List<cmbitem>();

            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });

            foreach (ModuleContractStatus status in ModuleContractStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }

            cmbModuleContractStatus.ItemsSource = cmbitems;
        }
        public class cmbitem
        {
            public string name { get; set; }
            public int id { get; set; }
            public string bcolor { get; set; }
            public string fcolor { get; set; }
        }

        private void ucinqstatuschange_Loaded(object sender, RoutedEventArgs e)
        {
            ModuleContract = ModuleContractRepo.get(ModuleContractid);
            loadModuleContractStatus();
            if (ModuleContract != null && ModuleContract.Id != 0)
            {
                txtStatus.Text = ModuleContract.ModuleContractStatus.Status.ToString();

                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(ModuleContract.ModuleContractStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                lblstatuscolor.Background = new SolidColorBrush(newColor);

                foreach (cmbitem cmbitem in cmbModuleContractStatus.Items)
                {
                    if (cmbitem.name == ModuleContract.ModuleContractStatus.Status)
                    {
                        cmbModuleContractStatus.SelectedItem = cmbitem;
                    }
                }
            }
            else
            {
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
                //((Panel)this.Parent).Children.Remove(this);
                MessageBox.Show("Couldn't load ModuleContract Data! Try again...!");
            }
        }

        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            if ((cmbModuleContractStatus.SelectedItem as cmbitem) != null)
            {

                ModuleContractStatus status = ModuleContractRepo.getstatus((cmbModuleContractStatus.SelectedItem as cmbitem).id);
                ModuleContract.ModuleContractStatus = status;
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
