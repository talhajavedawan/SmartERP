using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.Config;
using ERP_BL.Databases;

namespace ERP_BL.Databases
{
    public class ContactPersonRepo
    {

        DBContextERP context = new DBContextERP();

        public void AddContactPerson(ContactPerson contactPerson)
        {
            if (contactPerson == null)
                throw new NullReferenceException("Object can not be null");
            context.contactPersons.Add(contactPerson);
        }

        public void UpdateContactPerson(ContactPerson contactPerson)
        {
            if (contactPerson == null)
                throw new NullReferenceException("Object can not be null");

            var _contactPerson = context.contactPersons.FirstOrDefault(x => x.Id == contactPerson.Id);
            _contactPerson.person = contactPerson.person;
            _contactPerson.designation = contactPerson.designation;
            _contactPerson.contact = contactPerson.contact;

            context.SaveChanges();
        }

        public ContactPerson GetContactPerson(int Id)
        {
            return context.contactPersons.FirstOrDefault(x => x.Id == Id);
        }

        public List<ContactPerson> GetAllContactPersons()
        {
            return context.contactPersons.ToList();
        }

    }
}
