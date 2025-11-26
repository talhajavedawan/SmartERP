import { BusinessType } from "./shared/services/company-center/company/company.service";
import { Routes } from "@angular/router";
import { MainComponent } from "../app/shared/main/main.component";
import { LstEmployeeComponent } from "./Modules/hrm/employee/lst-employee/lst-employee.component";
import { EmployeeFormComponent } from "./Modules/hrm/employee/employee-form/employee-form.component";
import { LeavesComponent } from "./Modules/leaves/leaves.component";
import { CountriesComponent } from "./Modules/location/country/country.component";
import { StateComponent } from "./Modules/location/state/state.component";
import { CityComponent } from "./Modules/location/city/city.component";
import { UserFormComponent } from "./Modules/core/Users/user-information/user-information.component";
import { RolesComponent } from "./Modules/core/roles/roles.component";
import { AuthGuard } from "./shared/services/authentication/auth.guard";
import { RegisterComponent } from "./Auth/Register/register.component";
import { LoginComponent } from "./Auth/Login/login.component";
import { LstDepartmentComponent } from "./Modules/company-center/department/lst-department/lst-department.component";
import { FrmDepartmentComponent } from "./Modules/company-center/department/frm-department/frm-department.component";
import { LstGroupComponent } from "./Modules/company-center/groups/lst-group/lst-group.component";
import { LstCompanyComponent } from "./Modules/company-center/company/company-list/company-list.component";
import { CompanyCreateUpdateFormComponent } from "./Modules/company-center/company/company-create-update-form/company-create-update-form.component";
import { IndustriesComponent } from "./Modules/company-center/company/list/industryType/industries/industries.component";
import { BusinessesComponent } from "./Modules/company-center/company/list/businessType/businesses/businesses.component";
import { AgGridLayoutComponent } from "./shared/components/grids-layout/grids-layout.component";
import { FrmStateComponent } from "./Modules/location/state/frm-state/frm-state.component";
import { FrmCityComponent } from "./Modules/location/city/frm-city/frm-city.component";
import { ZoneComponent } from "./Modules/location/zone/zone.component";
import { FrmZoneComponent } from "./Modules/location/zone/frm-zone/frm-zone.component";
import { FrmRoleComponent } from "./Modules/core/roles/frm-role/frm-role.component";
import { FrmIndustriesComponent } from "./Modules/company-center/company/list/industryType/frm-industries/frm-industries.component";
import { FrmBusinessesComponent } from "./Modules/company-center/company/list/businessType/frm-businesses/frm-businesses.component";
import { FrmUserComponent } from "./Modules/core/Users/frm-user/frm-user.component";
import { FrmGroupComponent } from "./Modules/company-center/groups/frm-group/frm-group.component";
import { FrmCountryComponent } from "./Modules/location/country/frm-country/frm-country.component";
import { FrmCustomerComponent } from "./Modules/customer-center/frm-customer/frm-customer.component";
import { LstCustomerComponent } from "./Modules/customer-center/lst-customer/lst-customer.component";
import { LstJobTitleComponent } from "./Modules/hrm/employee/employee-list/lst-job-title/lst-job-title.component";
import { FrmJobTitleComponent } from "./Modules/hrm/employee/employee-list/frm-job-title/frm-job-title.component";
import { FrmCurrencyComponent } from "./Modules/location/currency/frm-currency/frm-currency.component";
import { LstCurrencyComponent } from "./Modules/location/currency/lst-currency/lst-currency.component";
import { FrmVendorNatureComponent } from "./Modules/vendor/vendor-nature/frm-vendor-nature/frm-vendor-nature.component";
import { LstVendorComponent } from "./Modules/vendor/vendor-center/lst-vendor/lst-vendor.component";
import { FrmVendorComponent } from "./Modules/vendor/vendor-center/frm-vendor/frm-vendor.component";
import { LstVendorNatureComponent } from "./Modules/vendor/vendor-nature/lst-vendor-nature/lst-vendor-nature.component";

