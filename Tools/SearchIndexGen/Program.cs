using System.Text.Json;
using System.Text.RegularExpressions;

internal class Program
{
    private static int Main(string[] args)
    {
        // Args: <webProjectRoot> <outputDirRelativeToWebRoot: wwwroot/search> <languages: en,tr>
        if (args.Length < 1)
        {
            Console.Error.WriteLine("Usage: SearchIndexGen <webProjectRoot> [outputRelativeDir] [languages]");
            return 1;
        }

        var webRoot = args[0];
        var outputDirRel = args.Length > 1 ? args[1] : "wwwroot/search";
        var languagesArg = args.Length > 2 ? args[2] : "en,tr";
        var languages = languagesArg.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var viewsRoot = Path.Combine(webRoot, "Views");
        var outputDir = Path.Combine(webRoot, outputDirRel);
        Directory.CreateDirectory(outputDir);

        foreach (var lang in languages)
        {
            var items = BuildStaticItemsForLanguage(viewsRoot, lang);
            var jsonPath = Path.Combine(outputDir, $"index.{lang}.json");
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions
            {
                WriteIndented = false
            });
            File.WriteAllText(jsonPath, json);
            Console.WriteLine($"Wrote {items.Count} items to {jsonPath}");
        }

        return 0;
    }

    private static List<object> BuildStaticItemsForLanguage(string viewsRoot, string language)
    {
        var pages = GetPageDefinitions(language);
        var results = new List<object>();
        foreach (var page in pages)
        {
            var fullPath = Path.Combine(viewsRoot, page.ViewPath);
            if (!File.Exists(fullPath))
            {
                continue;
            }

            var raw = File.ReadAllText(fullPath);
            var content = CleanViewContent(raw);
            var summary = TruncateText(content, 200);
            var keywords = ExtractKeywords(content);

            results.Add(new
            {
                id = $"page_{page.Key}",
                type = "page",
                title = page.Title,
                content,
                summary,
                keywords,
                url = GetPageUrl(page.Key, language),
                language,
                category = GetPageCategory(page.Key, language)
            });
        }
        return results;
    }

    private static List<PageDefinition> GetPageDefinitions(string language)
    {
        return new List<PageDefinition>
        {
            new("home", language == "en" ? "Home" : "AnaSayfa", Path.Combine(language, "Home", "Index.cshtml")),
            new("contact", language == "en" ? "Contact" : "İletişim", Path.Combine(language, "Contact", "Index.cshtml")),
            new("exhibition-identification", language == "en" ? "Exhibition Identification" : "Fuar Künyesi", Path.Combine(language, "AboutUs", "exhibitionidentification.cshtml")),
            new("exhibition-area", language == "en" ? "Exhibition Area" : "Fuar Alanı", Path.Combine(language, "AboutUs", "ExhibitionArea.cshtml")),
            new("how-to-reach", language == "en" ? "How to Reach" : "Ulaşım", Path.Combine(language, "AboutUs", "HowToReach.cshtml")),
            new("visitor-profile", language == "en" ? "Visitor Profile" : "Ziyaretçi Profili", Path.Combine(language, "AboutUs", "VisitorProfile.cshtml")),
            new("exhibitor-profile", language == "en" ? "Exhibitor Profile" : "Katılımcı Profili", Path.Combine(language, "AboutUs", "ExhibitorProfile.cshtml")),
            new("why-attend", language == "en" ? "Why Attend" : "Neden Katılmalısınız", Path.Combine(language, "AboutUs", "WhyAttend.cshtml")),
            new("b2b-meetings", language == "en" ? "B2B Meetings" : "B2B Toplantıları", Path.Combine(language, "AboutUs", "B2BMeeting.cshtml")),
            new("experiences", language == "en" ? "Experiences" : "Deneyimler", Path.Combine(language, "AboutUs", "Experiences.cshtml")),
            new("exhibitor-list", language == "en" ? "Exhibitor List" : "Katılımcı Listesi", Path.Combine(language, "AboutUs", "ExhibitorList.cshtml")),
            new("media-sponsorship", language == "en" ? "Media & Sponsorship" : "Medya ve Sponsorluk", Path.Combine(language, "AboutUs", "MediaSponsorship.cshtml")),
            new("frequently-asked-questions", language == "en" ? "FAQ" : "Sık Sorulan Sorular", Path.Combine(language, "AboutUs", "FrequentlyAskedQuestions.cshtml"))
        };
    }

    private static string GetPageUrl(string pageKey, string language)
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

    private static string GetPageCategory(string pageKey, string language)
    {
        if (pageKey == "home") return language == "en" ? "home" : "Anasayfa";
        if (pageKey == "contact") return language == "en" ? "Contact" : "İletişim";
        return language == "en" ? "About Us" : "Hakkımızda";
    }

    private static string CleanViewContent(string rawContent)
    {
        if (string.IsNullOrEmpty(rawContent)) return string.Empty;
        var content = rawContent;

        content = Regex.Replace(content, @"@\{[^{}]*(?:\{[^{}]*\}[^{}]*)*\}", " ", RegexOptions.Singleline);
        content = Regex.Replace(content, @"@\{.*?\}", " ", RegexOptions.Singleline);
        content = Regex.Replace(content, @"ViewData\[[^\]]*\]\s*=\s*[^;]*;", " ");
        content = Regex.Replace(content, @"ViewBag\.[^\s]*\s*=\s*[^;]*;", " ");
        content = Regex.Replace(content, @"@(model|section|if|foreach|for|while|using|inject|layout)[^\r\n]*", " ");
        content = Regex.Replace(content, @"@[A-Za-z][A-Za-z0-9_.]*(\([^)]*\))?", " ");
        content = Regex.Replace(content, @"[{}];?", " ");
        content = Regex.Replace(content, @"^\s*[;}]+\s*", " ", RegexOptions.Multiline);
        content = Regex.Replace(content, @"<script[^>]*>.*?</script>", " ", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        content = Regex.Replace(content, @"<style[^>]*>.*?</style>", " ", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        content = Regex.Replace(content, @"<[^>]+>", " ");
        content = System.Net.WebUtility.HtmlDecode(content);
        content = Regex.Replace(content, @"[;{}]+", " ");
        content = Regex.Replace(content, @"[.]{3,}", "...");
        content = Regex.Replace(content, @"\s+", " ");
        content = Regex.Replace(content, @"^\s+|\s+$", "");
        content = Regex.Replace(content, @"^[;{}\s]+", "");
        return content.Trim();
    }

    private static string TruncateText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text) || text.Length <= maxLength) return text ?? string.Empty;
        var truncated = text.Substring(0, maxLength);
        var lastSpace = truncated.LastIndexOf(' ');
        if (lastSpace > 0) truncated = truncated.Substring(0, lastSpace);
        return truncated + "...";
    }

    private static string[] ExtractKeywords(string content)
    {
        if (string.IsNullOrEmpty(content)) return Array.Empty<string>();
        var words = Regex.Matches(content, @"\b[a-zA-ZçğıöşüÇĞIİÖŞÜ]{4,}\b")
            .Cast<Match>()
            .Select(m => m.Value.ToLowerInvariant())
            .Where(w => !IsStopWord(w))
            .Distinct()
            .Take(15)
            .ToArray();
        return words;
    }

    private static bool IsStopWord(string word)
    {
        var stopWords = new HashSet<string>
        {
            "için", "olan", "olarak", "daha", "sonra", "kadar", "ancak", "bütün", "şayet",
            "ayrıca", "böyle", "hangi", "kendi", "neden", "burada", "şimdi", "sadece",
            "that", "with", "have", "this", "will", "you", "from", "they", "know",
            "want", "been", "good", "much", "some", "time", "very", "when", "come",
            "here", "just", "like", "long", "make", "many", "over", "such", "take"
        };
        return stopWords.Contains(word);
    }

    private record PageDefinition(string Key, string Title, string ViewPath);
}


