using LesExpo.Models.ViewModels.ExternalData;
using Newtonsoft.Json;
using System.Text;

namespace LesExpo.web.Services
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string API_USERNAME = "lesexpo_com_fair";
        private const string API_PASSWORD = "L2025e-16X?";

        public ExternalApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResponse<List<Ulke>>> GetStatesAsync()
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
                    return apiResponse ?? new ApiResponse<List<Ulke>> { success = false, data = new List<Ulke>() };
                }
                else
                {
                    return new ApiResponse<List<Ulke>> { success = false, data = new List<Ulke>() };
                }
            }
            catch (Exception)
            {
                return new ApiResponse<List<Ulke>> { success = false, data = new List<Ulke>() };
            }
        }

        public async Task<ApiResponse<List<Sehir>>> GetCitiesAsync(int ulkeId)
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
                    return apiResponse ?? new ApiResponse<List<Sehir>> { success = false, data = new List<Sehir>() };
                }
                else
                {
                    return new ApiResponse<List<Sehir>> { success = false, data = new List<Sehir>() };
                }
            }
            catch (Exception)
            {
                return new ApiResponse<List<Sehir>> { success = false, data = new List<Sehir>() };
            }
        }

        public async Task<ApiResponse<List<Sektor>>> GetSectorAsync()
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
                    return apiResponse ?? new ApiResponse<List<Sektor>> { success = false, data = new List<Sektor>() };
                }
                else
                {
                    return new ApiResponse<List<Sektor>> { success = false, data = new List<Sektor>() };
                }
            }
            catch (Exception)
            {
                return new ApiResponse<List<Sektor>> { success = false, data = new List<Sektor>() };
            }
        }

        public async Task<(bool Success, string ResponseContent, string ErrorMessage)> AddZiyaretciAsync(
            string firstName, string lastName, string email, string phone, string gender,
            string companyName, string position, string sector, int countryId, string city,
            bool isYabanci, int fuarId = 4139)
        {
            var httpClient = _httpClientFactory.CreateClient("FairApi");
            try
            {
                var apiData = new
                {
                    userName = API_USERNAME,
                    password = API_PASSWORD,
                    isYabanci = isYabanci,
                    ad = firstName,
                    soyad = lastName,
                    email = email,
                    cepNo = phone,
                    cinsiyet = gender,
                    firmaAdi = companyName,
                    pozisyon = position ?? "",
                    sektor = sector,
                    ulkeId = countryId,
                    sehir = city,
                    fuarId = fuarId
                };

                var jsonContent = JsonConvert.SerializeObject(apiData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://fair.smartexpo.com.tr/Api/AddZiyaretci", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return (true, responseContent, null);
                }
                else
                {
                    return (false, responseContent, $"API call failed with status {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return (false, null, $"Internal server error: {ex.Message}");
            }
        }
    }
} 