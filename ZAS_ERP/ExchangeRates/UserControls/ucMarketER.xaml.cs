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
    /// Interaction logic for ucMarketER.xaml
    /// </summary>
    public partial class ucMarketER : DXRibbonWindow
    {
        ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
        public ucMarketER()
        {
            InitializeComponent();
        }

        private void DXRibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            grdMERExchangesGroups.ItemsSource = exchangeRateGroupRepo.GetAllMER();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdMERExchangesGroups);

        }
        private void GrdExchangesGroups_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Exchange Rates") != null)
            {
                var selectedGroup = grdMERExchangesGroups.SelectedItem as ExchangeRateGroup;
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
            var selectedGroup = grdMERExchangesGroups.SelectedItem as ExchangeRateGroup;

            if (selectedGroup != null)
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
            lblHeading.Text = "Void Market Exchange Rates";
            grdMERExchangesGroups.ItemsSource = exchangeRateGroupRepo.GetAllMERVoid();
        }
        private void BarRefreshRates_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exchangeRateGroupRepo = new ExchangeRateGroupRepo();
            lblHeading.Text = "Market Exchange Rates Register";
            grdMERExchangesGroups.ItemsSource = exchangeRateGroupRepo.GetAllMER();
            grdMERExchangesGroups.RefreshData();
        }

        private void BarEditRate_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Exchange Rates") != null)
            {
                var selectedGroup = grdMERExchangesGroups.SelectedItem as ExchangeRateGroup;
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

        private void barSaveLayout_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var input = DXMessageBox.Show("Are you sure? do you want to save layout?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (input == System.Windows.MessageBoxResult.Yes)
            {
                SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdMERExchangesGroups);
                DXMessageBox.Show("Layout has been saved.", "Congratulations", MessageBoxButton.YesNo, MessageBoxImage.Information);
            }
        }
    }
}