import { LstEmployeeStatusComponent } from "./Modules/core/statuses/employeeStatus/lst-employee-status/lst-employee-status.component";
import { FrmEmployeeStatusComponent } from "./Modules/core/statuses/employeeStatus/frm-employee-status/frm-employee-status.component";
import { LeaveBalanceComponent } from "./Modules/leaves/pages/leave-balance/leave-balance.component";
//import { LeaveListPlaceholderComponent } from "../shared/services/leaves/leave-list-placeholder.component";
import { ApplyLeaveComponent } from "./Modules/leaves/pages/apply-leave/apply-leave.component";
import { LstVendorContactComponent } from "./Modules/vendor/vendor-contact/lst-vendor-contact/lst-vendor-contact.component";
import { FrmVendorContactComponent } from "./Modules/vendor/vendor-contact/frm-vendor-contact/frm-vendor-contact.component";
import { ChangeUserPasswordComponent } from "./Modules/core/Users/change-user-password/change-user-password.component";
import { DtlVendorComponent } from "./Modules/vendor/vendor-center/vendor-detail/dtl-vendor.component";
import { InquiryFormComponent } from "./Modules/procurement/inquiry/components/inquiry-form/inquiry-form.component";
import { LstEmployeeStatusClassComponent } from "./Modules/core/statusClass/employeeStatusClass/lst-employee-status-class/lst-employee-status-class.component";
import { FrmEmployeeStatusClassComponent } from "./Modules/core/statusClass/employeeStatusClass/frm-employee-status-class/frm-employee-status-class.component";
import { NotificationPanelComponent } from "./Modules/notifications/notification-panel.component";
import { NotificationPanelDemoComponent } from "./Modules/notifications/notification-panel-demo.component";

