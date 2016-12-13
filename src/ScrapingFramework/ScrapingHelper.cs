using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System.Text.RegularExpressions;

namespace ScrapingFramework
{
    public static class ScrapingHelper
    {
        private static HtmlParser _htmlParser = new HtmlParser();

        /// <summary>
        /// Return a manipulable DOM object.
        /// </summary>
        public static IDocument Parse(string html)
        {
            return _htmlParser.Parse(html);
        }

        /// <summary>
        /// Trim whitespace at beginning, end and middle (multiple consecutive spaces) of a string.
        /// </summary>
        public static string TrimAll(string input)
        {
            return Regex.Replace(input, @"\s+", " ").Trim();
        }
    }
}
