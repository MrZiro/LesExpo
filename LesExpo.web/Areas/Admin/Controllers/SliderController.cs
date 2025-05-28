using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.Models;
using LesExpo.Models.ViewModels;
using LesExpo.Utility;
using LesExpo.web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LesExpo.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class SliderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHelper _fileHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SliderController(
            IUnitOfWork unitOfWork,
            IFileHelper fileHelper,
            IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _fileHelper = fileHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Admin/Slider/Upsert/5
        public IActionResult Upsert(int? id)
        {
            SliderVM sliderVM = new SliderVM()
            {
                Slider = new Slider()
            };

            if (id == null || id == 0)
            {
                // Create
                return View(sliderVM);
            }
            else
            {
                // Update
                sliderVM.Slider = _unitOfWork.Slider.Get(u => u.Id == id);
                if (sliderVM.Slider == null)
                {
                    return NotFound();
                }
                return View(sliderVM);
            }
        }

        // POST: Admin/Slider/Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(SliderVM sliderVM)
        {
            if (ModelState.IsValid)
            {
                // Validate and save image file if provided
                if (sliderVM.ImageFile != null)
                {
                    if (!_fileHelper.ValidateImageFile(sliderVM.ImageFile, ModelState, nameof(sliderVM.ImageFile)))
                    {
                        return View(sliderVM);
                    }
                    
                    string? uploadPath = await _fileHelper.SaveFileAsync(
                        sliderVM.ImageFile, 
                        sliderVM.Slider.ImageUrl, 
                        "sliders");
                    
                    if (uploadPath != null)
                    {
                        sliderVM.Slider.ImageUrl = uploadPath;
                    }
                }

                if (sliderVM.Slider.Id == 0)
                {
                    // Create
                    sliderVM.Slider.CreatedDate = DateTime.Now;
                    _unitOfWork.Slider.Add(sliderVM.Slider);
                    TempData["success"] = "Slider created successfully";
                }
                else
                {
                    // Update
                    sliderVM.Slider.UpdatedDate = DateTime.Now;
                    _unitOfWork.Slider.Update(sliderVM.Slider);
                    TempData["success"] = "Slider updated successfully";
                }
                
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            
            return View(sliderVM);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var sliderList = _unitOfWork.Slider.GetAll().OrderBy(x => x.DisplayOrder);
            return Json(new { data = sliderList });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var slider = _unitOfWork.Slider.Get(u => u.Id == id);
            if (slider == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            // Delete the image file if it exists
            if (!string.IsNullOrEmpty(slider.ImageUrl))
            {
                await _fileHelper.DeleteFileAsync(slider.ImageUrl);
            }

            _unitOfWork.Slider.Remove(slider);
            _unitOfWork.Save();
            
            return Json(new { success = true, message = "Slider deleted successfully" });
        }

        #endregion
    }
}
