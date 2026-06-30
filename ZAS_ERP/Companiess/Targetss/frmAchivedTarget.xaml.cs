using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.LookUp;
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
    public partial class frmAchivedTarget: Window
    {
        DepartmentRepo repo = new DepartmentRepo();
        List<Target> targets = new List<Target>();
        List<AchivedTarget> achivedTargets = new List<AchivedTarget>();
        Target target = new Target();
        MyViewModel vm;
        public static int targetId;

        public frmAchivedTarget()
        {
            InitializeComponent();
            vm = new MyViewModel();
            DataContext = vm;
        }
        

        private void winItemAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdOnlyAchivedTargets);
            targetId = 0;
        }
        
        private void winItemAdd_Loaded(object sender, RoutedEventArgs e)
        {
            //cmbCurrency.ItemsSource= SystemLogic.loadCurrencies();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdOnlyAchivedTargets);
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
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            grdOnlyAchivedTargets.ItemsSource = achivedTargetsList;
            grdOnlyAchivedTargets.ShowLoadingPanel = false;
        }
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            CreateList();
            System.Threading.Thread.Sleep(1000);
        }

        private void CreateList()
        {
            
            achivedTargetsList = SYSTEM_STATIC.GetAchivedTargetByUser(Year,fieldname );
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            

        }
        int Year;
        string fieldname;
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            
            //vm.IsWaitIndicatorVisible = !vm.IsWaitIndicatorVisible;
            if (string.IsNullOrEmpty(txtYear.Text)|| txtYear.Text.Length!=4)
            {
                MessageBox.Show("Enter valid target Year");
                txtYear.Focus();
                return;
            }

            else if (string.IsNullOrEmpty(cmbField.Text) || cmbField.SelectedItem == null)
            {
                MessageBox.Show("Select a field from list to calculate Achivements for mentioned Year");
                txtYear.Focus();
                return;
            }
            Year = Convert.ToInt32(txtYear.Text);
            fieldname = (cmbField.SelectedItem as cmbitem).description;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            grdOnlyAchivedTargets.ShowLoadingPanel = true;
            bw.RunWorkerAsync();

            
            //vm.IsWaitIndicatorVisible = !vm.IsWaitIndicatorVisible;



        }
        List<AchivedTarget> achivedTargetsList = new List<AchivedTarget>();

        private void GrdOnlyAchivedTargets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdOnlyAchivedTargets.SelectedItem != null)
            {
                if (grdOnlyAchivedTargets.SelectedItem == null)
                {
                    return;
                }
                var target = grdOnlyAchivedTargets.SelectedItem as AchivedTarget;
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
    public partial class MyViewModel : INotifyPropertyChanged
    {

        public MyViewModel()
        {
            WaitIndicatorText = "Loading...";
        }

        bool _isWaitIndicatorVisible;
        string _waitIndicatorText;

        public bool IsWaitIndicatorVisible
        {
            get
            {
                return _isWaitIndicatorVisible;
            }

            set
            {
                _isWaitIndicatorVisible = value;
                RaisePropertyChanged("IsWaitIndicatorVisible");
            }
        }

        public string WaitIndicatorText
        {
            get
            {
                return _waitIndicatorText;
            }

            set
            {
                _waitIndicatorText = value;
                RaisePropertyChanged("WaitIndicatorText");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
