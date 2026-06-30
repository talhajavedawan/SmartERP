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

namespace ZAS_ERP.Commentss
{
    /// <summary>
    /// Interaction logic for ucNotificationFlagAdd.xaml
    /// </summary>
    public partial class ucFrmNotificationFlagAdd : UserControl
    {
        public Window FrmNotificationFlagWin = new Window();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public NotificationFlag flag = new NotificationFlag();


        public bool saveEditFlag = false;
        public ucFrmNotificationFlagAdd()
        {
            InitializeComponent();
        }
       

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtFlag.Text) || String.IsNullOrWhiteSpace(txtFlag.Text))
                {
                    MessageBox.Show("Collection Method cannot be empty!");
                    return;
                }
                else
                {
                    flag.Flag = txtFlag.Text;
                    flag.backcolor = cpFlag.Text;

                    if (chkisActive.IsChecked == true)
                        flag.isActive = true;
                    else
                        flag.isActive = false;


                    if (chkCanGlow.IsChecked == true)
                        flag.canGlow = true;
                    else
                        flag.canGlow = false;

                    if (saveEditFlag == true && flag.Id != 0)
                    {
                        notificationsRepo.UpdateNotificationFlag(flag);
                        MessageBox.Show("Successfully Updated!");
                        FrmNotificationFlagWin.Close();
                    }
                    else if (saveEditFlag == false && flag.Id == 0)
                    {
                        notificationsRepo.AddNotificationFlag(flag);
                        MessageBox.Show("Successfully Added!");
                        FrmNotificationFlagWin.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void CpStatus_ColorChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
