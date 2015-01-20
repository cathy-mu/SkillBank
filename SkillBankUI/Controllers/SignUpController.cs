﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using dotNetDR_OAuth2;
using dotNetDR_OAuth2.AccessToken;
using dotNetDR_OAuth2.APIs;
using dotNetDR_OAuth2.Net;
using dotNetDR_OAuth2.JSON;
using System.Net;

using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Common;
using SkillBank.Site.Services;

namespace SkillBankWeb.Controllers
{
    public class SignUpController : Controller
    {
        //
        // GET: /SignUp/

        public readonly ICommonService _commonService;
        private IAuthorizationCodeBase _authCode = Uf.C(CtorAT.Tencent);
        private TencentError _err = null;
        private IApi apit = Uf.C(CtorApi.Tencent);

        private IAuthorizationCodeBase _authCodes = Uf.C(CtorAT.Sina);
        private SinaError _errs = null;
        private IApi apis = Uf.C(CtorApi.Sina);

        public SignUpController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        public ActionResult Index(string code = "", string openid = "", string openkey = "")
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("sinup");
            ViewBag.MemberInfo = null;

            Byte socialType = 0;

            //OAuth2
            if (!String.IsNullOrEmpty(code))
            {
                //Tencent
                if (!String.IsNullOrEmpty(openid))
                {
                    socialType = 3;
                    if (Session["accessToken"] == null && !string.IsNullOrEmpty(code))
                    {
                        var redirectUrl = AccessTokenToolkit.GenerateHostPath(Request.Url) + Url.Action("Index", "SignUp");
                        ViewBag.SocialInfo = _authCode.GenerateAccessTokenUrl(redirectUrl, code);
                        var accessToken = _authCode.GetResult(_authCode.GenerateAccessTokenUrl(redirectUrl, code));

                        /*if (apit.WasError(accessToken, out _err))
                        {
                            Session["err"] = _err;
                            return RedirectToAction("Error");
                        }

                        accessToken.openid = openid; //注意腾讯微创新
                        accessToken.openkey = openkey; //注意腾讯微创新
                        Session.Add("accessToken", accessToken);

                        WebContext.Current.SocialAccount = openid;
                        WebContext.Current.SocialAccessInfo = String.Format("{0};{1}", accessToken.access_token, openkey);*/
                        WebContext.Current.SocialType = (Byte)Enums.SocialTpye.QQ;
                    }
                    
                    GetUserInfo(socialType);
                }

                //Sina
                else
                {
                    socialType = 1;
                    if (Session["accessToken"] == null && !string.IsNullOrEmpty(code))
                    {
                        var redirectUrl = AccessTokenToolkit.GenerateHostPath(Request.Url) + Url.Action("Index", "SignUp");
                        var accessToken = _authCodes.GetResult(_authCodes.GenerateAccessTokenUrl(redirectUrl, code));

                        if (apis.WasError(accessToken, out _errs))
                        {
                            Session["err"] = _errs;
                            return RedirectToAction("Error");
                        }
                        Session.Add("accessToken", accessToken);
                        WebContext.Current.SocialAccount = accessToken.uid;
                        WebContext.Current.SocialAccessInfo =  accessToken.access_token;
                        WebContext.Current.SocialType = (Byte)Enums.SocialTpye.Sina;
                    }
                    GetUserInfo(socialType);
                }
            }
            return View();
        }

        [NonAction]
        private void GetUserInfo(Byte socialType)
        {
            String socialAccount = "";
            var accessTokenObj = Session["accessToken"] as dynamic;
            var accessToken = accessTokenObj.access_token;

            //Sina
            if (socialType == 1)
            {
                var uid = accessTokenObj.uid;
                var model = apis.CallGet("users/show.json?uid=" + uid, accessToken);

                if (apis.WasError(model, out _err))
                {
                    Session["err"] = _err;
                }
                else
                {
                    ViewBag.SocialName = model.screen_name;
                    ViewBag.SocialAvatar = model.avatar_large;
                    socialAccount = uid;
                    //var blogStatus = apis.CallGet("statuses/user_timeline/ids.json?uid=" + uid, accessToken);
                    //ViewBag.SocialInfo = blogStatus.total_number;
                }
            }
            //Tencent
            else
            {
                var model = apit.CallGet("user/info?format=json", accessToken, false, GetOpenidOpenkeyParamsExt());

                if (apit.WasError(model, out _err))
                {
                    Session["err"] = _err;
                }
                else
                {
                    ViewBag.SocialName = model.nick;
                    ViewBag.SocialAvatar = model.head;
                    socialAccount = model.openid;
                }
            }

            ViewBag.SocialType = (Byte)socialType;
            if (!String.IsNullOrEmpty(socialAccount))
            {
                ViewBag.SocialAccount = socialAccount;
                WebContext.Current.SocialAccount = socialAccount;
                
                var memberInfo = _commonService.GetMemberInfo(socialAccount, (Byte)socialType);
                int memberId = (memberInfo == null ? 0 : memberInfo.MemberId);
                WebContext.Current.MemberId = memberId;
                ViewBag.MemberId = memberId;
            }
        }

