using System.Collections.Generic;
using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
    public class ContactsEditorController : BaseController
    {
        public ActionResult Index()
        {
            InitializeViewBag("Contacts editor");
            var model = new ContactsEditorModel();
            model.Contacts = new List<ContactsEditorContactModel>();
            model.Contacts.Add(new ContactsEditorContactModel
            {
                FirstName = "Danny",
                LastName = "LasRusso",
                Phones = new List<ContactsEditorPhoneModel>
          {
            new ContactsEditorPhoneModel {Type = "Mobile", Number = "(555) 121-2121"},
            new ContactsEditorPhoneModel {Type = "Home", Number = "(555) 123-4567"},
          }
            });
            model.Contacts.Add(new ContactsEditorContactModel
            {
                FirstName = "Sensei",
                LastName = "Miyagi",
                Phones = new List<ContactsEditorPhoneModel>
          {
            new ContactsEditorPhoneModel {Type = "Mobile", Number = "(555) 444-2222"},
            new ContactsEditorPhoneModel {Type = "Home", Number = "(555) 999-1212"},
          }
            });
            return View(model);
        }

        public ActionResult AddContact(ContactsEditorModel model)
        {
            model.AddContact();
            return Json(model);
        }

        public ActionResult DeleteContact(ContactsEditorModel model, int contactIndex)
        {
            model.DeleteContact(contactIndex);
            return Json(model);
        }

        public ActionResult AddPhone(ContactsEditorModel model, int contactIndex)
        {
            model.AddPhone(contactIndex);
            return Json(model);
        }

        public ActionResult DeletePhone(ContactsEditorModel model, int contactIndex, int phoneIndex)
        {
            model.DeletePhone(contactIndex, phoneIndex);
            return Json(model);
        }

        public ActionResult SaveJson(ContactsEditorModel model)
        {
            model.SaveJson();
            return Json(model);
        }
    }
}
