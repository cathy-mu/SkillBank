using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Services.Utility;
using SkillBank.Site.Web.ViewModel;

namespace SkillBankWeb.API
{
    public class HomeController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public class HomeModel
        {
            public List<TopBanner> TopBanner;
            public List<RecommendListItem> LatestClass;
            public List<RecommendListItem> MinorityClass;
            public Dictionary<String, int> Badge;
        }

        public HomeController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HomeModel Get(int id = 0)
        {
            MemberInfo memberInfo = id > 0 ? _commonService.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.ByMemberExtraInfo, id, 0) : null;
            int coverw = ConfigConstants.ThirdPartySetting.UpYun.ClassCoverSize["h"][0];
            int avatarw = 150;
            int classNum = 0;
            int pageSize = 15;
            Byte latestType = 2;
            Byte oddType = 3;
            
            String likes = (memberInfo == null) ? "" : memberInfo.ExtraInfo;
            Boolean hasLike = !String.IsNullOrEmpty(likes);
            String[] likeList = hasLike ? DataTagHelper.GetLikeList(likes) : null;
            var cityDic = _contentService.GetClassCities("cn");

            var latests = _commonService.GetCachedRecommendationClassList(latestType, 1, pageSize, 0, out classNum);
            var latestClass = latests.Select(c => new RecommendListItem()
            {
                Cover = String.Format("{0}{1}!{2}", ConfigConstants.ThirdPartySetting.UpYun.SpaceHost, c.Cover, coverw),
                Avatar = (c.Avatar.Contains("profile") ? String.Format("{0}{1}!{2}", ConfigConstants.ThirdPartySetting.UpYun.SpaceHost, c.Avatar, avatarw) : c.Avatar),
                MemberId = c.Member_Id,
                IsLike = (hasLike ? DataTagHelper.GetIsLike(likeList, c.ClassId) : false),
                Name = c.Name,
                ClassId = c.ClassId,
                LikeNum = c.LikeNum,
                Title = c.Title,
                CityName = cityDic.ContainsKey(c.CityId) ? cityDic[c.CityId].CityName : ""
            }).ToList();

            var minority = _commonService.GetCachedRecommendationClassList(oddType, 1, pageSize, 0, out classNum);
            var minorityClass = minority.Select(c => new RecommendListItem()
            {
                Cover = String.Format("{0}{1}!{2}", ConfigConstants.ThirdPartySetting.UpYun.SpaceHost, c.Cover, coverw),
                Avatar = (c.Avatar.Contains("profile") ? String.Format("{0}{1}!{2}", ConfigConstants.ThirdPartySetting.UpYun.SpaceHost, c.Avatar, avatarw) : c.Avatar),
                MemberId = c.Member_Id,
                IsLike = (hasLike ? DataTagHelper.GetIsLike(likeList, c.ClassId) : false),
                Name = c.Name,
                ClassId = c.ClassId,
                LikeNum = c.LikeNum,
                Title = c.Title,
                CityName = cityDic.ContainsKey(c.CityId) ? cityDic[c.CityId].CityName : ""
            }).ToList();

            Dictionary<String, int> badge = null;
            var alertList = _commonService.GetPopNotification(id, (Byte)Enums.DBAccess.NotificationAlterLoadType.MobileMenu);
            badge = APIHelper.GetMenuNotificationNums(alertList);

            HomeModel model = new HomeModel();
            model.TopBanner = _contentService.GetTopBannerLkp(); 
            model.LatestClass = latestClass;
            model.MinorityClass = minorityClass;
            model.Badge = badge;

            return model;
        }

    }
}
