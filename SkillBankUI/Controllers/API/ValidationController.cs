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

namespace SkillBankWeb.API
{
    public class ValidationController : ApiController
    {
        //public readonly ICommonService _commonService;
        public readonly ICommonService _commonService;


        public class ValidationItem
        {
            public String Mobile { get; set; }
        }


        //
        // GET: /Message/

        public ValidationController(ICommonService commonService)
        {
            //_contentService = contentService;
            _commonService = commonService;

        }
   

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns>0号码已占用　１验证码发送  2手机格式不对</returns>
        public Byte SendMobileVerifyCode(String mobile)
        {
            var result = _commonService.SendMobileVerifyCode(0, mobile, System.Configuration.ConfigurationManager.AppSettings["ENV"].Equals(ConfigConstants.EnvSetting.LiveEnvName));
            return result;
        }

        //public String Post(ValidationItem item)
        //{
        //    //var result = _commonService.SendMobileVerifyCode(0, mobile, true);
        //    return item.Mobile;
        //}


    }
}
