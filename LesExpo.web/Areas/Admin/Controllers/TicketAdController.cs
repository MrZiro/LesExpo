using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.Models;
using LesExpo.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LesExpo.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class TicketAdController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketAdController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ViewBag.Count = _unitOfWork.Ticket.GetCount();
            return View();
        }

        // GET: Admin/TicketAd/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Ticket ticket = _unitOfWork.Ticket.Get(u => u.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // DELETE: Admin/TicketAd/Delete/5
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var ticket = _unitOfWork.Ticket.Get(u => u.Id == id);
            if (ticket == null)
            {
                return Json(new { success = false, message = "Ticket not found." });
            }

            _unitOfWork.Ticket.Remove(ticket);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Ticket deleted successfully." });
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var tickets = _unitOfWork.Ticket.GetAll();
            return Json(new { data = tickets });
        }

        [HttpGet]
        public IActionResult GetByLanguage(string language)
        {
            var tickets = _unitOfWork.Ticket.GetTicketsByLanguage(language);
            return Json(new { data = tickets });
        }

        #endregion
    }
} 