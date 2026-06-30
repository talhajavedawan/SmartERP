using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.Chat
{
    class ucGroupReceiverBinding
    {
        public static string _msgBody;
        public string msgBody { set; get; }

       

        public static string _recName;
        public string recName { set; get; }

        public static string _msgId;
        public string msgId { set; get; }

        public static string _readReceipt;
        public string readReceipt { set; get; }
        public static string _seenStamp;
        public string seenStamp { set; get; }

        public static ucGroupReceiverBinding GetChatDetails()
        {
            var header = new ucGroupReceiverBinding()
            {
                msgId=_msgId,
                msgBody = _msgBody,   
                recName = _recName,
                readReceipt = _readReceipt,
                seenStamp = _seenStamp
            };
            return header;
        }
    }
}
