using Microsoft.AspNetCore.Mvc;

namespace LesExpo.web.Services
{
    public interface IUrlLocalizationService
    {
        /// <summary>
        /// Gets the localized route for a given action and language (defaults to Blogs controller)
        /// </summary>
        string GetLocalizedRoute(string action, string language);
        
        /// <summary>
        /// Gets the localized route for a given controller, action and language
        /// </summary>
        string GetLocalizedRoute(string controller, string action, string language);
        
        /// <summary>
        /// Checks if a route is valid for the given language
        /// </summary>
        bool IsValidRouteForLanguage(string route, string language, string action);
        
        /// <summary>
        /// Gets the canonical URL for a route and language
        /// </summary>
        string GetCanonicalUrl(string action, string language, object? routeValues = null);
        
        /// <summary>
        /// Gets all available languages for a route
        /// </summary>
        Dictionary<string, string> GetAlternateLanguageUrls(string action, string currentLanguage, object? routeValues = null);
    }
} 