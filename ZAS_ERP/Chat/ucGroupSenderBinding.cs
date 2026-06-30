using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.Chat
{
    class ucGroupSenderBinding
    {
        public static string _senderName;
        public string senderName { set; get; }

        public static string _msgBody;
        public string msgBody { set; get; }

        public static string _seenStamp;

        public string seenStamp { set; get; }

        public static long _msgId;

        public long msgId { set; get; }

        public static string _readReceipt;

        public string readReceipt { set; get; }

        public static ucGroupSenderBinding GetChatDetails()
        {
            var header = new ucGroupSenderBinding()
            {
                senderName=_senderName,
                msgBody=_msgBody,
                seenStamp=_seenStamp,
                readReceipt=_readReceipt
                
            };
            return header;
        }
    }
}
