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
    /// Interaction logic for frmTargetAdd.xaml
    /// </summary>
    public partial class frmTargetAdd : Window
    {
        DepartmentRepo repo = new DepartmentRepo();
        List<Target> targets = new List<Target>();
        Target target = new Target();
        
        public static int targetId;

        public frmTargetAdd()
        {
            InitializeComponent();
            
        }
        public void loadUnitOfMeasures()
        {



            List<TargetType> targetTypes= new List<TargetType>();
            targetTypes = repo.getActiveTargetTypes();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var unit in targetTypes)
            {

                cmbitems.Add(new cmbitem() { name = unit.Type, id = unit.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });


            cmbTargetType.ItemsSource = cmbitems;

        }
       
        private void btnSaveTarget_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lookupCompany.SelectedItem == null)
                {
                    DXMessageBox.Show("Select Company First");
                    lookupCompany.Focus();
                    return;
                }

                else if (lookupDepartment.SelectedItem== null)
                {
                    DXMessageBox.Show("Select Department First");
                    lookupDepartment.Focus();
                    return;
                }
                else if (cmbTargetType.SelectedIndex == -1)
                {
                    DXMessageBox.Show("Select target type to Continue");

                    cmbTargetType.Focus();
                    return;

                }
                else if (cmbCurrency.SelectedIndex== -1)
                {
                    DXMessageBox.Show("Select Currency First!");

                    cmbCurrency.Focus();
                    return;

                }
                else if (datYear.Text== null||datYear.Text.Length!=4)
                {
                    MessageBox.Show("Enter Valid target Year");
                    datYear.Focus();
                    return;

                }
                List<TargetAward> awards = new List<TargetAward>();
                awards = grdItems.ItemsSource as List<TargetAward>;
                TargetAward totalAward = awards.Find(x => x.name == "Total");
                if (totalAward != null)
                {
                    totalAward.Month = 14;
                    totalAward.TotalAward = awards.Sum(x => x.TotalAward);
                    totalAward.IndviualAward = awards.Sum(x => x.IndviualAward);
                    totalAward.NoOfEmployees = awards.Max(x => x.NoOfEmployees);
                    totalAward.target = awards.Sum(x => x.target);
                }
                else
                {
                    totalAward = new TargetAward() { name="Total", Month=14,TotalAward=awards.Sum(x=> x.TotalAward), IndviualAward = awards.Sum(x => x.IndviualAward), NoOfEmployees= awards.Max(x => x.NoOfEmployees), target = awards.Sum(x => x.target) };
                    
                };
                awards.Add(totalAward);
                target.TargetAwards = awards;
                target.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;
                target.departmentId = (lookupDepartment.SelectedItem as Department).Id;
                target.companyId = (lookupCompany.SelectedItem as Company).Id;

                target.Year = Convert.ToInt32( datYear.Text);
                target.typeId =(cmbTargetType.SelectedItem as cmbitem).id;
                target.isActive = (chkIsActive.IsChecked == true) ? true : false;

                //if (targetId == 0 )
                {
                    
                    
                    
                    
                    if (MainWindow.currentUserid != 0)
                        target.user_Id = MainWindow.currentUserid;
                    
                    if (target.Id == 0)
                    {
                        target.AddedDate = System.DateTime.Now;
                        //target.AchivedDate= System.DateTime.Now;
                        //target.EditDate = System.DateTime.Now;

                        repo.AddTarget(target);

                        DXMessageBox.Show("Target for year "+target.Year+  " is Added Succesfully!");
                        this.Close();

                    }
                    else
                    {
                        target.EditDate = System.DateTime.Now;

                        repo.UpdateTarget(target);

                        DXMessageBox.Show("Target for year "+target.Year+ " Updated Succesfully!");
                        this.Close();

                    }

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void winItemAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            targetId = 0;
        }
        
        private void winItemAdd_Loaded(object sender, RoutedEventArgs e)
        {
            cmbCurrency.ItemsSource= SYSTEM_STATIC.loadCurrencies();
            lookupDepartment.ItemsSource = SYSTEM_STATIC.LoadCurrentUserDepartments();
            lookupCompany.ItemsSource = SYSTEM_STATIC.LoadCurrentUserCompanies();
            cmbTargetType.ItemsSource = SYSTEM_STATIC.loadTargetTypes();
            grdItems.ItemsSource = SYSTEM_STATIC.TargetListforMonths();
            loaditeminfo();
        }

        public void loaditeminfo()
        {
            if (targetId != 0)
            {
                target = repo.getTarget(targetId);
                List<TargetAward> awards = target.TargetAwards;
                if (awards != null&& awards.Count!=0)
                {
                    awards.Remove(awards.Find(x => x.name == "Total"));
                }
                else
                {
                    awards = SYSTEM_STATIC.TargetListforMonths();
                }
                grdItems.ItemsSource = awards;
                datYear.Text = target.Year.ToString();
                if (target.Currency != null)
                {
                    var cmbsource = (List<cmbitem>)cmbCurrency.Items.SourceCollection;
                    cmbCurrency.SelectedItem = cmbCurrency.Items[cmbCurrency.Items.IndexOf(cmbsource.Find(x => x.id == target.currencyId))];
                }
                if (target.Type != null)
                {
                    var cmbsource = (List<cmbitem>)cmbTargetType.Items.SourceCollection;
                    cmbTargetType.SelectedItem = cmbTargetType.Items[cmbTargetType.Items.IndexOf(cmbsource.Find(x => x.id == target.typeId))];
                }
                if (target.Department != null ||target.departmentId!=null)
                {
                    lookupDepartment.Text = target.Department.DeptName;

                }
                if (target.Company != null || target.companyId!= null)
                {
                    lookupCompany.Text = target.Company.CompanyName;

                }
                chkIsActive.IsChecked = target.isActive;
                
            }
            return;
        }

        private void view_RowUpdated(object sender, RowEventArgs e)
        {

        }

        // private void TxtExtra_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        //{
        //     txtTotal.Text = "0";
        //     txtTotal.Text = ((string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDouble(txtTotal.Text)) + (string.IsNullOrEmpty(txtJanuary.Text) ? 0 : Convert.ToDouble(txtJanuary.Text))).ToString();

        //     txtTotal.Text = ((string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDouble(txtTotal.Text)) + (string.IsNullOrEmpty(txtFeburary.Text) ? 0 : Convert.ToDouble(txtFeburary.Text))).ToString();
        //     txtTotal.Text = ((string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDouble(txtTotal.Text)) + (string.IsNullOrEmpty(txtMarch.Text) ? 0 : Convert.ToDouble(txtMarch.Text))).ToString();
        //     txtTotal.Text = ((string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDouble(txtTotal.Text)) + (string.IsNullOrEmpty(txtApril.Text) ? 0 : Convert.ToDouble(txtApril.Text))).ToString();
        //     txtTotal.Text = ((string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDouble(txtTotal.Text)) + (string.IsNullOrEmpty(txtMay.Text) ? 0 : Convert.ToDouble(txtMay.Text))).ToString();
        //     txtTotal.Text = ((string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDouble(txtTotal.Text)) + (string.IsNullOrEmpty(txtJune.Text) ? 0 : Convert.ToDouble(txtJune.Text))).ToString();
        //     txtTotal.Text = ((string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDouble(txtTotal.Text)) + (string.IsNullOrEmpty(txtJuly.Text) ? 0 : Convert.ToDouble(txtJuly.Text))).ToString();
        //     txtTotal.Text = ((string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDouble(txtTotal.Text)) + (string.IsNullOrEmpty(txtAugust.Text) ? 0 : Convert.ToDouble(txtAugust.Text))).ToString();
        //     txtTotal.Text = ((string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDouble(txtTotal.Text)) + (string.IsNullOrEmpty(txtSeptember.Text) ? 0 : Convert.ToDouble(txtSeptember.Text))).ToString();
        //     txtTotal.Text = ((string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDouble(txtTotal.Text)) + (string.IsNullOrEmpty(txtOctober.Text) ? 0 : Convert.ToDouble(txtOctober.Text))).ToString();

        //     txtTotal.Text = ((string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDouble(txtTotal.Text)) + (string.IsNullOrEmpty(txtNovember.Text) ? 0 : Convert.ToDouble(txtNovember.Text))).ToString();
        //     txtTotal.Text = ((string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDouble(txtTotal.Text)) + (string.IsNullOrEmpty(txtDecember.Text) ? 0 : Convert.ToDouble(txtDecember.Text))).ToString();

        //     txtTotal.Text = ((string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDouble(txtTotal.Text)) + (string.IsNullOrEmpty(txtExtra.Text) ? 0 : Convert.ToDouble(txtExtra.Text))).ToString();



        // }
    }
}
