using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for ucPersonalInfoGrid.xaml
    /// </summary>
    public partial class ucPersonalInfoGrid : UserControl
    {
        public ucPersonalInfoGrid()
        {
            InitializeComponent();
        }

        public void refershGrid()
        {
            EmployeeRepo repo = new EmployeeRepo();
            var emp = repo.GetAllEmployees();

            grdEmployeeRegisters.ItemsSource = emp;
        }
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }

        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
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
                    newEmp.person.FatherName = emp.person.FatherName;
                    newEmp.person.CNIC = emp.person.CNIC;
                    newEmp.person.Gender = emp.person.Gender;
                    newEmp.person.DOB = emp.person.DOB;
                    newEmp.MaritalStatus = emp.MaritalStatus;
                    newEmp.Disability = emp.Disability;
                    newEmp.DisDescription = emp.DisDescription;
                    newEmp.isActive = emp.isActive;


                    repo.updateEmployee(newEmp);

                }
                DXMessageBox.Show(" All Employees Updated Successfully", "Successfull", MessageBoxButton.OK, MessageBoxImage.Information);
                //refershGrid();
            }
            catch
            {
                DXMessageBox.Show("Errorin updating Employees", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }


            loadingGif.Visibility = Visibility.Hidden;
        }

        private void SaveGriddata_Click(object sender, RoutedEventArgs e)
        {
            loadingGif.Visibility = Visibility.Visible;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync();


        }

        private void GrdEmployeeRegisters_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }
    }
}
