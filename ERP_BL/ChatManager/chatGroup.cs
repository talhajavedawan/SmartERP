using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ChatManager
{
   public class ChatGroup
    {

        public int id { get; set; }
        public string groupName { get; set; }
        public List<ChatUser> users { get; set; }
        public DateTime creationDate { get; set; }
        public int parentId { get; set; }
    }
    public class ChatUser
    {
        public int id { get; set; }
        public Int64 employeeId { get; set; }
        public string userName { get; set; }

        public reader toReader()
        {
            reader _reader = new reader();
            _reader.userId = employeeId;
            _reader.userName = userName;
            _reader.receipt = readReceipt.Sent;
            _reader.timestamp = DateTime.Now;
            return _reader;
        }

        public reader toReader(readReceipt receipt)
        {
            reader _reader = new reader();
            _reader.userId = employeeId;
            _reader.userName = userName;
            _reader.receipt = receipt;
            _reader.timestamp = DateTime.Now;
            return _reader;
        }

        public reader toReader(readReceipt receipt, DateTime timestamp)
        {
            reader _reader = new reader();
            _reader.userId = employeeId;
            _reader.userName = userName;
            _reader.receipt = receipt;
            _reader.timestamp = timestamp;
            return _reader;
        }
    }
}
