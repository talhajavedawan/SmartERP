using DevExpress.Xpf.Core;
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

namespace ZAS_ERP.Memos
{
    /// <summary>
    /// Interaction logic for ucSelectMemoType.xaml
    /// </summary>
    public partial class ucSelectMemoType : UserControl
    {
        public ucSelectMemoType()
        {
            InitializeComponent();
        }

        private void BtnContinue_Click(object sender, RoutedEventArgs e)
        {
            if (cmbxMemoType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Memo Type!");
                cmbxMemoType.Focus();
                return;
            }

            if (cmbxMemoType.SelectedIndex == 0)
            {
                    Window enterPaymentWin = new Window();
                    ucFrmMemoAdd frmMemoAdd = new ucFrmMemoAdd();
                    frmMemoAdd.editFlag = false;
                    frmMemoAdd.memoId = 0;
                    frmMemoAdd.memoType = ERP_BL.Enums.MemoType.Linked;
                    enterPaymentWin.Content = frmMemoAdd;
                    enterPaymentWin.Width = 400;
                    enterPaymentWin.Height = 500;
                    enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    enterPaymentWin.Show();

                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
              
            }
            else if (cmbxMemoType.SelectedIndex == 1)
            {
                
                    Window enterPaymentWin = new Window();
                    ucFrmMemoAdd frmMemoAdd = new ucFrmMemoAdd();
                    frmMemoAdd.editFlag = false;
                    frmMemoAdd.memoId = 0;
                    frmMemoAdd.memoType = ERP_BL.Enums.MemoType.Non_Linked;
                    enterPaymentWin.Content = frmMemoAdd;
                    enterPaymentWin.Width = 400;
                    enterPaymentWin.Height = 500;
                    enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    enterPaymentWin.Show();

                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
               

            }
            else if (cmbxMemoType.SelectedIndex == 2)
            {
                    Window enterPaymentWin = new Window();
                    ucFrmMemoAdd frmMemoAdd = new ucFrmMemoAdd();
                    frmMemoAdd.editFlag = false;
                    frmMemoAdd.memoId = 0;
                    frmMemoAdd.memoType = ERP_BL.Enums.MemoType.Group;
                    enterPaymentWin.Content = frmMemoAdd;
                    enterPaymentWin.Width = 400;
                    enterPaymentWin.Height = 500;
                    enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    enterPaymentWin.Show();

                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
              
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.MemoType.Group; i++)
            {
                cmbxMemoType.Items.Add(((ERP_BL.Enums.MemoType)i).ToString());
            }
        }

    }
}
