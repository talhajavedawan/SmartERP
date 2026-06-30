using DevExpress.Xpf.Core;
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
using System.Windows.Shapes;

namespace ZAS_ERP.Companiess.DepartmentLevel.Windows
{
    /// <summary>
    /// Interaction logic for winfrmDepartmentLevel.xaml
    /// </summary>
    public partial class winfrmDepartmentLevel : DXWindow
    {
        ERP_BL.Databases.DepartmentLevel level = new ERP_BL.Databases.DepartmentLevel();
        DepartmentRepo departmentRepo = new DepartmentRepo();
        public winfrmDepartmentLevel()
        {
            InitializeComponent();
        }
        public winfrmDepartmentLevel(ERP_BL.Databases.DepartmentLevel _level)
        {
            InitializeComponent();
            level = _level;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (level.Id != 0)
            {
                level.Title = txtLevel.Text;
                if (chkIsParent.IsChecked == true)
                {
                    if (lookupParentLevel.SelectedIndex > -1)
                    {
                        level.ParentID = (lookupParentLevel.SelectedItem as ERP_BL.Databases.DepartmentLevel).Id;
                    }
                }
                departmentRepo.UpdateDepartmentLevel(level);
                DXMessageBox.Show("Level updated Successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();


            }
            else
            {
                level.Title = txtLevel.Text;
                if(chkIsParent.IsChecked==true)
                {
                    if(lookupParentLevel.SelectedIndex>-1)
                    {
                        level.ParentID = (lookupParentLevel.SelectedItem as ERP_BL.Databases.DepartmentLevel).Id;
                    }
                }
                else
                {
                    level.ParentID = null;
                }
                departmentRepo.AddDepartmentLevel(level);
                DXMessageBox.Show("Level Added Successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadParentLevels();
            if(level.Id!=0)
            {
                txtLevel.Text = level.Title;
                if(level.ParentID!=null)
                {
                    chkIsParent.IsChecked = true;
                    lookupParentLevel.Text = level.parentLevel.Title;
                }
            }
        }
        public void loadParentLevels()
        {
            lookupParentLevel.ItemsSource= departmentRepo.getAllDepartmentLevels();
        }

        private void chkIsParent_Checked(object sender, RoutedEventArgs e)
        {
            grdParentLevels.Visibility = Visibility.Visible;
        }

        private void chkIsParent_Unchecked(object sender, RoutedEventArgs e)
        {
            grdParentLevels.Visibility = Visibility.Collapsed;
        }
    }
}
