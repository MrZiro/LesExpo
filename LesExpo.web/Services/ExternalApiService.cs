using LesExpo.Models.ViewModels.ExternalData;
using LesExpo.web.Models.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace LesExpo.web.Services
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ExternalApiService> _logger;
        private readonly ExternalApiConfig _config;

        public ExternalApiService(
            IHttpClientFactory httpClientFactory, 
            IMemoryCache cache,
            ILogger<ExternalApiService> logger,
            IOptions<ExternalApiConfig> config)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
            _logger = logger;
            _config = config.Value;
        }

        public async Task<ApiResponse<List<Ulke>>> GetStatesAsync()
        {
            const string cacheKey = "countries_list";
            
            // Try to get from cache first
            if (_cache.TryGetValue(cacheKey, out ApiResponse<List<Ulke>> cachedStates))
            {
                _logger.LogInformation("Countries retrieved from cache");
                return cachedStates;
            }

            _logger.LogInformation("Fetching countries from external API");
            
            var result = await ExecuteWithRetryAsync(async () =>
            {
                var httpClient = _httpClientFactory.CreateClient("FairApi");
                httpClient.Timeout = TimeSpan.FromSeconds(_config.Timeout);
                
                string requestUri = $"{_config.BaseUrl}Api/GetUlkeler?&UserName={_config.Username}&Password={_config.Password}";
                _logger.LogDebug("Making request to: {RequestUri}", requestUri.Replace(_config.Password, "***"));
                
                var response = await httpClient.GetAsync(requestUri);
                
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Ulke>>>(jsonString);
                    
                    if (apiResponse != null && apiResponse.success && apiResponse.data?.Any() == true)
                    {
                        _logger.LogInformation("Successfully fetched {Count} countries", apiResponse.data.Count);
                        return apiResponse;
                    }
                    else
                    {
                        _logger.LogWarning("API returned empty or invalid countries data");
                        return new ApiResponse<List<Ulke>> { success = false, data = new List<Ulke>() };
                    }
                }
                else
                {
                    _logger.LogError("API request failed with status code: {StatusCode}", response.StatusCode);
                    return new ApiResponse<List<Ulke>> { success = false, data = new List<Ulke>() };
                }
            });

            // Cache successful results for 1 hour
            if (result.success && result.data?.Any() == true)
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(30),
                    Priority = CacheItemPriority.High
                };
                _cache.Set(cacheKey, result, cacheOptions);
                _logger.LogInformation("Countries cached for 1 hour");
            }

            return result;
        }

        public async Task<ApiResponse<List<Sehir>>> GetCitiesAsync(int ulkeId)
        {
            string cacheKey = $"cities_list_{ulkeId}";
            
            // Try to get from cache first
            if (_cache.TryGetValue(cacheKey, out ApiResponse<List<Sehir>> cachedCities))
            {
                _logger.LogInformation("Cities for country {UlkeId} retrieved from cache", ulkeId);
                return cachedCities;
            }

            _logger.LogInformation("Fetching cities for country {UlkeId} from external API", ulkeId);
            
            var result = await ExecuteWithRetryAsync(async () =>
            {
                var httpClient = _httpClientFactory.CreateClient("FairApi");
                httpClient.Timeout = TimeSpan.FromSeconds(_config.Timeout);
                
                string requestUri = $"{_config.BaseUrl}Api/GetSehirler?UlkeId={ulkeId}&UserName={_config.Username}&Password={_config.Password}";
                _logger.LogDebug("Making request to: {RequestUri}", requestUri.Replace(_config.Password, "***"));
                
                var response = await httpClient.GetAsync(requestUri);
                
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Sehir>>>(jsonString);
                    
                    if (apiResponse != null && apiResponse.success)
                    {
                        _logger.LogInformation("Successfully fetched {Count} cities for country {UlkeId}", 
                            apiResponse.data?.Count ?? 0, ulkeId);
                        return apiResponse;
                    }
                    else
                    {
                        _logger.LogWarning("API returned empty or invalid cities data for country {UlkeId}", ulkeId);
                        return new ApiResponse<List<Sehir>> { success = false, data = new List<Sehir>() };
                    }
                }
                else
                {
                    _logger.LogError("API request failed with status code: {StatusCode} for country {UlkeId}", 
                        response.StatusCode, ulkeId);
                    return new ApiResponse<List<Sehir>> { success = false, data = new List<Sehir>() };
                }
            });

            // Cache successful results for 2 hours
            if (result.success)
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2),
                    SlidingExpiration = TimeSpan.FromHours(1),
                    Priority = CacheItemPriority.Normal
                };
                _cache.Set(cacheKey, result, cacheOptions);
                _logger.LogInformation("Cities for country {UlkeId} cached for 2 hours", ulkeId);
            }

            return result;
        }

        public async Task<ApiResponse<List<Sektor>>> GetSectorAsync()
        {
            const string cacheKey = "sectors_list";
            
            // Try to get from cache first
            if (_cache.TryGetValue(cacheKey, out ApiResponse<List<Sektor>> cachedSectors))
            {
                _logger.LogInformation("Sectors retrieved from cache");
                return cachedSectors;
            }

            _logger.LogInformation("Fetching sectors from external API");
            
            var result = await ExecuteWithRetryAsync(async () =>
            {
                var httpClient = _httpClientFactory.CreateClient("FairApi");
                httpClient.Timeout = TimeSpan.FromSeconds(_config.Timeout);
                
                string requestUri = $"{_config.BaseUrl}Api/GetSektorler?UserName={_config.Username}&Password={_config.Password}";
                _logger.LogDebug("Making request to: {RequestUri}", requestUri.Replace(_config.Password, "***"));
                
                var response = await httpClient.GetAsync(requestUri);
                
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Sektor>>>(jsonString);
                    
                    if (apiResponse != null && apiResponse.success && apiResponse.data?.Any() == true)
                    {
                        _logger.LogInformation("Successfully fetched {Count} sectors", apiResponse.data.Count);
                        return apiResponse;
                    }
                    else
                    {
                        _logger.LogWarning("API returned empty or invalid sectors data");
                        return new ApiResponse<List<Sektor>> { success = false, data = new List<Sektor>() };
                    }
                }
                else
                {
                    _logger.LogError("API request failed with status code: {StatusCode}", response.StatusCode);
                    return new ApiResponse<List<Sektor>> { success = false, data = new List<Sektor>() };
                }
            });

            // Cache successful results for 4 hours (sectors change less frequently)
            if (result.success && result.data?.Any() == true)
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4),
                    SlidingExpiration = TimeSpan.FromHours(2),
                    Priority = CacheItemPriority.High
                };
                _cache.Set(cacheKey, result, cacheOptions);
                _logger.LogInformation("Sectors cached for 4 hours");
            }

            return result;
        }

        public async Task<(bool Success, string ResponseContent, string ErrorMessage)> AddZiyaretciAsync(
            string firstName, string lastName, string email, string phone, string gender,
            string companyName, string position, string sector, int countryId, string city,
            bool isYabanci, int fuarId = 4139)
        {
            _logger.LogInformation("Submitting visitor registration for {Email} from {Company}", email, companyName);
            
            return await ExecuteWithRetryAsync(async () =>
            {
                var httpClient = _httpClientFactory.CreateClient("FairApi");
                httpClient.Timeout = TimeSpan.FromSeconds(_config.Timeout);
                
                var apiData = new
                {
                    userName = _config.Username,
                    password = _config.Password,
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

                _logger.LogDebug("Submitting visitor data to API for {Email}", email);
                var response = await httpClient.PostAsync($"{_config.BaseUrl}Api/AddZiyaretci", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully submitted visitor registration for {Email}", email);
                    return (true, responseContent, null);
                }
                else
                {
                    _logger.LogError("Visitor registration failed for {Email} with status {StatusCode}: {Response}", 
                        email, response.StatusCode, responseContent);
                    return (false, responseContent, $"API call failed with status {response.StatusCode}");
                }
            });
        }

        private async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation)
        {
            var retryCount = 0;
            var maxRetries = _config.RetryCount;
            var retryDelay = TimeSpan.FromSeconds(_config.RetryDelaySeconds);

            while (retryCount <= maxRetries)
            {
                try
                {
                    return await operation();
                }
                catch (HttpRequestException ex)
                {
                    retryCount++;
                    
                    if (retryCount > maxRetries)
                    {
                        _logger.LogError(ex, "HTTP request failed after {RetryCount} retries", maxRetries);
                        throw;
                    }

                    _logger.LogWarning(ex, "HTTP request failed, retrying {RetryCount}/{MaxRetries} after {DelaySeconds}s", 
                        retryCount, maxRetries, retryDelay.TotalSeconds);
                    
                    await Task.Delay(retryDelay * retryCount); // Exponential backoff
                }
                catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
                {
                    retryCount++;
                    
                    if (retryCount > maxRetries)
                    {
                        _logger.LogError(ex, "Request timeout after {RetryCount} retries", maxRetries);
                        throw;
                    }

                    _logger.LogWarning(ex, "Request timeout, retrying {RetryCount}/{MaxRetries} after {DelaySeconds}s", 
                        retryCount, maxRetries, retryDelay.TotalSeconds);
                    
                    await Task.Delay(retryDelay * retryCount);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error in API call");
                    throw;
                }
            }

            throw new InvalidOperationException("This should never be reached");
        }
    }
} 