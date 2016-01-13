using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Security;

using SkillBank.Site.Common;
using SkillBank.Site.Services;
using SkillBank.Site.DataSource;
using SkillBank.Site.Web;
using SkillBank.Site.Web.ViewModel;
using SkillBank.Site.Web.Context;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services.Providers;

using dotNetDR_OAuth2;
using dotNetDR_OAuth2.AccessToken;
using dotNetDR_OAuth2.APIs;
using dotNetDR_OAuth2.Net;
using dotNetDR_OAuth2.JSON;
using dotNetDR_OAuth2.APIs.WeChat;
using System.Net;

namespace SkillBank.Controllers
{
    public class MController : Controller
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        private IAuthorizationCodeBase _authCode = Uf.C(CtorAT.Tencent);
        private TencentError _err = null;
        private IApi apit = Uf.C(CtorApi.Tencent);

        private IAuthorizationCodeBase _authCodes = Uf.C(CtorAT.Sina);
        private SinaError _errs = null;
        private IApi apis = Uf.C(CtorApi.Sina);


        public MController(IContentService contentService, ICommonService commonService)
        {
            _contentService = contentService;
            _commonService = commonService;

            int memberId = WebContext.Current.MemberId;
            //Only handler order after member login
            if (memberId > 0)
            {
                var handleKey = OrderHandlerHelper.GetHandleMemberOrderKey(WebContext.Current.MemberId, WebContext.Current.OrderHandleDate);
                if (!String.IsNullOrEmpty(handleKey))
                {
                    _commonService.HandleMemberOrder(memberId);
                    WebContext.Current.OrderHandleDate = handleKey;
                }
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoginA()
        {
            return View();
        }

        public ActionResult LoginM()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }


        // GET: /M/

        /// <summary>
        /// 默认达人课程 
        /// </summary>
        /// <param name="by">默认 1</param>
        /// <param name="type">默认 1</param>
        /// <returns></returns>
        public ActionResult Index(Byte by = 1, Byte type = 2, String key = "", String city = "")
        {
            int cityId = 0;
            Decimal x = Convert.ToDecimal(121.4165);
            Decimal y = Convert.ToDecimal(31.2190);

            var metaTags = MetaTagHelper.GetMetaTags("classsearch");
            ViewBag.MetaTagTitle = metaTags[0];
            ViewBag.MetaTagKeyWords = metaTags[1];
            ViewBag.MetaTagDescription = metaTags[2];

            int memberId = GetCurrentMemberInfo(false);
            ViewBag.ActiveTab = 0;
            if (memberId > 0)
            {
                GetNotificationNums(memberId);
            }

            if (!String.IsNullOrEmpty(key))
            {
                by = 3;
                ViewBag.SearchKey = key;
            }

            ClassListModel classListModel = new ClassListModel();

            var categories = _contentService.GetAllCategories();
            classListModel.CategoryLkp = LookupHelper.GetCatagory4Picker(categories);

            var cityDic = _contentService.GetCities("cn");
            if (by.Equals((Byte)Enums.DBAccess.ClassTabListLoadType.NearBy) && !String.IsNullOrEmpty(city))
            {
                city = city.Replace("市", "");
                var cityItems = cityDic.Where(c => c.Value.CityName.StartsWith(city));
                if (cityItems.Count() > 0)
                {
                    cityId = cityItems.First().Key;
                }
            }

            var result = _commonService.GetClassTabList(by, type, memberId, key, x, y, cityId);
            classListModel.ClassList = result;
            return View(classListModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Class Id</param>
        /// <returns></returns>
        public ActionResult Course(int id = 0)
        {
            String className = "";
            int memberId = GetCurrentMemberInfo(false);

            ClassDetailModel classDetailModel = new ClassDetailModel();
            if (id > 0)
            {
                var classInfo = _commonService.GetClassInfoItem((Byte)Enums.DBAccess.ClassLoadType.ByClassAndCurrMemberId, id, memberId);
                if (classInfo != null)
                {
                    int reviewNum = 0, commentNum = 0, likeNum = 0;//current class, member other class, comment, like num
                    likeNum = classInfo.LikeNum;
                    classDetailModel.ClassInfo = classInfo;
                    int teacherId = classInfo.Member_Id;
                    ViewBag.ContactorId = teacherId;
                    //class owner
                    className = String.IsNullOrEmpty(classInfo.Title) ? ResourceHelper.GetTransText(560) : classInfo.Title;

                    var teacherInfo = _commonService.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.ByMemberIdAndRelatedMemberId, teacherId, memberId);
                    //teacherInfo.MemberId = teacherId;
                    classDetailModel.MemberInfo = teacherInfo;

                    var studentReview = _commonService.GetClassReviews((Byte)Enums.DBAccess.ReviewLoadType.ByClassAll, teacherId, id, 0, 0);
                    if (studentReview != null && studentReview.Count > 0)
                    {
                        var classReviews = studentReview.Where(r => r.TabId == 0).ToList();
                        if (classReviews != null && classReviews.Count() != 0)
                        {
                            classDetailModel.ClassReview = classReviews;
                            reviewNum = classReviews.Count();
                        }
                        
                        var classComments = studentReview.Where(r => r.TabId == 2).ToList();
                        if (classComments != null && classComments.Count() != 0)
                        {
                            classDetailModel.ClassComment = classComments;
                            commentNum = classComments.Count();
                        }
                    }

                    //var classList = _commonService.GetClassInfo((Byte)Enums.DBAccess.ClassLoadType.ByTeacherPublished, memberId);
                    //if (classList != null && classList.Count > 0)
                    //{
                    //    classDetailModel.ClassList = classList.Where(c => c.ClassId != id).ToList();
                    //}
                    //else
                    //{
                    //    classDetailModel.ClassList = null;
                    //}

                    classDetailModel.IsLogin = (memberId > 0);
                    classDetailModel.IsOwner = memberId.Equals(teacherId);
                    ViewBag.ContactMobile = (teacherInfo.NotifyTag & 1).Equals(1) ? teacherInfo.Phone : "";//for send SMS notify

                    //if (classDetailModel.IsOwner)
                    //{
                    //    _commonService.UpdateNotification((Byte)Enums.DBAccess.NotificationTagUpdateType.SetClassRelatedAsReadByClassId, memberId, id);
                    //}

                    var numDic = _commonService.GetNumsByMemberClass(teacherId, id, (Byte)Enums.DBAccess.MemberNumsLoadType.ByClassSummary);
                    //init numbers on page
                    numDic.Add(Enums.NumberDictionaryKey.StudentReview, reviewNum);
                    //numDic.Add(Enums.NumberDictionaryKey.Comment, commentNum);
                    numDic.Add(Enums.NumberDictionaryKey.Like, likeNum);
                    classDetailModel.ClassNumDic = numDic;


                    //WeChat share function paras
                    String timestamp = WeChatHelper.GetTimestamp().ToString();
                    String nonceStr = WeChatHelper.GetNonceStr();
                    String CurrUrl = Request.Url.AbsoluteUri;
                    String jsAPITicket = GetJsAPITicket(GetAccessToken());

                    String str4SHA1 = WeChatHelper.GenerateString4Signature(jsAPITicket, nonceStr, timestamp, CurrUrl);
                    String signature = FormsAuthentication.HashPasswordForStoringInConfigFile(str4SHA1, "SHA1").ToLower();


                    ViewBag.AppId = ConfigConstants.ThirdPartySetting.WeChatSetting.AppID;
                    ViewBag.TimeStamp = timestamp;
                    ViewBag.NonceStr = nonceStr;
                    ViewBag.Signature = signature;
                    ViewBag.JsAPITicket = jsAPITicket;
                }
                else
                {
                    classDetailModel.ClassInfo = null;
                    ViewBag.ErrorMessage = ResourceHelper.GetTransText(580);//No class info
                }
            }

            var metaTags = MetaTagHelper.GetMetaTags("classdetail");
            ViewBag.MetaTagTitle = metaTags[0].Replace("{0}", className);
            ViewBag.MetaTagKeyWords = metaTags[1];
            ViewBag.MetaTagDescription = metaTags[2];

            return View(classDetailModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Class Id</param>
        /// <returns></returns>
        public ActionResult CoursePreview(int id = 0)
        {
            String className = "";
            int memberId = GetCurrentMemberInfo(false);

            ClassDetailModel classDetailModel = new ClassDetailModel();
            if (id > 0)
            {
                var classInfo = _commonService.GetClassInfoItem((Byte)Enums.DBAccess.ClassLoadType.ByClassId, id, 0);
                //Class Owner
                if (classInfo != null && classInfo.Member_Id.Equals(memberId))
                {
                    classDetailModel.ClassInfo = classInfo;
                    className = String.IsNullOrEmpty(classInfo.Title) ? ResourceHelper.GetTransText(560) : classInfo.Title;
                }
                else
                {
                    classDetailModel.ClassInfo = null;
                    ViewBag.ErrorMessage = ResourceHelper.GetTransText(580);//No class info
                }
            }

            var metaTags = MetaTagHelper.GetMetaTags("classdetail");
            ViewBag.MetaTagTitle = metaTags[0].Replace("{0}", className);
            ViewBag.MetaTagKeyWords = metaTags[1];
            ViewBag.MetaTagDescription = metaTags[2];

            return View(classDetailModel);
        }

        public ActionResult Message()
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("message");
            ViewBag.ActiveTab = 1;

            int memberId = GetCurrentMemberInfo(true);

            MessageListModel messageListModel = new MessageListModel();
            if (memberId > 0)
            {
                var messages = _commonService.GetMessageList(memberId, 0, (Byte)Enums.DBAccess.MessageLoadType.DateAsc);
                messageListModel.Messages = messages;
                ViewBag.MaxMessageId = (messages == null ? 0 : messages.Max(m => m.MessageId));
                var unReadMessageNum = _commonService.GetMessageUnReadNum(memberId);
                messageListModel.UnReadMessageNumDic = unReadMessageNum;

                //_commonService.UpdateNotification((Byte)Enums.DBAccess.NotificationTagUpdateType.SetMessageAsPopedByMemberId, memberId, 0);

                var notifications = _commonService.GetNotification(memberId, (Byte)Enums.DBAccess.NotificationAlterLoadType.MobileNotification);
                messageListModel.Notifications = notifications;
                GetNotificationNums(memberId);
            }
            return View(messageListModel);
        }

        public ActionResult Chat(int id = 0)
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("chat");

            int contactId = id;
            int memberId = GetCurrentMemberInfo(true);
            //LoadNotificationAlert(currMemberId);


            MessageDetailModel messageDetailModel = new MessageDetailModel();
            if (memberId > 0 && memberId != contactId && contactId > 0)
            {
                var messages = _commonService.GetMessageDetail(memberId, contactId, (Byte)Enums.DBAccess.MessageLoadType.DateAsc);
                messageDetailModel.Messages = messages;
                var contact =  _commonService.GetMemberInfo(contactId);
                messageDetailModel.Contact = contact;
                //messageDetailModel.ContactClass = _commonService.GetClassInfoById(contactId, (Byte)Enums.DBAccess.ClassLoadType.ByTeacherPublished);
                messageDetailModel.MaxMessageId = (messages == null || messages.Count.Equals(0)) ? 0 : messageDetailModel.Messages.Max(m => m.MessageId);

                //TO DO : Move to client side with set max message id
                _commonService.SetMessageAsRead(0, memberId, contactId);
                ViewBag.ContactMobile = (contact.NotifyTag & 1).Equals(1) ? contact.Phone : "";//for send SMS notify
            }
            return View(messageDetailModel);
        }

        /// <summary>
        /// Personal page 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Personal(int id = 0)
        {
            String userName = "";//use in metatag
            int memberId = 0, sReviewNum = 0, tReviewNum = 0;
            int currMemberId = GetCurrentMemberInfo(false);
            ProfilelModel profileModel = new ProfilelModel();

            //view other member's page
            if (id > 0)
            {
                memberId = id;
                var memberInfo = _commonService.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.ByMemberIdAndRelatedMemberId, memberId, currMemberId);
                if (memberInfo != null)
                {
                    memberInfo.MemberId = memberId;
                    profileModel.MemberInfo = memberInfo;
                    ViewBag.ContactMobile = (memberInfo.NotifyTag & 1).Equals(1) ? memberInfo.Phone : "";//for send SMS notify
                    ViewBag.IsOwner = memberId.Equals(currMemberId);
                    userName = memberInfo.Name;
                }
                //member not exists
                else
                {
                    return View();
                }
            }
            else if (id == 0)
            {
                //Own page add notification
                memberId = currMemberId;
                profileModel.MemberInfo = ViewBag.MemberInfo;
                userName = ViewBag.MemberInfo.Name;
                ViewBag.IsOwner = true;
            }

            //if (ViewBag.IsOwner)
            //{
            //    _commonService.UpdateNotification((Byte)Enums.DBAccess.NotificationTagUpdateType.SetTeacherReviewAsRead, memberId);
            //}

            GetNotificationNums(currMemberId);

            //Get reviews
            if (memberId > 0)
            {
                profileModel.ClassList = _commonService.GetClassInfo((Byte)Enums.DBAccess.ClassLoadType.ByTeacherPublished, 0, memberId);
                var reviews = _commonService.GetMemberReviews((Byte)Enums.DBAccess.ReviewLoadType.ByMemberAll, memberId, 0, 0);
                if (reviews != null && reviews.Count > 0)
                {
                    var stuentReview = reviews.Where(r => r.TabId == 0).ToList();
                    if (stuentReview != null && stuentReview.Count() != 0)
                    {
                        profileModel.StuentReview = stuentReview;
                        sReviewNum = stuentReview.Count();
                    }

                    var teacherReview = reviews.Where(r => r.TabId == 1).ToList();
                    if (teacherReview != null && teacherReview.Count() != 0)
                    {
                        profileModel.TeacherReview = teacherReview;
                        tReviewNum = teacherReview.Count();
                    }
                }

                var numDic = _commonService.GetNumsByMember(memberId, (Byte)Enums.DBAccess.MemberNumsLoadType.ByMemberSummary);
                numDic.Add(Enums.NumberDictionaryKey.StudentReview, sReviewNum);
                numDic.Add(Enums.NumberDictionaryKey.TeacherReview, tReviewNum);

                profileModel.ProfileNumDic = numDic;
            }

            var metaTags = MetaTagHelper.GetMetaTags("profile");
            ViewBag.MetaTagTitle = metaTags[0].Replace("{0}", userName);
            ViewBag.MetaTagKeyWords = metaTags[1];
            ViewBag.MetaTagDescription = metaTags[2];

            return View(profileModel);
        }
        
        public ActionResult Register(string code = "", string openid = "", string openkey = "")
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
                        WebContext.Current.SocialAccessInfo = accessToken.access_token;
                        WebContext.Current.SocialType = (Byte)Enums.SocialTpye.Sina;
                    }

                }

                if (socialType > 0)
                {
                    GetUserInfo(socialType);
                }
            }
            return View();
        }


        #region Non-public functions

        /// <summary>
        /// Register and user infomation for social network
        /// </summary>
        /// <param name="socialType"></param>
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
            else if (socialType == 3)
            {
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shouldLogin"></param>
        /// <returns></returns>
        private int GetCurrentMemberInfo(Boolean shouldLogin)
        {
            int memberId = WebContext.Current.MemberId;
            var memberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;
            if (memberInfo != null)
            {
                ViewBag.MemberInfo = memberInfo;
                ViewBag.IsLogin = true;
            }
            else
            {
                ViewBag.IsLogin = false;
                if (shouldLogin)
                {
                    Response.Redirect("/m/login");
                }
            }
            return memberId;
        }
        
        /// <summary>
        /// Show on bottom menu and message page
        /// </summary>
        /// <param name="memberId"></param>
        private void GetNotificationNums(int memberId, Byte loadType = (Byte)Enums.DBAccess.NotificationAlterLoadType.MobileMenu)
        {
            if (memberId > 0)
            {
                var result = _commonService.GetPopNotification(memberId, loadType);
                Dictionary<String, int> alterNums = new Dictionary<string, int>();
                if (result != null)
                {
                    //message tab2
                    int systemNotiNum = result.Any(a => (a.Number > 0 && a.Type.Equals("s"))) ? 1 : 0;
                    int messageNotiNum = result.Any(a => (a.Number > 0 && a.Type.Equals("m"))) ? 1 : 0;
                    int actionNotiNum = result.Any(a => (a.Number > 0 && a.Type.Equals("a"))) ? 1 : 0;
                    alterNums.Add("nm", systemNotiNum + messageNotiNum + actionNotiNum);
                    alterNums.Add("s", systemNotiNum);
                    alterNums.Add("a", actionNotiNum);
                    //course tab3
                    int classNotiNum = result.Any(a => (a.Number > 0 && a.Type.Equals("c"))) ? 1 : 0;
                    int teacheNotiNum = result.Any(a => (a.Number > 0 && a.Type.Equals("t"))) ? 1 : 0;
                    int learnNotiNum = result.Any(a => (a.Number > 0 && a.Type.Equals("l"))) ? 1 : 0;
                    alterNums.Add("nc", (classNotiNum + teacheNotiNum + learnNotiNum));
                    alterNums.Add("c", classNotiNum);
                    alterNums.Add("t", teacheNotiNum);
                    alterNums.Add("l", learnNotiNum);
                    ViewBag.AlterNums = alterNums;
                }
            }
        }

        private string GetAccessToken()
        {

            //String token = null;
            //ConfigureInfo configInfo = _configureManager.GetConfigureByKey("wechat_token");
            //if (configInfo.ExpireDate < DateTime.Now)
            //{
            //    DateTime expireDate = new DateTime();
            //    token = WeChatHelper.GetToken(out expireDate);
            //    _configureManager.UpdateConfigureInfo("wechat_token", token, expireDate);
            //}
            //else
            //{
            //    token = configInfo.InfoValue;
            //}

            SocialCacheItem cachedToken;
            var accessToken = StaticCacheProvider.GetObject(Constants.CacheDicKeys.WeChatAccessToken);
            if (accessToken == null)
            {
                DateTime expireDate = new DateTime();
                String token = WeChatHelper.GetToken(out expireDate);
                cachedToken = new SocialCacheItem() { ItemValue = token, ExpireDate = expireDate };
                StaticCacheProvider.SetObject(Constants.CacheDicKeys.WeChatAccessToken, cachedToken);
                return token;
            }
            else
            {
                cachedToken = (SocialCacheItem)StaticCacheProvider.GetObject(Constants.CacheDicKeys.WeChatAccessToken);
                return cachedToken.ItemValue;
            }
        }

        private string GetJsAPITicket(String accessToken)
        {
            SocialCacheItem cachedTicket;
            var apiTicket = StaticCacheProvider.GetObject(Constants.CacheDicKeys.WeChatJsapiTicket);
            if (apiTicket == null)
            {
                DateTime expireDate = new DateTime();
                String ticket = WeChatHelper.GetJsAPITicket(accessToken, out expireDate);
                cachedTicket = new SocialCacheItem() { ItemValue = ticket, ExpireDate = expireDate };
                StaticCacheProvider.SetObject(Constants.CacheDicKeys.WeChatJsapiTicket, cachedTicket);
                return ticket;
            }
            else
            {
                cachedTicket = (SocialCacheItem)StaticCacheProvider.GetObject(Constants.CacheDicKeys.WeChatJsapiTicket);
                return cachedTicket.ItemValue;
            }
        }

       #endregion

        #region Phase 2 pages

        /// <summary>
        /// Get own favorite class (Apr1th without frontend)
        /// </summary>
        /// <returns></returns>
        public ActionResult Favorites()
        {
            ViewBag.ActiveTab = 3;

            var memberId = GetCurrentMemberInfo(true);
            var result = _commonService.GetClassCollection((Byte)Enums.DBAccess.ClassCollectionLoadType.ByMemberLiked, memberId, 0);
            GetNotificationNums(memberId);

            return View(result);
        }

        /// <summary>
        /// Get own or others follow members 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Following(int id = 0)
        {
            ViewBag.ActiveTab = 3;
            Byte loadType = 0;

            var memberId = GetCurrentMemberInfo(true);
            if (id.Equals(0) || id.Equals(memberId))//own fans 
            {
                id = memberId;
                ViewBag.Avatar = ViewBag.MemberInfo.Avatar;
            }
            else if (id > 0)//if view others fans 
            {
                var memberInfo = _commonService.GetMemberInfo(id);
                ViewBag.Avatar = (memberInfo == null ? "" : memberInfo.Avatar);
            }
            loadType = (Byte)Enums.DBAccess.FavoriteLoadType.ByFollwingMemberAViewerId;
            var result = _commonService.GetFavorites(loadType, id, memberId);
            GetNotificationNums(memberId);

            return View(result);
        }

        /// <summary>
        /// Get own or others fans (Apr1th without frontend)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Fans(int id = 0)
        {
            ViewBag.ActiveTab = 3;
            
            var memberId = GetCurrentMemberInfo(true);
            if (id.Equals(0) || id.Equals(memberId))//own fans 
            {
                id = memberId;
                ViewBag.Avatar = ViewBag.MemberInfo.Avatar;
            }
            else if (id > 0)//if view others fans 
            {
                var memberInfo = _commonService.GetMemberInfo(id);
                ViewBag.Avatar = (memberInfo == null ? "" : memberInfo.Avatar);
            }
            var result = _commonService.GetFavorites((Byte)Enums.DBAccess.FavoriteLoadType.ByFansMemberAViewerId, id, memberId);
            GetNotificationNums(memberId);
            
            return View(result);
        }

        /// <summary>
        /// Add or Edit class information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CourseEdit(int id = 0)
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("classadd");

            ViewBag.ActiveTab = 2;
            ViewBag.ActiveStep = 0;

            //update user get info use shared one
            var memberId = GetCurrentMemberInfo(true);

            //Get member city info
            var cityName = "";
            if (ViewBag.MemberInfo.CityId > 0)
            {
                var cityDic = _contentService.GetCities("cn");
                cityName = LookupHelper.GetCityNameById(cityDic, ViewBag.MemberInfo.CityId);
            }
            ViewBag.CityName = cityName;

            ClassEditModel classEditModel = new ClassEditModel();
            var categoryLkp = _contentService.GetAllCategories();
            var categories = LookupHelper.GetCatagory4Picker(categoryLkp);
            if (categories != null)
            {
                classEditModel.CategoryLkp = categories;
            }

            //var classInfo = _commonService.GetClassInfoItem((Byte)Enums.DBAccess.ClassLoadType.ByClassEditDetail, id, memberId);
            var classInfo = _commonService.GetClassInfoItem((Byte)Enums.DBAccess.ClassLoadType.ByClassId, id, memberId);
            //class owner ,allow for edit
            if (classInfo != null && classInfo.Member_Id.Equals(memberId))
            {
                if (classInfo.IsProved.Equals(3))
                {
                    ViewBag.ActiveStep = 3;
                    classEditModel.ClassInfo = classInfo;
                }
                else
                {
                    ViewBag.Editable = true;
                    //get member's editable class
                    if (id.Equals(0))
                    {
                        id = classInfo.ClassId;
                    }
                    classEditModel.ClassInfo = classInfo;
                    int categoryId = classInfo.Category_Id;
                    if (categoryId > 0 && categories != null)
                    {
                        String subCateKey = String.Format(";{0},", categoryId);
                        var parentCategory = categories.Where(i => (i.SubCategories != null && i.SubCategories.Contains(subCateKey))).FirstOrDefault();
                        if (parentCategory != null)
                        {
                            classEditModel.ParentCategoryId = parentCategory.CateId;
                        }
                        else
                        {
                            classEditModel.ParentCategoryId = categoryId;
                        }
                        classEditModel.ParentCategoryName = categoryLkp.ContainsKey(classEditModel.ParentCategoryId) ? categoryLkp[classEditModel.ParentCategoryId].CategoryInfo.CategoryName : "";
                    }
                    ViewBag.ClassId = id;
                }

                ViewBag.CharacterCountText = ResourceHelper.GetTransText(283).Replace("{0}", ";").Split(';');
            }
            else
            {
                ViewBag.ErrorMessage = ResourceHelper.GetTransText(580);
            }
            GetNotificationNums(memberId);
            
            return View(classEditModel);
        }

        public ActionResult ProfileEdit()
        {
            ViewBag.ActiveTab = 3;
            var memberId = GetCurrentMemberInfo(true);
            String cityName = "";
            if (ViewBag.MemberInfo.CityId > 0)
            {
                var cityDic = _contentService.GetCities("cn");
                cityName = LookupHelper.GetCityNameById(cityDic, ViewBag.MemberInfo.CityId);
            }
            ViewBag.CityName = cityName;
            return View();
        }

        public ActionResult Learning()
        {
            ViewBag.ActiveTab = 2;
            ViewBag.AlterNums = null;
            var memberId = GetCurrentMemberInfo(true);

            var shouldCheckOrder = false;// CheckOrderHandlerDate("TOrderHandleDate", memberId);
            var orders = _commonService.GetOrderListByStudent(memberId, shouldCheckOrder);
            GetNotificationNums(memberId, (Byte)Enums.DBAccess.NotificationAlterLoadType.MobileLearn);

            return View(orders);
        }

        public ActionResult Teaching()
        {
            ViewBag.ActiveTab = 2;
            ViewBag.AlterNums = null;
            var memberId = GetCurrentMemberInfo(true);

            var shouldCheckOrder = false;// CheckOrderHandlerDate("TOrderHandleDate", memberId);

            var orders = _commonService.GetOrderListByTeacher(memberId, shouldCheckOrder);
            GetNotificationNums(memberId, (Byte)Enums.DBAccess.NotificationAlterLoadType.MobileTeach);

            return View(orders);
        }

        public ActionResult MyCourses()
        {
            ViewBag.ActiveTab = 2;
            ViewBag.AlterNums = null;
            var memberId = GetCurrentMemberInfo(true);

            var ClassEditList = _commonService.GetClassEditInfoByMemberId(memberId, (Byte)Enums.DBAccess.ClassLoadType.ByTeacherId);
            GetNotificationNums(memberId, (Byte)Enums.DBAccess.NotificationAlterLoadType.MobileMyCourse);

            ViewBag.IsMobileVerified = ((ViewBag.MemberInfo.VerifyTag & 1) == 1);
            
            return View(ClassEditList);
        }

        public ActionResult Dashboard()
        {
            ViewBag.ActiveTab = 3;
            
            var memberId = GetCurrentMemberInfo(true);
            var numDic = _commonService.GetNumsByMember(memberId, (Byte)Enums.DBAccess.MemberNumsLoadType.ByMemberDashboard);
            String cityName = "";
            if (ViewBag.MemberInfo.CityId > 0)
            {
                var cityDic = _contentService.GetCities("cn");
                cityName = LookupHelper.GetCityNameById(cityDic, ViewBag.MemberInfo.CityId);
            }
            ViewBag.ContactMobile = (ViewBag.MemberInfo.NotifyTag & 1).Equals(1) ? ViewBag.MemberInfo.Phone : "";
            ViewBag.CityName = cityName;
            GetNotificationNums(memberId);

            //if (numDic != null && (numDic[Enums.NumberDictionaryKey.GotSharedCoins].Equals(0) || numDic[Enums.NumberDictionaryKey.Certification].Equals(0) || numDic[Enums.NumberDictionaryKey.Class].Equals(0)))
            //{
            //    ViewBag.ShowHowToGetCoins = true;
            //}
            //else
            //{
            //    ViewBag.ShowHowToGetCoins = false;
            //}
            if (ViewBag.MemberInfo.LastUpdateDate.ToString("yyyy-MM-dd").Equals(DateTime.Now.ToString("yyyy-MM-dd")))
            {
                ViewBag.IsSignIn = true;
            }
            else
            {
                ViewBag.IsSignIn = false;
            }

            //WeChat share function paras
            String timestamp = WeChatHelper.GetTimestamp().ToString();
            String nonceStr = WeChatHelper.GetNonceStr();
            String CurrUrl = Request.Url.AbsoluteUri;
            String jsAPITicket = GetJsAPITicket(GetAccessToken());

            String str4SHA1 = WeChatHelper.GenerateString4Signature(jsAPITicket, nonceStr, timestamp, CurrUrl);
            String signature = FormsAuthentication.HashPasswordForStoringInConfigFile(str4SHA1, "SHA1").ToLower();


            ViewBag.AppId = ConfigConstants.ThirdPartySetting.WeChatSetting.AppID;
            ViewBag.TimeStamp = timestamp;
            ViewBag.NonceStr = nonceStr;
            ViewBag.Signature = signature;
            ViewBag.JsAPITicket = jsAPITicket;

            return View(numDic);
        }

        public ActionResult Verification()
        {
            var memberId = GetCurrentMemberInfo(true);
            return View();
        }

        public ActionResult CourseCollection(int id = 0, Byte type = (Byte)Enums.DBAccess.ClassCollectionLoadType.ByTeacherId)
        {
            ViewBag.Type = type;
            ViewBag.Title = type.Equals((Byte)Enums.DBAccess.ClassCollectionLoadType.ByTeacherId) ? "发布课程" : (type.Equals((Byte)Enums.DBAccess.ClassCollectionLoadType.ByMemberLearnt) ? "学习记录" : "教授记录");
            int memberId = GetCurrentMemberInfo(false);//current member
            id = id.Equals(0) ? memberId : id;//page owner

            if (id.Equals(0))//invalid member
            {
                return View();
            }
            else if (id.Equals(memberId))//own page
            {
                ViewBag.Avatar = ViewBag.MemberInfo.Avatar;
            }
            else
            {
                var ownerInfo = _commonService.GetMemberInfo(id);
                ViewBag.Avatar = ownerInfo.Avatar;
            }
            GetNotificationNums(memberId);//current member

            var cityDic = _contentService.GetCities("cn");
            var classes = _commonService.GetClassCollection(type, id);
            var classList = DataMapper.Map(classes, cityDic);

            return View(classList);
        }

        public ActionResult Credit()
        {
            var memberId = GetCurrentMemberInfo(true);
            return View();
        }

        public ActionResult AboutCredit()
        {
            var memberId = GetCurrentMemberInfo(true);

            var numDic = _commonService.GetNumsByMember(memberId, (Byte)Enums.DBAccess.MemberNumsLoadType.ByCreditGetMethods);
            
            ViewBag.MissStudentReview = numDic[Enums.NumberDictionaryKey.MissStudentReview];
            ViewBag.MissTeacherReview = numDic[Enums.NumberDictionaryKey.MissTeacherReview];
            ViewBag.IsSignIn = numDic[Enums.NumberDictionaryKey.IsSignIn];

            return View();
        }

        #endregion

    }
}
