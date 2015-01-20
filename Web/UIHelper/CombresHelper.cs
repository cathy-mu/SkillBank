using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Combres.Mvc;

namespace SkillBank.Site.Web
{
    public static class CombresHelper
    {
        public static MvcHtmlString CombresCustomUrl(this HtmlHelper htmlHelper, string setName, bool isCached = true)
        {
            var cacheServer = "";// isCached == false ? "" : SchoolContextManager.Current.CacheServer.ToLower();
            var combresUrl = htmlHelper.CombresLink(setName).ToString();
            var regex = new Regex("<link.*/>");
            var urls = regex.Matches(combresUrl);
            var resultString = new StringBuilder();
            if (urls.Count > 0)
            {
                foreach (var url in urls)
                {
                    if (url.ToString().ToLower().Contains(cacheServer))
                    {
                        resultString.Append(url);
                    }
                    else
                    {
                        resultString.Append(url.ToString().Replace("href=\"", "href=\"" + cacheServer));
                    }
                    resultString.Append("\n");
                }
            }
            else
            {
                regex = new Regex("<script.*></script>");
                urls = regex.Matches(combresUrl);
                foreach (var url in urls)
                {
                    if (url.ToString().ToLower().Contains(cacheServer))
                    {
                        resultString.Append(url);
                    }
                    else
                    {
                        resultString.Append(url.ToString().Replace("src=\"", "src=\"" + cacheServer));
                    }
                    resultString.Append("\n");
                }
            }
            return MvcHtmlString.Create(resultString.ToString());
        }
    }
}