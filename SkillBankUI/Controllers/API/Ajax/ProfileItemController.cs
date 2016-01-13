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

namespace SkillBankWeb.API
{
    //TO DO: inprogress
    public class ProfileItemController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public class ProfileItem
        {
            public String Intro;
            public String Name;
            public String CityName;
            public Boolean Gender;

            public Decimal PosX;
            public Decimal PosY;
        }


        public ProfileItemController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        
        /// <summary>
        /// Save member profile info
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public Byte Put(int id, ProfileItem item)
        {
            if (item.PosX > 0 && item.PosY > 0)
            {
                Byte type =  (Byte)Enums.DBAccess.MemberSaveType.UpdatePosition;
                MemberInfo memberInfo = new MemberInfo() { MemberId = id, PosX = item.PosX, PosY = item.PosY };
                var isSaved = _commonService.UpdateMemberProfile(memberInfo, type);

                return (Byte)(isSaved ? 1 : 0);//saved or not
            } 
            else  
            {
                int cityId = 0;
                if (!String.IsNullOrEmpty(item.CityName))
                {
                    var cityDic = _contentService.GetCities("cn");
                    cityId = LookupHelper.GetCityIdByName(cityDic, item.CityName);
                    if (cityId.Equals(0))
                    {
                        return 3;//invalid cityname
                    }
                }
                if (String.IsNullOrEmpty(item.CityName))
                {
                    return 4;
                }
                
                Byte type = (Byte)Enums.DBAccess.MemberSaveType.UpdateBasicInfo;
                MemberInfo memberInfo = new MemberInfo() { MemberId = id, Name = item.Name, Gender = item.Gender, CityId = cityId, SelfIntro = item.Intro};
                var isSaved = _commonService.UpdateMemberProfile(memberInfo, type);

                return (Byte)(isSaved ? 1 : 0);//saved or not
            }
            
        }

    }
}
