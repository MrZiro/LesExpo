using LesExpo.Models.ViewModels.ExternalData;
using LesExpo.web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LesExpo.web.Controllers
{
    public class ExtranalDataController : Controller
    {
        private readonly IExternalApiService _externalApiService;
        private readonly ILogger<ExtranalDataController> _logger;

        public ExtranalDataController(
            IExternalApiService externalApiService,
            ILogger<ExtranalDataController> logger)
        {
            _externalApiService = externalApiService;
            _logger = logger;
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
                _logger.LogDebug("Getting countries data");
                var apiResponse = await _externalApiService.GetStatesAsync();
                _logger.LogDebug("Successfully retrieved countries data with {Count} items", 
                    apiResponse?.data?.Count ?? 0);
                return Json(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get countries data");
                return StatusCode(500, "Failed to retrieve countries data");
            }
        }

        public async Task<IActionResult> GetCities(int ulkeId)
        {
            try
            {
                _logger.LogDebug("Getting cities data for country {UlkeId}", ulkeId);
                var apiResponse = await _externalApiService.GetCitiesAsync(ulkeId);
                _logger.LogDebug("Successfully retrieved cities data for country {UlkeId} with {Count} items", 
                    ulkeId, apiResponse?.data?.Count ?? 0);
                return Json(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get cities data for country {UlkeId}", ulkeId);
                return StatusCode(500, "Failed to retrieve cities data");
            }
        }

        public async Task<IActionResult> GetSector()
        {
            try
            {
                _logger.LogDebug("Getting sectors data");
                var apiResponse = await _externalApiService.GetSectorAsync();
                _logger.LogDebug("Successfully retrieved sectors data with {Count} items", 
                    apiResponse?.data?.Count ?? 0);
                return Json(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get sectors data");
                return StatusCode(500, "Failed to retrieve sectors data");
            }
        }


        public async Task<IActionResult> CacheTest()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            var countries = await _externalApiService.GetStatesAsync();
            
            stopwatch.Stop();
            
            return Json(new
            {
                cached = stopwatch.ElapsedMilliseconds < 100, // If under 100ms, likely cached
                elapsed_ms = stopwatch.ElapsedMilliseconds,
                countries_count = countries?.data?.Count ?? 0,
                timestamp = DateTime.Now
            });
        }

        #endregion
    }
}
