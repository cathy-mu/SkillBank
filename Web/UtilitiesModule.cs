using System;
using System.Web;

namespace SkillBank.Site.Web
{
    public class UtilitiesModule
    {
        public static string GetCookieDomain()
        {
            HttpContext context = HttpContext.Current;
            string domain;
            if (context == null)
            {
                domain = null;
            }
            else
            {
                domain = UtilitiesModule.GetCookieDomain(context.Request);
            }
            return domain;
        }
        
        /// <summary>
        /// Returns the domain to be used for cookies.  If the domain ends with ".englishtown.com" then "englishtown.com" is returned.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetCookieDomain(HttpRequest request)
        {
            return UtilitiesModule.GetCookieDomain(request.Url.Host);
        }

        /// <summary>
        /// Returns the domain to be used for cookies.  If the domain ends with ".englishtown.com" then "englishtown.com" is returned.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static string GetCookieDomain(string host)
        {
            string domain;
            if (string.IsNullOrEmpty(host))
            {
                domain = "skill-bank.com";
            }
            else
            {
                domain = host.ToLowerInvariant();
                if (domain.EndsWith(".skill-bank.com", System.StringComparison.OrdinalIgnoreCase))
                {
                    domain = "skill-bank.com";
                }
                else
                {
                    if (domain.ToLowerInvariant() == "localhost")
                    {
                        //domain = null;
                        domain = "localhost";
                    }
                    else
                    {
                        domain = null;
                    }
                }
            }
            return domain;
        }

    }
}
