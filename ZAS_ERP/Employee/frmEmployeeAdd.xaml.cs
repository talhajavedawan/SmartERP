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
using ERP_BL.Databases;
using ERP_BL.Config;
using ERP_BL.Enums;
using DevExpress.Xpf.Grid;
using System.Collections.ObjectModel;
using ZAS_ERP.Validations;
using System.ComponentModel;
using ZAS_ERP.Employee;
using System.IO;
using DevExpress.Xpf.Core;
using Microsoft.Win32;
using System.Data;
using ERP_BL.ChartofAccounts;
using ERP_BL.Procurements.Memos;

namespace ZAS_ERP.Employeess
{
    /// <summary>
    /// Interaction logic for frmemployeeCenter.xaml
    /// </summary>
    public partial class frmEmployeeAdd : DXWindow
    {
        public bool isEdit = false;
        public int editEmpId, index = -1;
        public string onlychar { get; set; }
        public string onlynum { get; set; }
        public string required { get; set; }
        public string nospecchar { get; set; }
        ERP_BL.Databases.Employee Supervisor = new ERP_BL.Databases.Employee();
        public List<Qualification> newListQual = new List<Qualification>();
        public List<Qualification> lstQual = new List<Qualification>();

        public List<EmployeeWorkExperience> newListExp = new List<EmployeeWorkExperience>();
        public List<EmployeeWorkExperience> lstExp = new List<EmployeeWorkExperience>();

        List<ViewInfo> views = new List<ViewInfo>();
        Department department = new Department();
        UsersRepo UsersRepo = new UsersRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public int empIdInt;
        List<User> UsersForComments = new List<User>();

        CompanyRepo cmpRepo = new CompanyRepo();
        DepartmentRepo depRepo = new DepartmentRepo();
        EmployeeRepo empRepo = new EmployeeRepo();

        public bool isProfile = false;
        public bool isEmp = false;
        public bool isEmp1 = false;
        public bool isEmp2 = false;
        public bool isEmp3 = false;
        public bool isEmp4 = false;
        public bool isEmp5 = false;

        List<Department> allDepartmentList = new List<Department>();
        List<Department> selectedDepartmentList = new List<Department>();
        List<Company> allCompanyList = new List<Company>();
        List<Company> selectedCompanyList = new List<Company>();

        List<Company> allAdminBillCompanyList = new List<Company>();
        List<Company> selectedAdminBillCompanyList = new List<Company>();

        List<Company> allPettyCashCompanyList = new List<Company>();
        List<Company> selectedPettyCashCompanyList = new List<Company>();

