using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;
using SkillBank.Site.Services.CacheProviders;
using SkillBank.Site.Services.Managers;
using SkillBank.Site.Web.Context;
using System.Configuration;

namespace SkillBank.Site.Web
{
    public static class ResourceHelper
    {
        //TO DO10:Change to service later
        private static readonly IBlurbsProvider _blurbsProvider;

        static ResourceHelper()
        {
            _blurbsProvider = new BlurbsProvider(new BlurbsRepository());
        }

        /// <summary>
        /// Get Text resource
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="BlurbId"></param>
        /// <returns></returns>
        public static MvcHtmlString GetTrans(this HtmlHelper htmlHelper, int BlurbId)
        {
            //LanguageCode is get from Context, siteversion get from webconfig
            String languageCode = WebContext.Current.LanguageCode;
            Byte siteVersion = Convert.ToByte(ConfigurationManager.AppSettings["SiteVerion"]);

            //Should show blurb id on page
            Boolean showBlurbId = WebContext.Current.ShowLL.Equals("y", StringComparison.OrdinalIgnoreCase);

            var blurbText = _blurbsProvider.GetBlurb(BlurbId, languageCode, siteVersion);
            blurbText = showBlurbId ? blurbText + "[" + BlurbId + "]" : blurbText;

            return MvcHtmlString.Create(blurbText);
        }

        public static String GetTransText(int BlurbId)
        {
            //LanguageCode is get from Context, siteversion get from webconfig
            String languageCode = WebContext.Current.LanguageCode;
            Byte siteVersion = Convert.ToByte(ConfigurationManager.AppSettings["SiteVerion"]);

            //Should show blurb id on page
            Boolean showBlurbId = WebContext.Current.ShowLL.Equals("y", StringComparison.OrdinalIgnoreCase);

            var blurbText = _blurbsProvider.GetBlurb(BlurbId, languageCode, siteVersion);
            blurbText = showBlurbId ? blurbText + "[" + BlurbId + "]" : blurbText;

            return blurbText.Trim();
        }

    }
}