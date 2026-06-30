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
using ERP_BL;
using ERP_BL.Databases;

namespace ZAS_ERP.Userss
{
    /// <summary>
    /// Interaction logic for frmChangePassword.xaml
    /// </summary>
    public partial class frmChangePassword : Window
    {
        public frmChangePassword()
        {
            InitializeComponent();
            SystemLog.LogInfo(this.GetType(), "Form Intialized");

        }
        public static int userId;
        UsersRepo repo = new UsersRepo();
        User user = new User();
        pUser pUser = new pUser();
        private void btnUserSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (MainWindow.currentUserid != 0 && SystemLogic.LoggedInByPowerUser == false)

                if (txtusername.Text == "")
                {
                    MessageBox.Show("UserName Not Loaded Correctly reopen Form");
                    return;
                }
                else if (txtoldpassword.Password == "")
                {
                    MessageBox.Show("Enter Old Password");

                    return;
                }
                else if (txtnewpassword.Password == "")
                {
                    MessageBox.Show("Enter New password.");
                    //SystemLog.LogInfo(this.GetType(), "Password Mismatched");
                    txtnewpassword.Focus();
                    return;
                }
                else if (txtConfirmpassword.Password == "")
                {
                    MessageBox.Show("Enter Confirmation password.");
                    //SystemLog.LogInfo(this.GetType(), "Password Mismatched");
                    txtConfirmpassword.Focus();
                    return;
                }
                else if (txtnewpassword.Password != txtConfirmpassword.Password)
                {
                    MessageBox.Show("New password and confirmation password dosen't match");
                    SystemLog.LogInfo(this.GetType(), "Password Mismatched");
                    return;
                }
                else if (txtusername.Text != "" && txtoldpassword.Password != "")
                {


                    if (user.id == MainWindow.currentUserid && MainWindow.currentUserid != 0 && SYSTEM_STATIC.LoggedInByPowerUser == false)
                    {
                        if (user.password != SYSTEM_STATIC.GenerateSHA512String(txtoldpassword.Password.Trim()))
                        {
                            MessageBox.Show("Old password does not match confirmation password dosen't match");
                            SystemLog.LogInfo(this.GetType(), "Password Mismatched");
                            return;
                        }


                        user.password = SYSTEM_STATIC.GenerateSHA512String(txtConfirmpassword.Password.Trim());
                        repo.updateuser(user);
                        MessageBox.Show("Password Changed Succesfully!");
                        SystemLog.LogInfo(this.GetType(), " User name= "+user.userName + " Changed his Password !");
                    }


                    if (MainWindow.currentUserid == 0 && SYSTEM_STATIC.LoggedInByPowerUser == true)
                    {

                        user.password = SYSTEM_STATIC.GenerateSHA512String(txtnewpassword.Text);
                        
                        bool saved=repo.UpdatePowerUserPassword(txtusername.Text, SYSTEM_STATIC.GenerateSHA512String(txtoldpassword.Password), SYSTEM_STATIC.GenerateSHA512String(txtConfirmpassword.Password));
                        if (saved)
                        {
                            MessageBox.Show("Super Users Password Updated Succesfully!");
                            SystemLog.LogInfo(this.GetType(), " Super User= " + txtusername.Text + "Changed his Password !");
                        }
                        else
                        {
                            MessageBox.Show("Super Users Password Updation Failed Password not Matched!");
                            SystemLog.LogInfo(this.GetType(), " Super User= " + txtusername.Text + "Changed his Password !");
                        }


                    }


                    this.Close();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), "Error while Changing Password !"+ex.ToString());

            }
        }

        private void winChangePassword_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            userId = 0;
            SystemLog.LogInfo(this.GetType(), " Window Closed");

        }

        private void winChangePassword_Loaded(object sender, RoutedEventArgs e)
        {
            txtusername.Text = MainWindow.currentUserName;
            if (MainWindow.currentUserid != 0)
            {
                user = repo.getuser(MainWindow.currentUserid);

            }
        }
    }
}
