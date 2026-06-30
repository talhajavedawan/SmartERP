using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.FixedAssets.Classes
{
    public class Cities
    {
       //public  List<string> cities { get; set; }
        public Cities()
        {
            
            //cities = GetPakCityList();
        }

        public List<string> GetPakCityList()
        {
            List<string> pakCityList = new List<string>();
            pakCityList.Add("Karachi");
            pakCityList.Add("Lahore");
            pakCityList.Add("Faisalabad");
            pakCityList.Add("Rawalpindi");
            pakCityList.Add("Multan");
            pakCityList.Add("Hyderabad");
            pakCityList.Add("Gujranwala");
            pakCityList.Add("Peshawar");
            pakCityList.Add("Islamabad");
            pakCityList.Add("Bahawalpur");
            pakCityList.Add("Sargodha");
            pakCityList.Add("Sialkot");
            pakCityList.Add("Quetta");
            pakCityList.Add("Sukkur");
            pakCityList.Add("Jhang");
            pakCityList.Add("Shekhupura");
            pakCityList.Add("Mardan");
            pakCityList.Add("Gujrat");
            pakCityList.Add("Larkana");
            pakCityList.Add("Kasur");
            pakCityList.Add("Rahim Yar Khan");
            pakCityList.Add("Sahiwal");
            pakCityList.Add("Okara");
            pakCityList.Add("Wah Cantonment");
            pakCityList.Add("Dera Ghazi Khan");
            pakCityList.Add("Mingora");
            pakCityList.Add("Mirpur Khas");
            pakCityList.Add("Chiniot");
            pakCityList.Add("Nawabshah");
            pakCityList.Add("Kāmoke");
            pakCityList.Add("Burewala");
            pakCityList.Add("Jhelum");
            pakCityList.Add("Sadiqabad");
            pakCityList.Add("Khanewal");
            pakCityList.Add("Hafizabad");
            pakCityList.Add("Kohat");
            pakCityList.Add("Jacobabad");
            pakCityList.Add("Shikarpur");
            pakCityList.Add("Muzaffargarh");
            pakCityList.Add("Khanpur");
            pakCityList.Add("Gojra");
            pakCityList.Add("Bahawalnagar");
            pakCityList.Add("Abbottabad");
            pakCityList.Add("Muridke");
            pakCityList.Add("Pakpattan");
            pakCityList.Add("Khuzdar");
            pakCityList.Add("Jaranwala");
            pakCityList.Add("Chishtian");
            pakCityList.Add("Daska");
            pakCityList.Add("Bhalwal");
            pakCityList.Add("Mandi Bahauddin");
            pakCityList.Add("Ahmadpur East");
            pakCityList.Add("Kamalia");
            pakCityList.Add("Tando Adam");
            pakCityList.Add("Khairpur");
            pakCityList.Add("Dera Ismail Khan");
            pakCityList.Add("Vehari");
            pakCityList.Add("Nowshera");
            pakCityList.Add("Dadu");
            pakCityList.Add("Wazirabad");
            pakCityList.Add("Khushab");
            pakCityList.Add("Charsada");
            pakCityList.Add("Swabi");
            pakCityList.Add("Chakwal");
            pakCityList.Add("Mianwali");
            pakCityList.Add("Tando Allahyar");
            pakCityList.Add("Kot Adu");
            pakCityList.Add("Turbat");
            return pakCityList;
        }
        public List<string> GetUaeCityList()
        {
            List<string> uaeCityList = new List<string>();
            uaeCityList.Add("Umm al Qaywayn");
            uaeCityList.Add("Ras al-Khaimah");
            uaeCityList.Add("Khawr FakkÄn");
            uaeCityList.Add("Dubai");
            uaeCityList.Add("Dibba Al-Fujairah");
            uaeCityList.Add("Dibba Al-Hisn");
            uaeCityList.Add("Sharjah");
            uaeCityList.Add("Ar Ruways");
            uaeCityList.Add("Al Fujayrah");
            uaeCityList.Add("Al Ain");
            uaeCityList.Add("Ajman");
            uaeCityList.Add("Adh Dhayd");
            uaeCityList.Add("Abu Dhabi");
            return uaeCityList;
        }
    }
}
