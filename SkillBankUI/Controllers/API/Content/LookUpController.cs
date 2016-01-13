using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;

namespace SkillBankWeb.API
{
    public class LookUpController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;
        
        /// <summary>
        /// Return result
        /// </summary>
        public class LookUpModel
        {
            public List<ClassCategory> Categories;
            public Dictionary<int, CityInfo> CityLkp;
            public String Version;
        }

        public LookUpController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">0 All, 1 city only 2 category only</param>
        /// <param name="city">0 all , city id </param>
        /// <param name="version">content version code</param>
        /// <returns></returns>
        [HttpGet]
        public LookUpModel GetLoopUpList(Byte type = 0, int city = 0, String version = "")
        {
            LookUpModel model = new LookUpModel();

            List<ClassCategory> categories = null;
            Dictionary<int, CityInfo> cityLkp = null;

            if (String.IsNullOrEmpty(version) || !version.Equals(Constants.ContentSiteVersion.CityCategoryLookup))
            {
                if (type.Equals(0) || type.Equals(1))
                {
                    cityLkp = _contentService.GetClassCities("cn");
                }
                if (type.Equals(0) || type.Equals(2))
                {
                    categories = _contentService.GetCategories(city);
                }
            }
            model.Categories = categories;
            model.CityLkp = cityLkp;
            model.Version = Constants.ContentSiteVersion.CityCategoryLookup;
            return model;
        }

    }
}
