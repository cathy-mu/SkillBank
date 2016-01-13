using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;

namespace SkillBankWeb.API
{
    public class ClassIndexController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        /// <summary>
        /// Return result
        /// </summary>
        public class ClassIndexModel
        {
            public List<ClassCategory> Categories;
            public List<ClassLinkItem> ClassList;
            public Dictionary<int, CityInfo> CityLkp;
            //public Int16 Status;
            public String Version;
            public Dictionary<String, int> Badge;
        }


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
       
        public ClassIndexController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
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
        public ClassIndexModel GetClassList(Byte cate = 0, int city = 0, Double x = 0, Double y = 0, int mid = 0, int minId =1, int maxId = 10, Byte order = 0, String version="")
        {
            ClassIndexModel model = new ClassIndexModel();
            //mid = mid.Equals(0) ? WebContext.Current.MemberId : mid;
            int coverw = ConfigConstants.ThirdPartySetting.UpYun.ClassCoverSize["h"][0];
            int avatarw = 150;
            Boolean hasLkpList = false;

            var cityDic = _contentService.GetClassCities("cn");
            
            if (String.IsNullOrEmpty(version) || !version.Equals(Constants.ContentSiteVersion.CityCategoryLookup))
            {
                hasLkpList = true;
                model.CityLkp = cityDic;
            }
                        
            var result = _commonService.GetClassPagingList(cate, city, (Decimal)x, (Decimal)y, mid, minId, maxId, order);
            if (result != null)
            {
                List<ClassLinkItem> classList = result.Select(c => new ClassLinkItem()
                {
                    CityId = c.CityId,
                    Cover = String.Format("{0}{1}!{2}", ConfigConstants.ThirdPartySetting.UpYun.SpaceHost, c.Cover, coverw),
                    Avatar = (c.Avatar.Contains("profile") ? String.Format("{0}{1}!{2}", ConfigConstants.ThirdPartySetting.UpYun.SpaceHost, c.Avatar, avatarw) : c.Avatar),
                    MemberId = c.MemberId,
                    IsLike = c.IsLike,
                    Name = c.Name,
                    PosX = c.PosX,
                    PosY = c.PosY,
                    ClassId = c.ClassId,
                    LikeNum = c.LikeNum,
                    Title = c.Title,
                    CityName = (c.CityId > 0 && cityDic.ContainsKey(c.CityId) ? cityDic[c.CityId].CityName : "")
                }).ToList();
                model.ClassList = classList;

                if (hasLkpList)
                {
                    var categories = _contentService.GetCategories(city);
                    model.Categories = categories;
                }
            }
            
            var alertList = _commonService.GetPopNotification(mid, (Byte)Enums.DBAccess.NotificationAlterLoadType.MobileMenu);
            model.Badge = APIHelper.GetMenuNotificationNums(alertList);
           
            model.Version = Constants.ContentSiteVersion.CityCategoryLookup;

            return model;
        }


    }
}
