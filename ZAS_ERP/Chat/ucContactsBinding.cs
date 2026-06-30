using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.Chat
{
    class ucContactsBinding
    {
        public static string _initials;
        public string initials { get; set; }

        public static string _userName;       
        public string userName { get; set; }

        public static int _contactId;
        public string contactId { get; set; }


        public static ucContactsBinding GetChatDetails()
        {
            var header = new ucContactsBinding()
            {
                initials = _initials,
                userName = _userName,
                contactId = _contactId.ToString()
            };
            return header;
        }
    }    
}
