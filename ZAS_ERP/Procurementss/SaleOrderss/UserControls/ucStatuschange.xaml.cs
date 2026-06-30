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

namespace ZAS_ERP.Procurementss.SaleOrderss
{
    /// <summary>
    /// Interaction logic for ucStatuschange.xaml
    /// </summary>
    public partial class ucStatuschange : UserControl
    {
        public static int inActiveStatuses;
        public static int saleOrderid;
        public  int internalSaleOrderid;
        public static SaleOrder saleOrder = new SaleOrder();
        static SaleOrderRepo saleOrderRepo { get; set; }
        List<ERP_BL.Procurements.StatusClass.StatusClass> StatusClasses = new List<ERP_BL.Procurements.StatusClass.StatusClass>();

        public ucStatuschange(SaleOrderRepo _saleOrderrepo)
        {
            InitializeComponent();
            internalSaleOrderid = saleOrderid;
            saleOrder = new SaleOrder();
            saleOrderRepo = _saleOrderrepo;
        }
        public ucStatuschange()
        {
            InitializeComponent();
            internalSaleOrderid = saleOrderid;
            saleOrder = new SaleOrder();
            saleOrderRepo = new SaleOrderRepo();
        }
        public static void UpdateSaleOrder()
        {
            saleOrderRepo = new SaleOrderRepo();
            saleOrderRepo.updateStatusById(saleOrder.Id,saleOrder.saleOrderStatus);
            saleOrderRepo = null;
            //saleORepo.update(saleOrder);

            //Updatestatus();
        }
        internal static void UpdateSaleOrderStatusforInvoice()
        {
            SaleOrderRepo repo = new SaleOrderRepo();
            repo.updateStatusById(saleOrder.Id, saleOrder.saleOrderStatus.Id);
        }
        public static void Updatestatus()
        {
            //SaleOrderRepo saleORepo = new SaleOrderRepo();
            //saleOrder.saleOrderStatus = new SaleOrderStatus();
            //saleOrderRepo.updateStatusById(saleOrder.Id, saleOrder.saleOrderStatus);
        }

        private void cmbSaleOrderStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbSaleOrderStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbSaleOrderStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmSaleOrderStatusAdd statusAdd = new frmSaleOrderStatusAdd();
                    statusAdd.ShowDialog();
                    loadSaleOrderStatus();
                }
                else
                {
                    loadSaleOrderStatusClass(idd);

                }



            }

        }
        public void loadSaleOrderStatusClass(int _statusId)
        {
            cmbSaleOrderStatusClass.ItemsSource = null;
            var status = saleOrderRepo.getstatus(_statusId);
            if (status.soStatusSubClasses.Count > 0)
            {
                StatusClasses.Clear();
                StatusClasses.AddRange(status.soStatusSubClasses.Where(x => x.isDisable != true).ToList());
                List<cmbitem> cmbitems = new List<cmbitem>();
                Parallel.ForEach(StatusClasses, delegate (ERP_BL.Procurements.StatusClass.StatusClass subClass)
                {
                    cmbitems.Add(new cmbitem() { name = subClass.ClassName, id = subClass.Id, bcolor = subClass.backcolor, fcolor = "#FF000000" });
                });
                cmbSaleOrderStatusClass.ItemsSource = cmbitems;
            }

        }
        public void loadSaleOrderStatus()
        {

        List<SaleOrderStatus> saleOrderStatuses = new List<SaleOrderStatus>();
            if(inActiveStatuses==0)
            saleOrderStatuses = saleOrderRepo.getAllActiveSaleOrderStatus();
            else
                saleOrderStatuses = saleOrderRepo.getAllInActiveSaleOrderStatus();

            List<cmbitem> cmbitems = new List<cmbitem>();

            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });

            foreach (SaleOrderStatus status in saleOrderStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }

            cmbSaleOrderStatus.ItemsSource = cmbitems;
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

            saleOrder = saleOrderRepo.get(internalSaleOrderid);
            loadSaleOrderStatus();
            if (saleOrder != null && saleOrder.Id!=0)
            {
                txtStatus.Text = saleOrder.saleOrderStatus.Status.ToString();

                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(saleOrder.saleOrderStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                lblstatuscolor.Background = new SolidColorBrush(newColor);

                foreach (cmbitem cmbitem in cmbSaleOrderStatus.Items)
                {
                    if (cmbitem.name == saleOrder.saleOrderStatus.Status)
                    {
                        cmbSaleOrderStatus.SelectedItem = cmbitem;
                    }
                }
            }
            else
            {
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
                //((Panel)this.Parent).Children.Remove(this);
                MessageBox.Show("Couldn't load SaleOrder Data! Try again...!");
            }
        }

        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSaleOrderStatusClass.SelectedIndex > -1)
            {

                if ((cmbSaleOrderStatus.SelectedItem as cmbitem) != null)
                {
                    SaleOrderStatus status = saleOrderRepo.getstatus((cmbSaleOrderStatus.SelectedItem as cmbitem).id);
                    saleOrder.saleOrderStatus = new SaleOrderStatus();
                    saleOrder.saleOrderStatus = status;
                    if (cmbSaleOrderStatusClass.SelectedIndex > -1)
                    {
                        ERP_BL.Procurements.StatusClass.StatusClass statusClass = saleOrderRepo.GetStatusClass((cmbSaleOrderStatusClass.SelectedItem as cmbitem).id);
                        saleOrder.StatusClass = new ERP_BL.Procurements.StatusClass.StatusClass();
                        saleOrder.StatusClass = statusClass;
                    }
                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    MessageBox.Show("Please select New Status First");
                }
            }
            else
            {
                MessageBox.Show("Please select status class first");
                return;
            }
        }

        private void BtnStatusAvoid_Click(object sender, RoutedEventArgs e)
        {
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void cmbSaleOrderStatusClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
