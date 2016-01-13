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
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;
using SkillBank.Site.Services.Utility;

namespace SkillBankWeb.API
{
    public class RecommendClassController : ApiController
    {
        public readonly ICommonService _commonService;
        public readonly IContentService _contentService;

        public RecommendClassController(ICommonService commonService, IContentService contentService)
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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public List<RecommendListItem> Get(int id = 0, Byte type = 2, int minId = 1, int maxId = 10)
        {

            MemberInfo memberInfo = id > 0 ? _commonService.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.ByMemberExtraInfo, id, 0) : null;
            int coverw = ConfigConstants.ThirdPartySetting.UpYun.ClassCoverSize["h"][0];
            int avatarw = 150;
            int classNum = 0;
            
            List<RecommendListItem> result = null;
            var classList = _commonService.GetCachedRecommendationClassList(type, minId, maxId, id, out classNum);
            if (classList != null && classList.Count > 0)
            {
                var cityDic = _contentService.GetClassCities("cn");
                String likes = (memberInfo == null) ? "" : memberInfo.ExtraInfo;
                var likeList = DataTagHelper.GetLikeList(likes);

                result = classList.Select(c => new RecommendListItem()
                    {
                        Cover = String.Format("{0}{1}!{2}", ConfigConstants.ThirdPartySetting.UpYun.SpaceHost, c.Cover, coverw),
                        Avatar = (c.Avatar.Contains("profile") ? String.Format("{0}{1}!{2}", ConfigConstants.ThirdPartySetting.UpYun.SpaceHost, c.Avatar, avatarw) : c.Avatar),
                        MemberId = c.Member_Id,
                        IsLike = DataTagHelper.GetIsLike(likeList, c.ClassId),//c.IsLike,
                        Name = c.Name,
                        ClassId = c.ClassId,
                        LikeNum = c.LikeNum,
                        Title = c.Title,
                        CityName = cityDic.ContainsKey(c.CityId) ? cityDic[c.CityId].CityName : ""
                    }).ToList();
            }

            return result;
        }
    }
}
