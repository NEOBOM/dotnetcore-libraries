using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Library.Framework.Helpers.Extensions
{
    public static class StringExtensionHelper
    {
        public static string Without(this string source, string remove)
        {
            return source?.Replace(remove, string.Empty);
        }

        public static string Beetween(this string content, string startsWith, string endsWith)
        {
            var cropStart = content.IndexOf(startsWith, StringComparison.Ordinal);
            var cropped = content.Substring(cropStart);
            var cropEnd = cropped.IndexOf(endsWith, StringComparison.Ordinal);
            var cleaned = cropped.Substring(0, cropEnd).Substring(startsWith.Length);
            return cleaned;
        }

        public static string LastContent(this string content, string startsWith, string endsWith)
        {
            var cropStart = content.LastIndexOf(startsWith, StringComparison.Ordinal);
            var cropped = content.Substring(cropStart);
            var cropEnd = cropped.IndexOf(endsWith, StringComparison.Ordinal);
            var cleaned = cropped.Substring(0, cropEnd).Substring(startsWith.Length);
            return cleaned;
        }

        public static string EndWithContent(this string content, string endsWith)
        {
            if (!content.Contains(endsWith))
                return content;

            var cropEnd = content.IndexOf(endsWith, StringComparison.Ordinal);
            var cleaned = content.Substring(0, cropEnd);
            return cleaned;
        }

        public static string KeepOnlyNumbers(this string content)
        {
            return Regex.Replace(content, @"[^\d]", "", RegexOptions.Compiled);
        }

        public static string KeepFirstNumbers(this string content)
        {
            var matches = Regex.Matches(content, @"[\d]{1,}\,?\.?[\d]{0,}", RegexOptions.Compiled);
            return matches.FirstOrDefault().Value;
        }

        public static string KeepLastNumbers(this string content)
        {
            var matches = Regex.Matches(content, @"[\d]{1,}\,?\.?[\d]{0,}", RegexOptions.Compiled);
            return matches.LastOrDefault().Value;
        }
    }
}
