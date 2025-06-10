using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace LesExpo.Utility
{
    /// <summary>
    /// Provides functionality for generating URL-friendly slugs
    /// </summary>
    public static class SlugUtility
    {
        /// <summary>
        /// Converts a string to a URL-friendly slug
        /// Handles Turkish characters properly by converting them to their Latin equivalents
        /// </summary>
        /// <param name="text">The text to convert to a slug</param>
        /// <returns>The generated slug</returns>
        public static string GenerateSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            // Normalize the string to decompose Unicode characters
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            
            // Define a regular expression pattern for Turkish characters
            var turkishMap = new Dictionary<string, string>
            {
                { "ı", "i" }, { "ğ", "g" }, { "ü", "u" }, { "ş", "s" }, { "ö", "o" }, { "ç", "c" },
                { "İ", "I" }, { "Ğ", "G" }, { "Ü", "U" }, { "Ş", "S" }, { "Ö", "O" }, { "Ç", "C" }
            };

            // Replace Turkish characters
            foreach (var pair in turkishMap)
            {
                normalizedString = normalizedString.Replace(pair.Key, pair.Value);
            }

            // Convert to lowercase and replace diacritics
            var slug = new StringBuilder();
            foreach (char c in normalizedString)
            {
                // Check if the character is a letter or digit
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    slug.Append(c);
                }
            }

            // Replace spaces with hyphens and remove invalid characters
            string result = slug.ToString()
                .ToLowerInvariant()
                .Replace(" ", "-")       // Replace spaces with hyphens
                .Replace("_", "-");      // Replace underscores with hyphens

            // Remove all other invalid characters
            result = Regex.Replace(result, @"[^a-z0-9\-]", "");
            
            // Remove duplicate hyphens
            result = Regex.Replace(result, @"-{2,}", "-");
            
            // Remove leading and trailing hyphens
            result = result.Trim('-');

            return result;
        }
    }
} 