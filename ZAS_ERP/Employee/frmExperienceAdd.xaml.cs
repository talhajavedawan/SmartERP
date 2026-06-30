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

namespace ZAS_ERP.Employee
{
    /// <summary>
    /// Interaction logic for frmExperienceAdd.xaml
    /// </summary>
    public partial class frmExperienceAdd : Window
    {
        public bool isSave = false;
        public List<EmployeeWorkExperience> expList = new List<EmployeeWorkExperience>();
        public EmployeeWorkExperience experience = new EmployeeWorkExperience();

        public frmExperienceAdd()
        {
            InitializeComponent();
        }

        private void SaveWork_Click(object sender, RoutedEventArgs e)
        {
            EmployeeWorkExperience exp = new EmployeeWorkExperience();


            exp.Company = txtExpCompany.Text;
            exp.JobTitle = txtJobTitle.Text;
            exp.JobDescription = txtJobDescription.Text;
            exp.DateFrom = dateStart.DateTime;
            exp.DateTo = dateEnd.DateTime;
            exp.employerAddress = txtExpAddress.Text;
            exp.employerContact = txtExpPhone.Text;
            exp.isLatest = (bool)chckIsLatest.IsChecked;

            experience = exp;
            //qualList.Add(qual);
            this.Close();
            isSave = true;

        }
    }
}
