using ERP_BL.Procurements.Budget;
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

namespace ZAS_ERP.Procurementss.Budget.UserControls
{
    /// <summary>
    /// Interaction logic for ucStatusChange.xaml
    /// </summary>
    public partial class ucStatusChange : UserControl
    {
        public static int inActiveStatuses;
        public static int budgetid;
        public int internalBudgetid;
        public static BudgetCostSheet sheet = new BudgetCostSheet();
        static BudgetCostCenterRepo budgetCostSheetRepo { get; set; }
        public ucStatusChange(BudgetCostCenterRepo _budgetCostSheetRepo)
        {
            InitializeComponent();
            internalBudgetid = budgetid;
            sheet = new BudgetCostSheet();
            budgetCostSheetRepo = _budgetCostSheetRepo;
        }
        public ucStatusChange()
        {
            InitializeComponent();
            internalBudgetid = budgetid;
            sheet = new BudgetCostSheet();
            budgetCostSheetRepo = new BudgetCostCenterRepo();
        }
        public static void UpdateSaleOrder()
        {
            budgetCostSheetRepo = new BudgetCostCenterRepo();
            budgetCostSheetRepo.updateStatusById(sheet.Id, sheet.budgetCostSheetStatus.Id);
            budgetCostSheetRepo = null;
            //saleORepo.update(saleOrder);

            //Updatestatus();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            sheet = budgetCostSheetRepo.get(internalBudgetid);
            loadBudgetStatus();
            if (sheet != null && sheet.Id != 0)
            {
                txtStatus.Text = sheet.budgetCostSheetStatus.Status.ToString();

                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(sheet.budgetCostSheetStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                lblstatuscolor.Background = new SolidColorBrush(newColor);

                foreach (cmbitem cmbitem in cmbSaleOrderStatus.Items)
                {
                    if (cmbitem.name == sheet.budgetCostSheetStatus.Status)
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
                MessageBox.Show("Couldn't load Budget Data! Try again...!");
            }
        }
        private void cmbSaleOrderStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbSaleOrderStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbSaleOrderStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmBudgetStatusAdd statusAdd = new frmBudgetStatusAdd();
                    statusAdd.ShowDialog();
                    loadBudgetStatus();
                }



            }

        }
        internal static void UpdateSaleOrderStatusforInvoice()
        {
            BudgetCostCenterRepo repo = new BudgetCostCenterRepo();
            repo.updateStatusById(sheet.Id, sheet.budgetCostSheetStatus.Id);
        }
        public class cmbitem
        {
            public string name { get; set; }
            public int id { get; set; }
            public string bcolor { get; set; }
            public string fcolor { get; set; }
        }
        public void loadBudgetStatus()
        {

            List<BudgetCostSheetStatus> saleOrderStatuses = new List<BudgetCostSheetStatus>();
            if (inActiveStatuses == 0)
                saleOrderStatuses = budgetCostSheetRepo.getAllActiveBudgetCostStatus();
            else
                saleOrderStatuses = budgetCostSheetRepo.getAllActiveBudgetCostStatus();

            List<cmbitem> cmbitems = new List<cmbitem>();

            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });

            foreach (BudgetCostSheetStatus status in saleOrderStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }

            cmbSaleOrderStatus.ItemsSource = cmbitems;
        }
        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            if ((cmbSaleOrderStatus.SelectedItem as cmbitem) != null)
            {

                BudgetCostSheetStatus status = budgetCostSheetRepo.getstatus((cmbSaleOrderStatus.SelectedItem as cmbitem).id);
                sheet.budgetCostSheetStatus = new BudgetCostSheetStatus();
                sheet.budgetCostSheetStatus = status;
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else
            {

                MessageBox.Show("Please select New Status First");

            }
        }
        private void BtnStatusAvoid_Click(object sender, RoutedEventArgs e)
        {
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }
    }
}
