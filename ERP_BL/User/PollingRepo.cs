using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.User
{
    public class PollRepo
    {
        DBContextERP context = new DBContextERP();

        /// <summary>
        /// Add New Poll
        /// </summary>
        /// <param name="poll"></param>
        public void AddPoll(Poll poll)
        {
            context.polls.Add(poll);
            context.SaveChanges();
        }

        /// <summary>
        /// Update New Poll
        /// </summary>
        /// <param name="poll"></param>
        public void UpdatePoll(Poll poll)
        {
            var _poll = context.polls.FirstOrDefault(x=>x.Id == poll.Id);
            _poll = poll;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Poll by Id
        /// </summary>
        /// <param name="pollId"></param>
        /// <returns></returns>
        public Poll GetPoll(int pollId)
        {
            return context.polls.FirstOrDefault(x=>x.Id == pollId);
        }

        /// <summary>
        /// Delete Poll by Id
        /// </summary>
        /// <param name="pollId"></param>
        /// <returns></returns>
        public void DeletePoll(int pollId)
        {
            var poll = context.polls.FirstOrDefault(x=>x.Id == pollId);

            if (poll != null)
            {
                context.polls.Remove(poll);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Get All Polls
        /// </summary>
        /// <returns></returns> 
        public List<Poll> GetAllPolls(int userId)
        {
            return context.polls.Where(x=>x.taskGroup.users.FirstOrDefault(y=>y.id==userId) != null).ToList();
        }

        /// <summary>
        /// Get User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ERP_BL.Databases.User GetUser(int userId)
        {
            return context.Users.FirstOrDefault(x=>x.id == userId);
        }

        /// <summary>
        /// Get Pollings for Users

        /// </summary>
        /// 
        public int GetInstantPollForUser(int UserId)
        {
            DateTime? date = DateTime.Now;

            var polls = context.polls.Where(x => x.taskGroup.users.FirstOrDefault(y => y.id == UserId) != null &&x.ValidUntil >= DateTime.Now).Count();
            return polls;
        }
    }
}
