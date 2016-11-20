using AngleSharp.Dom;
using AngleSharp.Parser.Html;

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
    }
}
