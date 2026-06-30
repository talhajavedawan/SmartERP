using DevExpress.Xpf.Core;
using DevExpress.Xpf.Ribbon;
using ERP_BL.ExchangeRates;
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

namespace ZAS_ERP.ExchangeRates.UserControls
{
    /// <summary>
    /// Interaction logic for ucExchangeRegister.xaml
    /// </summary>
    public partial class ucExchangeRegister : DXRibbonWindow
    {
        ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
        public ucExchangeRegister()
        {
            InitializeComponent();
        }

        private void BarRefreshRates_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            lblHeading.Text = "Exchange Rates Register";
            grdExchangesGroups.ItemsSource = exchangeRateGroupRepo.GetAll();
        }

        private void BarEditRate_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Exchange Rates") != null)
            {
                var selectedGroup = grdExchangesGroups.SelectedItem as ExchangeRateGroup;
                if (selectedGroup != null)
                {
                    ucExchangeRateAdd ucExchangeRateAdd = new ucExchangeRateAdd();
                    ucExchangeRateAdd.editFlag = true;
                    ucExchangeRateAdd.groupId = selectedGroup.Id;
                    ucExchangeRateAdd.ShowDialog();
                }
            }
            else
            {
                DXMessageBox.Show("Permission Required to Edit Exchange Rates", "Permission Denied!");
            }
        }

        private void BarNewRate_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ucExchangeRateAdd ucExchangeRateAdd = new ucExchangeRateAdd();
            ucExchangeRateAdd.ShowDialog();
        }

        private void DXRibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            grdExchangesGroups.ItemsSource= exchangeRateGroupRepo.GetAll();
        }

        private void GrdExchangesGroups_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Exchange Rates") != null)
            {
                var selectedGroup = grdExchangesGroups.SelectedItem as ExchangeRateGroup;
                if (selectedGroup != null)
                {
                    ucExchangeRateAdd ucExchangeRateAdd = new ucExchangeRateAdd();
                    ucExchangeRateAdd.editFlag = true;
                    ucExchangeRateAdd.groupId = selectedGroup.Id;
                    ucExchangeRateAdd.ShowDialog();
                }
            }
            else
            {
                DXMessageBox.Show("Permission Required to Edit Exchange Rates", "Permission Denied!");
            }
        }

        private void BarVoid_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var selectedGroup = grdExchangesGroups.SelectedItem as ExchangeRateGroup;

            if(selectedGroup!=null)
            {
                ExchangeRateGroup group = new ExchangeRateGroup();
                group = exchangeRateGroupRepo.Get(selectedGroup.Id);
                if (group.isVoid == true)
                {
                    var info = DXMessageBox.Show("Exchange Rate is in void list do you want to un-void? ", "Congratulations", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (info == MessageBoxResult.Yes)
                    {
                        group.isVoid = false;
                        exchangeRateGroupRepo.Update(group);
                        DXMessageBox.Show("Exchange Rate Updated Successfully ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                        return;

                }
                else
                {
                    var info = DXMessageBox.Show("Are you sure? Do you want to void this Exchange Rate? ", "Congratulations", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (info == MessageBoxResult.Yes)
                    {
                        group.isVoid = true;
                        exchangeRateGroupRepo.Update(group);
                        DXMessageBox.Show("Exchange Rate Updated Successfully ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                        return;
                }
            }

        }

        private void BarVoidRates_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            lblHeading.Text = "Void Exchange Rates";
            grdExchangesGroups.ItemsSource = exchangeRateGroupRepo.GetAllVoid();
        }
    }
}
