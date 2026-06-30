using DevExpress.Xpf.Grid;
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
using ERP_BL;
using System.Security.Cryptography;

namespace ZAS_ERP.Userss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmUserssCenter : Window
    {
        EmployeeRepo emprepo = new EmployeeRepo();
        public frmUserssCenter()
        {
            InitializeComponent();
        }

        private void winusersscenter_Loaded(object sender, RoutedEventArgs e)
        {
            loademployeegrid();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdemployee);

        }
        private void loademployeegrid()
        {
            
            List<ERP_BL.Databases.Employee> employeelist = new List<ERP_BL.Databases.Employee>();
            employeelist = emprepo.GetActiveEmployees();
            this.grdemployee.ItemsSource = employeelist;
            grdemployee.Columns.GetColumnByFieldName("EmpId").Visible = false;
            grdemployee.Columns.GetColumnByFieldName("person").Visible = false;
            grdemployee.Columns.GetColumnByFieldName("address").Visible = false;
            grdemployee.Columns.GetColumnByFieldName("contact").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("Companies").Visible = false;
            grdemployee.Columns.GetColumnByFieldName("Desig").Visible = false;
            grdemployee.Columns.GetColumnByFieldName("Disability").Visible = false;
            grdemployee.Columns.GetColumnByFieldName("DisDescription").Visible = false;
            grdemployee.Columns.GetColumnByFieldName("MaritalStatus").Visible = false;
            grdemployee.Columns.GetColumnByFieldName("Status").Visible = false;
            grdemployee.Columns.GetColumnByFieldName("JoinDate").Visible = false;
            grdemployee.Columns.GetColumnByFieldName("BasicPay").Visible = false;
            grdemployee.Columns.Add(new GridColumn() { FieldName = "EmpId" });
            grdemployee.Columns.Add(new GridColumn() { FieldName = "person.FName" });
            grdemployee.Columns.GetColumnByFieldName("person.FName").Header = "First Name";
            grdemployee.Columns.Add(new GridColumn() { FieldName = "person.LName" });
            grdemployee.Columns.GetColumnByFieldName("person.LName").Header = "Last Name";
            
        }

        private void btnadduser_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (txtusername.Text != "" || txtpassword.Text != "")
                {
                    
                    if (chkisActive.IsChecked == true)
                        user.isActive = true;
                    else
                        user.isActive = false;
                    if (user.id == 0)
                    {


                        //DBContextERP cont = new DBContextERP();
                        // User user = new User();
                        user.employeeId = empid;
                        user.password = SYSTEM_STATIC.GenerateSHA512String(txtpassword.Text.Trim());
                        user.userName = txtusername.Text.Trim();

                        emprepo.Adduser(user);
                        //cont.Users.Add(user);
                        //cont.SaveChanges();
                        MessageBox.Show("User Created Succesfully!");

                    }
                    else
                    {

                        if (user.id == MainWindow.currentUserid|| MainWindow.currentUserid==0)
                        {
                            //if(user.password.Length<=15&& user.password !=txtpassword.Text)
                            //{
                            //    MessageBox.Show("Password Mismatch! ReEnter");
                            //    return;
                            //}

                            if ( user.password != SYSTEM_STATIC.GenerateSHA512String(txtpassword.Text))
                            {
                                MessageBox.Show("Password Mismatch! ReEnter");
                                return;

                            }
                            //SHA512 shaM = new SHA512Managed();
                            user.userName = txtusername.Text.Trim();
                            //user.password = shaM.ComputeHash(SystemLogic.ConvertToMemoryStream( txtnewpassword.Text.Trim())).ToString();
                            user.password = SYSTEM_STATIC.GenerateSHA512String(txtnewpassword.Text);
                            //MessageBox.Show(user.password);
                            //user.password = actualResult512;
                            emprepo.updateuser(user);
                            MessageBox.Show("User Updated Succesfully!");




                        }
                        else

                        {
                            MessageBox.Show("You are not authorized, to change this Users Password!");
                            return;

                        }
                        txtusername.Text = "";
                        txtpassword.Text = "";
                        txtnewpassword.Text = "";
                        user = new User();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            
        }
        int empid;
        User user = new User();

        public void showUsers()
        {
          
            
            user=emprepo.getemployeeuser(empid);

            
           if(user!=null&& user.id!=0)
              
                {
                    txtusername.Text = user.userName;
                    //txtpassword.Text = user.password;
               
                txtnewpassword.Visibility = Visibility.Visible;
                lblnewpassword.Visibility = Visibility.Visible;

                txtnewpassword.Text = "";
                chkisActive.IsChecked = user.isActive;
                btnadduser.Content = "Update User";
                //btnadduser.IsEnabled = false;
            }
            else
            {
                user = new User();
                txtusername.Text = "";
                txtpassword.Text = "";
                txtnewpassword.Text = "";
                
                txtnewpassword.Visibility = Visibility.Collapsed;
                lblnewpassword.Visibility = Visibility.Collapsed;

                btnadduser.Content = "Create User";
            }
           

          //  MessageBox.Show("nUserAccounts completed");

        }

        private void grdemployee_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            //txtpassword.Text = "";
            //txtusername.Text = "";
            //btnadduser.Content = "Add User";
            //showUsers();

            if (grdemployee.GetFocusedRowCellValue(grdemployee.Columns.GetColumnByFieldName("EmpId")) != null)
            {

                empid = (int)grdemployee.GetFocusedRowCellValue(grdemployee.Columns.GetColumnByFieldName("EmpId"));
                btnadduser.IsEnabled = true;
                showUsers();
                //MessageBox.Show(empid.ToString());
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            user = new User();
            txtusername.Text = "";
            txtpassword.Text = "";
            
            txtnewpassword.Visibility = Visibility.Collapsed;
            lblnewpassword.Visibility = Visibility.Collapsed;

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            //user = grdemployee.SelectedItem as User;
            user = emprepo.getemployeeuser(empid);
            if (user == null || user.id == 0)

            { MessageBox.Show("This Employee dose not have a User account.");
                return;
            }

                txtusername.Text = user.userName;
            
            txtpassword.Text = user.password;
            txtnewpassword.Visibility = Visibility.Visible;
            lblnewpassword.Visibility = Visibility.Visible;

            txtnewpassword.Text = "";
            chkisActive.IsChecked = user.isActive;
            btnadduser.Content = "Update User";
        }

        private void Winusersscenter_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdemployee);

        }
    }
}
