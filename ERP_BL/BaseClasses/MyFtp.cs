using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ERP_BL.BaseClasses
{
    class MyFtp
    {
    }

    public class ftpAddress
    {
        public string userName { get; set; }
        private byte[] ftpPassword;
        public string IPLan { get; set; }
        public string IPWan { get; set; }

        public string password { get; set; }
        //{
        //    get
        //    { return GetHashString(ftpPassword); }
        //    set { ftpPassword = GetHash(value);  }
        //}
        private  static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        private static string GetHashString(byte[] inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in inputString)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }

}
