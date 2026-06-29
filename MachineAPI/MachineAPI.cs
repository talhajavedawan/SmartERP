using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.NetworkInformation;

namespace MachineAPI
{
    public class MachineAPI
    {
        private static ManagementObjectSearcher baseboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
        private static ManagementObjectSearcher motherboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_MotherboardDevice");

        public bool validatekey(string key, String ProductName)
        {
            string[] keyVal = null;
            string ValidKey = null;
            int i = 0;
            try
            {
                if (Conversion.Val(key[key.Length - 1]) == 1)
                {
                    keyVal = key.Split(':');
                    while (i < keyVal.Length)
                    {
                        ValidKey = ValidKey + keyVal[i].Substring(0, keyVal[i].Length - 3).ToString() + ":";
                        i += 1;
                    }
                    ValidKey = ValidKey.Substring(0, ValidKey.Length - 1);
                }
                else
                {
                    keyVal = key.Split(':');
                    while (i < keyVal.Length)
                    {
                        ValidKey = ValidKey + keyVal[i] + ":";
                        i += 1;
                    }
                    ValidKey = ValidKey.Substring(0, ValidKey.Length - 1);
                }


                if (ValidKey.Replace(":", "").Replace(" ", "") == reshape(ProductName).ToString().Replace(":", "").Replace(" ", ""))
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public string GenKey(string ProductName)
        {
            return reshape(ProductName);
        }
        private string reshape(String ProductName)
        {

            string str = getCPU_ID() + GetBoardSerialNo() + ProductName;
            string shaped = null;
            int j = 1;
            int startIndex;
            startIndex = 0;

            for (var i = 1; i <= (str.Length / (double)3) - 1; i++)
            {
                shaped += str.Substring(startIndex, 3).Trim() + ":";
                startIndex += 3;
            }
            shaped += str.Substring(startIndex);
            return shaped;
        }

        public string getCPU_ID()
        { 
            string cpuID = string.Empty;
            try
            {
               
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((cpuID == string.Empty))
                        cpuID = mo.Properties["ProcessorId"].Value.ToString();
                }
            }
            catch
            { cpuID = "NOTFOUND"; }

            return cpuID;
        }

        public string getMacAddress()
        {
            string cpuID = string.Empty;
            try
            {
                // MsgBox("called MC")

                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((cpuID == string.Empty) == true)
                    {
                        var t = mo.Properties["MacAddress"].Value;
                        if (t != null)
                            cpuID = t.ToString();

                        return cpuID;
                    }
                }

                return cpuID;
            }
            catch (Exception ex)
            {
                cpuID = "NA";
                Interaction.MsgBox(ex.Message);
            }
            return cpuID;
        }

        public string GetBoardSerialNo()
        {

            try
            {
                foreach (ManagementObject queryObj in baseboardSearcher.Get())
                {
                    return queryObj["SerialNumber"].ToString();
                }
                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public string EncodeServerName(string serverName)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(serverName));
        }

        public string DecodeServerName(string encodedServername)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedServername));
        }
    }

}
