using System;
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

namespace SkillBankWeb.Controllers.API
{
    public class MemberController  : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public class MemberInfoItem
        {
            public String Name { get; set; }
            public Boolean Gender { get; set; }
            public String CityName { get; set; }
            public String Intro { get; set; }
            public String Avatar { get; set; }
            //web site check wechat binding for socail
            public Byte SocailType { get; set; }
            public String SocailAccount { get; set; }
            public String OpenId { get; set; }
        }

        //only for login user check own info
        public MemberController(ICommonService commonService, IContentService contentService)
        {
            _commonService = commonService;
            _contentService = contentService;
        }

        /// <summary>
        /// Mobile site update profile
        /// </summary>
        /// <param name="item"></param>
        /// <returns>1保存成功，2:数据保存失败，3 城市无效，4 名字无效，5 自我介绍为空</returns>
        public int Put(MemberInfoItem item)
        {
            int memberId = WebContext.Current.MemberId;

            if (String.IsNullOrEmpty(item.CityName))
            {
                return 3;//invalid cityname
            }
            else if (String.IsNullOrEmpty(item.Name))
            {
                return 4;//invalid name
            }
            else if (String.IsNullOrEmpty(item.Intro))
            {
                return 5;//invalid intro
            }
            else
            {
                var cityDic = _contentService.GetCities("cn");
                int cityId = LookupHelper.GetCityIdByName(cityDic, item.CityName);
                if (cityId.Equals(0))
                {
                    return 3;//invalid cityname
                }

                Byte saveType = (Byte)Enums.DBAccess.MemberSaveType.UpdateBasicInfo;
                MemberInfo memberInfo = new MemberInfo() { MemberId = memberId, Gender = item.Gender, Name = item.Name, SelfIntro = item.Intro, CityId = cityId, Avatar = item.Avatar };
                var result = _commonService.UpdateMemberProfile(memberInfo, saveType);

                return result;
            }
        }

        /// <summary>
        /// PC site check wechat binding
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MemberInfoItem Get(int id = 0)
        {
            int memberId = WebContext.Current.MemberId;
            memberId = !id.Equals(0) ? id : memberId;
            MemberInfo item = _commonService.GetMemberInfo(memberId);
            if (item != null)
            {
                var cityDic = _contentService.GetCities("cn");
                String cityName = LookupHelper.GetCityNameById(cityDic, item.CityId);
                return new MemberInfoItem { Gender = item.Gender, Name = item.Name, Intro = item.SelfIntro, CityName = cityName, SocailType = item.SocialType, SocailAccount = item.SocialAccount, OpenId = item.OpenId };
            }
            else
            {
                return null;
            }
        }




    }
}
