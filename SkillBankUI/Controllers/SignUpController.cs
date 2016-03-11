using System;
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
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services.Utility;

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

        private IAuthorizationCodeBase _authCodew = Uf.C(CtorAT.WeChat);
        private WeChatError _errw = null;
        private IApi apiw = Uf.C(CtorApi.WeChat);

        public SignUpController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        public ActionResult Index(string code = "", string openid = "", string openkey = "", string state = "")
        {
            int memberId = 0;
            //TO DO：Temp for testing server
            var backCookie = Request.Cookies["burl"];
            String backUrl = (backCookie == null || string.IsNullOrEmpty(backCookie.Value)) ? "" : backCookie.Value;
            //if (string.IsNullOrEmpty(backUrl))
            //{
            //    //    String backUrl = backCookie.Value;
            //    //ViewBag.isMobile = backCookie.Value.Contains("m.skill");
            //    ViewBag.isMobile = true;
            //Response.Redirect("/m/register?code=" + code + "&openid=" + openid + "&openkey=" + openkey);
            //}
            if (!String.IsNullOrEmpty(Request.QueryString["mb"]))
            {
                ViewBag.isMobile = true;
                backUrl = Request.QueryString["mb"];
            }
            else
            {
                ViewBag.isMobile = false;
                backUrl = String.IsNullOrEmpty(backUrl) ? "/" : HttpUtility.UrlDecode(backUrl);
            }

            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("sinup");
            ViewBag.MemberInfo = null;

            Byte socialType = 0;

            //OAuth2
            if (!String.IsNullOrEmpty(code))
            {
                //Tencent
                /*if (!String.IsNullOrEmpty(openid))
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
                        WebContext.Current.SocialAccessInfo = String.Format("{0};{1}", accessToken.access_token, openkey);
                        WebContext.Current.SocialType = (Byte)Enums.SocialTpye.QQ;
                    }
                }
                
                */
                //wechat
                if (!String.IsNullOrEmpty(state) && !String.IsNullOrEmpty(code))
                {
                    ViewBag.isMobile = true;
                    socialType = 4;
                    if (Session["accessToken"] == null || WebContext.Current.SocialType != socialType) 
                    {
                        var redirectUrl = AccessTokenToolkit.GenerateHostPath(Request.Url) + Url.Action("Index", "SignUp");
                        var accessToken = _authCodew.GetResult(_authCodew.GenerateAccessTokenUrl(redirectUrl, code));

                        if (apiw.WasError(accessToken, out _errw))
                        {
                            Session["err"] = _errw;
                            return RedirectToAction("Error");
                        }
                        Session.Add("accessToken", accessToken);
                        WebContext.Current.SocialAccessInfo = accessToken.access_token;
                        WebContext.Current.SocialType = (Byte)Enums.SocialTpye.WeChat;
                    }
                }
                //Sina
                else
                {
                    socialType = 1;
                    if ((Session["accessToken"] == null || WebContext.Current.SocialType != socialType) && !string.IsNullOrEmpty(code))
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
                        WebContext.Current.SocialAccessInfo = accessToken.access_token;
                        WebContext.Current.SocialType = (Byte)Enums.SocialTpye.Sina;
                    }
                }

                if (socialType > 0)
                {
                    Boolean isMobileVerified = false;
                    memberId = GetUserInfo(socialType, out isMobileVerified);
                    if (memberId > 0)
                    {
                        WebContext.Current.MemberId = memberId;
                        String domain = UtilitiesModule.GetCookieDomain(Request.Url.Host);
                        CookieManager.SetCookie(Constants.CookieKeys.MemberId, memberId.ToString(), /*isPersistent*/ true, domain, Response);
                        if (!string.IsNullOrEmpty(backUrl) && socialType < 3)
                        {
                            Response.Redirect(backUrl);
                        }
                        else if (socialType == 4)
                        {
                            Response.Redirect("/m");
                        }

                    }
                    ViewBag.MemberId = memberId;
                }

            }
            return View();
        }

        [NonAction]
        private int GetUserInfo(Byte socialType, out Boolean isMobileVerified)
        {
            int memberId = 0;
            isMobileVerified = false;
            String socialAccount = "";
            String unionId = "";
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
                    String gender = (model.gender == null ? "" : Convert.ToString(model.gender));
                    ViewBag.Gender = (!String.IsNullOrEmpty(gender) && gender.Equals("f")) ? 0 : 1;
                    socialAccount = uid;
                    //var blogStatus = apis.CallGet("statuses/user_timeline/ids.json?uid=" + uid, accessToken);
                    //ViewBag.SocialInfo = blogStatus.total_number;
                }
            }
            else if (socialType == 4)
            {
                //TO DO:APP WeChat1 switch url
                var model = apiw.CallGet("sns/userinfo", accessToken, false, GetOpenidOpenkeyParamsExt());
                //var model2 = apiw.CallGet("https://api.weixin.qq.com/cgi-bin/user/info", accessToken, false, GetOpenidOpenkeyParamsExt());

                if (apiw.WasError(model, out _err))
                {
                    Session["err"] = _err;
                }
                else
                {
                    ViewBag.SocialName = model.nickname;
                    String avatarImg = (model.headimgurl == null ? "" : Convert.ToString(model.headimgurl));
                    if (!String.IsNullOrEmpty(avatarImg))
                    {
                        int idx = avatarImg.LastIndexOf('/');
                        if (idx > 0)
                        {
                            avatarImg = String.Concat(avatarImg.Substring(0, idx + 1), "132");
                        }
                    }
                    ViewBag.SocialAvatar = avatarImg;
                    String gender = (model.sex == null ? "" : Convert.ToString(model.sex));
                    ViewBag.Gender = (!String.IsNullOrEmpty(gender) && gender.Equals("2")) ? 0 : 1;
                    unionId = (accessTokenObj.unionid != null) ? (String)accessTokenObj.unionid : "";
                    ViewBag.UnionId = unionId;
                    socialAccount = model.openid;
                    WebContext.Current.SocialType = (Byte)Enums.SocialTpye.WeChat;
                }
            }
            //Tencent
            //else if (socialType == 3)
            //{
            //    var model = apit.CallGet("user/info?format=json", accessToken, false, GetOpenidOpenkeyParamsExt());

            //    if (apit.WasError(model, out _err))
            //    {
            //        Session["err"] = _err;
            //    }
            //    else
            //    {
            //        ViewBag.SocialName = model.nick;
            //        ViewBag.SocialAvatar = model.head;
            //        socialAccount = model.openid;
            //    }
            //}
            //WeChat

            ViewBag.SocialType = (Byte)socialType;
            if (!String.IsNullOrEmpty(socialAccount))
            {
                ViewBag.SocialAccount = socialAccount;
                ViewBag.UnionId = unionId;
                WebContext.Current.SocialAccount = socialAccount;

                var memberInfo = _commonService.GetMemberInfo(socialAccount, (Byte)socialType, unionId);
                if (memberInfo == null)
                {
                    memberId = 0;
                }
                else
                {
                    memberId = memberInfo.MemberId;
                    isMobileVerified = (memberInfo.VerifyTag & 1).Equals(1);
                    
                    //get and save rong cloud token for old users
                    if (!memberId.Equals(0) && String.IsNullOrEmpty(memberInfo.RCToken))
                    {
                        String rcToken = RongCloudHelper.GetToken(System.Configuration.ConfigurationManager.AppSettings["ENV"], memberInfo.MemberId, memberInfo.Name, memberInfo.Avatar);
                        if (!String.IsNullOrEmpty(rcToken))
                        {
                            Byte type = (Byte)Enums.DBAccess.MemberSaveType.UpdateRCTokenADeviceToken;
                            MemberInfo updateInfo = new MemberInfo() { MemberId = memberId, Avatar = rcToken };
                            var updateResult = _commonService.UpdateMemberProfile(updateInfo, type);
                        }
                    }

                }
               
            }
            return memberId;
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
            //WeChat
            else
            { }
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
                jsonObj.Data = new { i = memberId, n = blogNum, t = accessToken, a = socialAccount };

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

                if (socialType.Equals(Enums.SocialTpye.Sina) && Session["accessToken"] != null)
                {
                    var accessTokenObj = Session["accessToken"] as dynamic;
                    var accessToken = accessTokenObj.access_token;
                    try
                    {
                        var result = apis.CallGet("https://api.weibo.com/oauth2/revokeoauth2", accessToken);
                        rs = Json(result, JsonRequestBehavior.AllowGet);
                    }
                    catch 
                    {
                    }
                }
            }

            Session["accessToken"] = null;
            WebContext.Current.SocialAccessInfo = "";
            WebContext.Current.SocialAccount = "";
            WebContext.Current.MemberId = 0;
            WebContext.Current.SocialType = Constants.DefaultSetting.SocialType;

            Request.Cookies.Clear();
            Response.Cookies.Clear();
            return Json("", JsonRequestBehavior.AllowGet);
        }


        //private dynamic CallGet(string url, string accessToken)
        //{
        //    var linkChar = url.IndexOf('?') > 0 ? "&" : "?";
        //    var queryStringAccessToken = linkChar + "access_token=" + accessToken;

        //    ClientRequest cr = new ClientRequest(url + queryStringAccessToken);
        //    cr.HttpMethod = WebRequestMethods.Http.Get;

        //    dynamic result = null;
        //    result = NetQuick.GetResponseForText(cr);

        //    return result;
        //}

    }
}
