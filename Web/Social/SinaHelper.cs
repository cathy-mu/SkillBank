using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetDimension.Weibo;
using NetDimension.Json;

namespace SkillBank.Site.Web.Social
{
    public static class SinaHelper
    {

        private static readonly OAuth _oAuth;
        
        static SinaHelper()
        {
            String appKey = "111240964";
            String appSecret = "3a9bd6965729abbb3b048a6dfd4670e2";
            String callBackUrl = "http://www.skillbank.cn/signup";
            _oAuth = new OAuth(appKey, appSecret, callBackUrl);
            
        }

        public static void Test()
        {
            //if (String.IsNullOrEmpty(accessToken))
            //{
            //}
        }


        public static AccessToken GetAccessTokenByAuthorizationCode(String code)
        {
            return _oAuth.GetAccessTokenByAuthorizationCode(code);
        }

        public static void GetUserInfo(string userId)
        {
            var SinaClient = new Client(_oAuth);
            var userInfo = SinaClient.API.Entity.Users.Show();
        }


       

        //public JsonResult Login()
        //{
        //    Wbm.SinaV2API.oAuthSina oauth = Wbm.SinaV2API.SinaBase.oAuth();
        //    string link = oauth.GetAuthorization();
        //    return Json("true", JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult Login()
        //{
        //    String returnUrl = "http://www.skillbank.cn/social/sina";
        //    String key = "111240964";
        //    String secret = "3a9bd6965729abbb3b048a6dfd4670e2";
        //    //String accessToken = "";
        //    NetDimension.Weibo.OAuth oAuth = new OAuth(key, secret, returnUrl);

        //    return Json("true", JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetAccessToken(String code)
        //{

        //    String key = "111240964";
        //    String secret = "3a9bd6965729abbb3b048a6dfd4670e2";
        //    //String accessToken = "";
        //    NetDimension.Weibo.OAuth oauth = new OAuth(key, secret);
        //    var accessToken = oauth.GetAccessTokenByAuthorizationCode(code);
        //    return Json(accessToken, JsonRequestBehavior.AllowGet);
        //}


    }
}