        List<Company> allTaskCompanyList = new List<Company>();
        List<Company> allCOACompanyList = new List<Company>();
        List<Company> selectedTaskCompanyList = new List<Company>();
        List<Company> selectedCOACompanyList = new List<Company>();

        ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();
        public frmEmployeeAdd()
        {
            InitializeComponent();



            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Skype and Teams Id") != null) ||
                (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Skype and Teams Id") != null))
            {
                grpBoxTeams.Visibility = Visibility.Visible;
                grpBoxSkype.Visibility = Visibility.Visible;
            }
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Skype and Teams Password") != null) ||
             (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Skype and Teams Password") != null))
            {
                grpBoxTeams.Visibility = Visibility.Visible;
                grpBoxSkype.Visibility = Visibility.Visible;
                txtSkypePass.IsEnabled = true;
                txtSkypePass1.IsEnabled = true;
                txtTeamsPass.IsEnabled = true;
                txtTeamsPass1.IsEnabled = true;
                //txtTeamsPass1.IsEnabled = true;

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View skype and Teams Password") != null)
            {
                grpBoxTeams.Visibility = Visibility.Visible;
                grpBoxSkype.Visibility = Visibility.Visible;
                txtSkypePass.IsEnabled = true;
                txtSkypePass1.IsEnabled = true;
                txtTeamsPass.IsEnabled = true;
                txtTeamsPass1.IsEnabled = true;
                chckShowPassSkype.IsEnabled = true;
                chckShowPassTeams.IsEnabled = true;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Employee Photo") != null)
            {
                userImage.ShowMenu = true;
                UserSignature.ShowMenu = true;
            }


        }
        EmployeeRepo emprepo = new EmployeeRepo();
        ERP_BL.Databases.Employee emp = new ERP_BL.Databases.Employee();
        Department dept = new Department();
        public class cmbitem
        {
            public string name { get; set; }
            public int id { get; set; }

        }




        private void LoadSupervisors()
        {
            //lookupSupervisorEmployee.ItemsSource = emprepo.GetActiveEmployees().Where(x => x.EmpId != frmEmployeeCenter.empid);
        }
        private void loaddepartments()
        {

            allDepartmentList = depRepo.GetActiveDepartments();

            gridDepartment.ItemsSource = allDepartmentList.Where(x => x.DeptName != "VDummy").ToList();
            //gridDepartment.ItemsSource = allDepartmentList;
            //  lookupDepartment.ItemsSource = allDepartmentList;

        }
        private void loadLookupDepartmentData()
        {
            var dept = depRepo.GetActiveDepartments();
            lookupDepartment.ItemsSource = dept;
        }
        private void loadcompdata()
        {

            allCompanyList = cmpRepo.GetActiveCompanies();
            this.gridCompany.ItemsSource = allCompanyList;
            // allAdminBillCompanyList = allCompanyList;
            // this.gridAdminBillCompany.ItemsSource = allAdminBillCompanyList;
            // lookupCompany.ItemsSource = allCompanyList;


        }
        private void loadLookupCompanyData()
        {
            var cmp = cmpRepo.GetActiveCompanies();
            lookupCompany.ItemsSource = cmp;
        }
        private void loadAdmincompdata()
        {
            allAdminBillCompanyList = cmpRepo.GetActiveCompanies();
            this.gridAdminBillCompany.ItemsSource = allAdminBillCompanyList;
            //lookupCompany.ItemsSource = allCompanyList;
        }

        private void LoadTaskcompdata()
        {
            allTaskCompanyList = cmpRepo.GetActiveCompanies();
            this.gridTaskCompany.ItemsSource = allTaskCompanyList;
            //lookupCompany.ItemsSource = allCompanyList;
        }

        private void LoadPettycashcompdata()
        {
            allPettyCashCompanyList = cmpRepo.GetActiveCompanies();
            this.gridPettyCashCompany.ItemsSource = allPettyCashCompanyList;
            //lookupCompany.ItemsSource = allCompanyList;
        }

        private void LoadCOAcompdata()
        {
            allCOACompanyList = cmpRepo.GetActiveCompanies();
            this.gridCOACompany.ItemsSource = allCOACompanyList;
        }

        private void btnaddEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lookupCompany.SelectedIndex > -1)
                    emp.coreCompanyId = (lookupCompany.SelectedItem as Company).Id;

                if (lookupDepartment.SelectedIndex > -1)
                    emp.coreDeptId = (lookupDepartment.SelectedItem as Department).Id;


                if (selectedDepartmentList.Count != 0)
                {
                    emp.departments = new List<Department>();
                    foreach (var _dept in selectedDepartmentList)
                    {
                        if (!emp.departments.Contains(_dept))
                        {
                            emp.departments.Add(_dept);
                        }
                    }
                }
                else
                {
                    emp.departments = null;
                }

                //if (gridDepartment.SelectedItems.Count != 0)
                //{
                //    emp.departments = new List<Department>();
                //    foreach (Department dept in gridDepartment.SelectedItems)
                //    {
                //        if (!emp.departments.Contains(dept))
                //        {
                //            emp.departments.Add(dept);
                //        }
                //    }

                //}

                if (selectedCompanyList.Count != 0)
                {
                    emp.Companies = new List<ERP_BL.Databases.Company>();
                    foreach (var _dept in selectedCompanyList)
                    {
                        if (!emp.Companies.Contains(_dept))
                        {
                            emp.Companies.Add(_dept);
                        }
                    }
                }
                else
                {
                    emp.Companies = null;
                }

                //if (gridCompany.SelectedItems.Count != 0)
                //{
                //    emp.Companies = new List<ERP_BL.Databases.Company>();
                //    foreach (ERP_BL.Databases.Company company in gridCompany.SelectedItems)
                //    {

                //        if (!emp.Companies.Contains(company))
                //        {
                //            emp.Companies.Add(company);
                //        }

                //    }

                //}

                if (selectedAdminBillCompanyList.Count != 0)
                {
                    emp.AdminBillCompanies = new List<ERP_BL.Databases.Company>();
                    foreach (var _dept in selectedAdminBillCompanyList)
                    {
                        if (!emp.AdminBillCompanies.Contains(_dept))
                        {
                            emp.AdminBillCompanies.Add(_dept);
                        }
                    }
                }
                else
                {
                    emp.AdminBillCompanies = null;
                }

                if (selectedTaskCompanyList.Count != 0)
                {
                    emp.TaskCompanies = new List<ERP_BL.Databases.Company>();
                    foreach (var _cmpny in selectedTaskCompanyList)
                    {
                        if (!emp.TaskCompanies.Contains(_cmpny))
                        {
                            emp.TaskCompanies.Add(_cmpny);
                        }
                    }
                }
                else
                {
                    emp.TaskCompanies = null;
                }

                if (selectedPettyCashCompanyList.Count != 0)
                {
                    emp.PettyCashCompanies = new List<ERP_BL.Databases.Company>();
                    foreach (var _cmpny in selectedPettyCashCompanyList)
                    {
                        if (!emp.PettyCashCompanies.Contains(_cmpny))
                        {
                            emp.PettyCashCompanies.Add(_cmpny);
                        }
                    }
                }
                else
                {
                    emp.PettyCashCompanies = null;
                }

                if (selectedCOACompanyList.Count>0 && emp.EmpId!=0)
                {
                    List<EmployeeCoaCompanies> COACompanyList = new List<EmployeeCoaCompanies>();

                    EmployeeRepo repo = new EmployeeRepo();
                    foreach(var _comp in selectedCOACompanyList)
                    {
                        EmployeeCoaCompanies employeeCoaCompanies = new EmployeeCoaCompanies()
                        {
                            compId = _comp.Id,
                            EmpId = emp.EmpId
                        };
                        COACompanyList.Add(employeeCoaCompanies);
                    }
                    repo.RemoveCoaCompanies(emp.EmpId);
                    repo.AddEmployeeCompanies(COACompanyList);
                }

                //if (gridAdminBillCompany.SelectedItems.Count != 0)
                //{
                //    emp.AdminBillCompanies = new List<ERP_BL.Databases.Company>();
                //    foreach (ERP_BL.Databases.Company company in gridAdminBillCompany.SelectedItems)
                //    {

                //        if (!emp.AdminBillCompanies.Contains(company))
                //        {
                //            emp.AdminBillCompanies.Add(company);
                //        }

                //    }

                //}

                //Newly added employe is always active
                emp.isActive = true;

                if (chkIsAdminBillType.IsChecked == true)
                    emp.isAdminBillType = true;
                else
                    emp.isAdminBillType = false;

                if (chkIsTaskType.IsChecked == true)
                    emp.isTaskType = true;
                else
                    emp.isTaskType = false;
                if (chkIsMultiUser.IsChecked == true)
                    emp.isMultiUser = true;
                else
                    emp.isMultiUser = false;
                //if (chkisActive.IsChecked == true)
                //    emp.isActive = true;
                //else
                //    emp.isActive = false;
                emp.JoinDate = (DateTime)dtpselectDate.DateTime;
                emp.MaritalStatus = cmbmaritalStatus.Text.Trim();
                if (cmbdisabledstatus.SelectedIndex == 0)
                {
                    emp.Disability = true;
                    emp.DisDescription = txtdisablityDisc.Text.Trim();
                }
                else
                {
                    emp.Disability = false;
                    emp.DisDescription = "";
                }

                //Employee Id for company 
                if (!String.IsNullOrEmpty(txtEmpId.Text))
                {
                    emp.EmployeeId = txtEmpId.Text;
                }
                else
                {
                    emp.EmployeeId = null;
                }

                if (chkisActive.IsChecked == true)
                {
                    emp.Status = EmployeeStatus.Active;
                }
                else
                    emp.Status = EmployeeStatus.Retired;
                emp.HireDate = (DateTime)dtpHireDate.DateTime;
                emp.JoinDate = (DateTime)dtpselectDate.DateTime;
                emp.BasicPay = string.IsNullOrEmpty(txtbasicSalary.Text.Trim()) ? 0 : Convert.ToDouble(txtbasicSalary.Text.Trim());
                emp.JobDescription = txtJobDescription.Text.Trim();
                emp.DesignationTitle = txtJobTitle.Text.Trim();
                if (Supervisor != null)
                {
                    if (Supervisor.EmpId != 0)
                        emp.SupervisorId = Supervisor.EmpId;
                }
                else
                    emp.SupervisorId = null;
                int currencyId = 0;
                if (cmbCurrency.SelectedItem != null) { currencyId = (cmbCurrency.SelectedItem as cmbitem).id; }
                //Employee Qualification

                emp.Qualifications = new List<Qualification>();
                foreach (Qualification q in qualDataGrid.VisibleItems)
                {

                    if (!emp.Qualifications.Contains(q))
                    {
                        emp.Qualifications.Add(q);
                    }
                }

                //Employee Experience
                emp.WorkExperience = new List<EmployeeWorkExperience>();
                foreach (EmployeeWorkExperience work in grdWorkExp.VisibleItems)
                {

                    if (!emp.WorkExperience.Contains(work))
                    {
                        emp.WorkExperience.Add(work);
                    }
                }

                if (cmbFunction.SelectedItem != null)
                {
                    emp.empFunction = cmbFunction.SelectedItem as Function;
                }

                if (cmbxDesignations.SelectedItem != null)
                {
                    emp.Desig = cmbxDesignations.SelectedItem as Designation;
                }

                //var qual = qualDataGrid.VisibleItems;
                //if (qual == null)
                //{
                //    MessageBox.Show("Qualification List is empty");
                //    return;
                //}
                //foreach (var _qual in qual)
                //{
                //    var qua = (Qualification)_qual;
                //    lstQual.Add(qua);
                //}
                //emp.Qualifications = lstQual;





                //Skype password encryption
                string userKey = GenerateKey();

                string encPassSkype = "";
                if (!String.IsNullOrEmpty(txtSkypePass.Text))
                {
                    var skypePass = txtSkypePass.Text.Trim();
                    if (!String.IsNullOrEmpty(userKey))
                    {
                        encPassSkype = ERP_BL.Employee.CryptoEngine.Encrypt(skypePass, userKey);
                    }
                }

                //Teams Password ENcryption
                string encPassTeams = "";
                if (!String.IsNullOrEmpty(txtTeamsPass.Text))
                {
                    var teamsPass = txtTeamsPass.Text.Trim();
                    if (!String.IsNullOrEmpty(userKey))
                    {
                        encPassTeams = ERP_BL.Employee.CryptoEngine.Encrypt(teamsPass, userKey);
                    }
                }
                //Employee Status 
                if ((cmbxEmployeeStatus.SelectedItem as ZAS_ERP.cmbitem) != null)
                {

                    var status = empRepo.GetEmployeeStatus((cmbxEmployeeStatus.SelectedItem as ZAS_ERP.cmbitem).id);
                    if (status != null)
                    {
                        emp.employeeStatus = status;
                    }
                }
                //

                //var bmImg = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
                //userImage.Source = bmImg;

                //var bytePhoto = GetByteArrayFromBitmapImage(userImage);
                //emp.person.Photo = 
                if (lookupReceivableAccounts.SelectedIndex > -1)
                {
                    emp.receivableAccountId = (lookupReceivableAccounts.SelectedItem as ChartofAccount).Id;
                }

                if (frmEmployeeCenter.editemp == 1 || isEdit == true)
                {
                    //Employee Permanent Adress Class  
                    emp.address.Country = txtCountry.Text.Trim();
                    emp.address.Line1 = txtAdressline1.Text.Trim();
                    emp.address.Line2 = txtAdressline2.Text.Trim();
                    emp.address.State = txtState.Text.Trim();
                    emp.address.City = txtCity.Text.Trim();
                    emp.address.Zip = string.IsNullOrEmpty(txtZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim());

                    //Employee postal address
                    if (emp.address2 == null)
                    {
                        Address add2 = new Address();
                        add2.Country = txtCountry_2.Text.Trim();
                        add2.Line1 = txtAdressline1_2.Text.Trim();
                        add2.Line2 = txtAdressline2_2.Text.Trim();
                        add2.State = txtState_2.Text.Trim();
                        add2.City = txtCity_2.Text.Trim();
                        add2.Zip = string.IsNullOrEmpty(txtZIP_2.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP_2.Text.Trim());

                        emp.address2 = add2;
                    }

                    emp.address2.Country = txtCountry_2.Text.Trim();
                    emp.address2.Line1 = txtAdressline1_2.Text.Trim();
                    emp.address2.Line2 = txtAdressline2_2.Text.Trim();
                    emp.address2.State = txtState_2.Text.Trim();
                    emp.address2.City = txtCity_2.Text.Trim();
                    emp.address2.Zip = string.IsNullOrEmpty(txtZIP_2.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP_2.Text.Trim());



                    //Employee Contact Class

                    emp.contact.ContactNo = txtPhonenum.Text.Trim();
                    emp.contact.Fax = txtFaxnum.Text.Trim();
                    emp.contact.Email = txtEmail.Text.Trim();
                    emp.contact.SMLink1 = txtLink1.Text.Trim();
                    emp.contact.SMLink2 = txtLink2.Text.Trim();
                    emp.contact.SMLink3 = txtLink3.Text.Trim();
                    emp.contact.Website = txtWebsite.Text.Trim();
                    if (txtOffSkype.Text != null)
                    { emp.contact.OfficialSkype = txtOffSkype.Text.Trim(); }
                    else
                    { emp.contact.OfficialSkype = ""; }

                    if (txtSkypePass.Text != null) { emp.contact.OffSkypePassword = encPassSkype; }
                    else { emp.contact.OffSkypePassword = ""; }

                    if (txtOffTeams.Text != null) { emp.contact.OfficialTeams = txtOffTeams.Text.Trim(); }
                    else { emp.contact.OfficialTeams = ""; }

                    if (txtTeamsPass.Text != null) { emp.contact.OffTeamsPassword = encPassTeams; }
                    else { emp.contact.OffTeamsPassword = ""; }

                    if (txtPersonSkype.Text != null)
                    {
                        emp.contact.PersonalSkype = txtPersonSkype.Text.Trim();
                    }
                    else { emp.contact.PersonalSkype = ""; }

                    if (txtPersonTeams.Text != null)
                    {
                        emp.contact.PersonalTeams = txtPersonTeams.Text.Trim();
                    }
                    else { emp.contact.PersonalTeams = ""; }

                    if (chkisActive.IsChecked == true)
                        emp.isActive = true;
                    else
                        emp.isActive = false;


                    //Employee  Person Class
                    emp.person.FName = txtfirstName.Text.Trim();
                    emp.person.LName = txtlastName.Text.Trim();
                    emp.person.CNIC = txtcnic.Text.Trim();
                    emp.person.DOB = dtpickerDOB.DateTime;
                    emp.person.FatherName = txtfathername.Text.Trim();
                    emp.person.Gender = (Gender)cmbgender.SelectedIndex;
                    emp.person.CNICexpiryDate = dateCnic.DateTime;
                    emp.person.passportExpiryDate = datePassport.DateTime;
                    emp.person.passportIssueDate = datePassportIssue.DateTime;

                 


                    if (UserSignature.Source != null)
                    {
                        try
                        {
                            var bmImg = (BitmapImage)UserSignature.Source;
                            if (bmImg != null)
                            {
                                var byteImg = GetByteArrayFromBitmapImage(bmImg);

                                emp.person.Signature = byteImg;
                            }
                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show("Error in uploading photo Please remove the photo and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }

                    }


                    if (userImage.Source != null)
                    {
                        try
                        {
                            var bmImg = (BitmapImage)userImage.Source;
                            if (bmImg != null)
                            {
                                var byteImg = GetByteArrayFromBitmapImage(bmImg);

                                emp.person.Photo = byteImg;
                            }
                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show("Error in uploading photo Please remove the photo and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }

                    }

                    emp.person.personPhotos = new List<PersonPhoto>();
                    //user Multi image update
                    if (empImage.Source != null)
                    {
                        try
                        {
                            PersonPhoto photo = new PersonPhoto();
                            var bmImg = (BitmapImage)empImage.Source;
                            if (bmImg != null)
                            {
                                //foreach (var _Previous in emp.person.personPhotos)
                                //{
                                //    if (_Previous.Id != 0)
                                //    {


                                //    }
                                //}
                                var bytee = GetByteArrayFromBitmapImage(bmImg);
                                emp.person.personPhotos.Add(new PersonPhoto()
                                {
                                    EmployeePhoto = bytee,
                                    PhotoName = "Photo",
                                    Person_Id = emp.person?.Id

                                });
                                //emp.person.EmployeePhoto = byteImg;
                            }
                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show("Error in uploading photo Please remove the photo and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }

                    }
                    if (empImage1.Source != null)
                    {
                        try
                        {
                            var bmImg = (BitmapImage)empImage1.Source;
                            if (bmImg != null)
                            {
                                var bytee = GetByteArrayFromBitmapImage(bmImg);
                                emp.person.personPhotos.Add(new PersonPhoto()
                                {
                                    EmployeePhoto = bytee,
                                    PhotoName = "Photo1",
                                    Person_Id = emp.person?.Id

                                });
                                //var byteImg = GetByteArrayFromBitmapImage(bmImg);

                                //emp.person.EmployeePhotoLeft = byteImg;
                            }
                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show("Error in uploading photo Please remove the photo and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }

                    }
                    if (empImage2.Source != null)
                    {
                        try
                        {
                            var bmImg = (BitmapImage)empImage2.Source;
                            if (bmImg != null)
                            {
                                var bytee = GetByteArrayFromBitmapImage(bmImg);
                                emp.person.personPhotos.Add(new PersonPhoto()
                                {
                                    EmployeePhoto = bytee,
                                    PhotoName = "Photo2",
                                    Person_Id = emp.person?.Id

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show("Error in uploading photo Please remove the photo and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }

                    }
                    if (empImage3.Source != null)
                    {
                        try
                        {
                            var bmImg = (BitmapImage)empImage3.Source;
                            if (bmImg != null)
                            {
                                var bytee = GetByteArrayFromBitmapImage(bmImg);
                                emp.person.personPhotos.Add(new PersonPhoto()
                                {
                                    EmployeePhoto = bytee,
                                    PhotoName = "Photo3",
                                    Person_Id = emp.person?.Id
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show("Error in uploading photo Please remove the photo and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }

                    }
                    if (empImage4.Source != null)
                    {
                        try
                        {
                            var bmImg = (BitmapImage)empImage4.Source;
                            if (bmImg != null)
                            {
                                var bytee = GetByteArrayFromBitmapImage(bmImg);
                                emp.person.personPhotos.Add(new PersonPhoto()
                                {
                                    EmployeePhoto = bytee,
                                    PhotoName = "Photo4",
                                    Person_Id = emp.person?.Id
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show("Error in uploading photo Please remove the photo and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }

                    }
                    if (empImage5.Source != null)
                    {
                        try
                        {
                            var bmImg = (BitmapImage)empImage5.Source;
                            if (bmImg != null)
                            {
                                var bytee = GetByteArrayFromBitmapImage(bmImg);
                                emp.person.personPhotos.Add(new PersonPhoto()
                                {
                                    EmployeePhoto = bytee,
                                    PhotoName = "Photo5",
                                    Person_Id = emp.person?.Id
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show("Error in uploading photo Please remove the photo and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }

                    }

                    if (cmbBloodGroup.SelectedItem != null)
                    { emp.person.BloodGroup = cmbBloodGroup.Text; }
                    if (!String.IsNullOrEmpty(txtPassport.Text))
                    {
                        emp.person.PassportNo = txtPassport.Text;
                    }
                    else
                    {
                        emp.person.PassportNo = null;
                    }

                    //Sales Target
                    if (emp.SalesTarget == null)
                        emp.SalesTarget = new SalesTarget();

                    if (currencyId == 0)
                    {
                        emp.SalesTarget.CurrencyId = null;
                    }
                    else
                    {
                        emp.SalesTarget.CurrencyId = currencyId;
                    }

                    emp.SalesTarget.EndDate = dtpEndDate.DateTime;
                    emp.SalesTarget.StartingDate = dtpStartDate.DateTime;
                    emp.SalesTarget.TargetAmount = string.IsNullOrEmpty(txtTargetValue.Text) ? 0 : Convert.ToDouble(txtTargetValue.Text);


                    //Employee Emergency Contact
                    if (emp.emergencyontact == null)
                    {
                        Emergencyontact emer = new Emergencyontact()
                        {
                            Name = txtEmerName.Text,
                            Relation = txtEmerRelation.Text,
                            contact = txtEmerPhone.Text,
                            Address = txtEmerAddress.Text
                        };
                        emp.emergencyontact = emer;
                    }
                    else
                    {
                        emp.emergencyontact.Name = txtEmerName.Text;
                        emp.emergencyontact.Relation = txtEmerRelation.Text;
                        emp.emergencyontact.contact = txtEmerPhone.Text;
                        emp.emergencyontact.Address = txtEmerAddress.Text;
                    }

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Employee") != null)
                    {
                        if (emp.employeeApproval.isVoid == true || emp.employeeApproval.PendingForClosing == true || emp.employeeApproval.isApproved == false)
                        { }
                        else
                        {
                            if (/*SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Employee without Approval") != null
                            || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for new added Employee") != null
                            ||*/ SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Employee without ReApproval") != null
                            || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Employee") != null)
                            {
                                emp.employeeApproval.isReApproved = true;
                                if (emp.employeeApproval.PendingForClosing == true)
                                {
                                    emp.employeeApproval.PendingForClosing = true;
                                }
                            }
                            else
                            {
                                // if (emp.employeeApproval.PendingForClosing == true || emp.employeeApproval.isReApproved == false || emp.employeeApproval.isApproved == false)
                                //{
                                //     DXMessageBox.Show("You don't have permission to edit employee in current state", "Un-Authorized", MessageBoxButton.OK, MessageBoxImage.Stop);
                                //     return;
                                //}
                                //else
                                //{
                                //if (emp.employeeApproval.isApproved == false || emp.employeeApproval.PendingForClosing == true)
                                //{
                                //    DXMessageBox.Show("You don't have permission to edit employee in current state", "Un-Authorized", MessageBoxButton.OK, MessageBoxImage.Stop);
                                //    return;
                                //}
                                emp.employeeApproval.isReApproved = false;
                                //}
                            }
                        }


                        emprepo.updateEmployee(emp);
                        DXMessageBox.Show("Employee Updated Succesfully", "Successfull", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        DXMessageBox.Show("You don't have permission to Update Employee", "Un-authorized", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
                else
                {
                    //permanent address
                    Address add = new Address()
                    {
                        Country = txtCountry.Text.Trim(),
                        Line1 = txtAdressline1.Text.Trim(),
                        Line2 = txtAdressline2.Text.Trim(),
                        State = txtState.Text.Trim(),
                        City = txtCity.Text.Trim(),
                        Zip = string.IsNullOrEmpty(txtZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim())
                    };
                    emp.address = add;
                    //postal address
                    Address add2 = new Address()
                    {
                        Country = txtCountry_2.Text.Trim(),
                        Line1 = txtAdressline1_2.Text.Trim(),
                        Line2 = txtAdressline2_2.Text.Trim(),
                        State = txtState_2.Text.Trim(),
                        City = txtCity_2.Text.Trim(),
                        Zip = string.IsNullOrEmpty(txtZIP_2.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP_2.Text.Trim())
                    };
                    emp.address2 = add2;

                    //Skype password encryption
                    //var key = GenerateKey();

                    //string encPassSkype = "";
                    //var skypePass = txtSkypePass.Text.Trim();
                    //if (!String.IsNullOrEmpty(key))
                    //{
                    //    encPassSkype = CryptoEngine.Encrypt(skypePass, key);
                    //}

                    ////Teams Password ENcryption
                    //string encPassTeams = "";
                    //var teamsPass = txtTeamsPass.Text.Trim();
                    //if (!String.IsNullOrEmpty(key))
                    //{
                    //    encPassTeams = CryptoEngine.Encrypt(teamsPass, key);
                    //}

                    Contact contact = new Contact()
                    {
                        ContactNo = txtPhonenum.Text.Trim(),
                        Fax = txtFaxnum.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        SMLink1 = txtLink1.Text.Trim(),
                        SMLink2 = txtLink2.Text.Trim(),
                        SMLink3 = txtLink3.Text.Trim(),
                        Website = txtWebsite.Text.Trim(),
                        OfficialSkype = txtOffSkype.Text.Trim(),
                        OffSkypePassword = encPassSkype,
                        OfficialTeams = txtOffTeams.Text.Trim(),
                        OffTeamsPassword = encPassTeams,
                        PersonalSkype = txtPersonSkype.Text.Trim(),
                        PersonalTeams = txtPersonTeams.Text.Trim()

                    };
                    emp.contact = contact;

                    Person person = new Person()
                    {
                        FName = txtfirstName.Text.Trim(),
                        LName = txtlastName.Text.Trim(),
                        CNIC = txtcnic.Text.Trim(),
                        DOB = (DateTime)dtpickerDOB.DateTime,
                        FatherName = txtfathername.Text.Trim(),
                        Gender = (Gender)cmbgender.SelectedIndex,
                        CNICexpiryDate = dateCnic.DateTime,
                        passportExpiryDate = datePassport.DateTime,
                        BloodGroup = cmbBloodGroup.Text,
                        PassportNo = txtPassport.Text,
                        passportIssueDate = datePassportIssue.DateTime
                    };
                    emp.person = person;


                    if (userImage.Source != null)
                    {
                        try
                        {
                            var bmImg = (BitmapImage)userImage.Source;
                            if (bmImg != null)
                            {
                                var byteImg = GetByteArrayFromBitmapImage(bmImg);

                                emp.person.Photo = byteImg;
                            }
                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show("Error in uploading photo Please remove the photo and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }

                    }
                    //user Multi image added
                    if (empImage.Source != null)
                    {
                        try
                        {
                            var bmImg = (BitmapImage)empImage.Source;
                            if (bmImg != null)
                            {
                                var bytee = GetByteArrayFromBitmapImage(bmImg);
                                emp.person.personPhotos.Add(new PersonPhoto()
                                {
                                    EmployeePhoto = bytee,
                                    PhotoName = "Photo"

                                });
                                //emp.person.EmployeePhoto = byteImg;
                            }
                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show("Error in uploading photo Please remove the photo and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }

                    }
                    if (empImage1.Source != null)
                    {
                        try
                        {
                            var bmImg = (BitmapImage)empImage1.Source;
                            if (bmImg != null)
                            {
                                var bytee = GetByteArrayFromBitmapImage(bmImg);
                                emp.person.personPhotos.Add(new PersonPhoto()
                                {
                                    EmployeePhoto = bytee,
                                    PhotoName = "Photo1"

                                });
                                //var byteImg = GetByteArrayFromBitmapImage(bmImg);

                                //emp.person.EmployeePhotoLeft = byteImg;
                            }
                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show("Error in uploading photo Please remove the photo and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }

                    }
                    if (empImage2.Source != null)
                    {
                        try
                        {
                            var bmImg = (BitmapImage)empImage2.Source;
                            if (bmImg != null)
                            {
                                var bytee = GetByteArrayFromBitmapImage(bmImg);
                                emp.person.personPhotos.Add(new PersonPhoto()
                                {
                                    EmployeePhoto = bytee,
                                    PhotoName = "Photo2"

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show("Error in uploading photo Please remove the photo and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }

                    }
                    if (empImage3.Source != null)
                    {
                        try
                        {
                            var bmImg = (BitmapImage)empImage3.Source;
                            if (bmImg != null)
                            {
                                var bytee = GetByteArrayFromBitmapImage(bmImg);
                                emp.person.personPhotos.Add(new PersonPhoto()
                                {
                                    EmployeePhoto = bytee,
                                    PhotoName = "Photo3"

                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show("Error in uploading photo Please remove the photo and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }

                    }
                    if (empImage4.Source != null)
                    {
                        try
                        {
                            var bmImg = (BitmapImage)empImage4.Source;
                            if (bmImg != null)
                            {
                                var bytee = GetByteArrayFromBitmapImage(bmImg);
                                emp.person.personPhotos.Add(new PersonPhoto()
                                {
                                    EmployeePhoto = bytee,
                                    PhotoName = "Photo4"

                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show("Error in uploading photo Please remove the photo and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }

                    }
                    if (empImage5.Source != null)
                    {
                        try
                        {
                            var bmImg = (BitmapImage)empImage5.Source;
                            if (bmImg != null)
                            {
                                var bytee = GetByteArrayFromBitmapImage(bmImg);
                                emp.person.personPhotos.Add(new PersonPhoto()
                                {
                                    EmployeePhoto = bytee,
                                    PhotoName = "Photo5"

                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show("Error in uploading photo Please remove the photo and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }

                    }


                    //Emergency Contact
                    Emergencyontact emerContact = new Emergencyontact()
                    {
                        Name = txtEmerName.Text,
                        Relation = txtEmerRelation.Text,
                        contact = txtEmerPhone.Text,
                        Address = txtEmerAddress.Text
                    };
                    emp.emergencyontact = emerContact;


                    if (chckIsApplied.IsChecked == true)
                    {
                        SalesTarget salesTarget = new SalesTarget()
                        {
                            CurrencyId = currencyId,
                            EndDate = (DateTime)dtpEndDate.DateTime,
                            StartingDate = (DateTime)dtpStartDate.DateTime,
                            TargetAmount = Convert.ToDouble(txtTargetValue.Text)
                        };
                        emp.SalesTarget = salesTarget;
                    }
                    else
                    {
                        emp.SalesTarget = new SalesTarget();
                    }




                    //CompanyRepo companyRepo = new CompanyRepo();
                    //List<ERP_BL.Databases.Company> company = new List<ERP_BL.Databases.Company>();
                    //company.Add(companyRepo.GetCompany(1));
                    //emp.Companies = company;
                    //CompanyRepo rep = new CompanyRepo();
                    //DepartmentRepo dep = new DepartmentRepo();

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Employee") != null)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Employee without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Employee") != null)
                        {
                            EmployeeApproval approval = new EmployeeApproval()
                            {
                                isApproved = true,
                                ApprovedDate = (DateTime)DateTime.Now,


                            };
                            emp.employeeApproval = approval;
                        }
                        else
                        {
                            EmployeeApproval approval = new EmployeeApproval()
                            {
                                isApproved = false,
                                //ApprovedDate = DateTime.Now,
                            };
                            emp.employeeApproval = approval;
                        }


                        emprepo.addEmployee(emp);
                        DevExpress.Xpf.Core.DXMessageBox.Show("Employee Added Succesfully", "Successfull", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                    {
                        DXMessageBox.Show("You don't have permission to Add Employee", "Un-authorized", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }


                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void loadFunctions()
        {
            //EmployeeRepo empRepo = new EmployeeRepo();
            var fnx = empRepo.GetAllFunctions();
            cmbFunction.ItemsSource = fnx;

        }
        public void loadDesignations()
        {
            //EmployeeRepo empRepo = new EmployeeRepo();
            var desig = empRepo.getAllDesignation();
            cmbxDesignations.ItemsSource = desig;
        }
        private void loadCurrencies()
        {

            CurrencyRepo currencyRepo = new CurrencyRepo();
            List<Currency> currencies = currencyRepo.getAll();
            List<cmbitem> cmbitems = new List<cmbitem>();
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            foreach (Currency cur in currencies)
            {

                //cmbCurrency.Items.Add(new cmbitem() { name = cur.CurrencyName, id = cur.Id });
                cmbitems.Add(new cmbitem() { name = cur.CurrencyName + "(" + cur.Abbrivation + " " + cur.Symbol + ")", id = cur.Id });
            }

            cmbCurrency.ItemsSource = cmbitems;

        }
        public void LoadChartofAccounts()
        {
            lookupReceivableAccounts.ItemsSource=coaRepo.getAllActive(SYSTEM_STATIC.currentUser.id);
        }

        private void LoadMemos()
        {
            if (emp.EmployeeUsers != null && emp.EmployeeUsers.Count > 0)
            {
                var userss = emp.EmployeeUsers.ToList();

                MemoRepo memoRepo = new MemoRepo();
                grdMemo.ItemsSource = memoRepo.GetAllLinkedMemos(userss[0].id);
            }
        }
        private void winemployeeadd_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                loadCurrencies();
                chkisActive.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark Employee as InActive") != null) ? true : false;
                loaddepartments();
                loadLookupDepartmentData();
                loadcompdata();
                loadLookupCompanyData();
                LoadTaskcompdata();
                LoadPettycashcompdata();
                LoadCOAcompdata();
                LoadSupervisors();
                loadFunctions();
                loadDesignations();
                //loadUserPhoto();
                loadAdmincompdata();
                loadEmployeeWorkingStatus();
                LoadChartofAccounts();
                LoadMemos();

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Company and Department") == null)
                {
                    tabComp.IsEnabled = false;
                }

                Config config = new Config();
                List<string> gentype = config.getGenderType();
                if (gentype.Count > 0)
                {
                    foreach (string IND in gentype)
                    {
                        cmbgender.Items.Add(IND);
                    }
                }

                if (frmEmployeeCenter.editemp == 1 || isEdit == true)
                {

                    //Permissions
                    tabPersonalInfo.IsEnabled = false;
                    tabAddress.IsEnabled = false;
                    tabContact.IsEnabled = false;
                    tabComp.IsEnabled = false;
                    qualificationTab.IsEnabled = false;
                    empInfo.IsEnabled = false;
                    tabSales.IsEnabled = false;
                    experienceTab.IsEnabled = false;
                    tabUserPictures.Visibility = Visibility.Collapsed;
                    grdPersonalInfo.Visibility = Visibility.Collapsed;

                    UserSignature.Visibility = Visibility.Collapsed;
                    userSignatureTxt.Visibility = Visibility.Collapsed;
                    //userSignatureTxt.Visibility = Visibility.Collapsed;


                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Personal Info") != null)
                    {
                        grdPersonalInfo.Visibility = Visibility.Visible;
                        tabPersonalInfo.IsEnabled = true;
                        UserSignature.Visibility = Visibility.Visible;
                        userSignatureTxt.Visibility = Visibility.Visible;

                    }
                    else
                    {
                        userImage.Visibility = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Employee Photo") != null) ? Visibility.Visible : Visibility.Collapsed;
                        grdPersonalInfo.Visibility = Visibility.Visible;
                        tabPersonalInfo.IsEnabled = true;
                        txtEmpId.IsEnabled = false;
                        chkIsAdminBillType.IsEnabled = false;
                        txtfirstName.IsEnabled = false;
                        txtlastName.IsEnabled = false;
                        txtfathername.IsEnabled = false;
                        cmbmaritalStatus.IsEnabled = false;
                        cmbgender.IsEnabled = false;
                        dtpickerDOB.IsEnabled = false;
                        txtcnic.IsEnabled = false;
                        dateCnic.IsEnabled = false;
                        cmbdisabledstatus.IsEnabled = false;
                        txtdisablityDisc.IsEnabled = false;
                        cmbBloodGroup.IsEnabled = false;
                        cmbxDesignations.IsEnabled = false;
                        cmbFunction.IsEnabled = false;
                        cmbxEmployeeStatus.IsEnabled = false;
                        txtPassport.IsEnabled = false;
                        datePassport.IsEnabled = false;
                        datePassportIssue.IsEnabled = false;
                        UserSignature.ShowMenu = false;
                        tabAdminBillComp.IsEnabled = false;

                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Contact and Address") != null)
                    {
                        tabAddress.IsEnabled = true;
                        tabContact.IsEnabled = true;
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Company and Department") != null)
                    {
                        tabComp.IsEnabled = true;
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Qualification") != null)
                    {
                        qualificationTab.IsEnabled = true;
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Employment Info") != null)
                    {
                        empInfo.IsEnabled = true;

                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Employee Sale Target") != null)
                    {
                        tabSales.IsEnabled = true;
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Employee Work Experience") != null)
                    {
                        experienceTab.IsEnabled = true;
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Employee Picture") != null)
                    {
                        tabUserPictures.Visibility = Visibility.Visible;
                    }

                    winEmployeeAdd.Title = "Edit Employee";

                    btnaddEmployee.Content = "Update Employee";
                    if (isEdit == true)
                    {
                        emp = emprepo.GetEmployeeForForm(editEmpId);
                    }
                    else
                    {
                        emp = emprepo.GetEmployeeForForm(frmEmployeeCenter.empid);
                    }

                    LoadMemos();

                    if (emp.isAdminBillType == true)
                        chkIsAdminBillType.IsChecked = true;

                    if (emp.isTaskType == true)
                        chkIsTaskType.IsChecked = true;
                    if (emp.isMultiUser == true)
                        chkIsMultiUser.IsChecked = true;

                    chkisActive.IsChecked = emp.isActive;
                    txtfirstName.Text = emp.person.FName;
                    txtlastName.Text = emp.person.LName;
                    txtfathername.Text = emp.person.FatherName;
                    dtpickerDOB.DateTime = (DateTime)emp.person.DOB;
                    txtcnic.Text = emp.person.CNIC;
                    txtEmpId.Text = emp.EmployeeId;
                    EmployeeIdStr.Text = emp.EmpId.ToString();
                    if (emp.person.CNICexpiryDate != null)
                        dateCnic.DateTime = (DateTime)emp.person.CNICexpiryDate;
                    if (emp.person.passportExpiryDate != null)
                        datePassport.DateTime = (DateTime)emp.person.passportExpiryDate;

                    if (emp.person.passportIssueDate != null)
                    {
                        datePassportIssue.DateTime = (DateTime)emp.person.passportIssueDate;
                    }

               

                    cmbgender.SelectedIndex = Convert.ToInt32(emp.person.Gender);
                    if (emp.Disability == false)
                    {
                        cmbdisabledstatus.SelectedIndex = 1;
                        txtdisablityDisc.Text = "";
                    }
                    //Comment

                    else
                    {
                        cmbdisabledstatus.SelectedIndex = 0;
                        txtdisablityDisc.Text = emp.DisDescription;
                    }
                    if (emp.Status == EmployeeStatus.Active)
                    {
                        chkisActive.IsChecked = true;

                    }
                    if (emp.receivableAccountId != null)
                    {
                        lookupReceivableAccounts.Text = emp.receivableAccount.accountName;
                    }
                    dtpHireDate.DateTime = (DateTime)emp.HireDate;
                    dtpselectDate.DateTime = (DateTime)emp.JoinDate;
                    txtbasicSalary.Text = emp.BasicPay.ToString();
                    txtJobDescription.Text = emp.JobDescription;
                    txtJobTitle.Text = emp.DesignationTitle;
                    if (emp.SupervisorId != null && emp.Supervisor != null)
                    {
                        Supervisor = emp.Supervisor;
                        lookupSupervisorEmployee.Text = emp.Supervisor.person.FName;

                    }

                    cmbmaritalStatus.Text = emp.MaritalStatus;

                    if (emp.CoreCompany != null)
                        lookupCompany.EditValue = emp.coreCompanyId;

                    if (emp.CoreDepartment != null)
                        lookupDepartment.EditValue = emp.coreDeptId;

                    //Start fetching department

                    foreach (var _selectedDept in emp.departments)
                    {
                        selectedDepartmentList.Add(_selectedDept);

                    }
                    gridDepartmentSelected.ItemsSource = null;
                    gridDepartmentSelected.ItemsSource =  selectedDepartmentList.Where(x => x.DeptName != "VDummy").ToList(); 


                    foreach (var rrr in selectedDepartmentList)
                    {
                        if (allDepartmentList.Find(x => x.Id == rrr.Id) != null)
                        {
                            allDepartmentList.Remove(allDepartmentList.FirstOrDefault(x => x.Id == rrr.Id));
                        }
                    }
                    gridDepartment.ItemsSource = null;
                    gridDepartment.ItemsSource = allDepartmentList;

                    //End Fetching Deparments

                    //Start fetching Company

                    foreach (var _selectedDept in emp.Companies)
                    {
                        selectedCompanyList.Add(_selectedDept);

                    }
                    gridCompanySelected.ItemsSource = null;
                    gridCompanySelected.ItemsSource = selectedCompanyList;

                    foreach (var rrr in selectedCompanyList)
                    {
                        if (allCompanyList.Find(x => x.Id == rrr.Id) != null)
                        {
                            allCompanyList.Remove(allCompanyList.FirstOrDefault(x => x.Id == rrr.Id));
                        }
                    }
                    gridCompany.ItemsSource = null;
                    gridCompany.ItemsSource = allCompanyList;

                    //End Fetching Company

                    //Start fetching Admin bill Company

                    foreach (var _cmpny in emp.AdminBillCompanies)
                    {
                        selectedAdminBillCompanyList.Add(_cmpny);

                    }
                    gridAdminBillCompanySelected.ItemsSource = null;
                    gridAdminBillCompanySelected.ItemsSource = selectedAdminBillCompanyList;

                    foreach (var _cmpny in selectedAdminBillCompanyList)
                    {
                        if (allAdminBillCompanyList.Find(x => x.Id == _cmpny.Id) != null)
                        {
                            allAdminBillCompanyList.Remove(allAdminBillCompanyList.FirstOrDefault(x => x.Id == _cmpny.Id));
                        }
                    }
                    gridAdminBillCompany.ItemsSource = null;
                    gridAdminBillCompany.ItemsSource = allAdminBillCompanyList;

                    //Start fetching Petty cash Company
                    foreach (var _cmpny in emp.PettyCashCompanies)
                    {
                        selectedPettyCashCompanyList.Add(_cmpny);

                    }
                    gridPettyCashCompanySelected.ItemsSource = null;
                    gridPettyCashCompanySelected.ItemsSource = selectedPettyCashCompanyList;

                    foreach (var _cmpny in selectedPettyCashCompanyList)
                    {
                        if (allPettyCashCompanyList.Find(x => x.Id == _cmpny.Id) != null)
                        {
                            allPettyCashCompanyList.Remove(allPettyCashCompanyList.FirstOrDefault(x => x.Id == _cmpny.Id));
                        }
                    }
                    gridPettyCashCompany.ItemsSource = null;
                    gridPettyCashCompany.ItemsSource = allPettyCashCompanyList;

                    if (emp.EmpId != 0)
                    {
                        List<EmployeeCoaCompanies> empCOACompanies = new List<EmployeeCoaCompanies>();

                         empCOACompanies = emprepo.GetAllCOAEmployeesCompanies(emp.EmpId);

                        
                            List<Company> coaCompanies = new List<Company>();
                            foreach (var comp in empCOACompanies)
                            {
                                coaCompanies.Add(comp.Company);
                            }
                            foreach (var _comp in coaCompanies)
                            {
                                selectedCOACompanyList.Add(_comp);
                            }
                            gridCOACompanySelected.ItemsSource = null;
                            gridCOACompanySelected.ItemsSource = selectedCOACompanyList;

                            foreach (var _cmpny in selectedCOACompanyList)
                            {
                                if (allCOACompanyList.Find(x => x.Id == _cmpny.Id) != null)
                                {
                                    allCOACompanyList.Remove(allCOACompanyList.FirstOrDefault(x => x.Id == _cmpny.Id));
                                }
                            }
                            gridCOACompany.ItemsSource = null;
                            gridCOACompany.ItemsSource = allCOACompanyList;
                        

                    }
                    foreach (var _cmpny in emp.TaskCompanies)
                    {
                        selectedTaskCompanyList.Add(_cmpny);

                    }

                    gridTaskCompanySelected.ItemsSource = null;
                    gridTaskCompanySelected.ItemsSource = selectedTaskCompanyList;

                    foreach (var _cmpny in selectedTaskCompanyList)
                    {
                        if (allTaskCompanyList.Find(x => x.Id == _cmpny.Id) != null)
                        {
                            allTaskCompanyList.Remove(allTaskCompanyList.FirstOrDefault(x => x.Id == _cmpny.Id));
                        }
                    }
                    gridTaskCompany.ItemsSource = null;
                    gridTaskCompany.ItemsSource = allTaskCompanyList;

                    //End Fetching Company


                    // select all mapped departments
                    //foreach (Department dept in  emp.departments)
                    //    gridDepartment.SelectItem(gridDepartment.FindRowByValue(gridDepartment.Columns.GetColumnByFieldName("Id"), dept.Id));

                    //foreach (ERP_BL.Databases.Company company in emp.Companies)
                    //    gridCompany.SelectItem(gridCompany.FindRowByValue(gridCompany.Columns.GetColumnByFieldName("Id"), company.Id));

                    //foreach (ERP_BL.Databases.Company company in emp.AdminBillCompanies)
                    //    gridAdminBillCompany.SelectItem(gridAdminBillCompany.FindRowByValue(gridAdminBillCompany.Columns.GetColumnByFieldName("Id"), company.Id));

                    //load Employee adress to the adress tab
                    txtCountry.Text = emp.address.Country;
                    txtAdressline1.Text = emp.address.Line1;
                    txtAdressline2.Text = emp.address.Line2;
                    txtState.Text = emp.address.State;
                    txtCity.Text = emp.address.City;

                    txtZIP_2.Text = emp.address.Zip.ToString();
                    if (emp.address2 != null)
                    {
                        txtCountry_2.Text = emp.address2.Country;
                        txtAdressline1_2.Text = emp.address2.Line1;
                        txtAdressline2_2.Text = emp.address2.Line2;
                        txtState_2.Text = emp.address2.State;
                        txtCity_2.Text = emp.address2.City;
                        txtZIP_2.Text = emp.address2.Zip.ToString();
                    }


                    // Employee contact info to contact tab
                    txtPhonenum.Text = emp.contact.ContactNo;
                    txtFaxnum.Text = emp.contact.Fax;
                    txtEmail.Text = emp.contact.Email;
                    txtLink1.Text = emp.contact.SMLink1;
                    txtLink2.Text = emp.contact.SMLink2;
                    txtLink3.Text = emp.contact.SMLink3;
                    txtWebsite.Text = emp.contact.Website;
                    //changes
                    txtOffSkype.Text = emp.contact.OfficialSkype;
                    var key = GenerateKey();
                    string skypePass = "";
                    string teamsPass = "";
                    if (key != null)
                    {
                        if (emp.contact != null)
                        {
                            if (!String.IsNullOrEmpty(emp.contact.OffSkypePassword))
                            {
                                skypePass = ERP_BL.Employee.CryptoEngine.Decrypt(emp.contact.OffSkypePassword, key);
                            }
                            if (!String.IsNullOrEmpty(emp.contact.OffTeamsPassword))
                            {
                                teamsPass = ERP_BL.Employee.CryptoEngine.Decrypt(emp.contact.OffTeamsPassword, key);
                            }
                        }
                    }


                    txtSkypePass.Text = skypePass;
                    txtOffTeams.Text = emp.contact.OfficialTeams;
                    txtTeamsPass.Text = teamsPass;
                    txtPersonSkype.Text = emp.contact.PersonalSkype;
                    txtPersonTeams.Text = emp.contact.PersonalTeams;

                    if (emp.SalesTarget != null || emp.SalesTargetId != null)
                    {
                        txtTargetValue.Text = emp.SalesTarget.TargetAmount.ToString();
                        if (emp.SalesTarget.StartingDate != null)
                            dtpStartDate.DateTime = (DateTime)emp.SalesTarget.StartingDate;

                        if (emp.SalesTarget.EndDate != null)
                            dtpEndDate.DateTime = (DateTime)emp.SalesTarget.EndDate;
                        foreach (cmbitem cmbitem in cmbCurrency.Items)
                        {
                            if (cmbitem.id == emp.SalesTarget.CurrencyId)
                            {
                                cmbCurrency.SelectedItem = cmbitem;
                                break;
                            }
                        }

                    }

                    //laod Qualification
                    if (emp.Qualifications != null)
                    {
                        var empQual = emp.Qualifications;
                        qualDataGrid.ItemsSource = empQual;
                    }

                    //Load Experience
                    if (emp.WorkExperience != null)
                    {
                        var empwork = emp.WorkExperience;
                        grdWorkExp.ItemsSource = empwork;
                    }

                    if (emp.person != null)
                    {
                        //load Passport Number
                        if (!String.IsNullOrEmpty(emp.person.PassportNo))
                        { txtPassport.Text = emp.person.PassportNo; }

                        //load Blood Group
                        if (!String.IsNullOrEmpty(emp.person.BloodGroup))
                        {
                            cmbBloodGroup.Text = emp.person.BloodGroup;
                        }




                    }

                    //Load Function
                    if (emp.empFunction != null)
                    {
                        cmbFunction.Text = emp.empFunction.Title;
                    }

                    //Load Designation
                    if (emp.Desig != null)
                    {
                        cmbxDesignations.Text = emp.Desig.Title;
                    }

                    //Load Employee working Status
                    if (emp.employeeStatus != null)
                    {
                        var status = emp.employeeStatus;

                        for (int i = 0; i < cmbxEmployeeStatus.Items.Count; i++)
                        {
                            var _item = cmbxEmployeeStatus.Items[i] as ZAS_ERP.cmbitem;
                            //var item = _item as cmbitem;
                            if (_item.name == emp.employeeStatus.Status && _item.id == emp.employeeStatus.Id)
                            {
                                index = i;
                                break;
                            }
                        }
                        cmbxEmployeeStatus.SelectedIndex = index;
                    }

                    //Load User Photo
                    if (emp.person.Photo != null)
                    {
                        var byteImg = emp.person.Photo;
                        if (byteImg != null)
                        {
                            var image = GetBitmapImageFromByteArray(byteImg);
                            userImage.Source = image;
                        }

                    }
                    ////Load User Photo
                    //if (emp.person.Photo != null)
                    //{
                    //    var byteImg = emp.person.Photo;
                    //    if (byteImg != null)
                    //    {
                    //        var image = GetBitmapImageFromByteArray(byteImg);
                    //        empImage.Source = image;
                    //    }

                    //}

                    //Load User Photo
                    foreach (var _img in emp.person.personPhotos)
                    {
                        switch (_img.PhotoName)
                        {
                            case "Photo":
                                var image = GetBitmapImageFromByteArray(_img.EmployeePhoto);
                                empImage.Source = image;
                                break;
                            case "Photo1":
                                var image1 = GetBitmapImageFromByteArray(_img.EmployeePhoto);
                                empImage1.Source = image1;
                                break;
                            case "Photo2":
                                var image2 = GetBitmapImageFromByteArray(_img.EmployeePhoto);
                                empImage2.Source = image2;
                                break;
                            case "Photo3":
                                var image3 = GetBitmapImageFromByteArray(_img.EmployeePhoto);
                                empImage3.Source = image3;
                                break;
                            case "Photo4":
                                var image4 = GetBitmapImageFromByteArray(_img.EmployeePhoto);
                                empImage4.Source = image4;
                                break;
                            case "Photo5":
                                var image5 = GetBitmapImageFromByteArray(_img.EmployeePhoto);
                                empImage5.Source = image5;
                                break;
                        }
                    }



                    //Load User Signature
                    if (emp.person.Signature != null)
                    {
                        var byteImg = emp.person.Signature;
                        if (byteImg != null)
                        {
                            var image = GetBitmapImageFromByteArray(byteImg);
                            UserSignature.Source = image;
                        }

                    }
                    //Load Emergency Contacts
                    if (emp.emergencyontact != null)
                    {
                        txtEmerName.Text = emp.emergencyontact.Name;
                        txtEmerRelation.Text = emp.emergencyontact.Relation;
                        txtEmerPhone.Text = emp.emergencyontact.contact;
                        txtEmerAddress.Text = emp.emergencyontact.Address;

                    }


                    loadonEmployeedata();
                    if (EmployeeIdStr.Text != null)
                    {
                        views = UsersRepo.getViwerInfo(Convert.ToInt32(EmployeeIdStr.Text), 14);
                        grdUsers.ItemsSource = views;
                    }
                    //

                    //loaddepartments();


                }
                else
                {
                    gridCompanySelected.ItemsSource = selectedCompanyList;
                    gridDepartmentSelected.ItemsSource = selectedDepartmentList.Where(x => x.DeptName != "VDummy").ToList(); 
                    gridAdminBillCompanySelected.ItemsSource = selectedAdminBillCompanyList;
                    gridTaskCompanySelected.ItemsSource = selectedTaskCompanyList;
                    this.Title = "Add New Employee";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        public void loadEmployeeWorkingStatus()
        {
            //Adding status list to combobox
            //EmployeeRepo empRepo = new EmployeeRepo();
            List<ZAS_ERP.cmbitem> empStatusLst = new List<ZAS_ERP.cmbitem>();
            var allEmpStatus = empRepo.GetAllEmployeeStatus();
            if (allEmpStatus != null)
            {
                Parallel.ForEach(allEmpStatus, delegate (EmployeeWorkingStatus status) // foreach (Employee status in EmployeeStatuses)
                {

                    empStatusLst.Add
                    (new ZAS_ERP.cmbitem()
                    {
                        name = status.Status,
                        id = status.Id,
                        bcolor = status.backcolor,
                        fcolor = "#FF000000"
                    });


                });
                cmbxEmployeeStatus.ItemsSource = empStatusLst;

            }
        }
        public void loadUserPhoto()
        {
            EmployeeRepo repo = new EmployeeRepo();

            ERP_BL.Databases.Employee employee = repo.GetEmployee(SYSTEM_STATIC.currentUser.employeeId); /*SystemLogic.currentUser.employee*/;
            if (employee != null)
            {
                var byteImg = employee.person.Photo;
                if (byteImg != null)
                {
                    var image = GetBitmapImageFromByteArray(byteImg);
                    userImage.Source = image;
                }
            }
        }
        private void DXTabControl_SelectionChanged(object sender, DevExpress.Xpf.Core.TabControlSelectionChangedEventArgs e)
        {

        }


        private void gridCompany_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {

        }

        private void WinEmployeeAdd_Unloaded(object sender, RoutedEventArgs e)
        {
            //SystemLogic.SaveUserSettingForCurrentWindow(gridDepartment);
            //SystemLogic.SaveUserSettingForCurrentWindow(gridCompany);
            frmEmployeeCenter.empid = 0;

            if (EmployeeIdStr.Text != "")
            {
                UsersRepo.Add(TransactionInfo.viewed, Convert.ToInt32(EmployeeIdStr.Text), 14, "Viewed details of Employee");
            }
            //SystemLogic.SaveUserSettingForCurrentWindow(grdCntrlSalesReceipt);
        }

        private void LookupSupervisorEmployee_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            Supervisor = lookupSupervisorEmployee.SelectedItem as ERP_BL.Databases.Employee;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lookupSupervisorEmployee.Text = "";
            lookupSupervisorEmployee.SelectedItem = null;
        }

        private void ChckSameAdd_Checked(object sender, RoutedEventArgs e)
        {
            if (chckSameAdd.IsChecked == true)
            {
                txtAdressline1_2.Text = txtAdressline1.Text;
                txtAdressline2_2.Text = txtAdressline2.Text;
                txtCity_2.Text = txtCity.Text;
                txtState_2.Text = txtState.Text;
                txtCountry_2.Text = txtCountry.Text;
                txtZIP_2.Text = txtZIP.Text;

                txtAdressline1_2.IsEnabled = false;
                txtAdressline2_2.IsEnabled = false;
                txtCity_2.IsEnabled = false;
                txtState_2.IsEnabled = false;
                txtCountry_2.IsEnabled = false;
                txtZIP_2.IsEnabled = false;
            }
        }

        private void ChckSameAdd_Unchecked(object sender, RoutedEventArgs e)
        {
            txtAdressline1_2.Text = "";
            txtAdressline2_2.Text = "";
            txtCity_2.Text = "";
            txtState_2.Text = "";
            txtCountry_2.Text = "";
            txtZIP_2.Text = "";


            txtAdressline1_2.IsEnabled = true;
            txtAdressline2_2.IsEnabled = true;
            txtCity_2.IsEnabled = true;
            txtState_2.IsEnabled = true;
            txtCountry_2.IsEnabled = true;
            txtZIP_2.IsEnabled = true;

        }

        private void AddNewDegree_Click(object sender, RoutedEventArgs e)
        {
            frmQualificationAdd frm = new frmQualificationAdd();
            List<Qualification> qListOld = new List<Qualification>();

            if (frmEmployeeCenter.editemp == 1)
            {
                //Generating Old Lists
                var qList = qualDataGrid.VisibleItems;

                if (qList != null)
                {
                    foreach (var item in qList)
                    {
                        var _item = (Qualification)item;
                        qListOld.Add(_item);
                    }
                }
                //Showing dialog
                frm.ShowDialog();
                if (frm.isSave == true)
                {

                    if (frm.qualification != null)
                    {
                        qListOld.Add(frm.qualification);
                    }


                    qualDataGrid.ItemsSource = qListOld;
                }
                //var qList = qualDataGrid.VisibleItems;
                //if (qList != null)
                //{
                //    foreach (var item in qList)
                //    {
                //        var _item = (Qualification)item;
                //        newListQual.Add(_item);
                //    }
                //}                
            }


            else
            {

                frm.ShowDialog();
                if (frm.isSave == true)
                {
                    var list = frm.qualList;
                    newListQual = new List<Qualification>();
                    newListQual = (qualDataGrid.ItemsSource as List<Qualification>) == null ? new List<Qualification>() : qualDataGrid.ItemsSource as List<Qualification>;

                    //if (list != null)
                    //{
                    //    qualDataGrid.ItemsSource = list;
                    //}
                    if (frm.qualification != null)
                    {
                        newListQual.Add(frm.qualification);
                    }
                    //qualDataGrid.ItemsSource= null;


                    qualDataGrid.ItemsSource = newListQual;
                }

            }


        }

        private void AddNewCert_Click(object sender, RoutedEventArgs e)
        {
            //frmCertificateAdd frm = new frmCertificateAdd();
            //frm.ShowDialog();
        }

        private void BtnEditQual_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var degItem = (Qualification)qualDataGrid.GetFocusedRow();
                if (degItem != null)
                {

                    frmQualificationAdd frm = new frmQualificationAdd();
                    //if (degItem.DegreeType == DegreeType.Certification)

                    if (degItem.DegreeType == "Certification")
                    {
                        frm.degreeSwitch.IsChecked = true;
                        if (degItem.DegreeTitle != null)
                        { frm.txtCertTitle.Text = degItem.DegreeTitle; }
                        if (degItem.MarksObtained != 0)
                        { frm.txtObtMarks.Text = degItem.MarksObtained.ToString(); }
                        if (degItem.MarksTotal != 0)
                        { frm.txtTotMarks.Text = degItem.MarksTotal.ToString(); }
                        if (degItem.StartYear != null)
                        { frm.dateStarting.DateTime = (DateTime)degItem.StartYear; }
                        if (degItem.PassingYear != null)
                        { frm.datePassing.DateTime = (DateTime)degItem.PassingYear; }

                        if (!String.IsNullOrEmpty(degItem.Institute))
                        { frm.txtInst.Text = degItem.Institute; }

                        if (!String.IsNullOrEmpty(degItem.Location))
                        { frm.txtCountry.Text = degItem.Location; }

                        frm.chckIsValid.IsChecked = degItem.IsValid;
                        if (degItem.validTill != null)
                        { frm.validTill.DateTime = (DateTime)degItem.validTill; }

                        if (!String.IsNullOrEmpty(degItem.Score))
                        { frm.txtScore.Text = degItem.Score; }

                    }
                    else
                    {
                        frm.degreeSwitch.IsChecked = false;
                        if (degItem.DegreeType != null)
                        {
                            frm.cmbxDegreeType.SelectedItem = degItem.DegreeType;
                        }
                        if (degItem.DegreeTitle != null)
                        {
                            frm.txtDegreeTitle.Text = degItem.DegreeTitle;
                        }
                        if (degItem.Specialization != null)
                        {
                            frm.txtSubject.Text = degItem.Specialization;
                        }

                        if (degItem.MarksObtained != 0)
                        { frm.txtObtMarks.Text = degItem.MarksObtained.ToString(); }
                        if (degItem.MarksTotal != 0)
                        { frm.txtTotMarks.Text = degItem.MarksTotal.ToString(); }

                        if (degItem.MarksPercentage != 0)
                        { frm.txtPercent.Text = degItem.MarksPercentage.ToString(); }

                        if (degItem.Division != null)
                        {
                            frm.txtDiv.Text = degItem.Division;
                        }

                        if (degItem.StartYear != null)
                        { frm.dateStarting.DateTime = (DateTime)degItem.StartYear; }
                        if (degItem.PassingYear != null)
                        { frm.datePassing.DateTime = (DateTime)degItem.PassingYear; }

                        if (!String.IsNullOrEmpty(degItem.Institute))
                        {
                            frm.txtInst.Text = degItem.Institute;
                        }

                        if (!String.IsNullOrEmpty(degItem.Location))
                        {
                            frm.txtCountry.Text = degItem.Location;
                        }

                        frm.chckIsLatest.IsChecked = degItem.IsLatest;
                        frm.chckIsDistint.IsChecked = degItem.IsDistinction;
                        frm.txtDist.Text = degItem.DistDetails;

                        frm.chckIsComp.IsChecked = degItem.IsCompleted;


                    }
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDeleteQual_Click(object sender, RoutedEventArgs e)
        {
            tblQual.DeleteRow(tblQual.FocusedRowHandle);

        }

        private void ChckIsApplied_Checked(object sender, RoutedEventArgs e)
        {
            cmbCurrency.IsEnabled = true;
            txtTargetValue.IsEnabled = true;
            dtpStartDate.IsEnabled = true;
            dtpEndDate.IsEnabled = true;
        }

        private void ChckIsApplied_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbCurrency.IsEnabled = false;
            txtTargetValue.IsEnabled = false;
            dtpStartDate.IsEnabled = false;
            dtpEndDate.IsEnabled = false;
        }
        public string GenerateKey()
        {
            string key = "";
            string userName = "";
            string strId = "";
            string prodName = "ZAS0";
            string name = "";
            var user = SYSTEM_STATIC.currentUser;
            if (user != null)
            {
                var id = user.id;
                strId = id.ToString("0000");
                userName = user.userName.ToString();
                if (userName.Length < 6)
                {
                    userName = userName + userName;
                }
                name = userName.Substring(0, 6);
            }

            key = prodName + "-" + strId + "-" + name;
            return key;
        }
        public void ValidateKey()
        {

        }

        private void ChckShowPassSkype_Checked(object sender, RoutedEventArgs e)
        {
            txtSkypePass1.Text = txtSkypePass.Text;
            txtSkypePass.Visibility = Visibility.Collapsed;
            txtSkypePass1.Visibility = Visibility.Visible;


        }

        private void ChckShowPassSkype_Unchecked(object sender, RoutedEventArgs e)
        {
            txtSkypePass.Text = txtSkypePass1.Text;
            txtSkypePass1.Visibility = Visibility.Collapsed;
            txtSkypePass.Visibility = Visibility.Visible;


        }

        private void ChckShowPassTeams_Checked(object sender, RoutedEventArgs e)
        {

            txtTeamsPass1.Text = txtTeamsPass.Text;
            txtTeamsPass.Visibility = Visibility.Collapsed;
            txtTeamsPass1.Visibility = Visibility.Visible;

        }

        private void ChckShowPassTeams_Unchecked(object sender, RoutedEventArgs e)
        {
            txtTeamsPass.Text = txtTeamsPass1.Text;
            txtTeamsPass1.Visibility = Visibility.Collapsed;
            txtTeamsPass.Visibility = Visibility.Visible;
        }

        public BitmapImage GetBitmapImageFromByteArray(byte[] bytesArr)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                stream.Write(bytesArr, 0, bytesArr.Length);
                stream.Position = 0;
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage returnImage = new BitmapImage();
                returnImage.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                returnImage.StreamSource = ms;
                returnImage.EndInit();

                return returnImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }


        public byte[] GetByteArrayFromBitmapImage(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }


        private void ImageEditLoadToolButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSaveImage_Click(object sender, RoutedEventArgs e)
        {
            var mbResult = DXMessageBox.Show("Do you want to Save this picture?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (mbResult == MessageBoxResult.Yes)
            {
                EmployeeRepo rep = new EmployeeRepo();
                var user = SYSTEM_STATIC.currentUser;

                var bmImg = (BitmapImage)userImage.Source;

                var empImg = (BitmapImage)empImage.Source;
                var empImg1 = (BitmapImage)empImage1.Source;
                var empImg2 = (BitmapImage)empImage2.Source;
                var empImg3 = (BitmapImage)empImage3.Source;
                var empImg4 = (BitmapImage)empImage4.Source;
                var empImg5 = (BitmapImage)empImage5.Source;

                if (empImg != null)
                {
                    var byteImg = GetByteArrayFromBitmapImage(bmImg);
                    if (user != null)
                    {

                        rep.AddUserImage(byteImg, user);
                    }

                    MessageBox.Show("Image saved successfully");
                }
                if (empImg1 != null)
                {
                    var byteImg = GetByteArrayFromBitmapImage(bmImg);
                    if (user != null)
                    {

                        rep.AddUserImage(byteImg, user);
                    }

                    MessageBox.Show("Image saved successfully");
                }
                if (empImg2 != null)
                {
                    var byteImg = GetByteArrayFromBitmapImage(bmImg);
                    if (user != null)
                    {

                        rep.AddUserImage(byteImg, user);
                    }

                    MessageBox.Show("Image saved successfully");
                }
                if (empImg3 != null)
                {
                    var byteImg = GetByteArrayFromBitmapImage(bmImg);
                    if (user != null)
                    {

                        rep.AddUserImage(byteImg, user);
                    }

                    MessageBox.Show("Image saved successfully");
                }
                if (empImg4 != null)
                {
                    var byteImg = GetByteArrayFromBitmapImage(bmImg);
                    if (user != null)
                    {

                        rep.AddUserImage(byteImg, user);
                    }

                    MessageBox.Show("Image saved successfully");
                }
                if (empImg5 != null)
                {
                    var byteImg = GetByteArrayFromBitmapImage(bmImg);
                    if (user != null)
                    {

                        rep.AddUserImage(byteImg, user);
                    }

                    MessageBox.Show("Image saved successfully");
                }


                //var byteImg =  getJPGFromImageControl(imgUser.Source as BitmapImage);
                if (bmImg != null)
                {
                    var byteImg = GetByteArrayFromBitmapImage(bmImg);



                    if (user != null)
                    {
                        //var img =  imgUser.Source;
                        /// var a = ImageToByteArray(imgUser);
                        rep.AddUserImage(byteImg, user);
                    }

                    MessageBox.Show("Image saved successfully");
                }
                else
                {
                    if (user != null)
                    {
                        rep.ClearUserImage(user);
                    }
                    MessageBox.Show("Image is Cleared!");

                }
            }
            else if (mbResult == MessageBoxResult.No)
            {
                return;
            }

        }

        private void BtnDelImage_Click(object sender, RoutedEventArgs e)
        {
            var mbResult = DXMessageBox.Show("Are you sure to want to delete the picture?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (mbResult == MessageBoxResult.Yes)
            {
                userImage.Source = null;
            }
            else if (mbResult == MessageBoxResult.No)
            {
                return;
            }
        }

        private void ImageEditLoadToolButton_Click_1(object sender, RoutedEventArgs e)
        {

        }



        private void BtnBrowseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;* *.bmp;";
            // DialogResult result = openFileDialog.ShowDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                var filePath = System.IO.Path.GetFullPath(openFileDialog.FileName);
                var bmImg = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
                userImage.Source = bmImg;
            }
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            if (gridTracker.Visibility == Visibility.Collapsed)
            {
                gridTracker.Visibility = Visibility.Visible;
            }
            else
            {
                gridTracker.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 500;
        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 1200;
        }

        private void GrdUsers_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            ViewInfo viewInfo = views.Find(x => x.Id == (grdUsers.SelectedItem as ViewInfo).Id);
            if (e.Column.FieldName == "Name" && e.IsGetData)

            {
                // if (e.GetListSourceFieldValue("employee.person.FName") != null|| e.GetListSourceFieldValue("employee.person.LName") != null )
                {


                    string fname = viewInfo.User.employee.person.FName;// Convert.ToString(e.GetListSourceFieldValue("usera.employee.person.FName"));
                    string lname = viewInfo.User.employee.person.LName;// Convert.ToString(e.GetListSourceFieldValue("usera.employee.person.LName"));

                    //DateTime date;

                    e.Value = fname + " " + lname;
                }
            }
        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnReject_Click(object sender, RoutedEventArgs e)
        {

        }
        public void GenerateUsersForComments()
        {
            frmDepartmentSelect frm = new frmDepartmentSelect();
            frm.ShowDialog();
            if (frm.department != null)
            {
                var dept = frm.department;
                var users = UsersRepo.getusersByDepartment(dept.Id);
                if (users != null)
                {
                    UsersForComments = users;

                    if (isProfile == true)
                    {
                        UsersForComments.Add(SYSTEM_STATIC.currentUser);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(EmployeeIdStr.Text))
                        {
                            var user = empRepo.GetUserFromEmployee(Convert.ToInt32(EmployeeIdStr.Text));
                            if (user != null)
                            {
                                UsersForComments.Add(user);

                            }
                        }
                    }
                }
            }
            //frm.Dispose();
        }
        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            GenerateUsersForComments();
            //department = cmbxDepartments.SelectedItem as Department;
            //if (department != null && department.Id != 0 /*&& department.users!=null&& department.users.Count!=0*/)
            //{
            frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersForComments, TransactionItemType.Employee);
            inputBox.ShowDialog();

            //}
            //else
            //{
            // frmInputBox inputBox = new frmInputBox();
            //inputBox.ShowDialog();
            //}

            if (EmployeeIdStr.Text != "")
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.comment != "" && EmployeeIdStr.Text != "")
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        foreach (var user in frmInputBox.Comment.TaggedList)
                        {
                            if (frmInputBox.FlagForTag == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Employee #" + EmployeeIdStr.Text, Convert.ToInt32(EmployeeIdStr.Text), TransactionItemType.Employee, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Employee #" + EmployeeIdStr.Text, Convert.ToInt32(EmployeeIdStr.Text), TransactionItemType.Employee, frmInputBox.comment, user.id, "New Comment ", null);
                        }
                        foreach (var user in frmInputBox.Comment.CCUsersList)
                        {
                            if (frmInputBox.FlagForCC == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Employee #" + EmployeeIdStr.Text, Convert.ToInt32(EmployeeIdStr.Text), TransactionItemType.Employee, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Employee #" + EmployeeIdStr.Text, Convert.ToInt32(EmployeeIdStr.Text), TransactionItemType.Employee, frmInputBox.comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    procurementRepo.Add(Convert.ToInt32(EmployeeIdStr.Text), TransactionItemType.Employee, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");
                    loadcomments();
                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (EmployeeIdStr.Text == "")
                {
                    DXMessageBox.Show("Kindly save Employee first to add a comment!");
                }

            }
        }
        public void loadcomments()
        {
            try
            {
                if (EmployeeIdStr.Text != "")
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(Convert.ToInt32(EmployeeIdStr.Text), TransactionItemType.Employee);
                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }
        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                grdComments.Visibility = Visibility.Visible;
                loadcomments();
            }
            else
                grdComments.Visibility = Visibility.Collapsed;
        }


        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        //void OnSearchStringToFilterCriteria(object sender, SearchStringToFilterCriteriaEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(e.SearchString))
        //        e.Filter = new BinaryOperator("Tags", e.SearchString.Trim().ToLower(), BinaryOperatorType.Equal);
        //    e.ApplyToColumnsFilter = true;
        //}
        //GridControl gc = new GridControl();
        //gc.DataController.VisibleRowCountChanged += DataController_VisibleRowCountChanged;  

        void DataController_VisibleRowCountChanged(object sender, EventArgs e)
        {
            //but it is not hitting when visiblerowcount change.  
        }
        //void DataController_VisibleRowCountChanged(object sender, EventArgs e)
        //{
        //    //but it is not hitting when visiblerowcount change.  
        //}
        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //var searchPanels = grdComments.ChildrenOfType<GridViewSearchPanel>();
            //foreach (var _searchPanel in searchPanels)
            //{
            //    (_searchPanel.DataContext as Telerik.Windows.Controls.GridView.SearchPanel.SearchViewModel).SearchText = this.SearchTextbox.Text;
            //}
            //var searchPanel = grdComments.ChildrenOfType<>().FirstOrDefault();
            //txtBox.SetBinding(TextBox.TextProperty, new Binding("SearchText") { Source = searchPanel.DataContext, Mode = BindingMode.TwoWay });
            ////department = cmbxDepartments.SelectedItem as Department;
            if (grdCommentss.SelectedItem != null)
            {
                var comment = grdCommentss.SelectedItem as CommentLog;
                if (Convert.ToInt32(EmployeeIdStr.Text) > 0)
                {
                    //        if (department != null && department.Id != 0 /*&& department.users!=null&& department.users.Count!=0*/)
                    //        {
                    //            frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartment(department.Id), comment, TransactionItemType.FixedAssets);
                    //            inputBox.ShowDialog();

                    //        }
                    //        else
                    //        {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                    // }

                    if (Convert.ToInt32(EmployeeIdStr.Text) > 0)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.comment != "" && EmployeeIdStr.Text != "")
                        {
                            if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Employee #" + EmployeeIdStr.Text, Convert.ToInt32(EmployeeIdStr.Text), TransactionItemType.Employee, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                                    else
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Employee #" + EmployeeIdStr.Text, Convert.ToInt32(EmployeeIdStr.Text), TransactionItemType.Employee, frmInputBox.comment, user.id, "New Comment ", null);
                                }
                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Fixed Asset #" + EmployeeIdStr.Text, Convert.ToInt32(EmployeeIdStr.Text), TransactionItemType.Employee, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                                    else
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Fixed Asset #" + EmployeeIdStr.Text, Convert.ToInt32(EmployeeIdStr.Text), TransactionItemType.Employee, frmInputBox.comment, user.id, "New Comment ", null);
                                }
                            }

                            procurementRepo.Add(Convert.ToInt32(EmployeeIdStr.Text), TransactionItemType.Employee, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);

                            loadcomments();
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (EmployeeIdStr.Text == "")
                        {
                            DXMessageBox.Show("Kindly save Employee first to add a comment!");
                        }

                    }
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
        }


        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach.Visibility == Visibility.Visible)
                grdAttach.Visibility = Visibility.Collapsed;
            else
            {
                grdAttach.Visibility = Visibility.Visible;
            }

        }


        private void BtnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {
            var empIdInt = Convert.ToInt32(EmployeeIdStr.Text);
            try
            {


                Button thisButton = (Button)sender;
                string str = thisButton.Tag.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        //Button thisButton = (Button)sender;

                        ERP_BL.Attach attachment = new ERP_BL.Attach(str);
                        var result = attachment.startDownload(str, TransactionItemType.Employee);
                        if (!string.IsNullOrEmpty(result.Item2))
                        {
                            System.Diagnostics.Process.Start(result.Item2);
                        }
                        else

                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                            });
                        }
                    });
                    thread.Start();
                }
                else
                    return;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), ex.ToString());


            }
        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
            else
            {
                grdAttachments.Visibility = Visibility.Visible;
            }
        }

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(EmployeeIdStr.Text))
            {
                empIdInt = Convert.ToInt32(EmployeeIdStr.Text);
                string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
                string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
                if (cmbCategory.SelectedItem != null)
                {
                    if (EmployeeIdStr.Text != "")
                    {
                        try
                        {
                            int CategoryId = (cmbCategory.SelectedItem as ZAS_ERP.cmbitem).id;
                            OpenFileDialog fileDialog = new OpenFileDialog();
                            fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";
                            fileDialog.Multiselect = false;
                            string sourceFile = @"";
                            string exePath = System.Environment.GetCommandLineArgs()[0];
                            string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                            destination += "Attachments\\Employee\\ToUpload\\";
                            //string destination = @"D:\MovedFiles\new\";
                            System.IO.Directory.CreateDirectory(destination);
                            if (fileDialog.ShowDialog() == true) // Test result.
                            {
                                imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                                btnAttachNew.ToolTip = "Uploading";
                                btnAttachNew.IsEnabled = true;

                                btnAttachment.Content = "Uploading File . . .";
                                sourceFile = fileDialog.FileName;
                                destination += Convert.ToInt32(EmployeeIdStr.Text) + "_" + TransactionItemType.Employee.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                                if (sourceFile.Length < 74)
                                {
                                    System.IO.File.Move(sourceFile, destination);

                                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                                    {
                                        ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                        var result = attachment.startUploading(TransactionItemType.Employee);
                                        if (result.Item1)
                                        {
                                            AttachmentsRepo repo = new AttachmentsRepo();
                                            //Attachment attachmen= new Attachment();
                                            repo.Add(System.IO.Path.GetFileName(result.Item2), empIdInt, TransactionItemType.Employee, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                            UsersRepo.Add(TransactionInfo.Attachment_Uploaded, empIdInt, 14, "Added a New attachment");

                                            this.Dispatcher.Invoke(() =>
                                            {
                                                treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(empIdInt, TransactionItemType.Employee);
                                                imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                                btnAttachNew.ToolTip = "Attach";
                                                btnAttachNew.IsEnabled = true;
                                                btnAttachment.Content = "Select";
                                            });
                                        }
                                    });
                                    thread.Start();
                                }
                                else
                                {
                                    MessageBox.Show("Invalid File name size");
                                    return;
                                }


                                //MessageBox.Show("Attachment Uploaded");


                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                            btnAttachNew.ToolTip = "Attach";
                            btnAttachNew.IsEnabled = true;
                        }
                        finally
                        {

                        }
                    }
                    else
                        return;
                }
                else
                {
                    DXMessageBox.Show("Select a Attachment Category in which you want to upload an attachment.", "Select Category", MessageBoxButton.OK, MessageBoxImage.Stop);
                }


            }

        }

        public void loadonEmployeedata()
        {
            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(Convert.ToInt32(EmployeeIdStr.Text), TransactionItemType.Employee);
            cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                var id = EmployeeIdStr.Text;
                if (!String.IsNullOrEmpty(id))
                {
                    var emp = empRepo.GetEmployee(Convert.ToInt32(id));

                    if (emp.employeeApproval.isVoid == false)
                    {

                        var res = DXMessageBox.Show("Do you sure to want to mark this Employee void?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (MessageBoxResult.Yes == res)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Employee") != null)
                            {
                                emp.employeeApproval.isVoid = true;
                                empRepo.updateEmployee(emp);
                                DXMessageBox.Show("Employee is being marked as void successfully", "Successfull", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                this.Close();
                            }
                            else
                            {
                                DXMessageBox.Show("You are not allowed to Mark the Employee void", "Unauthorize", MessageBoxButton.OK, MessageBoxImage.Stop);
                            }
                            return;
                        }
                    }

                    if (emp.employeeApproval.isVoid == true)
                    {

                        var res = DXMessageBox.Show("Do you sure to want to Un-Mark this Employee from void?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (MessageBoxResult.Yes == res)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Employee") != null)
                            {
                                emp.employeeApproval.isVoid = false;
                                empRepo.updateEmployee(emp);
                                DXMessageBox.Show("Employee is being marked as Un-void successfully", "Successfull", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                this.Close();
                            }
                            else
                            {
                                DXMessageBox.Show("You are not allowed to Un-Mark the Employee void", "Unauthorize", MessageBoxButton.OK, MessageBoxImage.Stop);
                            }
                            return;
                        }
                    }

                }



            }

            catch (Exception ex) { MessageBox.Show(ex.Message); }



        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }
        public void loadWorkExperience()
        {
            try
            {
                var expList = empRepo.GetAllWork();
                grdWorkExp.ItemsSource = expList;

                grdWorkExp.Columns.GetColumnByFieldName("Id").Visible = false;
                grdWorkExp.Columns.GetColumnByFieldName("employeeId").Visible = false;
                grdWorkExp.Columns.GetColumnByFieldName("employee").Visible = false;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void AddWork_Click(object sender, RoutedEventArgs e)
        {

            frmExperienceAdd frm = new frmExperienceAdd();
            List<EmployeeWorkExperience> workListOld = new List<EmployeeWorkExperience>();

            if (frmEmployeeCenter.editemp == 1)
            {
                //Generating Old Lists
                //var workList = grdWorkExp.VisibleItems;
                var workList = grdWorkExp.VisibleItems;

                if (workList != null)
                {
                    foreach (var item in workList)
                    {
                        var _item = (EmployeeWorkExperience)item;
                        workListOld.Add(_item);
                    }
                }
                //Showing dialog
                frm.ShowDialog();
                if (frm.isSave == true)
                {

                    if (frm.experience != null)
                    {
                        workListOld.Add(frm.experience);
                    }


                    grdWorkExp.ItemsSource = workListOld;
                }

            }


            else
            {

                frm.ShowDialog();
                if (frm.isSave == true)
                {
                    var list = frm.expList;
                    newListExp = new List<EmployeeWorkExperience>();
                    newListExp = (grdWorkExp.ItemsSource as List<EmployeeWorkExperience>) == null ? new List<EmployeeWorkExperience>() : grdWorkExp.ItemsSource as List<EmployeeWorkExperience>;

                    //if (list != null)
                    //{
                    //    qualDataGrid.ItemsSource = list;
                    //}
                    if (frm.experience != null)
                    {
                        newListExp.Add(frm.experience);
                    }


                    grdWorkExp.ItemsSource = newListExp;
                }

            }
        }

        private void BtnDeleteWork_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEditWork_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var workItem = (EmployeeWorkExperience)grdWorkExp.GetFocusedRow();
                if (workItem != null)
                {

                    frmExperienceAdd frm = new frmExperienceAdd();
                    //if (degItem.DegreeType == DegreeType.Certification)

                    frm.txtExpCompany.Text = workItem.Company;
                    frm.txtJobTitle.Text = workItem.JobTitle;
                    frm.txtJobDescription.Text = workItem.JobDescription;
                    frm.txtExpAddress.Text = workItem.employerAddress;
                    frm.txtExpPhone.Text = workItem.employerContact;
                    frm.dateStart.DateTime = workItem.DateFrom.Value;
                    frm.dateEnd.DateTime = workItem.DateTo.Value;
                    frm.chckIsLatest.IsChecked = workItem.isLatest;

                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDeletefWork_Click(object sender, RoutedEventArgs e)
        {
            tblWork.DeleteRow(tblWork.FocusedRowHandle);

        }

        private void BtnUserSignature_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;* *.bmp;";

            if (openFileDialog.ShowDialog() == true)
            {
                var fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                var filePath = System.IO.Path.GetFullPath(openFileDialog.FileName);
                var bmImg = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
                UserSignature.Source = bmImg;
            }

            EmployeeRepo rep = new EmployeeRepo();
            var user = SYSTEM_STATIC.currentUser;
            var bmSignature = (BitmapImage)UserSignature.Source;
            if (bmSignature != null)
            {
                var byteImg = GetByteArrayFromBitmapImage(bmSignature);
                if (user != null)
                {
                    rep.AddUserSignature(byteImg, user);
                }

                MessageBox.Show("Signature saved successfully");
            }
            else
            {
                if (user != null)
                {
                    //rep.ClearUserImage(user);
                }
                MessageBox.Show("Please Upload signature first!");

            }
        }

        private void BtnDelSignatureImage_Click(object sender, RoutedEventArgs e)
        {

            var mbResult = DXMessageBox.Show("Are you sure to want to delete the picture?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (mbResult == MessageBoxResult.Yes)
            {
                EmployeeRepo rep = new EmployeeRepo();
                var user = SYSTEM_STATIC.currentUser;
                if (user != null)
                {
                    rep.ClearUserSignature(user);
                    UserSignature.Source = null;
                }
                MessageBox.Show("Signature is Cleared!");

            }
            else if (mbResult == MessageBoxResult.No)
            {
                return;
            }

        }


        private void BtnBrowseEmpImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;* *.bmp;";
                // DialogResult result = openFileDialog.ShowDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    var fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                    var filePath = System.IO.Path.GetFullPath(openFileDialog.FileName);
                    var bmImg = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
                    empImage.Source = bmImg;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BtnDelEmpImage_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    var mbResult = DXMessageBox.Show("Are you sure to want to delete the picture?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            //    if (mbResult == MessageBoxResult.Yes)
            //    {
            //        EmployeeRepo rep = new EmployeeRepo();
            //        var user = SYSTEM_STATIC.currentUser;
            //        if (user != null)
            //        {
            //            rep.ClearUserSignature(user);
            //            empImage.Source = null;
            //        }
            //        MessageBox.Show("Signature is Cleared!");

            //    }
            //    else if (mbResult == MessageBoxResult.No)
            //    {
            //        return;
            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }
        private void BtnBrowseEmpImage1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;* *.bmp;";
                // DialogResult result = openFileDialog.ShowDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    var fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                    var filePath = System.IO.Path.GetFullPath(openFileDialog.FileName);
                    var bmImg = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
                    empImage1.Source = bmImg;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BtnDelEmpImage1_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BtnBrowseEmp2Image_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;* *.bmp;";
                // DialogResult result = openFileDialog.ShowDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    var fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                    var filePath = System.IO.Path.GetFullPath(openFileDialog.FileName);
                    var bmImg = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
                    empImage2.Source = bmImg;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BtnDelEmp2Image_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnBrowseEmp3Image_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;* *.bmp;";
                // DialogResult result = openFileDialog.ShowDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    var fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                    var filePath = System.IO.Path.GetFullPath(openFileDialog.FileName);
                    var bmImg = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
                    empImage3.Source = bmImg;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BtnDelEmp3Image_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnBrowseEmp4Image_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;* *.bmp;";
                // DialogResult result = openFileDialog.ShowDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    var fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                    var filePath = System.IO.Path.GetFullPath(openFileDialog.FileName);
                    var bmImg = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
                    empImage4.Source = bmImg;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BtnDelEmp4Image_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnBrowseEmp5Image_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;* *.bmp;";
                // DialogResult result = openFileDialog.ShowDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    var fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                    var filePath = System.IO.Path.GetFullPath(openFileDialog.FileName);
                    var bmImg = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
                    empImage5.Source = bmImg;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BtnDelEmp5Image_Click(object sender, RoutedEventArgs e)
        {
            empImage5.Clear();
        }




        private void EmpImagePreiviw_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {

                if (empImage.Source != null && isEmp == true)
                {
                    frmPreviewEmployePicture frmPreview = new frmPreviewEmployePicture(editEmpId, 0);
                    frmPreview.WindowState = WindowState.Maximized;
                    frmPreview.ShowDialog();
                    return;
                }



                if (empImage1.Source != null && isEmp1 == true)
                {
                    frmPreviewEmployePicture frmPreview = new frmPreviewEmployePicture(editEmpId, 1);
                    frmPreview.WindowState = WindowState.Maximized;
                    frmPreview.ShowDialog();
                    return;
                }
                if (empImage2.Source != null && isEmp2 == true)
                {
                    frmPreviewEmployePicture frmPreview = new frmPreviewEmployePicture(editEmpId, 2);
                    frmPreview.WindowState = WindowState.Maximized;
                    frmPreview.ShowDialog();
                    return;
                }



                if (empImage3.Source != null && isEmp3 == true)
                {
                    frmPreviewEmployePicture frmPreview = new frmPreviewEmployePicture(editEmpId, 3);
                    frmPreview.WindowState = WindowState.Maximized;
                    frmPreview.ShowDialog();
                    return;
                }


                if (empImage4.Source != null && isEmp4 == true)
                {
                    frmPreviewEmployePicture frmPreview = new frmPreviewEmployePicture(editEmpId, 4);
                    frmPreview.WindowState = WindowState.Maximized;
                    frmPreview.ShowDialog();
                    return;
                }

                if (empImage5.Source != null && isEmp5 == true)
                {
                    frmPreviewEmployePicture frmPreview = new frmPreviewEmployePicture(editEmpId, 5);
                    frmPreview.WindowState = WindowState.Maximized;
                    frmPreview.ShowDialog();
                    return;
                }
            }
            catch (Exception)
            {

                throw;
            }



        }
        private void EmpImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (isEdit == false)
                {
                    if (empImage.Source != null)
                    {
                        empImagePreiviw.Source = empImage.Source;
                    }
                }
                else
                {
                    if (empImage.Source != null)
                    {
                        empImagePreiviw.Source = empImage.Source;
                        isEmp = true;
                        isEmp1 = false;
                        isEmp2 = false;
                        isEmp3 = false;
                        isEmp4 = false;
                        isEmp5 = false;
                    }

                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void EmpImage1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (isEdit == false)
                {
                    if (empImage1.Source != null)
                    {
                        empImagePreiviw.Source = empImage1.Source;
                    }
                }
                else
                {
                    if (empImage1.Source != null)
                    {
                        //empImagePreiviw.Source = null;
                        empImagePreiviw.Source = empImage1.Source;
                        isEmp = false;
                        isEmp1 = true;
                        isEmp2 = false;
                        isEmp3 = false;
                        isEmp4 = false;
                        isEmp5 = false;
                    }

                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void EmpImage2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {

                if (isEdit == false)
                {
                    if (empImage2.Source != null)
                    {
                        empImagePreiviw.Source = empImage2.Source;
                    }
                }
                else
                {
                    if (empImage2.Source != null)
                    {
                        empImagePreiviw.Source = empImage2.Source;
                        isEmp = false;
                        isEmp1 = false;
                        isEmp2 = true;
                        isEmp3 = false;
                        isEmp4 = false;
                        isEmp5 = false;
                    }

                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void EmpImage3_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (isEdit == false) //Add
                {
                    if (empImage3.Source != null)
                    {
                        empImagePreiviw.Source = empImage3.Source;
                    }
                }
                else //update
                {
                    if (empImage3.Source != null)
                    {
                        empImagePreiviw.Source = empImage3.Source;
                        isEmp = false;
                        isEmp1 = false;
                        isEmp2 = false;
                        isEmp3 = true;
                        isEmp4 = false;
                        isEmp5 = false;
                    }
                    //if (empImage3.Source != null)
                    //{
                    //    empImagePreiviw.Source = null;
                    //    if(emp.person.personPhotos.Count > 0)
                    //    {
                    //        foreach(var _img in emp.person.personPhotos)
                    //        {
                    //            switch (_img.PhotoName)
                    //            {
                    //                case "Photo3":
                    //                    var image = GetBitmapImageFromByteArray(_img.EmployeePhoto);
                    //                    empImagePreiviw.Source = image;
                    //                    isEmp = false;
                    //                    isEmp1 = false;
                    //                    isEmp2 = false;
                    //                    isEmp3 = true;
                    //                    isEmp4 = false;
                    //                    isEmp5 = false;
                    //                    break;
                    //            }
                    //        }
                    //    }
                    //    if (emp.person.EmployeePhotoFront != null)
                    //    {
                    //        var byteImg = emp.person.EmployeePhotoFront;
                    //        if (byteImg != null)
                    //        {
                    //            var image = GetBitmapImageFromByteArray(byteImg);
                    //            empImagePreiviw.Source = image;
                    //            isEmp = false;
                    //            isEmp1 = false;
                    //            isEmp2 = false;
                    //            isEmp3 = true;
                    //            isEmp4 = false;
                    //            isEmp5 = false;

                    //        }

                    //    }
                    //}
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void EmpImage4_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (isEdit == false)
                {
                    if (empImage4.Source != null)
                    {
                        empImagePreiviw.Source = empImage4.Source;
                    }
                }
                else
                {
                    if (empImage4.Source != null)
                    {
                        empImagePreiviw.Source = empImage4.Source;
                        isEmp = false;
                        isEmp1 = false;
                        isEmp2 = false;
                        isEmp3 = false;
                        isEmp4 = true;
                        isEmp5 = false;
                    }

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void EmpImage5_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (isEdit == false)
                {
                    if (empImage5.Source != null)
                    {
                        empImagePreiviw.Source = empImage5.Source;
                    }
                }
                else
                {
                    if (empImage5.Source != null)
                    {
                        empImagePreiviw.Source = empImage5.Source;
                        isEmp = false;
                        isEmp1 = false;
                        isEmp2 = false;
                        isEmp3 = false;
                        isEmp4 = false;
                        isEmp5 = true;
                    }

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void imgLeftToRight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRight.Width = 25;
            imgLeftToRight.Height = 25;
        }

        private void imgLeftToRight_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRight.Width = 32;
            imgLeftToRight.Height = 32;

            try
            {
                var selectedItem = gridCompany.SelectedItem as Company;

                if (selectedItem != null)
                {
                    allCompanyList.Remove(selectedItem);
                    if (!selectedCompanyList.Contains(selectedItem))
                        selectedCompanyList.Add(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First!");
                }
                gridCompanySelected.RefreshData();
                gridCompany.RefreshData();
                gridCompany.SelectedItem = null;
                gridCompanySelected.SelectedItem = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnLeftMoveCustomer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeft.Width = 25;
            imgRightToLeft.Height = 25;
        }

        private void btnLeftMoveCustomer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeft.Width = 32;
            imgRightToLeft.Height = 32;

            try
            {
                var selectedItem = gridCompanySelected.SelectedItem as Company;

                if (selectedItem != null)
                {
                    if (!allCompanyList.Contains(selectedItem))
                        allCompanyList.Add(selectedItem);
                    selectedCompanyList.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First!");
                }
                gridCompanySelected.RefreshData();
                gridCompany.RefreshData();
                gridCompany.SelectedItem = null;
                gridCompanySelected.SelectedItem = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void imgLeftToRightDept_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightDept.Width = 25;
            imgLeftToRightDept.Height = 25;
        }

        private void imgLeftToRightDept_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightDept.Width = 32;
            imgLeftToRightDept.Height = 32;

            try
            {
                var selectedItem = gridDepartment.SelectedItem as Department;

                if (selectedItem != null)
                {
                    allDepartmentList.Remove(selectedItem);
                    if (!selectedDepartmentList.Contains(selectedItem))
                        selectedDepartmentList.Add(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First!");
                }
                gridDepartmentSelected.ItemsSource = selectedDepartmentList;
                gridDepartmentSelected.RefreshData();

                gridDepartment.ItemsSource = allDepartmentList;
                gridDepartment.RefreshData();
                gridDepartment.SelectedItem = null;
                gridDepartmentSelected.SelectedItem = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnLeftMoveCustomerDept_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftDept.Width = 32;
            imgRightToLeftDept.Height = 32;

            try
            {
                var selectedItem = gridDepartmentSelected.SelectedItem as Department;

                if (selectedItem != null)
                {
                    if (!allDepartmentList.Contains(selectedItem))
                        allDepartmentList.Add(selectedItem);
                    selectedDepartmentList.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First!");
                }
                gridDepartmentSelected.ItemsSource = selectedDepartmentList;
                gridDepartmentSelected.RefreshData();

                gridDepartment.ItemsSource = allDepartmentList;
                gridDepartment.RefreshData();
                gridDepartment.SelectedItem = null;
                gridDepartmentSelected.SelectedItem = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnLeftMoveCustomerDept_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftDept.Width = 25;
            imgRightToLeftDept.Height = 25;
        }

        private void imgLeftToRightAdminBill_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightAdminBill.Width = 25;
            imgLeftToRightAdminBill.Height = 25;
        }

        private void imgLeftToRightAdminBill_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightAdminBill.Width = 32;
            imgLeftToRightAdminBill.Height = 32;

            try
            {
                var selectedItem = gridAdminBillCompany.SelectedItem as Company;

                if (selectedItem != null)
                {
                    allAdminBillCompanyList.Remove(selectedItem);
                    if (!allAdminBillCompanyList.Contains(selectedItem))
                        selectedAdminBillCompanyList.Add(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First!");
                }
                gridAdminBillCompanySelected.RefreshData();
                gridAdminBillCompany.RefreshData();
                gridAdminBillCompany.SelectedItem = null;
                gridAdminBillCompanySelected.SelectedItem = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnLeftMoveCustomerAdminBill_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftAdminBill.Width = 32;
            imgRightToLeftAdminBill.Height = 32;

            try
            {
                var selectedItem = gridAdminBillCompanySelected.SelectedItem as Company;

                if (selectedItem != null)
                {
                    if (!allAdminBillCompanyList.Contains(selectedItem))
                        allAdminBillCompanyList.Add(selectedItem);
                    selectedAdminBillCompanyList.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First!");
                }
                gridAdminBillCompanySelected.RefreshData();
                gridAdminBillCompany.RefreshData();
                gridAdminBillCompany.SelectedItem = null;
                gridAdminBillCompanySelected.SelectedItem = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnLeftMoveCustomerAdminBill_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftAdminBill.Width = 25;
            imgRightToLeftAdminBill.Height = 25;
        }

        private void imgLeftToRightTask_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightTask.Width = 25;
            imgLeftToRightTask.Height = 25;
        }

        private void imgLeftToRightTask_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightTask.Width = 32;
            imgLeftToRightTask.Height = 32;

            try
            {
                var selectedItem = gridTaskCompany.SelectedItem as Company;

                if (selectedItem != null)
                {
                    allTaskCompanyList.Remove(selectedItem);
                    if (!allTaskCompanyList.Contains(selectedItem))
                        selectedTaskCompanyList.Add(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First!");
                }
                gridTaskCompanySelected.RefreshData();
                gridTaskCompany.RefreshData();
                gridTaskCompany.SelectedItem = null;
                gridTaskCompanySelected.SelectedItem = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnLeftMoveCustomerTask_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftTask.Width = 25;
            imgRightToLeftTask.Height = 25;
        }

        private void btnLeftMoveCustomerTask_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftTask.Width = 32;
            imgRightToLeftTask.Height = 32;

            try
            {
                var selectedItem = gridTaskCompanySelected.SelectedItem as Company;

                if (selectedItem != null)
                {
                    if (!allTaskCompanyList.Contains(selectedItem))
                        allTaskCompanyList.Add(selectedItem);
                    selectedTaskCompanyList.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First!");
                }
                gridTaskCompanySelected.RefreshData();
                gridTaskCompany.RefreshData();
                gridTaskCompany.SelectedItem = null;
                gridTaskCompanySelected.SelectedItem = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void imgLeftToRightCOA_MouseDown(object sender, MouseButtonEventArgs e)
        {

            imgLeftToRightCOA.Width = 25;
            imgLeftToRightCOA.Height = 25;
        }

        private void imgLeftToRightCOA_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightCOA.Width = 32;
            imgLeftToRightCOA.Height = 32;

            try
            {
                var selectedItem = gridCOACompany.SelectedItem as Company;

                if (selectedItem != null)
                {
                    allCOACompanyList.Remove(selectedItem);
                    if (!allCOACompanyList.Contains(selectedItem))
                        selectedCOACompanyList.Add(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First!");
                }
                gridCOACompanySelected.RefreshData();
                gridCOACompany.RefreshData();
                gridCOACompany.SelectedItem = null;
                gridCOACompanySelected.SelectedItem = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void imgRightToLeftCOA_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftCOA.Width = 25;
            imgRightToLeftCOA.Height = 25;
        }

        private void imgRightToLeftCOA_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftCOA.Width = 32;
            imgRightToLeftCOA.Height = 32;

            try
            {
                var selectedItem = gridCOACompanySelected.SelectedItem as Company;

                if (selectedItem != null)
                {
                    if (!allCOACompanyList.Contains(selectedItem))
                        allCOACompanyList.Add(selectedItem);
                    selectedCOACompanyList.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First!");
                }
                gridCOACompanySelected.RefreshData();
                gridCOACompany.RefreshData();
                gridCOACompany.SelectedItem = null;
                gridCOACompanySelected.SelectedItem = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void ImgLeftToRightPettyCash_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ImgLeftToRightPettyCash_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var selectedItem = gridPettyCashCompany.SelectedItem as Company;

                if (selectedItem != null)
                {
                    allPettyCashCompanyList.Remove(selectedItem);
                    if (!allPettyCashCompanyList.Contains(selectedItem))
                        selectedPettyCashCompanyList.Add(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First!");
                }
                gridPettyCashCompanySelected.RefreshData();
                gridPettyCashCompany.RefreshData();
                gridPettyCashCompany.SelectedItem = null;
                gridPettyCashCompanySelected.SelectedItem = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnLeftMovePettyCash_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnLeftMovePettyCash_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var selectedItem = gridPettyCashCompanySelected.SelectedItem as Company;

                if (selectedItem != null)
                {
                    if (!allPettyCashCompanyList.Contains(selectedItem))
                        allPettyCashCompanyList.Add(selectedItem);
                    selectedPettyCashCompanyList.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First!");
                }
                gridPettyCashCompanySelected.RefreshData();
                gridPettyCashCompany.RefreshData();
                gridTaskCompany.SelectedItem = null;
                gridTaskCompanySelected.SelectedItem = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void tableViewMemo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void tableViewMemo_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {

        }

        private void grdMemo_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                string userName = "";
                var row = grdMemo.GetRowByListIndex(e.ListSourceRowIndex) as Memo;
                switch (e.Column.FieldName)
                {
                    case "CreatedByy":
                        if (row.createdBy != null && row.createdBy.employee != null && row.createdBy.employee.person != null)
                        {
                            userName = row.createdBy.employee.person.FName + " " + row.createdBy.employee.person.LName;
                        }
                        e.Value = userName;
                        break;
                    case "CreatedForr":
                        if (row.createdFor != null && row.createdFor.employee != null && row.createdFor.employee.person != null)
                        {
                            userName = row.createdFor.employee.person.FName + " " + row.createdFor.employee.person.LName;
                        }
                        e.Value = userName;
                        break;
                }
            }
        }

        private void grdMemo_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {

        }

        private void grdMemo_FilterChanged(object sender, RoutedEventArgs e)
        {

        }

        private void ColorEditStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}

