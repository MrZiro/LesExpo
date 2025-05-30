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
using System.Text.RegularExpressions;

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
                // Create - set default values
                sliderVM.Slider.MediaType = "image";
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
                // Handle media based on type
                if (sliderVM.Slider.MediaType == "image")
                {
                    // Clear video-related fields when switching to image
                    sliderVM.Slider.VideoUrl = null;
                    sliderVM.Slider.YoutubeUrl = null;
                    sliderVM.Slider.VideoSource = null;

                    // Handle image upload
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
                }
                else if (sliderVM.Slider.MediaType == "video")
                {
                    // Clear image field when switching to video
                    sliderVM.Slider.ImageUrl = null;

                    if (sliderVM.Slider.VideoSource == "upload")
                    {
                        // Clear YouTube URL when using upload
                        sliderVM.Slider.YoutubeUrl = null;

                        // Handle video file upload
                        if (sliderVM.VideoFile != null)
                        {
                            if (!ValidateVideoFile(sliderVM.VideoFile))
                            {
                                ModelState.AddModelError(nameof(sliderVM.VideoFile), "Geçersiz video dosyası.");
                                return View(sliderVM);
                            }
                            
                            string? uploadPath = await _fileHelper.SaveFileAsync(
                                sliderVM.VideoFile, 
                                sliderVM.Slider.VideoUrl, 
                                "sliders/videos");
                            
                            if (uploadPath != null)
                            {
                                sliderVM.Slider.VideoUrl = uploadPath;
                            }
                        }
                    }
                    else if (sliderVM.Slider.VideoSource == "youtube")
                    {
                        // Clear video file when using YouTube
                        sliderVM.Slider.VideoUrl = null;

                        // Validate YouTube URL
                        if (!string.IsNullOrEmpty(sliderVM.Slider.YoutubeUrl))
                        {
                            if (!IsValidYouTubeUrl(sliderVM.Slider.YoutubeUrl))
                            {
                                ModelState.AddModelError(nameof(sliderVM.Slider.YoutubeUrl), "Geçerli bir YouTube URL'si girin.");
                                return View(sliderVM);
                            }
                            
                            // Convert to embed URL for storage
                            sliderVM.Slider.YoutubeUrl = ConvertToEmbedUrl(sliderVM.Slider.YoutubeUrl);
                        }
                    }
                }

                if (sliderVM.Slider.Id == 0)
                {
                    // Create
                    sliderVM.Slider.CreatedDate = DateTime.Now;
                    _unitOfWork.Slider.Add(sliderVM.Slider);
                    TempData["success"] = "Slider başarıyla oluşturuldu";
                }
                else
                {
                    // Update
                    sliderVM.Slider.UpdatedDate = DateTime.Now;
                    _unitOfWork.Slider.Update(sliderVM.Slider);
                    TempData["success"] = "Slider başarıyla güncellendi";
                }
                
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            
            return View(sliderVM);
        }

        private bool ValidateVideoFile(IFormFile videoFile)
        {
            var allowedExtensions = new[] { ".mp4", ".avi", ".mov", ".wmv", ".webm" };
            var allowedMimeTypes = new[] { "video/mp4", "video/avi", "video/mov", "video/wmv", "video/webm" };
            const long maxSize = 50 * 1024 * 1024; // 50MB

            var extension = Path.GetExtension(videoFile.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(extension))
                return false;
                
            if (!allowedMimeTypes.Contains(videoFile.ContentType.ToLowerInvariant()))
                return false;
                
            if (videoFile.Length > maxSize)
                return false;

            return true;
        }

        private bool IsValidYouTubeUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            var youtubeRegex = new Regex(@"^(https?://)?(www\.)?(youtube\.com/watch\?v=|youtu\.be/)([a-zA-Z0-9_-]{11})");
            return youtubeRegex.IsMatch(url);
        }

        private string ConvertToEmbedUrl(string url)
        {
            var videoId = ExtractVideoId(url);
            if (!string.IsNullOrEmpty(videoId))
            {
                return $"https://www.youtube.com/embed/{videoId}";
            }
            return url;
        }

        private string ExtractVideoId(string url)
        {
            var match = Regex.Match(url, @"(?:youtube\.com/watch\?v=|youtu\.be/)([a-zA-Z0-9_-]{11})");
            return match.Success ? match.Groups[1].Value : null;
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
                return Json(new { success = false, message = "Silinirken hata oluştu" });
            }

            // Delete the media files if they exist
            if (!string.IsNullOrEmpty(slider.ImageUrl))
            {
                await _fileHelper.DeleteFileAsync(slider.ImageUrl);
            }
            
            if (!string.IsNullOrEmpty(slider.VideoUrl))
            {
                await _fileHelper.DeleteFileAsync(slider.VideoUrl);
            }

            _unitOfWork.Slider.Remove(slider);
            _unitOfWork.Save();
            
            return Json(new { success = true, message = "Slider başarıyla silindi" });
        }

        #endregion
    }
}