using System.Text.RegularExpressions;
using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.Models;

namespace LesExpo.web.Services
{
    public class ContentIndexService : IContentIndexService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ContentIndexService> _logger;

        public ContentIndexService(IWebHostEnvironment webHostEnvironment, ILogger<ContentIndexService> logger, IUnitOfWork unitOfWork)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<object> GetSearchIndexAsync(string language, string query = "")
        {
            var results = new List<object>();

            // 1) Load pre-generated static JSON from wwwroot/search/index.{lang}.json
            try
            {
                var staticPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "search", $"index.{language}.json");
                if (File.Exists(staticPath))
                {
                    var json = await File.ReadAllTextAsync(staticPath);
                    var staticItems = System.Text.Json.JsonSerializer.Deserialize<List<PageItem>>(json) ?? new List<PageItem>();
                    // Recompute contextual summaries if a query is provided
                    if (!string.IsNullOrEmpty(query))
                    {
                        foreach (var item in staticItems)
                        {
                            item.summary = GetContextualSummary(item.content ?? string.Empty, query, 200);
                        }
                    }
                    results.AddRange(staticItems);
                }
                else
                {
                    _logger.LogWarning("Static search index file not found: {Path}", staticPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed loading static search index");
            }

            // 2) Append dynamic items from DB (Blogs)
            try
            {
                var blogs = _unitOfWork.Blog.GetAll(includeProperties: "ContentType")
                    .Where(b => b.IsPublished && b.Language.ToLower() == language.ToLower())
                    .OrderByDescending(b => b.CreatedAt)
                    .ToList();

                foreach (var b in blogs)
                {
                    var content = CleanViewContent((b.MetaDescription ?? b.Content) ?? string.Empty);
                    var summary = string.IsNullOrEmpty(query)
                        ? TruncateText(content, 200)
                        : GetContextualSummary(content, query, 200);

                    var keywords = string.IsNullOrWhiteSpace(b.MetaKeywords)
                        ? ExtractKeywords(content)
                        : b.MetaKeywords.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                            .Select(k => k.ToLowerInvariant()).Distinct().Take(15).ToArray();

                    results.Add(new
                    {
                        id = $"blog_{b.Id}",
                        type = "blog",
                        title = b.Title,
                        content,
                        summary,
                        keywords,
                        url = BuildBlogUrl(b, language),
                        language = b.Language.ToLower(),
                        category = b.ContentType?.Name ?? (language == "en" ? "Blog" : "Blog")
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed loading dynamic search items");
            }

            return results;
        }

        public string ExtractTextFromView(string viewPath)
        {
            try
            {
                var fullPath = Path.Combine(_webHostEnvironment.ContentRootPath, viewPath);
                if (!File.Exists(fullPath))
                {
                    _logger.LogWarning("View file not found: {ViewPath}", viewPath);
                    return string.Empty;
                }

                var rawContent = File.ReadAllText(fullPath);
                return CleanViewContent(rawContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading view file: {ViewPath}", viewPath);
                return string.Empty;
            }
        }

        public object CreatePageSearchItem(string pageKey, string title, string content, string language, string query = "")
        {
            var summary = GetContextualSummary(content, query, 200);
            var keywords = ExtractKeywords(content);
            var url = GetPageUrl(pageKey, language);

            return new
            {
                id = $"page_{pageKey}",
                type = "page",
                title = title,
                content = content,
                summary = summary,
                keywords = keywords,
                url = url,
                language = language,
                category = GetPageCategory(pageKey, language)
            };
        }

        private List<PageDefinition> GetPageDefinitions(string language)
        {
            return new List<PageDefinition>
            {
                new("home", 
                    language == "en" ? "Home" : "AnaSayfa", 
                    $"Views/{language}/Home/Index.cshtml"),
                
                new("contact", 
                    language == "en" ? "Contact" : "İletişim", 
                    $"Views/{language}/Contact/Index.cshtml"),
                
                new("exhibition-identification", 
                    language == "en" ? "Exhibition Identification" : "Fuar Künyesi", 
                    $"Views/{language}/AboutUs/exhibitionidentification.cshtml"),
                
                new("exhibition-area", 
                    language == "en" ? "Exhibition Area" : "Fuar Alanı", 
                    $"Views/{language}/AboutUs/ExhibitionArea.cshtml"),
                
                new("how-to-reach", 
                    language == "en" ? "How to Reach" : "Ulaşım", 
                    $"Views/{language}/AboutUs/HowToReach.cshtml"),
                
                new("visitor-profile", 
                    language == "en" ? "Visitor Profile" : "Ziyaretçi Profili", 
                    $"Views/{language}/AboutUs/VisitorProfile.cshtml"),
                
                new("exhibitor-profile", 
                    language == "en" ? "Exhibitor Profile" : "Katılımcı Profili", 
                    $"Views/{language}/AboutUs/ExhibitorProfile.cshtml"),
                
                new("why-attend", 
                    language == "en" ? "Why Attend" : "Neden Katılmalısınız", 
                    $"Views/{language}/AboutUs/WhyAttend.cshtml"),
                
                new("b2b-meetings", 
                    language == "en" ? "B2B Meetings" : "B2B Toplantıları", 
                    $"Views/{language}/AboutUs/B2BMeeting.cshtml"),
                
                new("experiences", 
                    language == "en" ? "Experiences" : "Deneyimler", 
                    $"Views/{language}/AboutUs/Experiences.cshtml"),
                
                new("exhibitor-list", 
                    language == "en" ? "Exhibitor List" : "Katılımcı Listesi", 
                    $"Views/{language}/AboutUs/ExhibitorList.cshtml"),
                
                new("media-sponsorship", 
                    language == "en" ? "Media & Sponsorship" : "Medya ve Sponsorluk", 
                    $"Views/{language}/AboutUs/MediaSponsorship.cshtml"),
                
                new("frequently-asked-questions", 
                    language == "en" ? "FAQ" : "Sık Sorulan Sorular", 
                    $"Views/{language}/AboutUs/FrequentlyAskedQuestions.cshtml")
            };
        }

        private string CleanViewContent(string rawContent)
        {
            if (string.IsNullOrEmpty(rawContent))
                return string.Empty;

            var content = rawContent;

            // Remove Razor code blocks - enhanced patterns
            content = Regex.Replace(content, @"@\{[^{}]*(?:\{[^{}]*\}[^{}]*)*\}", " ", RegexOptions.Singleline);
            content = Regex.Replace(content, @"@\{.*?\}", " ", RegexOptions.Singleline);
            
            // Remove ViewData/ViewBag assignments
            content = Regex.Replace(content, @"ViewData\[[^\]]*\]\s*=\s*[^;]*;", " ");
            content = Regex.Replace(content, @"ViewBag\.[^\s]*\s*=\s*[^;]*;", " ");
            
            // Remove Razor directives and statements
            content = Regex.Replace(content, @"@(model|section|if|foreach|for|while|using|inject|layout)[^\r\n]*", " ");
            content = Regex.Replace(content, @"@[A-Za-z][A-Za-z0-9_.]*(\([^)]*\))?", " ");
            
            // Remove leftover brackets and semicolons from Razor syntax
            content = Regex.Replace(content, @"[{}];?", " ");
            content = Regex.Replace(content, @"^\s*[;}]+\s*", " ", RegexOptions.Multiline);
            
            // Remove HTML/script/style tags but keep text content
            content = Regex.Replace(content, @"<script[^>]*>.*?</script>", " ", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"<style[^>]*>.*?</style>", " ", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"<[^>]+>", " ");
            
            // Clean up HTML entities
            content = System.Net.WebUtility.HtmlDecode(content);
            
            // Remove multiple punctuation marks
            content = Regex.Replace(content, @"[;{}]+", " ");
            content = Regex.Replace(content, @"[.]{3,}", "...");
            
            // Clean up whitespace
            content = Regex.Replace(content, @"\s+", " ");
            content = Regex.Replace(content, @"^\s+|\s+$", "");
            
            // Remove any remaining standalone punctuation at the start
            content = Regex.Replace(content, @"^[;{}\s]+", "");
            
            return content.Trim();
        }

        private string GetContextualSummary(string content, string query, int maxLength)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            // If no query provided, use regular truncation
            if (string.IsNullOrEmpty(query))
                return TruncateText(content, maxLength);

            // Normalize the query for better matching (handle Turkish characters)
            var normalizedQuery = query.ToLowerInvariant()
                .Replace("ı", "i")
                .Replace("ğ", "g")
                .Replace("ü", "u")
                .Replace("ş", "s")
                .Replace("ö", "o")
                .Replace("ç", "c");

            var normalizedContent = content.ToLowerInvariant()
                .Replace("ı", "i")
                .Replace("ğ", "g")
                .Replace("ü", "u")
                .Replace("ş", "s")
                .Replace("ö", "o")
                .Replace("ç", "c");

            // Find the position of the first match
            var matchIndex = normalizedContent.IndexOf(normalizedQuery);
            
            if (matchIndex == -1)
            {
                // No match found, return regular truncation
                return TruncateText(content, maxLength);
            }

            // Calculate context window
            var contextLength = maxLength / 2;
            var startIndex = Math.Max(0, matchIndex - contextLength);
            var endIndex = Math.Min(content.Length, matchIndex + query.Length + contextLength);

            // Adjust to word boundaries
            if (startIndex > 0)
            {
                var spaceIndex = content.IndexOf(' ', startIndex);
                if (spaceIndex != -1 && spaceIndex < startIndex + 20)
                    startIndex = spaceIndex + 1;
            }

            if (endIndex < content.Length)
            {
                var spaceIndex = content.LastIndexOf(' ', endIndex);
                if (spaceIndex != -1 && spaceIndex > endIndex - 20)
                    endIndex = spaceIndex;
            }

            var excerpt = content.Substring(startIndex, endIndex - startIndex).Trim();
            
            // Add ellipsis if needed
            var prefix = startIndex > 0 ? "..." : "";
            var suffix = endIndex < content.Length ? "..." : "";
            
            return $"{prefix}{excerpt}{suffix}";
        }

        private string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                return text ?? string.Empty;

            var truncated = text.Substring(0, maxLength);
            var lastSpace = truncated.LastIndexOf(' ');
            
            if (lastSpace > 0)
                truncated = truncated.Substring(0, lastSpace);
            
            return truncated + "...";
        }

        private string[] ExtractKeywords(string content)
        {
            if (string.IsNullOrEmpty(content))
                return new string[0];

            // Extract meaningful Turkish and English words (4+ characters)
            var words = Regex.Matches(content, @"\b[a-zA-ZçğıöşüÇĞIİÖŞÜ]{4,}\b")
                .Cast<Match>()
                .Select(m => m.Value.ToLowerInvariant())
                .Where(w => !IsStopWord(w))
                .Distinct()
                .Take(15)
                .ToArray();

            return words;
        }

        private bool IsStopWord(string word)
        {
            var stopWords = new HashSet<string>
            {
                // Turkish stop words
                "için", "olan", "olarak", "daha", "sonra", "kadar", "ancak", "bütün", "şayet",
                "ayrıca", "böyle", "hangi", "kendi", "neden", "burada", "şimdi", "sadece",
                
                // English stop words
                "that", "with", "have", "this", "will", "you", "from", "they", "know",
                "want", "been", "good", "much", "some", "time", "very", "when", "come",
                "here", "just", "like", "long", "make", "many", "over", "such", "take"
            };

            return stopWords.Contains(word);
        }

        private string GetPageUrl(string pageKey, string language)
        {
            var urlMap = new Dictionary<string, string>
            {
                { "home", $"/{language}/" },
                { "contact", $"/{language}/" + (language == "en" ? "contact" : "iletisim") },
                { "exhibition-identification", $"/{language}/exhibition-identification" },
                { "exhibition-area", $"/{language}/exhibition-area" },
                { "transportation", $"/{language}/transportation" },
                { "visitor-profile", $"/{language}/visitor-profile" },
                { "exhibitor-profile", $"/{language}/exhibitor-profile" },
                { "why-attend", $"/{language}/why-attend" },
                { "b2b-meetings", $"/{language}/b2b-meetings" },
                { "experiences", $"/{language}/experiences" },
                { "exhibitor-list", $"/{language}/exhibitor-list" },
                { "media-sponsorship", $"/{language}/media-sponsorship" },
                { "frequently-asked-questions", $"/{language}/faq" }
            };

            return urlMap.GetValueOrDefault(pageKey, $"/{language}/{pageKey}");
        }

        private string GetPageCategory(string pageKey, string language)
        {
            if (pageKey == "home") return language == "en" ? "home" : "Anasayfa";
            if (pageKey == "contact") return language == "en" ? "Contact" : "İletişim";
            return language == "en" ? "About Us" : "Hakkımızda";
        }

        private record PageDefinition(string Key, string Title, string ViewPath);

        private class PageItem
        {
            public string id { get; set; } = string.Empty;
            public string type { get; set; } = string.Empty;
            public string title { get; set; } = string.Empty;
            public string? content { get; set; }
            public string? summary { get; set; }
            public string[]? keywords { get; set; }
            public string url { get; set; } = string.Empty;
            public string language { get; set; } = string.Empty;
            public string category { get; set; } = string.Empty;
        }

        private static string BuildBlogUrl(Blog blog, string language)
        {
            var route = language.ToLower() == "en" ? "blog-detail" : "blog-detay";
            return $"/{language}/{route}/{blog.Slug}";
        }
    }
} 