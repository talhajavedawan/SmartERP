using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ZAS_ERP.Targetss
{
    /// <summary>
    /// Interaction logic for frmTargetAchivedByDepartment.xaml
    /// </summary>
    public partial class frmTargetRegisterByDepartment : Window
    {
        DepartmentRepo repo = new DepartmentRepo();
        List<Target> targets = new List<Target>();
        List<AchivedTarget> achivedTargets = new List<AchivedTarget>();
        Target target = new Target();

        public static int targetId;

        public frmTargetRegisterByDepartment()
        {
            InitializeComponent();

        }
        //public void loadTargetTypes()
        //{

        //    var So =new  SaleOrder();

        //   var a= So.GetType().MakeArrayType();

        //    List<TargetType> targetTypes= new List<TargetType>();
        //    targetTypes = repo.getActiveTargetTypes();

        //    List<cmbitem> cmbitems = new List<cmbitem>();



        //    foreach (var unit in a.Attributes)
        //    {

        //        cmbitems.Add(new cmbitem() { name = unit.Type, id = unit.Id });


        //    }
        //    //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });


        //    cmbTargetType.ItemsSource = cmbitems;

        //}


        //private void btnSaveTarget_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (lookupDepartment.SelectedItem== null)
        //        {
        //            DXMessageBox.Show("Select Department First");
        //            lookupDepartment.Focus();
        //            return;
        //        }
        //        else if (cmbTargetType.SelectedIndex == -1)
        //        {
        //            DXMessageBox.Show("Select target type to Continue");

        //            cmbTargetType.Focus();
        //            return;

        //        }
        //        else if (cmbCurrency.SelectedIndex== -1)
        //        {
        //            DXMessageBox.Show("Select Currency First!");

        //            cmbCurrency.Focus();
        //            return;

        //        }
        //        else if (datYear.EditValue== null)
        //        {
        //            MessageBox.Show("Select target Year");
        //            datYear.Focus();
        //            return;

        //        }
        //        target.January = Convert.ToDouble(txtJanuary.Text.Trim());
        //        target.Feburary = Convert.ToDouble(txtFeburary.Text.Trim());
        //        target.March = Convert.ToDouble(txtMarch.Text.Trim());
        //        target.April = Convert.ToDouble(txtMay.Text.Trim());
        //        target.June = Convert.ToDouble(txtJune.Text.Trim());
        //        target.July = Convert.ToDouble(txtJuly.Text.Trim());
        //        target.August = Convert.ToDouble(txtAugust.Text.Trim());
        //        target.September = Convert.ToDouble(txtSeptember.Text.Trim());
        //        target.October = Convert.ToDouble(txtOctober.Text.Trim());
        //        target.November = Convert.ToDouble(txtNovember.Text.Trim());
        //        target.December = Convert.ToDouble(txtDecember.Text.Trim());
        //        target.Extra = Convert.ToDouble(txtExtra.Text.Trim());
        //        target.Total = Convert.ToDouble(txtTotal.Text.Trim());
        //        target.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;
        //        target.departmentId = (lookupDepartment.SelectedItem as Department).Id;
        //        target.Year = datYear.DateTime.Year;
        //        target.typeId =(cmbTargetType.SelectedItem as cmbitem).id;
        //        target.isActive = (chkIsActive.IsChecked == true) ? true : false;

        //        //if (targetId == 0 )
        //        {




        //            if (MainWindow.currentUserid != 0)
        //                target.user_Id = MainWindow.currentUserid;

        //            if (target.Id == 0)
        //            {
        //                repo.AddTarget(target);

        //                DXMessageBox.Show("Target for year "+target.Year+  " is Added Succesfully!");
        //                this.Close();

        //            }
        //            else
        //            {
        //                repo.UpdateTarget(target);

        //                DXMessageBox.Show("Target for year "+target.Year+ " Updated Succesfully!");
        //                this.Close();

        //            }

        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        private void winItemAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdActualAchivedTarget);
            targetId = 0;
        }

        private void winItemAdd_Loaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdActualAchivedTarget);
            //cmbCurrency.ItemsSource= SystemLogic.loadCurrencies();
            lookupDepartment.ItemsSource = SYSTEM_STATIC.LoadCurrentUserDepartments();
            lookupCompany.ItemsSource = SYSTEM_STATIC.LoadCurrentUserCompanies();
            cmbField.ItemsSource = SYSTEM_STATIC.GetSoFields();
            //cmbTargetType.ItemsSource = SystemLogic.loadTargetTypes();
            //loaditeminfo();
        }

        private void CmbField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void LoadAchivedTargetpercents()

        {


        }
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (lookupDepartment.SelectedItem == null)
            {
                DXMessageBox.Show("Select Department First");
                lookupDepartment.Focus();
                return;
            }

            else if (lookupCompany.SelectedIndex == -1)
            {
                DXMessageBox.Show("Select Currency First!");

                lookupCompany.Focus();
                return;

            }
            else if (string.IsNullOrEmpty(txtYear.Text) || txtYear.Text.Length != 4)
            {
                MessageBox.Show("Entera valid target Year");
                txtYear.Focus();
                return;
            }

            else if (string.IsNullOrEmpty(cmbField.Text) || cmbField.SelectedItem == null)
            {
                MessageBox.Show("Select a field from list to calculate Achivements for mentioned Year");
                txtYear.Focus();
                return;
            }
            int companyId = (lookupCompany.SelectedItem as Company).Id;

            int departmentId = (lookupDepartment.SelectedItem as Department).Id;
            int Year = Convert.ToInt32(txtYear.Text);
            grdActualAchivedTarget.ItemsSource = SYSTEM_STATIC.GetAchivedTarget(departmentId, companyId, Year, (cmbField.SelectedItem as cmbitem).description);
            return;

        }

        private void GrdActualAchivedTarget_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdActualAchivedTarget.SelectedItem != null)
            {
                //if (grdActualAchivedTarget.SelectedItem == null)
                //{
                //    return;
                //}
                var target = grdActualAchivedTarget.SelectedItem as AchivedTarget;
                Procurementss.SaleOrderss.frmTargetSaleRegisterGrid frmTargetSaleRegister = new Procurementss.SaleOrderss.frmTargetSaleRegisterGrid(target.Target.Company.Id, target.Target.Department.Id, target.year, target.Month, target.frequency);
                frmTargetSaleRegister.Show();
            }
        }

        //public void loaditeminfo()
        //{
        //    if (targetId != 0)
        //    {
        //        target = repo.getTarget(targetId);
        //        txtJanuary.Text = target.January.ToString();
        //        txtFeburary.Text = target.Feburary.ToString();
        //        txtMarch.Text = target.March.ToString();
        //        txtApril.Text = target.April.ToString();
        //        txtMay.Text = target.May.ToString();
        //        txtJune.Text = target.June.ToString();
        //        txtJuly.Text = target.July.ToString();
        //        txtAugust.Text = target.August.ToString();
        //        txtSeptember.Text = target.September.ToString();
        //        txtOctober.Text = target.October.ToString();
        //        txtNovember.Text = target.November.ToString();
        //        txtDecember.Text = target.December.ToString();
        //        txtExtra.Text = target.Extra.ToString();
        //        txtTotal.Text = target.Total.ToString();
        //        datYear.Text = target.Year.ToString();
        //        if (target.Currency != null)
        //        {
        //            var cmbsource = (List<cmbitem>)cmbCurrency.Items.SourceCollection;
        //            cmbCurrency.SelectedItem = cmbCurrency.Items[cmbCurrency.Items.IndexOf(cmbsource.Find(x => x.id == target.currencyId))];
        //        }
        //        if (target.Type != null)
        //        {
        //            var cmbsource = (List<cmbitem>)cmbTargetType.Items.SourceCollection;
        //            cmbTargetType.SelectedItem = cmbTargetType.Items[cmbTargetType.Items.IndexOf(cmbsource.Find(x => x.id == target.typeId))];
        //        }
        //        if (target.Department != null ||target.departmentId!=null)
        //        {
        //            lookupDepartment.Text = target.Department.DeptName;

        //        }
        //        chkIsActive.IsChecked = target.isActive;

        //    }
        //    return;
        //}
    }
}
