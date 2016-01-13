using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;

namespace SkillBankWeb.Controllers.API
{
    public class LogoutController : ApiController
    {
        public readonly ICommonService _commonService;
      
        public LogoutController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        public string Options()
        {
            return null; // HTTP 200 response with empty body 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Boolean Post()
        {
            WebContext.Current.SocialAccessInfo = "";
            WebContext.Current.SocialAccount = "";
            WebContext.Current.OpenId = "";
            WebContext.Current.MemberId = 0;
            WebContext.Current.SocialType = 0;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Boolean Get()
        {
            WebContext.Current.SocialAccessInfo = "";
            WebContext.Current.SocialAccount = "";
            WebContext.Current.OpenId = "";
            WebContext.Current.MemberId = 0;
            WebContext.Current.SocialType = 0;
            return true;
        }
        
        
    }
}
