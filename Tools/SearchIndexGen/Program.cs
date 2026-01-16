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

            // About Us section
            new("about-us", language == "en" ? "About Us" : "Hakkımızda", Path.Combine(language, "AboutUs", "Index.cshtml")),
            new("exhibition-identification", language == "en" ? "Exhibition Identification" : "Fuar Künyesi", Path.Combine(language, "AboutUs", "exhibitionidentification.cshtml")),
            new("exhibition-area", language == "en" ? "Exhibition Area" : "Fuar Alanı", Path.Combine(language, "AboutUs", "ExhibitionArea.cshtml")),
            new("how-to-reach", language == "en" ? "How to Reach" : "Ulaşım", Path.Combine(language, "AboutUs", "HowToReach.cshtml")),
            new("show-report", language == "en" ? "Show Report" : "Fuar Sonuç Raporu", Path.Combine(language, "AboutUs", "ShowReport.cshtml")),
            new("visitor-profile", language == "en" ? "Visitor Profile" : "Ziyaretçi Profili", Path.Combine(language, "AboutUs", "VisitorProfile.cshtml")),
            new("exhibitor-profile", language == "en" ? "Exhibitor Profile" : "Katılımcı Profili", Path.Combine(language, "AboutUs", "ExhibitorProfile.cshtml")),
            new("why-visit", language == "en" ? "Why Visit" : "Neden Ziyaret Etmelisiniz", Path.Combine(language, "AboutUs", "WhyVisiting.cshtml")),
            new("b2b-meetings", language == "en" ? "B2B Meetings" : "B2B Toplantıları", Path.Combine(language, "AboutUs", "B2BMeeting.cshtml")),
            new("insights", language == "en" ? "Insights" : "Insights", Path.Combine(language, "AboutUs", "Insights.cshtml")),
            new("experiences", language == "en" ? "LES Experiences" : "LES Experiences", Path.Combine(language, "AboutUs", "Experiences.cshtml")),
            new("media-logo", language == "en" ? "Media Logo" : "Medya Logo", Path.Combine(language, "AboutUs", "MediaLogo.cshtml")),
            new("exhibitor-list", language == "en" ? "Exhibitor List" : "Katılımcı Listesi", Path.Combine(language, "AboutUs", "ExhibitorList.cshtml")),
            new("why-exhibit", language == "en" ? "Why Exhibit" : "Neden Katılmalısınız", Path.Combine(language, "AboutUs", "WhyExhibit.cshtml")),
            new("faq", language == "en" ? "FAQ" : "Sık Sorulan Sorular", Path.Combine(language, "AboutUs", "Faq.cshtml")),

            // Other public pages
            new("online-ticket", language == "en" ? "Online Ticket" : "Online Bilet", Path.Combine(language, "Ticket", "Index.cshtml")),
            new("pre-registration-form", language == "en" ? "Pre-Registration Form" : "Ön Kayıt Formu", Path.Combine(language, "Registration", "OnKayitFormu.cshtml")),
            new("all-news", language == "en" ? "All News" : "Tüm Haberler", Path.Combine(language, "Blogs", "Index.cshtml"))
        };
    }

    private static string GetPageUrl(string pageKey, string language)
    {
        var urlMap = new Dictionary<string, string>
        {
            { "home", $"/{language}/" + (language == "en" ? "home" : "anasayfa") },
            { "contact", $"/{language}/" + (language == "en" ? "contact" : "iletisim") },

            { "about-us", $"/{language}/" + (language == "en" ? "about-us" : "hakkimizda") },
            { "exhibition-identification", $"/{language}/" + (language == "en" ? "exhibition-identification" : "fuar-kunyesi") },
            { "exhibition-area", $"/{language}/" + (language == "en" ? "exhibition-area" : "fuar-alani") },
            { "how-to-reach", $"/{language}/" + (language == "en" ? "how-to-reach" : "ulasim") },
            { "show-report", $"/{language}/" + (language == "en" ? "show-report" : "fuar-sonuc-raporu") },
            { "visitor-profile", $"/{language}/" + (language == "en" ? "visitor-profile" : "ziyaretci-profili") },
            { "exhibitor-profile", $"/{language}/" + (language == "en" ? "exhibitor-profile" : "katilimci-profili") },
            { "why-visit", $"/{language}/" + (language == "en" ? "why-visit" : "neden-ziyaret-etmelisiniz") },
            { "b2b-meetings", $"/{language}/" + (language == "en" ? "b2b-meetings" : "b2b-toplantilari") },
            { "insights", $"/{language}/insights" },
            { "experiences", $"/{language}/les-experiences" },
            { "media-logo", $"/{language}/" + (language == "en" ? "media-logo" : "medya-logo") },
            { "exhibitor-list", $"/{language}/" + (language == "en" ? "exhibitor-list" : "katilimci-listesi") },
            { "why-exhibit", $"/{language}/" + (language == "en" ? "why-exhibit" : "neden-katilmalisiniz") },
            { "faq", $"/{language}/" + (language == "en" ? "faq" : "sikca-sorulan-sorular") },

            { "online-ticket", $"/{language}/" + (language == "en" ? "online-ticket" : "online-bilet") },
            { "pre-registration-form", $"/{language}/" + (language == "en" ? "pre-registration-form" : "on-kayit-formu") },
            { "all-news", $"/{language}/" + (language == "en" ? "all-news" : "tum-haberler") }
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


