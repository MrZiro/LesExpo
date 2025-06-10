using LesExpo.web.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LesExpo.web.Services
{
    public class UrlLocalizationService : IUrlLocalizationService
    {
        private readonly ILogger<UrlLocalizationService> _logger;
        private readonly LinkGenerator _linkGenerator;
        private readonly LocalizedRoutesConfig _routesConfig;
        
        public UrlLocalizationService(
            ILogger<UrlLocalizationService> logger, 
            LinkGenerator linkGenerator,
            IOptions<LocalizedRoutesConfig> routesConfig)
        {
            _logger = logger;
            _linkGenerator = linkGenerator;
            _routesConfig = routesConfig.Value;
        }
        
        public string GetLocalizedRoute(string action, string language)
        {
            // Default to "Blogs" controller for backward compatibility
            return GetLocalizedRoute("Blogs", action, language);
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
        
        public bool IsValidRouteForLanguage(string route, string language, string action)
        {
            var expectedRoute = GetLocalizedRoute(action, language);
            return string.Equals(route, expectedRoute, StringComparison.OrdinalIgnoreCase);
        }
        
        public string GetCanonicalUrl(string action, string language, object? routeValues = null)
        {
            var localizedRoute = GetLocalizedRoute(action, language);
            
            var values = new RouteValueDictionary(routeValues)
            {
                ["lang"] = language.ToLower(),
                ["action"] = localizedRoute
            };
            
            return _linkGenerator.GetPathByAction(action, "Blogs", values) ?? $"/{language}/{localizedRoute}";
        }
        
        public Dictionary<string, string> GetAlternateLanguageUrls(string action, string currentLanguage, object? routeValues = null)
        {
            var alternateUrls = new Dictionary<string, string>();
            var actionRoutes = _routesConfig.GetAllRoutesForAction("Blogs", action);
            
            foreach (var kvp in actionRoutes)
            {
                var lang = kvp.Key;
                if (lang != currentLanguage.ToLower())
                {
                    alternateUrls[lang] = GetCanonicalUrl(action, lang, routeValues);
                }
            }
            
            return alternateUrls;
        }
        
        /// <summary>
        /// Gets all valid routes for a specific action across all languages
        /// </summary>
        public IEnumerable<string> GetAllRoutesForAction(string action)
        {
            return _routesConfig.GetAllRoutesForAction("Blogs", action).Values;
        }
        
        /// <summary>
        /// Finds the correct language for a given route and action
        /// </summary>
        public string? GetLanguageForRoute(string route, string action)
        {
            return _routesConfig.GetLanguageForRoute("Blogs", action, route);
        }
    }
} 