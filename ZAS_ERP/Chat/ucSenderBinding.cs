using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.Chat
{
    class ucSenderBinding
    {
        public static string _msgBody; 
        public string msgBody { set; get; }

        public static string _recId; 
        public string recId { set; get; }

        public static string _readReceipt;

        public string readReceipt { set; get; }

        public static string _seenStamp;

        public string seenStamp { set; get; }

        public static ucSenderBinding GetChatDetails()
        {
            var header = new ucSenderBinding()
            {
                msgBody = _msgBody,
                recId= _recId,
                readReceipt = _readReceipt,
                seenStamp=_seenStamp
            };
            return header;
        }
    }
}
