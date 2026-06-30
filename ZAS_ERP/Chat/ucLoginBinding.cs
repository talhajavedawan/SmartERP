using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.Chat
{
    class ucLoginBinding
    {
        public static string _userName;
        public string userName { set; get; }
        public static string _initials;
        public string initials { set; get; }
        public static ucLoginBinding GetDetails()
        {
            var header = new ucLoginBinding()
            {
                userName = _userName,
                initials = _initials
            };
            return header;
        }
    }
}
