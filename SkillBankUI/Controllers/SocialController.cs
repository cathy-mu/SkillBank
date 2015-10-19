using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SkillBank.Site.Common;
using SkillBank.Site.Services;
using SkillBank.Site.Web.Context;
using SkillBank.Site.DataSource.Data;

namespace SkillBankWeb.Controllers
{
    public class SocialController : Controller
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public SocialController(IContentService contentService, ICommonService commonService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        // GET: /Social/

        public ActionResult QQ()
        {
            return View();
        }

        public ActionResult QQShare()
        {
            return View();
        }
        
        public ActionResult RR()
        {
            return View();
        }

        public ActionResult Sina()
        {
            return View();
        }
        public ActionResult r2()
        {
            return View();
        }

        public ActionResult s2()
        {
            return View();
        }

        public ActionResult s3()
        {
            return View();
        }

        public ActionResult q3()
        {
            return View();
        }


        public ActionResult q2()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult SocialLogin(String accout, Byte socialType, String openId)
        {
            Boolean result = false;
            WebContext.Current.SocialAccount = accout;
            WebContext.Current.SocialType = socialType;
            //WebContext.Current.SocialAccount = "cathy.test@aaa.com";
            //WebContext.Current.SocialType = Enums.SocialTpye.QQ;
            //TO DO: Check save social account or openid later

            //set member type
            var memberInfo = this._commonService.GetMemberInfo(accout, socialType);
            //Let exists member login 
            if (memberInfo != null)
            {
                WebContext.Current.MemberId = memberInfo.MemberId;
                result = true;
            }
            else// not regist yet
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
