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

namespace SkillBankWeb.API
{
    //TO DO: inprogress
    public class DashboardController : ApiController
    {
        public readonly ICommonService _commonService;
        public readonly IContentService _contentService;

        public class MemberDashboardInfo
        {
            //public decimal PosX;
            //public decimal PosY;
            public String CityName;
            public String Avatar;
            public String Name;
            public Boolean Gender;
            public String Address;
            public String SelfIntro;
            public int Coins;
            public int CoinsLocked;
            public int Credit;
            public Byte SocialType;
            public String SocialAccount;
            public Boolean MobileValidation;
        }

        public class DashboardModel
        {
            public MemberDashboardInfo Member;
            public Dictionary<Enum, int> NumDic;
            public Boolean IsSignIn;
            public String MobileText;
            public Dictionary<String, int> Badge;
        }

        public DashboardController(ICommonService commonService, IContentService contentService)
        {
            _commonService = commonService;
            _contentService = contentService;
        }

        /// <summary>
        /// Dashboard page (public) v1.0
        /// </summary>
        /// <param name="id">current member id</param>
        /// <returns></returns>
        public DashboardModel Get(int id)
        {
            MemberInfo memberInfo = id > 0 ? _commonService.GetMemberInfo(id) : null;
            if (memberInfo != null)
            {
                var numDic = _commonService.GetNumsByMember(id, (Byte)Enums.DBAccess.MemberNumsLoadType.ByMemberDashboard);
                String cityName = (memberInfo.CityId > 0) ? _contentService.GetCityNameById(Constants.BizConfig.SingleLocalCode, memberInfo.CityId) : "";
                
                //Boolean socialSina = memberInfo.SocialType.Equals(1);
                //Boolean socialWechat = memberInfo.SocialType.Equals(4) || (memberInfo.SocialType.Equals(1) && memberInfo.SocialAccount.Equals(memberInfo.OpenId));
                
                DashboardModel model = new DashboardModel();
                model.NumDic = numDic;
                Boolean mobileValidation = (memberInfo.VerifyTag & 1).Equals(1);
                model.Member = new MemberDashboardInfo() { Name = memberInfo.Name, Avatar = memberInfo.Avatar, CityName = cityName, Credit = memberInfo.Credit, Coins = memberInfo.Coins, CoinsLocked = memberInfo.CoinsLocked, Address = memberInfo.Address, SocialType = memberInfo.SocialType, SocialAccount = memberInfo.SocialType.Equals(1) ? memberInfo.SocialAccount : "", MobileValidation = mobileValidation, SelfIntro = memberInfo.SelfIntro, Gender = memberInfo.Gender };
                model.MobileText = mobileValidation ? System.Text.RegularExpressions.Regex.Replace(memberInfo.Phone, @"(?im)(\d{3})(\d{6})(\d{2})", "$1******$3") : "";

                if (memberInfo.LastUpdateDate.ToString("yyyy-MM-dd").Equals(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    model.IsSignIn = true;
                }
                else
                {
                    model.IsSignIn = false;
                }
                
                Dictionary<String, int> badge = null;
                var alertList = _commonService.GetPopNotification(id, (Byte)Enums.DBAccess.NotificationAlterLoadType.MobileMenu);
                badge = APIHelper.GetMenuNotificationNums(alertList);
                model.Badge = badge;

                return model;
            }
            return null;
        }

    }
}
