using LesExpo.Models.ViewModels.ExternalData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LesExpo.web.Controllers
{
    public class ExtranalDataController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string API_USERNAME = "lesexpo_com_fair";
        private const string API_PASSWORD = "L2025e-16X%3F";
        public ExtranalDataController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region ApiCalls
        public async Task<IActionResult> GetStates()
        {
            var httpClient = _httpClientFactory.CreateClient("FairApi");
            try
            {
                string statesRequestUri = $"https://fair.smartexpo.com.tr/Api/GetUlkeler?&UserName={API_USERNAME}&Password={API_PASSWORD}";
                var response = await httpClient.GetAsync(statesRequestUri);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();

                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Ulke>>>(jsonString);
                    if (apiResponse != null)
                    {
                        return Json(apiResponse);
                    }
                    else
                    {
                        return StatusCode(500, "Invalid API response format.");
                    }
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Failed to retrieve countries from the API. Response: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}. StackTrace: {ex.StackTrace}");
            }
        }

        public async Task<IActionResult> GetCities(int ulkeId)
        {
            var httpClient = _httpClientFactory.CreateClient("FairApi");
            try
            {
                string citiesRequestUri = $"https://fair.smartexpo.com.tr/Api/GetSehirler?UlkeId={ulkeId}&UserName={API_USERNAME}&Password={API_PASSWORD}";
                var response = await httpClient.GetAsync(citiesRequestUri);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();

                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Sehir>>>(jsonString);
                    if (apiResponse != null)
                    {
                        return Json(apiResponse);
                    }
                    else
                    {
                        return StatusCode(500, "Invalid API response format.");
                    }
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Failed to retrieve cities from the API. Response: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}. StackTrace: {ex.StackTrace}");
            }
        }

        public async Task<IActionResult> GetSector()
        {
            var httpClient = _httpClientFactory.CreateClient("FairApi");
            try
            {
                string sectorRequestUri = $"https://fair.smartexpo.com.tr/Api/GetSektorler?UserName={API_USERNAME}&Password={API_PASSWORD}";
                var response = await httpClient.GetAsync(sectorRequestUri);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();

                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Sektor>>>(jsonString);
                    if (apiResponse != null)
                    {
                        return Json(apiResponse);
                    }
                    else
                    {
                        return StatusCode(500, "Invalid API response format.");
                    }
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Failed to retrieve sectors from the API. Response: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}. StackTrace: {ex.StackTrace}");
            }
        }


        #endregion
    }
}
