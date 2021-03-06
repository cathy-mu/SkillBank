﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;

namespace SkillBankWeb.API
{
    public class ClassListsController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;
        public class GetClassListPara
        {
            public Byte LoadBy;
            public Byte CategoryId;
            public String SearchKey;
            public Decimal PosX;
            public Decimal PosY;
            public int MemberId;
            public int PageSize;
            public int PageId;
        }
       
        public ClassListsController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        /// <summary>
        /// Moblie site get class list for tabs
        /// </summary>
        /// <param name="by"></param>
        /// <param name="type"></param>
        /// <param name="mid"></param>
        /// <param name="key"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="city">City Name Keywords</param>
        /// <returns></returns>
        public List<ClassListItem> GetClassList(Byte by, Byte type, int mid = 0, String key = "", Double x = 0, Double y = 0, String city = "")
        {
            mid = WebContext.Current.MemberId;
            int coverw = ConfigConstants.ThirdPartySetting.UpYun.ClassCoverSize["h"][0];
            int avatarw = 150;
            key = String.IsNullOrEmpty(key) ? "" : key;
            var cityDic = _contentService.GetCities("cn");
            int cityId = 0;
            //If can get user position for query near by classes
            if (by.Equals((Byte)Enums.DBAccess.ClassTabListLoadType.NearBy) && !String.IsNullOrEmpty(city))
            {
                city = city.Replace("市", "");
                var cityItems = cityDic.Where(c => c.Value.CityName.StartsWith(city));
                if (cityItems.Count() > 0)
                {
                    cityId = cityItems.First().Key;
                }
            }

            var result = _commonService.GetClassTabList(by, type, mid, key, (Decimal)x, (Decimal)y, cityId);
            if (result != null)
            {
                List<ClassListItem> classList = result.Select(c => new ClassListItem()
                {
                    CityId = c.CityId,
                    ClassNum = c.ClassNum,
                    Cover = String.Format("{0}{1}!{2}", ConfigConstants.ThirdPartySetting.UpYun.SpaceHost, c.Cover, coverw),
                    Avatar = (c.Avatar.Contains("profile") ? String.Format("{0}{1}!{2}", ConfigConstants.ThirdPartySetting.UpYun.SpaceHost, c.Avatar, avatarw) : c.Avatar),
                    Level = c.Level,
                    Member_Id = c.Member_Id,
                    IsLike = c.IsLike,
                    Name = c.Name,
                    PosX = c.PosX,
                    PosY = c.PosY,
                    ReviewNum = c.ReviewNum,
                    ClassId = c.ClassId,
                    LikeNum = c.LikeNum,
                    Title = c.Title,
                    CityName = (cityDic.ContainsKey(c.CityId) ? cityDic[c.CityId].CityName : "")
                }).ToList();
                return classList;
            }
            else
            {
                return null;
            }
           
        }


    }
}
