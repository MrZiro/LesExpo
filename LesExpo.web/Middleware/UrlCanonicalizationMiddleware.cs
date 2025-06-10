using LesExpo.web.Models.Configuration;
using LesExpo.web.Services;
using Microsoft.Extensions.Options;

namespace LesExpo.web.Middleware
{
    /// <summary>
    /// Middleware that ensures URLs use the correct language-specific routes
    /// Redirects tr/all-news -> tr/tum-haberler and en/tum-haberler -> en/all-news
    /// </summary>
    public class UrlCanonicalizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UrlCanonicalizationMiddleware> _logger;
        
        public UrlCanonicalizationMiddleware(RequestDelegate next, ILogger<UrlCanonicalizationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext context, IUrlLocalizationService urlService, IOptions<LocalizedRoutesConfig> routesConfig)
        {
            // Only process GET requests to avoid interfering with POST/PUT operations
            if (context.Request.Method != "GET")
            {
                await _next(context);
                return;
            }
            
            var path = context.Request.Path.Value?.ToLower();
            if (string.IsNullOrEmpty(path))
            {
                await _next(context);
                return;
            }
            
            var config = routesConfig.Value;
            
            // Check if this is a localized route
            var pathParts = path.Trim('/').Split('/');
            if (pathParts.Length < 2)
            {
                await _next(context);
                return;
            }
            
            var lang = pathParts[0];
            var route = pathParts[1];
            
            // Only process supported languages
            if (!config.SupportedLanguages.Contains(lang))
            {
                await _next(context);
                return;
            }
            
            // Check all controllers and actions to find a match
            foreach (var controllerKvp in config.Controllers)
            {
                var controllerName = controllerKvp.Key;
                var controllerRoutes = controllerKvp.Value;
                
                foreach (var actionKvp in controllerRoutes)
                {
                    var actionName = actionKvp.Key;
                    var actionRoutes = actionKvp.Value;
                    
                    // Check if any language has this route
                    if (actionRoutes.Values.Any(r => string.Equals(r, route, StringComparison.OrdinalIgnoreCase)))
                    {
                        var correctRoute = urlService.GetLocalizedRoute(controllerName, actionName, lang);
                        
                        // If we found a route that needs canonicalization
                        if (!string.IsNullOrEmpty(correctRoute) && !string.Equals(route, correctRoute, StringComparison.OrdinalIgnoreCase))
                        {
                            // Build the canonical URL
                            var canonicalPath = path.Replace($"/{route}", $"/{correctRoute}");
                            var canonicalUrl = $"{canonicalPath}{context.Request.QueryString}";
                            
                            _logger.LogInformation($"Redirecting {path} to {canonicalPath} for SEO canonicalization");
                            
                            // 301 Permanent Redirect for SEO
                            context.Response.StatusCode = 301;
                            context.Response.Headers.Location = canonicalUrl;
                            return;
                        }
                    }
                }
            }
            
            await _next(context);
        }
    }
    
    /// <summary>
    /// Extension method to register the middleware
    /// </summary>
    public static class UrlCanonicalizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseUrlCanonicalization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UrlCanonicalizationMiddleware>();
        }
    }
} 