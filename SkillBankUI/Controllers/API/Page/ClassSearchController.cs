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

namespace SkillBankWeb.API
{
    public class ClassSearchController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public class ClassSearchModel
        {
            public List<ClassCollectionItem> ClassList;
            //public Dictionary<int, CityInfo> CityLkp;
            public List<CityInfo> CityLkp;
        }

        public ClassSearchController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="city"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="pageId"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet]
        public ClassSearchModel GetClassList(int mid = 0, String key = "", int city = 0, Double x = 0, Double y = 0, int minId = 1, int maxId = 10, Byte order = 1)
        {
            ClassSearchModel model = new ClassSearchModel();
            int coverw = ConfigConstants.ThirdPartySetting.UpYun.ClassCoverSize["h"][0];

            var cityDic = _contentService.GetClassCities("cn");
            var classes = _commonService.GetClassSearchList(key, city, (Decimal)x, (Decimal)y, minId, maxId, order);
            if (classes != null && classes.Count > 0)
            {
                var classList = DataMapper.Map(classes, cityDic);
                model.ClassList = classList;
            }
            model.CityLkp = cityDic.Values.ToList<CityInfo>();

            return model;
        }


    }
}
