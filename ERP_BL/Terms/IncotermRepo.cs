using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class IncotermRepo
    {
        DBContextERP context = new DBContextERP();

        /// <summary>
        /// Add new Incoterm Company 
        /// </summary>
        /// <param name="incoterm">Incoterm Company Object</param>
        public void Add(Incoterm incoterm)
        {
            context.incoterms.Add(incoterm);
            context.SaveChanges();
        }





        /// <summary>
        /// return all Incoterm Companies list.
        /// </summary>
        /// <returns></returns>
        public List<Incoterm> getAll()
        {
            return context.incoterms
                .ToList();
        }

        /// <summary>
        /// get incotermCompany matching to ID
        /// </summary>
        /// <param name="incotermCompID">Incoterm Company ID</param>
        /// <returns></returns>
        public Incoterm get(int incotermID)
        {
            return context.incoterms.FirstOrDefault(x => x.Id == incotermID);
        }



        /// <summary>
        /// update Incoterm Company object details
        /// </summary>
        /// <param name="incoterm">Incoterm Company Object</param>
        public void Update(Incoterm incoterm)
        {
            Incoterm compToUpdate = context.incoterms.FirstOrDefault(x => x.Id == incoterm.Id);
            compToUpdate = incoterm;
            context.SaveChanges();
        }



        /// <summary>
        /// Get All Active Incoterms
        /// </summary>
        /// <returns></returns>
        public List<Incoterm> getActiveIncoterm()
        {
            return context.incoterms.Where(x => x.isActive == true).ToList();

        }
        /// <summary>
        /// Get All inActive Incoterms
        /// </summary>
        /// <returns></returns>
        public List<Incoterm> getinActiveIncoterms()
        {
            return context.incoterms.Where(x => x.isActive == false).ToList();

        }
        
    }
}