export const routes: Routes = [
  { path: "", component: MainComponent, pathMatch: "full" },

  // Auth
  {
    path: "register",
    component: RegisterComponent,
    data: { title: "Register" },
  },

  { path: "login", component: LoginComponent, data: { title: "Login" } },
  {
    path: "User-info",
    component: UserFormComponent,
  },


  {
    path: "change-password",
    component: ChangeUserPasswordComponent,
    canActivate: [AuthGuard],
    data: { title: "change-password" },
  },

  {
    path: "",
    loadComponent: () =>
      import("./Theme/layout").then((m) => m.DefaultLayoutComponent),
    data: {
      title: "Home",
    },
    children: [
      {
        path: "dashboard",
        loadChildren: () =>
          import("./Theme/views/dashboard/routes").then((m) => m.routes),
        canActivate: [AuthGuard],
        data: { title: "Dashboard" },
      },
      {
        path: "theme",
        loadChildren: () =>
          import("./Theme/views/theme/routes").then((m) => m.routes),
        canActivate: [AuthGuard],
      },
      {
        path: "widgets",
        loadChildren: () =>
          import("./Theme/views/widgets/routes").then((m) => m.routes),
        canActivate: [AuthGuard],
      },

      {
        path: "leaves",
        component: LeavesComponent,
        children: [
          // {
          //   path: "",
          //   pathMatch: "full",
          //   component: LeaveListPlaceholderComponent,
          // },
          { path: "apply", component: ApplyLeaveComponent },
          { path: "balance", component: LeaveBalanceComponent },
        ],
      },
      // ERP Modules (all protected)
      {
        path: "company-list",
        component: LstCompanyComponent,
        canActivate: [AuthGuard],
        data: { title: "Company List" },
      },
      {
        path: "company-create-update",
        component: CompanyCreateUpdateFormComponent,
        canActivate: [AuthGuard],
        data: { title: "Create Company" },
      },
      {
        path: "leaves",
        component: LeavesComponent,
        canActivate: [AuthGuard],
        data: { title: "Leaves" },
      },
      {
        path: "company-create-update/:id",
        component: CompanyCreateUpdateFormComponent,
        canActivate: [AuthGuard],
        data: { title: "Update Company" },
      },

      // ERP Modules (all protected)
      {
        path: "company-list",
        component: LstCompanyComponent,
        canActivate: [AuthGuard],
        data: { title: "Company List" },
      },
      {
        path: "company-create-update",
        component: CompanyCreateUpdateFormComponent,
        canActivate: [AuthGuard],
        data: { title: "Create Company" },
      },
      {
        path: "company-create-update/:id",
        component: CompanyCreateUpdateFormComponent,
        canActivate: [AuthGuard],
        data: { title: "Update Company" },
      },

      {
        path: "industries",
        component: IndustriesComponent,
        canActivate: [AuthGuard],
        data: { title: "Industries List" },
      },
      {
        path: "industry-form",
        component: FrmIndustriesComponent,
        canActivate: [AuthGuard],
        data: { title: "Industry Form" },
      },
      {
        path: "businesses",
        component: BusinessesComponent,
        canActivate: [AuthGuard],
        data: { title: "Business List" },
      },
      {
        path: "business-form",
        component: FrmBusinessesComponent,
        canActivate: [AuthGuard],
        data: { title: "Business Form" },
      },
      {
        path: "job-titles",
        component: LstJobTitleComponent,
        canActivate: [AuthGuard],
        data: { title: "Job Titles" },
      },
      {
        path: "Job-title-form",
        component: FrmJobTitleComponent,
        canActivate: [AuthGuard],
        data: { title: "Job Title Form" },
      },
      {
        path: "lst-employee",
        component: LstEmployeeComponent,
        canActivate: [AuthGuard],
        data: { title: "Employee List" },
      },
      {
        path: "employee-form",
        component: EmployeeFormComponent,
        canActivate: [AuthGuard],
        data: { title: "Employee Form" },
      },
      {
        path: "groups",
        component: LstGroupComponent,
        canActivate: [AuthGuard],
        data: { title: "Group List" },
      },
      {
        path: "frmGroups",
        component: FrmGroupComponent,
        canActivate: [AuthGuard],
        data: { title: "Group List" },
      },

      {
        path: "layout",
        component: AgGridLayoutComponent,
        canActivate: [AuthGuard],
      },

      {
        path: "country",
        component: CountriesComponent,
        canActivate: [AuthGuard],
        data: { title: "Country List" },
      },
      {
        path: "frmCountry",
        component: FrmCountryComponent,
        canActivate: [AuthGuard],
        data: { title: "Country Form" },
      },

      {
        path: "zone",
        component: ZoneComponent,
        canActivate: [AuthGuard],
        data: { title: "Zone List" },
      },
      {
        path: "frmZone",
        component: FrmZoneComponent,
        canActivate: [AuthGuard],
        data: { title: "Zone Form" },
      },
      {
        path: "state",
        component: StateComponent,
        canActivate: [AuthGuard],
        data: { title: "State List" },
      },
      {
        path: "frmState",
        component: FrmStateComponent,
        canActivate: [AuthGuard],
        data: { title: "State Form" },
      },
      {
        path: "city",
        component: CityComponent,
        canActivate: [AuthGuard],
        data: { title: "City List" },
      },
      {
        path: "frmCity",
        component: FrmCityComponent,

        canActivate: [AuthGuard],
      },

      {
        path: "frmCurrency",
        component: FrmCurrencyComponent,
        canActivate: [AuthGuard],
        data: { title: "Currency Form" },
      },
      {
        path: "frmCurrency/:id",
        component: FrmCurrencyComponent,
        canActivate: [AuthGuard],
        data: { title: "Edit Currency" },
      },

      {
        path: "lstCurrency",
        component: LstCurrencyComponent,
        canActivate: [AuthGuard],

        data: { title: "Currency Lst" },
      },
      {
        path: "users",
        component: UserFormComponent,
        canActivate: [AuthGuard],
        data: { title: "Users' List" },
      },
      {
        path: "user-form",
        component: FrmUserComponent,
        canActivate: [AuthGuard],
        data: { title: "User Form" },
      },
      {
        path: "roles",
        component: RolesComponent,
        canActivate: [AuthGuard],
        data: { title: "Roles" },
      },
      {
        path: "frmRoles",
        component: FrmRoleComponent,
        canActivate: [AuthGuard],
        data: { title: "Role Form" },
      },
      {
        path: "lst-department",
        component: LstDepartmentComponent,
        canActivate: [AuthGuard],
        data: { title: "Department List" },
      },
      {
        path: "frm-department",
        component: FrmDepartmentComponent,
        canActivate: [AuthGuard],
        data: { title: "Department Form" },
      },

      {
        path: "frm-department/:id",
        component: FrmDepartmentComponent,
        canActivate: [AuthGuard],
        data: { title: "Update Department" },
      },
      {
        path: "frm-vendor",
        component: FrmVendorComponent,
        canActivate: [AuthGuard],
        data: { title: "Vendor Form" },
      },
      {
        path: "frm-vendor/:id",
        component: FrmVendorComponent,
        canActivate: [AuthGuard],
        data: { title: "Edit Vendor" },
      },
      {
        path: "lst-vendor",
        component: LstVendorComponent,
        canActivate: [AuthGuard],
        data: { title: "Vendor List" },
      },

   {
  path: "dtl-vendor/:id",
  component: DtlVendorComponent,
  canActivate: [AuthGuard],
  data: { title: "Vendor Details" },
},


      {
        path: "frm-vendor-nature",
        component: FrmVendorNatureComponent,
        canActivate: [AuthGuard],
        data: { title: "Vendor-nature Form" },
      },
      {
        path: "lst-vendor-nature",
        component: LstVendorNatureComponent,
        canActivate: [AuthGuard],
        data: { title: "Vendor-nature List" },
      },
      {
        path: "frm-vendor-contact",
        component: FrmVendorContactComponent,
        canActivate: [AuthGuard],
        data: { title: "Vendor-nature List" },
      },
      {
        path: "lst-vendor-contact",
        component: LstVendorContactComponent,
        canActivate: [AuthGuard],
        data: { title: "Vendor-contact List" },
      },

      {
        path: "lst-customer",
        component: LstCustomerComponent,
        canActivate: [AuthGuard],

        data: { title: "Customer List" },
      },
      {
        path: "frm-customer",
        component: FrmCustomerComponent,
        canActivate: [AuthGuard],

        data: { title: "Customer Form" },
      },
      {
        path: "frm-customer/:id",
        component: FrmCustomerComponent,
        canActivate: [AuthGuard],

        data: { title: "Edit Customer" },
      },
      {
        path: "inquiry-form",
        component: InquiryFormComponent,
        canActivate: [AuthGuard],
        data: { title: "Inquiry Form" },
      },
      {
        path: "notifications",
        component: NotificationPanelComponent,
        canActivate: [AuthGuard],
        data: { title: "Notifications" },
      },
      {
        path: "notifications-demo",
        component: NotificationPanelDemoComponent,
        data: { title: "Notifications Demo" },
      },
      {
        path: "employeeStatus-list",
        component: LstEmployeeStatusComponent,
        canActivate: [AuthGuard],
        data: { title: "Employee Status List" },
      },
      {
        path: "employeeStatus-form",
        component: FrmEmployeeStatusComponent,
        canActivate: [AuthGuard],
        data: { title: "EmployeeStatusForm" },
      },
      {
        path: "employeeStatusClass-list",
        component: LstEmployeeStatusClassComponent,
        canActivate: [AuthGuard],
        data: { title: "Employee Status Class List" },
      },
      {
        path: "employeeStatusClass-form",
        component: FrmEmployeeStatusClassComponent,
        canActivate: [AuthGuard],
        data: { title: "Employee Status Class Form" },
      },
    ],
  },

  { path: "**", redirectTo: "login" },
];

//route
