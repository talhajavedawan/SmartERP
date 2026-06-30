using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.EmailIntegration
{
    public class Email
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public string BodyHtml { get; set; }
        public List<EmailAttachment> Attachments { get; set; }
        public bool isUnRead { get; set; }
    }
    public class EmailAttachment
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
