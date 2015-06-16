using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SkillBank.Site.Services;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Web;
using SkillBank.Site.Web.ViewModel;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Common;
using SkillBank.Site.Web.CDN;

namespace SkillBankWeb.Controllers
{
    public class MemberHelperController : Controller
    {
        //
        // GET: /ClassHelper/

        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public MemberHelperController(IContentService contentService, ICommonService commonService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        [HttpPost]
        public JsonResult CreateMember(String account, Byte socialType, String memberName, String email, String avatar = "", string mobile = "", string code = "", Boolean gender = true)
        {
            int memberId;
            
            String etag = (WebContext.Current.Etag == null)? "" : WebContext.Current.Etag;
            var result = _commonService.CreateMember(out memberId, account, socialType, memberName, email, avatar, mobile, code, etag, gender);
            if (memberId > 0)
            {
                WebContext.Current.MemberId = memberId;
                WebContext.Current.SocialAccount = account;
                String message = ResourceHelper.GetTransText(284);
                _commonService.AddMessage(Constants.BizConfig.AdminMemberId, memberId, message);
            }
            var jsonObj = new JsonResult();
            jsonObj.Data = new { r = result, i = memberId };
 
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateMemberInfo(Byte saveType, String saveValue, String saveValue2 = "")
        {
            int memberId = WebContext.Current.MemberId;
            var isSaved = _commonService.UpdateMemberInfo(memberId, saveType, saveValue, saveValue2);
            var jsonObj = new JsonResult();
            jsonObj.Data = new object[] { new { t = saveType, r = isSaved ? 1 : 0 } };

            //Create class
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// For member profile page to update member info
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isMale"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="cityId"></param>
        /// <param name="intro"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateMemberProfile(String name, Boolean isMale, int year, int month, int day, String email, String phone, int cityId, String intro)
        {
            //Add cityId Later
            int memberId = WebContext.Current.MemberId;
            MemberInfo memberInfo = new MemberInfo();
            memberInfo.MemberId = memberId;
            memberInfo.Name = name;
            memberInfo.Gender = isMale;
            memberInfo.BirthDate = (year == 0 || month == 0 || day == 0) ? new DateTime(1900,01,01) : new DateTime(year, month, day);
            memberInfo.Phone = phone;
            memberInfo.Email = email;
            memberInfo.SelfIntro = intro;
            memberInfo.CityId = cityId;
            
            var result = _commonService.UpdateMemberProfile(memberInfo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveEmailAccount(String name,String email)
        {
            _commonService.SaveEmailAccount(name, email);
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMemberInfoBySocialAccount(String socialId, Byte socialType)
        {
            var memberInfo = _commonService.GetMemberInfo(socialId, socialType);
            int memberId = (memberInfo==null?0:memberInfo.MemberId);
            return Json(memberId, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendMobileVerifyCode(String mobile)
        {
            int memberId = WebContext.Current.MemberId;
            var result = _commonService.SendMobileVerifyCode(memberId, mobile, System.Configuration.ConfigurationManager.AppSettings["ENV"].Equals(ConfigConstants.EnvSetting.LiveEnvName));
            
            var jsonObj = new JsonResult();
            jsonObj.Data = new { r = result };
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult VerifyMobile(String mobile, String code)
        {
            int memberId = WebContext.Current.MemberId;
            var result = _commonService.VerifyMobile(memberId, mobile, code);
            var jsonObj = new JsonResult();
            jsonObj.Data = new { r = result };
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateMemberLikeTag(int relatedId, Boolean isLike)
        {
            int memberId = WebContext.Current.MemberId;
            _commonService.UpdateMemberLikeTag(memberId, relatedId, isLike);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
                
    }
}
