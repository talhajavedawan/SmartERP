using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Procurements.Memos
{
    public class MemoRepo
    {
        DBContextERP context = new DBContextERP();

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        public List<ERP_BL.Databases.User> getAllusers()
        {
            return context.Users.Where(x=>x.isActive == true).ToList();
        }

        /// <summary>
        /// Add New Memo
        /// </summary>
        /// <param name="memo"></param>
        public void AddMemo(Memo memo)
        {
            List<int> ccIds = memo.CCUsersList.Select(x=>x.id).ToList();

            memo.CCUsersList = new List<Databases.User>();
            foreach(var idd in ccIds)
            {
                memo.CCUsersList.Add(context.Users.FirstOrDefault(x=>x.id == idd));
            }

            context.memos.Add(memo);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Memo
        /// </summary>
        /// <param name="memo"></param>
        public void UpdateMemo(Memo memo)
        {
            var _memo = context.memos.FirstOrDefault(x=>x.Id == memo.Id);

            List<int> ccIds = memo.CCUsersList.Select(x => x.id).ToList();

            memo.CCUsersList = new List<Databases.User>();
            foreach (var idd in ccIds)
            {
                memo.CCUsersList.Add(context.Users.FirstOrDefault(x => x.id == idd));
            }

            _memo = memo;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Memo by Id
        /// </summary>
        /// <param name="memoId"></param>
        /// <returns></returns>
        public Memo GetMemo(int memoId)
        {
            return context.memos.FirstOrDefault(x=>x.Id == memoId);
        }

       /// <summary>
       /// Get All Memos
       /// </summary>
       /// <returns></returns>
        public List<Memo> GetAllMemos(int uId)
        {
            var memos = context.memos.Where(x => (x.createdById == uId 
            || x.createdForId == uId 
            ||  x.taskGroup.users.Any(y => y.id == uId)
            ||  x.CCUsersList.Any(z => z.id == uId)
            )
            && x.isVoid != true).ToList();

            return memos;
        }

        /// <summary>
        /// Get All Memos Count
        /// </summary>
        /// <returns></returns>
        public int GetAllMemosCount(int uId)
        {
            return context.memos.Where(x => (x.createdById == uId
            || x.createdForId == uId
            || x.taskGroup.users.Any(y => y.id == uId)
            || x.CCUsersList.Any(z => z.id == uId)
            )
            && x.isVoid != true).Count();
        }

        /// <summary>
        /// Get All Memos
        /// </summary>
        /// <returns></returns>
        public List<Memo> GetAllLinkedMemos(int uId)
        {
            var memos = context.memos.Where(x => ( x.createdForId == uId && x.memoType == Enums.MemoType.Linked)
            && x.isVoid != true).ToList();

            return memos;
        }

        /// <summary>
        /// Get All Memos
        /// </summary>
        /// <param name="task"></param>
        public List<Memo> GetAllVoidMemos(int uId)
        {


            return context.memos
                .Where(x => (x.createdById == uId
            || x.createdForId == uId
            || x.taskGroup.users.Any(y => y.id == uId)
            || x.CCUsersList.Any(z => z.id == uId)
            )
            && x.isVoid == true)
                .ToList();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllVoidMemosCount(int uId)
        {
            return context.memos.Where(x => (x.createdById == uId
            || x.createdForId == uId
            || x.taskGroup.users.Any(y => y.id == uId)
            || x.CCUsersList.Any(z => z.id == uId)
            )
            && x.isVoid == true)
                .Count();
        }


        ///// <summary>
        ///// Add New Memo Group
        ///// </summary>
        ///// <param name="group"></param>
        //public void AddGroupMemo(MemoGroup group)
        //{
        //    context.memoGroups.Add(group);
        //    context.SaveChanges();
        //}

        ///// <summary>
        ///// Update Memo Group
        ///// </summary>
        ///// <param name="group"></param>
        //public void UpdateGroupMemo(MemoGroup group)
        //{
        //    var _group = context.memoGroups.FirstOrDefault(x => x.Id == group.Id);

        //    _group = group;
        //    context.SaveChanges();
        //}

        ///// <summary>
        ///// Get Memo Group by Id
        ///// </summary>
        ///// <param name="groupId"></param>
        ///// <returns></returns>
        //public MemoGroup GetGroupMemo(int groupId)
        //{
        //    return context.memoGroups.FirstOrDefault(x => x.Id == groupId);
        //}

        ///// <summary>
        ///// Get All Memo Groups
        ///// </summary>
        ///// <returns></returns>
        //public List<MemoGroup> GetAllGroupMemos()
        //{
        //    return context.memoGroups.ToList();
        //}

        /// <summary>
        /// Add New Performance Review
        /// </summary>
        /// <param name="performanceReview"></param>
        public void AddPerformanceReview(EmployeePerformanceReview performanceReview)
        {
            context.employeePerformanceReviews.Add(performanceReview);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Performance Review
        /// </summary>
        /// <param name="performanceReview"></param>
        public void UpdatePerformanceReview(EmployeePerformanceReview performanceReview)
        {
            var _performanceReview = context.employeePerformanceReviews.FirstOrDefault(x => x.Id == performanceReview.Id);

            var userIds = performanceReview.HiddenChatUsers?.Select(x=>x.id).ToList();

            _performanceReview = performanceReview;

            if(userIds != null && userIds.Count > 0)
            {
                _performanceReview.HiddenChatUsers = new List<Databases.User>();

                foreach (var id in userIds)
                    _performanceReview.HiddenChatUsers.Add(context.Users.FirstOrDefault(x=>x.id == id));
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Get Performance Review by Id
        /// </summary>
        /// <param name="performanceReviewId"></param>
        /// <returns></returns>
        public EmployeePerformanceReview GetPerformanceReview(int performanceReviewId)
        {
            return context.employeePerformanceReviews.FirstOrDefault(x => x.Id == performanceReviewId);
        }

        /// <summary>
        /// Get All Performance Indicator
        /// </summary>
        /// <param name=""></param>
        public List<EmployeePerformanceReview> GetAllPerformanceReviews()
        {
            return context.employeePerformanceReviews.ToList();
        }

        /// <summary>
        /// Get Performance Review by MemoId
        /// </summary>
        /// <param name="memoId"></param>
        /// <returns></returns>
        public EmployeePerformanceReview GetPerformanceReviewByMemoId(int memoId)
        {
            return context.employeePerformanceReviews.FirstOrDefault(x => x.MemoId == memoId);
        }

        /// <summary>
        /// Add New Performance Indicator
        /// </summary>
        /// <param name="performanceIndicator"></param>
        public void AddPerformanceIndicatorDefinition(PerformanceIndicatorDefinition performanceIndicator)
        {
            context.performanceIndicatorDefinitions.Add(performanceIndicator);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Performance Indicators
        /// </summary>
        /// <param name="performanceIndicator"></param>
        public void UpdatePerformanceIndicatorDefinition(PerformanceIndicatorDefinition performanceIndicator)
        {
            var _performanceIndicator = context.performanceIndicatorDefinitions.FirstOrDefault(x => x.Id == performanceIndicator.Id);

            _performanceIndicator = performanceIndicator;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Performance Indicator by Id
        /// </summary>
        /// <param name="performanceIndicatorId"></param>
        /// <returns></returns>
        public PerformanceIndicatorDefinition GetPerformanceIndicatorDefinition(int performanceIndicatorId)
        {
            return context.performanceIndicatorDefinitions.FirstOrDefault(x => x.Id == performanceIndicatorId);
        }

        /// <summary>
        /// Get All Performance Indicator
        /// </summary>
        /// <param name=""></param>
        public List<PerformanceIndicatorDefinition> GetAllPerformanceIndicators()
        {


            return context.performanceIndicatorDefinitions.ToList();
        }

        /// <summary>
        /// Get All Active Performance Indicator
        /// </summary>
        /// <param name=""></param>
        public List<PerformanceIndicatorDefinition> GetAllActivePerformanceIndicators()
        {
            return context.performanceIndicatorDefinitions.Where(x=>x.IsActive == true).ToList();
        }

        /// <summary>
        /// Get All Performance Ratings
        /// </summary>
        /// <param name=""></param>
        public List<PerformanceIndicatorRating> GetAllPerformanceRatings()
        {
            return context.performanceIndicatorRatings.ToList();
        }

        public List<Department> GetDepartments()
        {
            return context.Departments.Where(x => x.isActive == true).ToList();
        }
    }
}
