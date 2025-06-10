using Microsoft.AspNetCore.Mvc;

namespace LesExpo.web.Services
{
    public interface IUrlLocalizationService
    {
        /// <summary>
        /// Gets the localized route for a given controller, action and language
        /// </summary>
        string GetLocalizedRoute(string controller, string action, string language);
        
        /// <summary>
        /// Checks if a route is valid for the given language
        /// </summary>
        bool IsValidRouteForLanguage(string controller, string route, string language, string action);
        
        /// <summary>
        /// Gets the canonical URL for a specific controller, action and language
        /// </summary>
        string GetCanonicalUrl(string controller, string action, string language, object? routeValues = null);
        
        /// <summary>
        /// Gets all available languages for a specific controller and action
        /// </summary>
        Dictionary<string, string> GetAlternateLanguageUrls(string controller, string action, string currentLanguage, object? routeValues = null);
    }
} 