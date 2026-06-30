using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class BidRepo
    {
       
        DBContextERP context = new DBContextERP();


        /// <summary>
        /// Add Bid in database
        /// </summary>
        /// <param name="bid">Bid Object</param>
        public void Add(Bid bid)
        {
            context.bids.Add(bid);
            context.SaveChanges();
        }
         
       
       


        /// <summary>
        /// Update Bid
        /// </summary>
        /// <param name="bid"></param>
        public void update(Bid bid)
        {
            Bid bidtoUpdate = context.bids.FirstOrDefault(x => x.Id == bid.Id);
            bidtoUpdate = bid;
            context.SaveChanges();
        }


        /// <summary>
        /// Get all bids.
        /// </summary>
        /// <returns></returns>
        public List<Bid> getAll()
        {
            return context.bids.ToList();
        }

        /// <summary>
        /// Get bid by id.
        /// </summary>
        /// <param name="bidid">Bid Id</param>
        /// <returns>an object of type bid</returns>
        public Bid get(int bidid)
        {
            return context.bids.FirstOrDefault(x => x.Id == bidid);
        }
    }
}
