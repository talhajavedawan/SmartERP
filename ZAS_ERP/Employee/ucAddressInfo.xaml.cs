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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZAS_ERP.Employee
{
    /// <summary>
    /// Interaction logic for ucAddressInfo.xaml
    /// </summary>
    public partial class ucAddressInfo : UserControl
    {
        public ucAddressInfo()
        {
            InitializeComponent();

        }

        private void SaveGriddata_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmployeeRepo repo = new EmployeeRepo();
                var gridItems = grdEmployeeRegisters.VisibleItems;
                foreach (var _item in gridItems)
                {
                    ERP_BL.Databases.Employee emp = new ERP_BL.Databases.Employee();
                    emp = (ERP_BL.Databases.Employee)_item;
                    var newEmp = repo.GetEmployee(emp.EmpId);
                    if (newEmp == null)
                    { return; }
                    newEmp.person.FName = emp.person.FName;
                    newEmp.person.LName = emp.person.LName;
                    if(emp.address != null)
                    {
                        newEmp.address.Line1 = emp.address.Line1;
                        newEmp.address.Line2 = emp.address.Line2;
                        newEmp.address.City = emp.address.City;
                        newEmp.address.Country = emp.address.Country;
                        newEmp.address.State = emp.address.State;
                        newEmp.address.Zip = emp.address.Zip;
                    }

                    if (emp.address2 != null)
                    {
                        newEmp.address2.Line1 = emp.address2.Line1;
                        newEmp.address2.Line2 = emp.address2.Line2;
                        newEmp.address2.City = emp.address2.City;
                        newEmp.address2.Country = emp.address2.Country;
                        newEmp.address2.State = emp.address2.State;
                        newEmp.address2.Zip = emp.address2.Zip;
                    }


                    if (emp.contact != null)
                    {
                        newEmp.contact.ContactNo = emp.contact.ContactNo;
                        newEmp.contact.Email = emp.contact.Email;
                        newEmp.contact.Fax = emp.contact.Fax;
                        newEmp.contact.SMLink1 = emp.contact.SMLink1;
                        newEmp.contact.SMLink2 = emp.contact.SMLink2;
                        newEmp.contact.SMLink3 = emp.contact.SMLink3;
                        newEmp.contact.Website = emp.contact.Website;
                        newEmp.contact.OfficialSkype = emp.contact.OfficialSkype;
                        newEmp.contact.PersonalSkype = emp.contact.PersonalSkype;
                        newEmp.contact.OfficialTeams = emp.contact.OfficialTeams;
                        newEmp.contact.PersonalTeams = emp.contact.PersonalTeams;
                    }
                    

                    repo.updateEmployee(newEmp);

                }
                DXMessageBox.Show(" All Employees Updated Successfully", "Successfull", MessageBoxButton.OK, MessageBoxImage.Information);
                //refershGrid();
            }
            catch
            {
                DXMessageBox.Show("Errorin updating Employees", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        private void GrdEmployeeRegisters_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }
    }
}
