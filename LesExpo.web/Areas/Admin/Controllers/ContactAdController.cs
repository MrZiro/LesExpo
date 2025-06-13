using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.Models;
using LesExpo.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LesExpo.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ContactAdController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactAdController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ViewBag.Count = _unitOfWork.Contact.GetCount();
            return View();
        }

        // GET: Admin/Contact/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Contact contact = _unitOfWork.Contact.Get(u => u.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // DELETE: Admin/Contact/Delete/5
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var contact = _unitOfWork.Contact.Get(u => u.Id == id);
            if (contact == null)
            {
                return Json(new { success = false, message = "Contact not found." });
            }

            _unitOfWork.Contact.Remove(contact);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Contact deleted successfully." });
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var contacts = _unitOfWork.Contact.GetAll();
            return Json(new { data = contacts });
        }

        [HttpGet]
        public IActionResult GetByLanguage(string language)
        {
            var contacts = _unitOfWork.Contact.GetContactsByLanguage(language);
            return Json(new { data = contacts });
        }

        #endregion
    }
} 