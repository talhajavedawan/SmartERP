using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.Chat
{
    class HeaderBinding
    {
        public static string _userName { set; get; }
        public string userName;

        public static HeaderBinding GetDetails()
        {
            var header = new HeaderBinding
            {
                userName = _userName
            };
            return header;
        }
    

    }
    
}
