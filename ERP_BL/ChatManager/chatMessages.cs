using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ChatManager
{
    public enum readReceipt
    {
        Sent,
        Delivered,
        Seen
    }
    public class One2OneMessage
    {
        public Int64 msgId { get; set; }
        public int sender { get; set; }
        public int receiver { get; set; }
        public string messageBody { get; set; }
        public string attachPath { get; set; }
        public readReceipt readReceipt { get; set; }
        public DateTime seenStamp { get; set; }
        

    }

    public class GroupMessage
    {
        public Int64 msgId { get; set; }
        public string MessageBody { get; set; }
        public int sender { get; set; }
        public string attachPath { get; set; }
        public List<reader> readers { get; set; }

        //public static implicit operator GroupMessage(GroupMessage v)
        //{
        //    throw new NotImplementedException();
        //}
    }

    public class Annoucement
    {
        public DateTime expiryTime { get; set; }
        public string MessageBody { get; set; }
        public int sender { get; set; }
        public string attachPath { get; set; }
        public List<readReceipt> readReceipts { get; set; }
    }

    public class reader
    {
        public Int64 userId { get; set; }
        public string userName { get; set; }
        public readReceipt receipt { get; set; }
        public DateTime timestamp { get; set; }

        public static explicit operator reader(List<ChatUser> v)
        {
            throw new NotImplementedException();
        }
    }

}
