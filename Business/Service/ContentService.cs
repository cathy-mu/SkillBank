using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;
using SkillBank.Site.Services.CacheProviders;
using SkillBank.Site.Services.Managers;
using SkillBank.Site.Services.Models;

namespace SkillBank.Site.Services
{
    public interface IContentService
    {
        String GetTranslation(int BlurbId);//, Boolean showBlurbId = false
        String GetTranslation(int BlurbId, String LanguageCode, Byte siteVersion);

        ClassCategory GetCategory(int categoryId);
        List<ClassCategory> GetCategories(int cityId);
        Dictionary<int, CategoryLkpItem> GetAllCategories();

        Dictionary<int, CityInfo> GetCities(String localeCode);
        Dictionary<int, CityInfo> GetClassCities(String localeCode);
        String GetCityNameById(String localeCode, int cityId);
    }

    public class ContentService : IContentService
    {
        private readonly IContentManager _contentMgr;
        private readonly IBlurbsProvider _blurbProvider;
        private readonly ICityLkpProvider _cityLkpProvider;
        private readonly ICategoryProvider _categoryProvider;
        private readonly IMetaTagProvider _metaTagProvider;
        private readonly ICategoryLkpProvider _categoryLkpProvider;

        #region Constructors

        public ContentService(IContentManager contentMgr, IBlurbsProvider blurbProvider, ICityLkpProvider cityLkpProvider, IMetaTagProvider metaTagProvider, ICategoryLkpProvider categoryLkpProvider, ICategoryProvider categoryProvider)
        {
            this._contentMgr = contentMgr;
            this._blurbProvider = blurbProvider;
            this._cityLkpProvider = cityLkpProvider;
            this._metaTagProvider = metaTagProvider;
            this._categoryLkpProvider = categoryLkpProvider;
            this._categoryProvider = categoryProvider;
        }

        #endregion

        //Language layer
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BlurbId"></param>
        /// <param name="LanguageCode"></param>
        /// <param name="siteVersion"></param>
        /// <returns></returns>
        public String GetTranslation(int BlurbId, String LanguageCode, Byte siteVersion)
        {
            return _blurbProvider.GetBlurb(BlurbId, LanguageCode, siteVersion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BlurbId"></param>
        /// <returns></returns>
        public String GetTranslation(int BlurbId)
        {
            String languageCode = "cs";
            Byte siteVersion = 1;

            //Should show blurb id on page
            Boolean showBlurbId = false;
            var cookie = HttpContext.Current.Request.Cookies[Constants.CookieName.ShowLL];
            if (cookie != null && cookie.Value == "1")
            {
                showBlurbId = true;
            }

            var blurbText = _blurbProvider.GetBlurb(BlurbId, languageCode, siteVersion);
            return showBlurbId ? blurbText + "[" + BlurbId + "]" : blurbText;
        }

        //Meta tag
        /// <summary>
        /// 
        /// </summary>
        /// <param name="metaKey"></param>
        /// <returns></returns>
        public MetaTag GetPageMetaTags(String metaKey)
        {
            return _metaTagProvider.GetMetaTag(metaKey);
        }


        #region Lookp ups

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, CategoryLkpItem> GetAllCategories()
        {
            var result = _categoryLkpProvider.GetCategoryLkp();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localeCode"></param>
        /// <returns></returns>
        public Dictionary<int, CityInfo> GetCities(String localeCode)
        {
            var result = _cityLkpProvider.GetCityLkp(localeCode);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localeCode"></param>
        /// <returns></returns>
        public Dictionary<int, CityInfo> GetClassCities(String localeCode)
        {
            var result = _cityLkpProvider.GetClassCityLkp(localeCode);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localeCode"></param>
        /// <returns></returns>
        public String GetCityNameById(String localeCode,int cityId)
        {
            var result = _cityLkpProvider.GetCityLkp(localeCode);
            if (result.ContainsKey(cityId))
            {
                return result[cityId].CityName;
            }
            else
            {
                return "";
            }

        }

        public List<ClassCategory> GetCategories(int cityId)
        {
            var result = _categoryProvider.GetCategoryLkp(cityId);
            return result;
        }

        public ClassCategory GetCategory(int categoryId)
        {
            var result = _categoryProvider.GetCategory(categoryId);
            return result;
        }
        
        #endregion

    }
}
