using LesExpo.web.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LesExpo.web.Services
{
    public class UrlLocalizationService : IUrlLocalizationService
    {
        private readonly ILogger<UrlLocalizationService> _logger;
        private readonly LocalizedRoutesConfig _routesConfig;
        
        public UrlLocalizationService(
            ILogger<UrlLocalizationService> logger, 
            IOptions<LocalizedRoutesConfig> routesConfig)
        {
            _logger = logger;
            _routesConfig = routesConfig.Value;
        }
        
        public string GetLocalizedRoute(string controller, string action, string language)
        {
            var route = _routesConfig.GetRoute(controller, action, language);
            if (!string.IsNullOrEmpty(route))
            {
                return route;
            }
            
            _logger.LogWarning($"No localized route found for controller '{controller}', action '{action}' and language '{language}'");
            return action.ToLower(); // Fallback
        }
        
        public bool IsValidRouteForLanguage(string controller, string route, string language, string action)
        {
            var expectedRoute = GetLocalizedRoute(controller, action, language);
            return string.Equals(route, expectedRoute, StringComparison.OrdinalIgnoreCase);
        }
        
        public string GetCanonicalUrl(string controller, string action, string language, object? routeValues = null)
        {
            var localizedRoute = GetLocalizedRoute(controller, action, language);
            
            // Build the URL based on the localized route configuration
            var baseUrl = language.ToLower() == "tr" ? "https://lesexpo.com/tr" : "https://lesexpo.com/en";
            
            // For AboutUs controller, add the appropriate path segment
            if (controller.Equals("AboutUs", StringComparison.OrdinalIgnoreCase))
            {
                baseUrl += language.ToLower() == "tr" ? "/hakkimizda" : "/about-us";
            }
            
            return $"{baseUrl}/{localizedRoute}";
        }
        
        public Dictionary<string, string> GetAlternateLanguageUrls(string controller, string action, string currentLanguage, object? routeValues = null)
        {
            var alternateUrls = new Dictionary<string, string>();
            var actionRoutes = _routesConfig.GetAllRoutesForAction(controller, action);
            
            foreach (var kvp in actionRoutes)
            {
                var lang = kvp.Key;
                if (lang != currentLanguage.ToLower())
                {
                    alternateUrls[lang] = GetCanonicalUrl(controller, action, lang, routeValues);
                }
            }
            
            return alternateUrls;
        }
        
        /// <summary>
        /// Gets all valid routes for a specific controller and action across all languages
        /// </summary>
        public IEnumerable<string> GetAllRoutesForAction(string controller, string action)
        {
            return _routesConfig.GetAllRoutesForAction(controller, action).Values;
        }
        
        /// <summary>
        /// Finds the correct language for a given route, controller and action
        /// </summary>
        public string? GetLanguageForRoute(string controller, string route, string action)
        {
            return _routesConfig.GetLanguageForRoute(controller, action, route);
        }
    }
} 