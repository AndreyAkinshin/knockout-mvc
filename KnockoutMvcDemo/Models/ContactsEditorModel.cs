using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace KnockoutMvcDemo.Models
{
    public class ContactsEditorPhoneModel
    {
        public string Type { get; set; }
        public string Number { get; set; }
    }

    public class ContactsEditorContactModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<ContactsEditorPhoneModel> Phones { get; set; }

        public void AddPhone()
        {
            Phones.Add(new ContactsEditorPhoneModel());
        }

        public void DeletePhone(int phoneIndex)
        {
            if (phoneIndex >= 0 && phoneIndex < Phones.Count)
                Phones.RemoveAt(phoneIndex);
        }
    }

    public class ContactsEditorModel
    {
        public List<ContactsEditorContactModel> Contacts { get; set; }
        public string LastSavedJson { get; set; }

        public void AddContact()
        {
            Contacts.Add(new ContactsEditorContactModel());
        }

        public void DeleteContact(int contactIndex)
        {
            if (contactIndex >= 0 && contactIndex < Contacts.Count)
                Contacts.RemoveAt(contactIndex);
        }

        public void AddPhone(int contactIndex)
        {
            if (contactIndex >= 0 && contactIndex < Contacts.Count)
                Contacts[contactIndex].AddPhone();
        }

        public void DeletePhone(int personIndex, int phoneIndex)
        {
            if (personIndex >= 0 && personIndex < Contacts.Count)
                Contacts[personIndex].DeletePhone(phoneIndex);
        }

        public void SaveJson()
        {
            LastSavedJson = new JavaScriptSerializer().Serialize(this);
        }
    }
}