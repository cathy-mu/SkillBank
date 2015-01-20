using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SkillBank.Site.Web.Social;
using SkillBank.Site.Common;
using NetDimension.Weibo;
//using QWeiboSDK;

//using RennDotSDK;
//using RennDotSDK.Model;
//using RennDotSDK.APIUtility;

namespace SkillBankWeb.Controllers
{
    public class SocialHelperController : Controller
    {
        //
        // GET: /SocialHelper/


        [HttpPost]
        public JsonResult GetAccessToken(Byte type, String code)
        {
            //String appKkey = "111240964";
            //String appSecret = "3a9bd6965729abbb3b048a6dfd4670e2";
            //String accessToken = "";
            if (type.Equals(1))//sian
            {
                var accessToken = SinaHelper.GetAccessTokenByAuthorizationCode(code);
                return Json(accessToken, JsonRequestBehavior.AllowGet);
            }
            else if (type.Equals(3))//Tencet
            {
                OAuth oauth = new OAuth(ConfigConstants.ThirdPartySetting.SocialNetwork.QQ_APPKey, ConfigConstants.ThirdPartySetting.SocialNetwork.QQ_APPSecret);
                var accessToken = oauth.GetAccessTokenByAuthorizationCode(code);
                return Json(accessToken, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetAccessTokenQQ()
        {
            
            return Json("false", JsonRequestBehavior.AllowGet);
        }




    }
}
