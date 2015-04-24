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
    public class RegisteController  : ApiController
    {
        public readonly ICommonService _commonService;
        public class RegisterInfo
        {
            public String Name { get; set; }
            public String Avatar { get; set; }
            public String Mobile { get; set; }
            public String Code { get; set; }
        }
        
        //
        // GET: /Message/

        public RegisteController(ICommonService commonService)
        {
            _commonService = commonService;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>-1 数据格式不符 0社交账号存在 1成功 2验证不通过 3手机已被验证</returns>
        public int Post(RegisterInfo item)
        {
            String etag = (WebContext.Current.Etag == null) ? "" : WebContext.Current.Etag;
            String socialAccount = WebContext.Current.SocialAccount;
            Byte socialType = WebContext.Current.SocialType;

            int memberId = 0;
            var result = 0;

            var isMobileValid = System.Text.RegularExpressions.Regex.IsMatch(item.Mobile, Constants.ValidationExpressions.Mobile);
            var isCodeValid = System.Text.RegularExpressions.Regex.IsMatch(item.Code, Constants.ValidationExpressions.ValidationCode);

            if (isMobileValid && isCodeValid && socialType > 0 && socialType < 3 && !String.IsNullOrEmpty(socialAccount) && !String.IsNullOrEmpty(item.Name))
            {
                result = _commonService.CreateMember(out memberId, socialAccount, socialType, item.Name, "", item.Avatar, item.Mobile, item.Code, etag);
                if (memberId > 0)
                {
                    WebContext.Current.MemberId = memberId;
                    WebContext.Current.SocialAccount = socialAccount;
                    String message = ResourceHelper.GetTransText(284);
                    _commonService.AddMessage(Constants.BizConfig.AdminMemberId, memberId, message);
                }
            }
            else
            {
                result = -1;
            }
            return result;

        }




    }
}
