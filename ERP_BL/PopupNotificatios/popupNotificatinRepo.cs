using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.PopupNotificatios
{
 
    public class popupNotificatinRepo
    {
        DBContextERP context = new DBContextERP();
        public void AddPopupNotifications(popupNotifications popup)
        {            var removePopup = context.popupNotifications.ToList();            context.popupNotifications.RemoveRange(removePopup);            ERP_BL.PopupNotificatios.popupNotifications popupNoti = new ERP_BL.PopupNotificatios.popupNotifications();
 
            context.popupNotifications.Add(popup);            context.SaveChanges();        }
        public void DeletePopupNotifications(popupNotifications popup)
        {
            var removePopup = context.popupNotifications.ToList();
            context.popupNotifications.RemoveRange(removePopup);
            context.SaveChanges();
        }
        public ERP_BL.PopupNotificatios.popupNotifications getShareAllPopup()
        {
            try
            {
                var lastid = context.popupNotifications.Where(x => x.EmployeeIds == null).ToList().LastOrDefault();
                if (lastid == null)
                {
                    return null;
                }
                else
                {

                    lastid.Id.ToString();
                    return lastid;
                }
            }
            catch (Exception)
            {

                throw;
            }
 
          
        }

        public ERP_BL.PopupNotificatios.popupNotifications getSpecificPopup() 
        {
            var lastid = context.popupNotifications.Where(x => x.EmployeeIds != null).ToList().LastOrDefault();
            if (lastid == null)
            {
                return null;
            }
            else
            {
                lastid.Id.ToString();
                return lastid;
            }
        }
    }
  
}
