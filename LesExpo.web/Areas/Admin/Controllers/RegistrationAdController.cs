using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.Models;
using LesExpo.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LesExpo.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class RegistrationAdController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrationAdController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ViewBag.Count = _unitOfWork.Registration.GetCount();
            return View();
        }

        // GET: Admin/Registration/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Registration registration = _unitOfWork.Registration.Get(u => u.Id == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // DELETE: Admin/Registration/Delete/5
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var registration = _unitOfWork.Registration.Get(u => u.Id == id);
            if (registration == null)
            {
                return Json(new { success = false, message = "Registration not found." });
            }

            _unitOfWork.Registration.Remove(registration);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Registration deleted successfully." });
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var registrations = _unitOfWork.Registration.GetAll();
            return Json(new { data = registrations });
        }

        [HttpGet]
        public IActionResult GetByLanguage(string language)
        {
            var registrations = _unitOfWork.Registration.GetRegistrationsByLanguage(language);
            return Json(new { data = registrations });
        }

        #endregion
    }
} 