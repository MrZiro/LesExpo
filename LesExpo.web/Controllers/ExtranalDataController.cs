using LesExpo.Models.ViewModels.ExternalData;
using LesExpo.web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LesExpo.web.Controllers
{
    public class ExtranalDataController : Controller
    {
        private readonly IExternalApiService _externalApiService;

        public ExtranalDataController(IExternalApiService externalApiService)
        {
            _externalApiService = externalApiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region ApiCalls
        public async Task<IActionResult> GetStates()
        {
            try
            {
                var apiResponse = await _externalApiService.GetStatesAsync();
                return Json(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetCities(int ulkeId)
        {
            try
            {
                var apiResponse = await _externalApiService.GetCitiesAsync(ulkeId);
                return Json(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetSector()
        {
            try
            {
                var apiResponse = await _externalApiService.GetSectorAsync();
                return Json(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        #endregion
    }
}
