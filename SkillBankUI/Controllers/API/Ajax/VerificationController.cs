using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;

namespace SkillBankWeb.API
{
    //Will Be removed form the solution later
    public class VerificationController : ApiController//Validation
    {
        public readonly ICommonService _commonService;

        //TO DO :update  js *****************************************
        public class ValidationItem
        {
            public Byte Type { get; set; } //1 Socail register  2 Socail user Valid  5 mobile registe check  6not register member get password
            public String Mobile { get; set; }
            public String Code { get; set; }
            public int? MemberId { get; set; }
        }


        //
        // GET: /Message/
        public VerificationController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        public string Options()
        {
            return null; // HTTP 200 response with empty body 
        }

        /// <summary>
        /// Send code and verify mobile
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code">Code为空:发送验证码 Code不空:验证</param>
        /// <returns>0号码已占用/验证 1验证码发送/验证成功 2手机/验证码 格式不对    3sina   4mobile  6wechat</returns>
        [HttpPost]
        public Byte UpdateVerification(ValidationItem verifyItem)
        {
            int memberId = (verifyItem.MemberId.HasValue && verifyItem.MemberId.Value > 0) ? verifyItem.MemberId.Value : 0;
            String mobile = verifyItem.Mobile.Trim();
            var isMobileValid = System.Text.RegularExpressions.Regex.IsMatch(verifyItem.Mobile, Constants.ValidationExpressions.Mobile);
            if (isMobileValid)
            {
                //send code
                if (String.IsNullOrEmpty(verifyItem.Code))
                {
                    //not login user reset pass, new user for mobile register
                    Byte type = verifyItem.Type.Equals(Enums.DBAccess.MobileVerificationSaveType.GetVerifyCode4Visit) ? verifyItem.Type : (Byte)Enums.DBAccess.MobileVerificationSaveType.GetVerifyCode;
                    //var result = _commonService.SendMobileVerifyCode((Byte)Enums.DBAccess.MobileVerificationSaveType.GetVerifyCode, 0, mobile, System.Configuration.ConfigurationManager.AppSettings["ENV"].Equals(ConfigConstants.EnvSetting.LiveEnvName));
                    var result = _commonService.SendMobileVerifyCode((Byte)Enums.DBAccess.MobileVerificationSaveType.GetVerifyCode, 0, mobile,true);
                    return result;
                }
                //verify mobile
                else
                {
                    memberId = memberId > 0 ? memberId : GetMemberId(false);
                    var result = _commonService.VerifyMobile(memberId, mobile, verifyItem.Code);
                    return result;
                }
            }
            else
            {
                return 2;
            }
        }

        private int GetMemberId(Boolean shouldAuthorize)
        {

            int memberId = WebContext.Current.MemberId;
            if (shouldAuthorize && memberId == 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.StatusCode = 401;
                HttpContext.Current.Response.End();
            }
            return memberId;
        }


    }
}
