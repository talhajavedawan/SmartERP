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

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucContactPerson.xaml
    /// </summary>
    public partial class ucContactPerson : UserControl
    {
        public ucContactPerson()
        {
            InitializeComponent();
        }

        private void Add_New_Cntct_Person_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Window win = new Window();
            ucFrmAddContactPerson obj = new ucFrmAddContactPerson();

            win.Content = obj;
            win.Title = "Add Contact Person";
            win.Height = 300;
            win.Show();
        }
    }
    public class Customer2
        {
            public string BankName { get; set; }
            public string Location { get; set; }
            public string PersonName { get; set; }
            public string Designation { get; set; }
            public string PhoneNo { get; set; }
            public string MobileNo { get; set; }
            public string Email { get; set; }
        }

        public class MainWindowViewModel2
        {
            public List<Customer2> Customers { get; private set; }
            //public event PropertyChangedEventHandler PropertyChanged;
            public MainWindowViewModel2()
            {
                List<Customer2> people2 = new List<Customer2>();



                people2.Add(new Customer2() { BankName = "Askari", Location = "Hong Kong", PersonName = "XXX", Designation = "Manager", PhoneNo = "0300938499", MobileNo = "0300938499", Email = "example@gmail.com" });
                people2.Add(new Customer2() { BankName = "Askari", Location = "Madrid", PersonName = "XXX", Designation = "Manager", PhoneNo = "0300938499", MobileNo = "0300938499", Email = "example@gmail.com" });
                people2.Add(new Customer2() { BankName = "Askari", Location = "Los Angeles", PersonName = "XXX", Designation = "Manager", PhoneNo = "0300938499", MobileNo = "0300938499", Email = "example@gmail.com" });
                people2.Add(new Customer2() { BankName = "Askari", Location = "London", PersonName = "XXX", Designation = "Manager", PhoneNo = "0300938499", MobileNo = "0300938499", Email = "example@gmail.com" });
                people2.Add(new Customer2() { BankName = "Askari", Location = "Hong Kong", PersonName = "XXX", Designation = "Manager", PhoneNo = "0300938499", MobileNo = "0300938499", Email = "example@gmail.com" });
                people2.Add(new Customer2() { BankName = "Askari", Location = "Los Angeles", PersonName = "XXX", Designation = "Manager", PhoneNo = "0300938499", MobileNo = "0300938499", Email = "example@gmail.com" });

                Customers = people2;
            }



        }
    }
