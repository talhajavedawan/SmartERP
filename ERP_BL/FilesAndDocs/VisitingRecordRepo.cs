using ERP_BL.Countryy;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.FilesAndDocs
{
    public class VisitingRecordRepo
    {
        DBContextERP context = new DBContextERP();

        /// <summary>
        /// Get last Payment
        /// </summary>
        /// <returns></returns>
        public int GetLastTransactionId(string type)
        {
            if(type == "Traveling Record")
            {
                var travelingRecord = context.travelingRecords.OrderByDescending(q => q.Id).FirstOrDefault();
                if (travelingRecord != null)
                    return travelingRecord.transactionGroupId;
                else
                    return 0;
            }
            else
            {
                var visitingCountry = context.visitingCountries.OrderByDescending(q => q.Id).FirstOrDefault();
                if (visitingCountry != null)
                    return visitingCountry.transactionGroupId;
                else
                    return 0;
            }
            
        }

        int intGroupId;
        private void GroupIdCalculation(string type)
        {
            string groupId;
            string year;
            string month;
            string id;

            var lastPaymentId = GetLastTransactionId(type);
            if (lastPaymentId == 0)
            {
                year = DateTime.Now.Year.ToString();
                month = DateTime.Now.Month.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                id = "1";
                groupId = year + month + id;
            }
            else
            {
                // var last = history.Last();
                var lastId = lastPaymentId/*.transactionGroupId*/;
                string fullId = lastId.ToString();
                int length = fullId.Length;
                //length = length - 1;

                year = fullId.Substring(0, 4);
                if (year != DateTime.Now.Year.ToString())
                {
                    year = DateTime.Now.Year.ToString();
                }
                month = fullId.Substring(4, 2);

                var currentMonth = DateTime.Now.Month.ToString();
                if (currentMonth.Length == 1)
                {
                    currentMonth = "0" + currentMonth;
                }

                int idLen = length - 6;
                id = fullId.Substring(6, idLen);

                if (month != currentMonth)
                {
                    month = currentMonth;

                    id = "1";
                }
                else
                {
                    int intId = Convert.ToInt32(id);
                    intId = intId + 1;

                    id = intId.ToString();
                }
                if (id.Length == 1)
                    id = "0" + id;
                groupId = year + month + id; 
            }

            intGroupId = Convert.ToInt32(groupId);
        }

        /// <summary>
        /// Add New Traveling Record
        /// </summary>
        /// <param name="travelingRecord"></param>
        public void AddTravelingRecord(TravelingRecords travelingRecord)
        {
            GroupIdCalculation("Traveling Record");
            travelingRecord.transactionGroupId = intGroupId;
            travelingRecord.SystemRefNo = "Visit-"+ intGroupId;
            context.travelingRecords.Add(travelingRecord);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Traveling Record
        /// </summary>
        /// <param name="travelingRecord"></param>
        public void UpdateTravelingRecord(TravelingRecords travelingRecord)
        {
            var _travelingRecord = context.travelingRecords.FirstOrDefault(x=>x.Id == travelingRecord.Id);
            travelingRecord = _travelingRecord;
            context.SaveChanges();
        }

        /// <summary>
        /// Get All Traveling Records
        /// </summary>
        /// <returns></returns>
        public List<VisitingCountry> GetVisitingCountries()
        {
            return context.visitingCountries.ToList();
        }

        /// <summary>
        /// Get Traveling Record
        /// </summary>
        /// <returns></returns>
        public TravelingRecords GetTravelingRecord(int recordId)
        {
            return context.travelingRecords.FirstOrDefault(x=>x.Id == recordId);
        }

        /// <summary>
        /// Add New Traveler
        /// </summary>
        /// <param name="traveler"></param>
        public void AddTraveler(Traveler traveler)
        {
            context.travelers.Add(traveler);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Traveler
        /// </summary>
        /// <param name="traveler"></param>
        public void UpdateTraveler(Traveler traveler)
        {
            var _traveler = context.travelers.FirstOrDefault(x => x.Id == traveler.Id);
            _traveler = traveler;
            context.SaveChanges();
        }

        /// <summary>
        /// Get All Traveler
        /// </summary>
        /// <returns></returns>
        public List<Traveler> GetAllTravelers()
        {
            return context.travelers.ToList();
        }

        /// <summary>
        /// Get Traveler
        /// </summary>
        /// <returns></returns>
        public Traveler GetTraveler(int travelerId)
        {
            return context.travelers.FirstOrDefault(x=>x.Id == travelerId);
        }

        /// <summary>
        /// Add New Visiting Countries
        /// </summary>
        /// <param name="traveler"></param>
        public void AddVisitingCountries(List<VisitingCountry> visitingCountries)
        {
            GroupIdCalculation("Visit");
            foreach (var _country in visitingCountries)
            {
                _country.transactionGroupId = intGroupId;
                _country.SystemRefNo = "Stay-" + intGroupId;
                context.visitingCountries.Add(_country);
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Delete Visiting Countries
        /// </summary>
        /// <param name="traveler"></param>
        public void DeleteVisitingCountries(VisitingCountry visitingCountry)
        {

            context.visitingCountries.Remove(visitingCountry);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Visiting Countries
        /// </summary>
        /// <param name="traveler"></param>
        public void UpdateVisitingCountries(List<VisitingCountry> visitingCountries)
        {
            if(visitingCountries!= null && visitingCountries.Count > 0)
            {
                if(visitingCountries[0].transactionGroupId > 0)
                {
                    intGroupId = visitingCountries[0].transactionGroupId;
                }
                else
                {
                    GroupIdCalculation("Visit");
                }
                foreach (var _country in visitingCountries)
                {
                    if (_country.Id > 0)
                    {
                        var visitCountry = context.visitingCountries.FirstOrDefault(x => x.Id == _country.Id);
                        visitCountry = _country;
                    }
                    else
                    {
                        _country.transactionGroupId = intGroupId;
                        _country.SystemRefNo = "Stay-"+intGroupId;
                        context.visitingCountries.Add(_country);
                    }
                }
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Get All Visiting Countries by Group Id
        /// </summary>
        /// <returns></returns>
        public List<VisitingCountry> GetAllVisitingCountriesByGroupId(int groupId, int recordId)
        {
            return context.visitingCountries.Where(x=>x.transactionGroupId == groupId && x.TravelingRecordsId == recordId).ToList();
        }


        /// <summary>
        /// Get all currencies as list
        /// </summary>
        /// <returns></returns>
        public List<Currency> getAllCurrencies()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of All Currencies");

            return context.currencies.Where(x => x.isVoid != true).ToList();
        }

        /// <summary>
        /// Get all Countries
        /// </summary>
        /// <returns></returns>
        public List<Country> GetAllCountries()
        {
            return context.countries.ToList();
        }

        /// <summary>
        /// Add New Traveling Status
        /// </summary>
        /// <param name="paymentStatus"></param>
        public void AddTravelingStatus(TravelingStatus travelingStatus)
        {
            context.travelingStatuses.Add(travelingStatus);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Traveling Status
        /// </summary>
        /// <param name="paymentStatus"></param>
        public void UpdateTravelingStatus(TravelingStatus travelingStatus)
        {
            TravelingStatus _travelingStatus = context.travelingStatuses.FirstOrDefault(x => x.Id == travelingStatus.Id);
            _travelingStatus.Status = travelingStatus.Status;
            _travelingStatus.isActive = travelingStatus.isActive;
            _travelingStatus.forecolor = travelingStatus.forecolor;
            _travelingStatus.backcolor = travelingStatus.backcolor;
            context.SaveChanges();
        }

        /// <summary>
        /// Get A Payment Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public TravelingStatus GetTravelingStatus(int statusId)
        {
            return context.travelingStatuses
                //.Include("payments")
                .FirstOrDefault(x => x.Id == statusId);
        }


        /// <summary>
        /// Get All Traveling Records Statuses
        /// </summary>
        /// <returns></returns>
        public List<TravelingStatus> GetAllTravelingStatuses()
        {
            return context.travelingStatuses
                //.Include("payments")
                .ToList();
        }

        public List<TravelingStatus> GetAllClosedStatus()
        {
            var statusList = context.travelingStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;

        }


        /// <summary>
        /// Get Count pending Traveling Records by Departments
        /// <returns></returns>
        public int getAllPendingForApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.visitingCountries

                .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains((int)x.travelingRecord.companyId) && x.travelingRecord.isApproved == false && x.travelingRecord.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending Traveling Records by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.visitingCountries

                .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains((int)x.travelingRecord.companyId) && (x.travelingRecord.Creator.employee.SupervisorId == user.employee.EmpId || x.travelingRecord.Creator.employee.EmpId == user.employee.EmpId || x.travelingRecord.Creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.travelingRecord.Creator.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.travelingRecord.isApproved == false && x.travelingRecord.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Own Count pending Traveling Records
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.visitingCountries

                .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains((int)x.travelingRecord.companyId) && (x.travelingRecord.Creator.employee.EmpId == user.employee.EmpId) && x.travelingRecord.isApproved == false && x.travelingRecord.isVoid != true)
                .Count();
        }



        /// <summary>
        /// Get Count pending Traveling Records by Departments
        /// <returns></returns>
        public int getAllPendingForReApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.visitingCountries

                .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains((int)x.travelingRecord.companyId) && x.travelingRecord.isApproved == true && x.travelingRecord.isReApproved == false && x.travelingRecord.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending Traveling Records by user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForReApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.visitingCountries

                .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains((int)x.travelingRecord.companyId) && (x.travelingRecord.Creator.employee.SupervisorId == user.employee.EmpId || x.travelingRecord.Creator.employee.EmpId == user.employee.EmpId || x.travelingRecord.Creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.travelingRecord.Creator.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.travelingRecord.isReApproved == false && x.travelingRecord.isApproved == true && x.travelingRecord.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Own Count pending Traveling Records user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForReApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.visitingCountries

                .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains((int)x.travelingRecord.companyId) && (x.travelingRecord.Creator.employee.EmpId == user.employee.EmpId) && x.travelingRecord.isReApproved == false && x.travelingRecord.isVoid != true && x.travelingRecord.isApproved == true)
                .Count();
        }

        /// <summary>
        /// Get all Void Traveling Records for user count. 
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.visitingCountries
                .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains((int)x.travelingRecord.companyId) && x.travelingRecord.isVoid == true)
                .Count();
        }


        /// <summary>
        /// Get all Void Traveling Records.
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.visitingCountries
    .Where(x => x.travelingRecord.isVoid == true)
    .Count();
        }


        /// <summary>
        /// Get all Traveling Records for user count. 
        /// </summary>
        /// <returns></returns>
        public int getBankTransferRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.visitingCountries
                .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains((int)x.travelingRecord.companyId) && x.travelingRecord.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get all Traveling Records  own count. 
        /// </summary>
        /// <returns></returns>
        public int getBankTransferRegisterCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.visitingCountries
                .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains((int)x.travelingRecord.companyId) && (x.travelingRecord.Creator.employee.EmpId == user.employee.EmpId) && x.travelingRecord.isVoid != true)
                .Count();
        }

        /// Get Count Traveling Records.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.visitingCountries

                .Where(x => x.travelingRecord.isApproved == false && x.travelingRecord.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get all Traveling Records.
        /// </summary>
        /// <returns></returns>
        public int getInterBankTransferAdministratorCount()
        {
            return context.visitingCountries
                .Where(x => x.travelingRecord.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get all pending Traveling Records by Departmental Count
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.visitingCountries

                .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains((int)x.travelingRecord.companyId) && x.travelingRecord.isApproved == true && x.travelingRecord.PendingForClosing == true && x.travelingRecord.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending for closing Traveling Records by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.visitingCountries

                .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains((int)x.travelingRecord.companyId) && (x.travelingRecord.Creator.employee.SupervisorId == user.employee.EmpId || x.travelingRecord.Creator.employee.EmpId == user.employee.EmpId || x.travelingRecord.Creator.employee.Supervisor.SupervisorId == user.employee.EmpId || x.travelingRecord.Creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.travelingRecord.isApproved == true && x.travelingRecord.PendingForClosing == true && x.travelingRecord.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending for closing Traveling Records own <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.visitingCountries

                .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains((int)x.travelingRecord.companyId) && (x.travelingRecord.Creator.employee.EmpId == user.employee.EmpId) && x.travelingRecord.isApproved == true && x.travelingRecord.PendingForClosing == true && x.travelingRecord.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count Traveling Records.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.visitingCountries

                .Where(x => x.travelingRecord.isApproved == true && x.travelingRecord.PendingForClosing == true && x.travelingRecord.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get All Loans and Advances
        /// </summary>
        /// <returns></returns>
        public List<TravelingRecords> GetAllTravelRecordss(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.travelingRecords

           .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) )
           .ToList();


        }

        /// <summary>
        /// Get All Loans and Advances
        /// </summary>
        /// <returns></returns>
        public List<VisitingCountry> GetAllActiveTravelRecords(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.visitingCountries

           .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains(x.travelingRecord.company.Id) && x.travelingRecord.Status.isActive == true && x.travelingRecord.isReApproved != false && x.travelingRecord.isApproved == true && x.travelingRecord.PendingForClosing != true && x.travelingRecord.isVoid != true)
           .ToList();


        }

        /// <summary>
        /// Get All Pending For Approval Traveling Records
        /// </summary>
        /// <returns></returns>
        public List<VisitingCountry> GetAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.visitingCountries

        .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains(x.travelingRecord.company.Id) && x.travelingRecord.isApproved == false && x.travelingRecord.isVoid != true)
         .ToList();
        }

        /// <summary>
        /// Admin Bills Pending for re-approval
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<VisitingCountry> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.visitingCountries

         .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains(x.travelingRecord.company.Id) && x.travelingRecord.isReApproved == false && x.travelingRecord.isApproved == true && x.travelingRecord.isVoid != true)
             .ToList();

        }

        /// <summary>
        /// Get all pending for closing Loans and Advances by Departmental
        /// </summary>
        /// <returns></returns>
        public List<VisitingCountry> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.visitingCountries
            .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains(x.travelingRecord.company.Id) && x.travelingRecord.PendingForClosing == true && x.travelingRecord.isVoid != true)
            .ToList();
        }

        /// <summary>
        /// Get All Loans and Advances
        /// </summary>
        /// <returns></returns>
        public List<VisitingCountry> GetAllTravelingRecords(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
            {
                companyIds.Add(comp.Id);

            }
            return context.visitingCountries

           .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains(x.travelingRecord.company.Id) && x.travelingRecord.isVoid != true)
           .ToList();


        }

        /// <summary>
        /// Get all Void Inter-Bank Transfer own.
        /// </summary>
        /// <returns></returns>
        public List<VisitingCountry> getVoidRegisterOwn(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.visitingCountries
         .Where(x => (deptIds.Contains(x.travelingRecord.department.Id) || x.travelingRecord.creatorId == uid) && companyIds.Contains((int)x.travelingRecord.company.Id) && x.travelingRecord.isVoid == true)
             .ToList();


        }


        public void AddAirline(Airline airline)
        {
            context.airlines.Add(airline);
            context.SaveChanges();
        }

        public void UpdateAirline(Airline airline)
        {
            var _Airline = context.airlines.FirstOrDefault(x => x.Id == airline.Id);
            _Airline = airline;
            context.SaveChanges();
        }

        public Airline GetAirline(int airlineId)
        {
            return context.airlines
                .FirstOrDefault(x => x.Id == airlineId);
        }

        public List<Airline> GetAllAirlines()
        {
            return context.airlines.ToList();
        }


    }
}
