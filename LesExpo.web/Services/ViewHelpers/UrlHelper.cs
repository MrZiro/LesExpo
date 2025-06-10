using LesExpo.web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LesExpo.web.Services.ViewHelpers
{
    /// <summary>
    /// Static helper class for generating localized URLs in views
    /// Now uses the configuration-based URL localization service
    /// </summary>
    public static class LocalizedUrlHelper
    {
        /// <summary>
        /// Generates a localized URL for any controller action
        /// </summary>
        public static string LocalizedUrl(IUrlHelper urlHelper, IUrlLocalizationService urlService, string controller, string action, string language, object? routeValues = null)
        {
            var route = urlService.GetLocalizedRoute(controller, action, language);
            
            var values = new RouteValueDictionary(routeValues)
            {
                ["lang"] = language.ToLower()
            };

            return urlHelper.Action(route, controller, values);
        }
        
        /// <summary>
        /// Generates a localized URL for blog actions (backward compatibility)
        /// </summary>
        public static string BlogUrl(IUrlHelper urlHelper, IUrlLocalizationService urlService, string action, string language, object? routeValues = null)
        {
            return LocalizedUrl(urlHelper, urlService, "Blogs", action, language, routeValues);
        }
        
        /// <summary>
        /// Generates language switch URLs
        /// </summary>
        public static string SwitchLanguageUrl(IUrlHelper urlHelper, IUrlLocalizationService urlService, string controller, string currentAction, string currentLang, string targetLang, object? routeValues = null)
        {
            return LocalizedUrl(urlHelper, urlService, controller, currentAction, targetLang, routeValues);
        }
    }
} 