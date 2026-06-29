using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ErpChatService.Models
{
   public class ChatGroup
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public int id { get; set; }
        [BsonElement("Name")]
        public string groupName { get; set; }
        public List<ChatUser> users { get; set; }
        public DateTime creationDate { get; set; }
        public int parentId { get; set; }
    }
    public class ChatUser
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public int id { get; set; }
        public Int64 employeeId { get; set; }
        public string name { get; set; }

        //public reader toReader()
        //{
        //    reader _reader = new reader();
        //    _reader.userId = employeeId;
        //    _reader.userName = name;
        //    _reader.receipt = readReceipt.Sent;
        //    _reader.timestamp = DateTime.Now;
        //    return _reader;
        //}

        //public reader toReader(readReceipt receipt)
        //{
        //    reader _reader = new reader();
        //    _reader.userId = employeeId;
        //    _reader.userName = name;
        //    _reader.receipt = receipt;
        //    _reader.timestamp = DateTime.Now;
        //    return _reader;
        //}

        //public reader toReader(readReceipt receipt, DateTime timestamp)
        //{
        //    reader _reader = new reader();
        //    _reader.userId = employeeId;
        //    _reader.userName = name;
        //    _reader.receipt = receipt;
        //    _reader.timestamp = timestamp;
        //    return _reader;
        //}
    }
}