        [NonAction]
        private IDictionary<string, object> GetOpenidOpenkeyParamsExt()
        {
            var result = new Dictionary<string, object>();

            var accessTokenObj = Session["accessToken"] as dynamic;
            var openid = accessTokenObj.openid;
            var openkey = accessTokenObj.openkey;

            result.Add("openid", openid);
            result.Add("openkey", openkey);

            return result;
        }

        [NonAction]
        private int GetMicroBlogNums(Byte socialType)
        {
            int blogNum = -2;
            String socialAccount = WebContext.Current.SocialAccount;
            
            //Sina
            if (socialType == (Byte)Enums.SocialTpye.Sina)
            {
                String accessToken = WebContext.Current.SocialAccessInfo;
                var uid = socialAccount;

                var blogStatus = apis.CallGet("statuses/user_timeline/ids.json?uid=" + uid, accessToken);
                if (apis.WasError(blogStatus, out _err))
                {
                    Session["err"] = _err;
                }
                else
                {
                    //Numbers of micro blog
                    blogNum = Convert.ToInt32(blogStatus.total_number);
                }
            }

            return blogNum;
        }

        [NonAction]
        private void CreateFriendShip(Byte socialType)
        {
            String socialAccount = "";
            var accessTokenObj = Session["accessToken"] as dynamic;
            var accessToken = accessTokenObj.access_token;

            //Sina
            if (socialType == 1)
            {
                var uid = accessTokenObj.uid;
                var model = apis.CallGet("friendships/create.json?uid=" + uid, accessToken);

                if (apis.WasError(model, out _err))
                {
                    Session["err"] = _err;
                }
                else
                {
                    ViewBag.SocialName = model.screen_name;
                    ViewBag.SocialAvatar = model.avatar_large;
                    socialAccount = uid;
                    //var blogStatus = apis.CallGet("statuses/user_timeline/ids.json?uid=" + uid, accessToken);
                    //ViewBag.SocialInfo = blogStatus.total_number;
                }
            }
            //Tencent
            else
            {

            }
        }

        [HttpPost]
        public JsonResult BeforeShareClassOnSocial(int checknum)
        {
            int memberId = WebContext.Current.MemberId;
            //Check is this student got share class coins before
            Boolean gotCoinsBefore = _commonService.HasShareClassCoin(memberId);
            
            //got share class coin before
            if (gotCoinsBefore)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
            else
            {
                Byte socialType = Convert.ToByte(WebContext.Current.SocialType);
                String accessToken = WebContext.Current.SocialAccessInfo;
                String socialAccount = WebContext.Current.SocialAccount;
               
                int blogNum = GetMicroBlogNums(socialType);

                if (checknum >= 0 && blogNum > checknum)
                {
                    _commonService.AddMembersCoin(memberId, Constants.BizConfig.FreeCoinForShareClass, (Byte)Enums.DBAccess.CoinUpdateType.ClassShareOnSocial);
                }

                var jsonObj = new JsonResult();
                jsonObj.Data = new { i = memberId, n = blogNum, t = accessToken, a = socialAccount};

                return Json(jsonObj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult LogOutSocialAccount()
        {
            if (WebContext.Current.SocialAccessInfo != null)
            {
                Enums.SocialTpye socialType = (Enums.SocialTpye)WebContext.Current.SocialType;
                JsonResult rs = null;

                if (socialType.Equals(Enums.SocialTpye.Sina))
                {
                    var accessTokenObj = Session["accessToken"] as dynamic;
                    var accessToken = accessTokenObj.access_token;
                    //var result = apis.CallGet("oauth2/revokeoauth2", accessToken);
                    var result = CallGet("https://api.weibo.com/oauth2/revokeoauth2", accessToken);
                    rs = Json(result, JsonRequestBehavior.AllowGet);
                }
                else if (socialType.Equals(Enums.SocialTpye.Sina))
                {
                    //var result = apit.CallGet("account/end_session.json?uid=" + uid, accessToken);
                    //rs = Json(result, JsonRequestBehavior.AllowGet);
                }

                Session["accessToken"] = null;
                WebContext.Current.SocialAccessInfo = "";
                WebContext.Current.SocialAccount = "";
                WebContext.Current.MemberId = 0;
                WebContext.Current.SocialType = Constants.DefaultSetting.SocialType;

                return rs;
            }
            else
            {
                return Json("NoLoginInfo", JsonRequestBehavior.AllowGet);
            }

        }


        private dynamic CallGet(string url, string accessToken)
        {
            var linkChar = url.IndexOf('?') > 0 ? "&" : "?";
            var queryStringAccessToken = linkChar + "access_token=" + accessToken;

            ClientRequest cr = new ClientRequest(url + queryStringAccessToken);
            cr.HttpMethod = WebRequestMethods.Http.Get;
                        
            dynamic result = null;
            result = NetQuick.GetResponseForText(cr);
           
            return result;
        }

    }
}