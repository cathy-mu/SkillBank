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
    public class ClassListController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;
        public class GetClassListPara
        {
            public Byte Category;
            public String Key;
            public int City;//Check if should change into String
            public Decimal X;
            public Decimal Y;
            public int MemberId;
            public int Id;
            public int Size;
            public Byte Order;//UpdateDate   Comment Number   Distince   
        }
       
        public ClassListController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        public string Options()
        {
            return null; // HTTP 200 response with empty body 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cate"></param>
        /// <param name="key"></param>
        /// <param name="city"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="mid"></param>
        /// <param name="pageId"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public List<ClassLinkItem> GetClassList(Byte cate = 0, String key = "", int city = 0, Double x = 0, Double y = 0, int mid = 0, int minId = 1, int maxId = 20, Byte order = 1, Boolean reload = false)
        {
            mid = mid.Equals(0) ? WebContext.Current.MemberId : mid;
            
            var cityDic = _contentService.GetCities("cn");
            
            var result = _commonService.GetClassPagingList(cate, city, (Decimal)x, (Decimal)y, mid, minId, maxId, order);
            if (result != null)
            {
                var classList = DataMapper.Map(result,cityDic);
                return classList;
            }

            
            return null;
        }


    }
}
