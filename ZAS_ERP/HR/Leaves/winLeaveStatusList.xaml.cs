using ERP_BL.HR;
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

namespace ZAS_ERP.HR.Leaves
{
    /// <summary>
    /// Interaction logic for winLeaveStatusList.xaml
    /// </summary>
    public partial class winLeaveStatusList : Window
    {
        public winLeaveStatusList()
        {
            InitializeComponent();
        }
        public void loadLeaveStatus()
        {
            LeaveStatusListVM leaveStatusVM = new LeaveStatusListVM();

            grdLeaveStatus.ItemsSource = leaveStatusVM.leaveStatuses;
            grdLeaveStatus.Columns["Id"].Visible = false;
            grdLeaveStatus.Columns["isApproved"].Visible = false;
            grdLeaveStatus.Columns["HierarchicalIndex"].Visible = false;

        }

        public void newLeaveStatus()
        {
            frmLeaveStatusAdd newLeaveStatus = new frmLeaveStatusAdd();
            newLeaveStatus.isEdit = false;

            newLeaveStatus.ShowDialog();

            loadLeaveStatus();
        }
        private void WinLeaveStatus_Loaded(object sender, RoutedEventArgs e)
        {
            loadLeaveStatus();
        }

        private void WinLeaveStatus_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnNewLeaveStatus_Click(object sender, RoutedEventArgs e)
        {
            newLeaveStatus();
        }

        private void BtnNewLeaveStatus_Click(object sender, RoutedEventArgs e)
        {
            newLeaveStatus();

        }

        private void BtnEditLeaveStatus_Click(object sender, RoutedEventArgs e)
        {
            if (grdLeaveStatus.SelectedItem != null)
            {
                var item = (LeaveStatusList)grdLeaveStatus.SelectedItem;
                var itemId = item.Id.ToString();

                frmLeaveStatusAdd newLeaveStatus = new frmLeaveStatusAdd();
                newLeaveStatus.leaveStatusTxtbx.Text = item.StatusName;
                newLeaveStatus.leaveStatusIsActive.IsChecked = item.isActive;
                newLeaveStatus.leaveStatusColorEdit.Text = item.BgColor.ToString();
                newLeaveStatus.leaveStatusId.Text = itemId;
                newLeaveStatus.isEdit = true;
                
                newLeaveStatus.ShowDialog();

                loadLeaveStatus();

            }
        }
    }
    public class LeaveStatusList
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        public bool isApproved { get; set; }
        public bool isActive { get; set; }
        public Color BgColor { get; set; }
        public Color FgColor { get; set; }
        public int HierarchicalIndex { get; set; }
    }
    public class LeaveStatusListVM
    {
        HrRepo repo = new HrRepo();
        public List<LeaveStatusList> leaveStatuses = new List<LeaveStatusList>();
        public LeaveStatusListVM()
        {
            List<LeaveStatusList> _leaveStatuses = new List<LeaveStatusList>();
            var status = repo.GetAllLeaveStatus();
            foreach (var _status in status)
            {
                LeaveStatusList lst = new LeaveStatusList();
                if (!String.IsNullOrEmpty(_status.backcolor) )
                {
                    Color color = (Color)ColorConverter.ConvertFromString(_status.backcolor);

                    lst.BgColor = color;
                }
                if (_status.forecolor != null)
                {
                    Color color = (Color)ColorConverter.ConvertFromString(_status.forecolor);

                    lst.FgColor = color;
                }

                if (_status.HierarchicalIndex != 0)
                {
                    lst.HierarchicalIndex = _status.HierarchicalIndex;

                }

                if (_status.Id != 0)
                {
                    lst.Id = _status.Id;

                }

                lst.isActive = (bool)_status.isActive;
                lst.isApproved = (bool)_status.isApproved;

                if (_status.Status != null)
                {
                    lst.StatusName = _status.Status;
                }


                _leaveStatuses.Add(lst); ;

                //_assetStatuses.Add(_status);
            }
            leaveStatuses = _leaveStatuses;

        }
    }
}
