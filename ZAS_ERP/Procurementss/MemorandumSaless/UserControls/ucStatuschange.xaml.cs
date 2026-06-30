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

namespace ZAS_ERP.Procurementss.MemorandumSaless
{
    /// <summary>
    /// Interaction logic for ucStatuschange.xaml
    /// </summary>
    public partial class ucStatuschange : UserControl
    {
        public static int memorandumSaleid;
        public int internalSaleOrderid;
        public static MemorandumSale memorandumSale = new MemorandumSale();
        public static MemorandumSaleRepo memorandumSaleRepo { get; set; }
        public ucStatuschange(MemorandumSaleRepo _memorandumSaleRepo)
        {
            InitializeComponent();
            internalSaleOrderid = memorandumSaleid;
            memorandumSale = new MemorandumSale();
            memorandumSaleRepo = _memorandumSaleRepo;
        }
        public ucStatuschange()
        {
            InitializeComponent();
            internalSaleOrderid = memorandumSaleid;
            memorandumSale = new MemorandumSale();
            memorandumSaleRepo = new MemorandumSaleRepo();
        }
        private void cmbMemorandumSaleStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbMemorandumSaleStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbMemorandumSaleStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmMemorandumSaleStatusAdd statusAdd = new frmMemorandumSaleStatusAdd();
                    statusAdd.ShowDialog();
                    loadMemorandumSaleStatus();
                }



            }

        }
        public void loadMemorandumSaleStatus()
        {

            List<MemorandumSaleStatus> memorandumSaleStatuses = new List<MemorandumSaleStatus>();
            memorandumSaleStatuses = memorandumSaleRepo.getAllInActiveMemorandumSaleStatus();

            List<cmbitem> cmbitems = new List<cmbitem>();

            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });

            foreach (MemorandumSaleStatus status in memorandumSaleStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }

            cmbMemorandumSaleStatus.ItemsSource = cmbitems;
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
            memorandumSale = memorandumSaleRepo.get(memorandumSaleid);
            loadMemorandumSaleStatus();
            if (memorandumSale != null && memorandumSale.Id!=0)
            {
                if (memorandumSale.memorandumSaleStatus != null)
                {
                    txtStatus.Text = memorandumSale.memorandumSaleStatus.Status.ToString();

                    System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(memorandumSale.memorandumSaleStatus.backcolor);
                    System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                    lblstatuscolor.Background = new SolidColorBrush(newColor);

                    foreach (cmbitem cmbitem in cmbMemorandumSaleStatus.Items)
                    {
                        if (cmbitem.name == memorandumSale.memorandumSaleStatus.Status)
                        {
                            cmbMemorandumSaleStatus.SelectedItem = cmbitem;
                        }
                    }
                }
            }
            else
            {
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
                //((Panel)this.Parent).Children.Remove(this);
                MessageBox.Show("Couldn't load MemorandumSale Data! Try again...!");
            }
        }

        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            if ((cmbMemorandumSaleStatus.SelectedItem as cmbitem) != null)
            {

                MemorandumSaleStatus status = memorandumSaleRepo.getstatus((cmbMemorandumSaleStatus.SelectedItem as cmbitem).id);
                memorandumSale.memorandumSaleStatus = status;
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
