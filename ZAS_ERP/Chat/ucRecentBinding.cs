using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.Chat
{
    class ucRecentBinding
    {
        public static string _initials;
        public string initials { get; set; }

        public static string _userName;
        public string userName { get; set; }

        public static int _sender;
        public string sender { get; set; }

        public static int _receiver;
        public string receiver { get; set; }

        public static long _msgId;
        public int msgId { get; set; }

        public static string _msgBody;
        public string msgBody { get; set; }

        public static string _readReceipt;
        public string readReceipt { get; set; }

        public static string _seenStamp;
        public string seenStamp { get; set; }

        public static int _contactId;
        public string contactId { get; set; }

        public static ucRecentBinding GetDetails()
        {
            var header = new ucRecentBinding()
            {
                initials=_initials,
                userName = _userName,
                sender = _sender.ToString(),
                receiver = _receiver.ToString(),
                msgId=Convert.ToInt32(_msgId),
                msgBody = _msgBody,
                readReceipt = _readReceipt,
                seenStamp = _seenStamp,
                contactId=_contactId.ToString() 
            };
            return header;

        }
    }
}
