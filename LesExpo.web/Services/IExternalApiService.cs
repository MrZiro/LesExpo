using LesExpo.Models.ViewModels.ExternalData;

namespace LesExpo.web.Services
{
    public interface IExternalApiService
    {
        Task<ApiResponse<List<Ulke>>> GetStatesAsync();
        Task<ApiResponse<List<Sehir>>> GetCitiesAsync(int ulkeId);
        Task<ApiResponse<List<Sektor>>> GetSectorAsync();
        Task<(bool Success, string ResponseContent, string ErrorMessage)> AddZiyaretciAsync(
            string firstName, string lastName, string email, string phone, string gender,
            string companyName, string position, string sector, int countryId, string city,
            bool isYabanci, int fuarId = 4139);
    }
} 