using ERP_BL.Entities.Base.Addresses;
using System;


namespace ERP_BL.Entities
{
    public static class AddressExtensions
    {
        public static void ClearNavs(this Address address)
        {
            if (address == null) return;

            address.Country = null;
            address.State = null;
            address.City = null;
            address.Zone = null;
        }
    }
}