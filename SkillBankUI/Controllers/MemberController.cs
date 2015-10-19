using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions; 

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Web;
using SkillBank.Site.Common;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;
using dotNetDR_OAuth2.APIs.WeChat;
using SkillBank.Site.Services.Providers;

namespace SkillBankWeb.Controllers
{
    public class MemberController : Controller
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public MemberController(IContentService contentService, ICommonService commonService)
        {
            _contentService = contentService;
            _commonService = commonService;

            int memberId = WebContext.Current.MemberId;
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

        /// <summary>
        /// Student's Booked Class and BookedOrder
        /// </summary>
        /// <returns></returns>
        public ActionResult Credit()
        {
            int memberId = WebContext.Current.MemberId;
            var memberInfo = (memberId > 0) ? _commonService.GetMemberInfo(memberId) : null;
            ViewBag.MemberInfo = memberInfo;
            ViewBag.ShowSignUp = true;
           
            if (memberInfo.LastUpdateDate.ToString("yyyy-MM-dd").Equals(DateTime.Now.ToString("yyyy-MM-dd")))
            {
                ViewBag.ShowSignUp = false;
            }
    
            return View();

        }

        /// <summary>
        /// Dashboard
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (IsMobile())
            {
                Response.Redirect(Constants.PageURL.MobileDashboard);
            }
            
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("dashboard");

            var memberId = WebContext.Current.MemberId;
            var memberInfo = (memberId > 0) ? _commonService.GetMemberInfo(memberId) : null;
            ViewBag.MemberInfo = memberInfo;
            int unreadMessageNum = 0;

            DashboardModel dashboardModel = new DashboardModel();

            if (memberId > 0 && memberInfo != null)
            {
                LoadNotificationAlert(memberId);
                var notification = LoadNotification(memberId);
                if (notification != null)
                {
                    var classNotifications = notification.Where(i => i.TypeId < 10);
                    dashboardModel.ClassNotifications = (classNotifications == null ? null : classNotifications.ToList<NotificationItem>());

                    var orderNotifications = notification.Where(i => i.TypeId > 9 && i.TypeId < 20);
                    dashboardModel.OrderNotifications = (orderNotifications == null ? null : orderNotifications.ToList<NotificationItem>());
                }
                dashboardModel.ClassEditList = _commonService.GetClassEditInfoByMemberId(memberId, (Byte)Enums.DBAccess.ClassLoadType.ByTeacherDashboard);

                var messages = _commonService.GetMessageList(memberId, Constants.ContentPageSize.DashboardMessage);
                dashboardModel.Messages = messages;

                var unReadMessageNumDic = _commonService.GetMessageUnReadNum(memberId);
                if (unReadMessageNumDic != null && unReadMessageNumDic.Count > 0)
                {
                    unreadMessageNum = unReadMessageNumDic.Values.Sum();
                    dashboardModel.UnReadMessageNumDic = unReadMessageNumDic;
                }
                else
                {
                    dashboardModel.UnReadMessageNumDic = null;
                }
                ViewBag.MessageTitle = ResourceHelper.GetTransText(405).Replace("{0}", unreadMessageNum.ToString());
                ViewBag.IsMobileVerified = (memberInfo.VerifyTag & 1).Equals(1);
                dashboardModel.HasBasicInfo = (memberInfo.CityId > 0);
                dashboardModel.HasClass = (dashboardModel.ClassEditList != null && dashboardModel.ClassEditList.Count > 0);

                ViewBag.ShowQRCode = false;
                if (memberInfo.SocialType.Equals(1))
                {
                    if (memberInfo.SocialAccount.Equals(memberInfo.OpenId))
                    {
                        ViewBag.ShowQRCode = true;
                        String scenceId = String.Concat("1", memberId.ToString());
                        String accessTonken = GetAccessToken();
                        ViewBag.AccessToken = accessTonken;
                        String postJsonData = "{\"expire_seconds\": 604800, \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\":" + scenceId + "}}}";
                        String ticket = "";
                        String QRCodeUrl = WeChatHelper.GetQRCode(accessTonken, postJsonData, ref ticket);

                        ViewBag.QRCodeURL = QRCodeUrl;
                        if (memberId > 0)
                        {
                            var result = _commonService.SaveWeChatEvent((Byte)Enums.DBAccess.WeChatEventSaveType.AddEvent, memberId, "", scenceId, ticket);
                        }
                    }

                    var ShareText = ResourceHelper.GetTransText(606).Replace("{0}", ConfigConstants.ThirdPartySetting.SocialNetwork.SkillBankAccountName[memberInfo.SocialType]).Replace("{1}", ";").Split(';');
                    ViewBag.ShareText1 = ShareText[0];
                    ViewBag.ShareText2 = ShareText.Length > 1 ? ShareText[1] : "";
                    ViewBag.IsSocialLogin = CheckLoginStatus(true, true) ? 1 : 0;
                }
                

                return View(dashboardModel);
            }
            else
            {
                return View();
            }
        }
        
        /// <summary>
        /// Student's Booked Class and BookedOrder
        /// </summary>
        /// <returns></returns>
        public ActionResult Learn()
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("learn");

            int memberId = WebContext.Current.MemberId;
            var memberInfo =  (memberId > 0) ? _commonService.GetMemberInfo(memberId) : null;
            ViewBag.MemberInfo = memberInfo;

            var shouldCheckOrder = false;//CheckOrderHandlerDate("SOrderHandleDate", memberId);

            if (memberId > 0 && memberInfo != null)
            {
                LoadNotificationAlert(memberId);
                MemberLearnModel learnModel = new MemberLearnModel();
                //learnModel.ClassList = _commonService.GetMemberClass(memberId);
                learnModel.Orders = _commonService.GetOrderListByStudent(memberId, shouldCheckOrder);
                return View(learnModel);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Teacher's published Class and request Order
        /// </summary>
        /// <returns></returns>
        public ActionResult Teach()
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("teach");
            int memberId = WebContext.Current.MemberId;
            var memberInfo = (memberId > 0) ? _commonService.GetMemberInfo(memberId) : null;
            ViewBag.MemberInfo = memberInfo;

            if (memberId > 0 && memberInfo != null)
            {

                LoadNotificationAlert(memberId);
                var shouldCheckOrder = false;// CheckOrderHandlerDate("TOrderHandleDate", memberId);

                MemberTeachModel teachModel = new MemberTeachModel();
                teachModel.ClassEditList = _commonService.GetClassEditInfoByMemberId(memberId, (Byte)Enums.DBAccess.ClassLoadType.ByTeacherId);
                teachModel.Orders = _commonService.GetOrderListByTeacher(memberId, shouldCheckOrder);
                teachModel.IsMemberInfoCompleted = (memberInfo.CityId>0);
                
                if (ConfigConstants.ThirdPartySetting.SocialNetwork.SkillBankAccountName.ContainsKey(memberInfo.SocialType))
                {
                    var ShareText = ResourceHelper.GetTransText(606).Replace("{0}", ConfigConstants.ThirdPartySetting.SocialNetwork.SkillBankAccountName[memberInfo.SocialType]).Replace("{1}", ";").Split(';');
                    ViewBag.ShareText1 = ShareText[0];
                    ViewBag.ShareText2 = ShareText.Length > 1 ? ShareText[1] : "";
                    ViewBag.IsSocialLogin = CheckLoginStatus(true, true) ? 1 : 0;
                }
                return View(teachModel);
            }
            else
            {
                return View();
            }
        }
        
        public ActionResult Info()
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("memberinfo");
            
            int memberId = WebContext.Current.MemberId;
            LoadNotificationAlert(memberId);
            ViewBag.MemberInfo = (memberId > 0) ? _commonService.GetMemberInfo(memberId) : null;
            
            return View();
        }
        
        public ActionResult Photo()
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("memberphoto");

            var memberId = WebContext.Current.MemberId;
            LoadNotificationAlert(memberId);
            ViewBag.MemberInfo = (memberId > 0) ? _commonService.GetMemberInfo(memberId) : null;

            return View();
        }
        
        public ActionResult Location()
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("memberlocation");
            
            var memberId = WebContext.Current.MemberId;
            LoadNotificationAlert(memberId);
            ViewBag.MemberInfo = (memberId > 0) ? _commonService.GetMemberInfo(memberId) : null;
                        
            return View();
        }

        public ActionResult Verification()
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("memberverification");
            
            var memberId = WebContext.Current.MemberId;
            if (memberId > 0)
            {
                LoadNotificationAlert(memberId);
                var memberInfo = (memberId > 0) ? _commonService.GetMemberInfo(memberId) : null;
                ViewBag.MemberInfo = memberInfo;

                var socialName = TextContentHelper.GetSocialName(memberInfo.SocialType);
                ViewBag.SocialName = socialName;
                ViewBag.SocialInfo = ResourceHelper.GetTransText(434).Replace("{0}", socialName);

                ViewBag.IsMobileVerified = (memberInfo.VerifyTag & 1).Equals(1);
                //_commonService.CheckIsMobileVerified(memberId).Equals(1);
            }
            return View();
        }


        private Boolean CheckOrderHandlerDate(String cookieName, int memberId)
        {
            Boolean shouldCheckOrder = false;
            
            var dateCookie = Request.Cookies[cookieName];
            String dateKey = String.Format("{0}_{1}", DateTime.Now.ToString("yy-MM-dd"), memberId);
            if (dateCookie == null || String.IsNullOrEmpty(dateCookie.Value))
            {
                HttpCookie cookies = new HttpCookie(cookieName, dateKey);
                cookies.Expires = DateTime.Now.AddYears(1);
                this.Response.SetCookie(cookies);
                shouldCheckOrder = true;
            }
            else if (!String.Equals(dateCookie.Value, dateKey))
            {
                this.Response.Cookies[cookieName].Value = dateKey;
                shouldCheckOrder = true;
            }

            return shouldCheckOrder;
        }

        /// <summary>
        /// Not Action , just for check memer login Status
        /// </summary>
        /// <param name="shouldCheckSocial"></param>
        /// <param name="checkShareCoin"></param>
        /// <returns></returns>
        public Boolean CheckLoginStatus(Boolean shouldCheckSocial, Boolean checkShareCoin)
        {
            int memberId = WebContext.Current.MemberId;
            Boolean hasLoginInfo = false;
            if (memberId > 0)
            {
                LoadNotificationAlert(memberId);
                if (checkShareCoin)
                {
                    Boolean gotCoinsBefore = _commonService.HasShareClassCoin(memberId);
                    if (gotCoinsBefore)
                    {
                        return true;
                    }
                }

                //if not got coin before or just check social login
                if (shouldCheckSocial)
                {
                    var socialType = WebContext.Current.SocialType;
                    var socialAccount = WebContext.Current.SocialAccount;
                    var socialInfo = WebContext.Current.SocialAccessInfo;
                    //No social login info on live ENV
                    if ((socialType <= 0 || String.IsNullOrEmpty(socialAccount) || String.IsNullOrEmpty(socialInfo)) && System.Configuration.ConfigurationManager.AppSettings["ENV"].Equals(ConfigConstants.EnvSetting.LiveEnvName))
                    {
                        return false;
                    }
                    else//TO DO : try check if token is valid
                    //TO DO : Update for other social after roll out 
                    {
                        hasLoginInfo = true;
                    }
                }

            }
            return hasLoginInfo;
        }

        public void LoadNotificationAlert(int memberId)
        {
            if (memberId > 0)
            {
                Byte checkStatus = (Byte)((Session == null || Session["AlertStatus"] == null) ? Enums.DBAccess.NotificationAlterLoadType.WebCheckStatus : Enums.DBAccess.NotificationAlterLoadType.Web);
                var alerts = _commonService.GetPopNotification(memberId, checkStatus);
                if (alerts != null && alerts.Count() > 0)
                {
                    ViewBag.Notification = alerts;
                    var newAlertNum = alerts.Count(i => i.PopNum > 0);
                    ViewBag.NotificationNum = newAlertNum > 0 ? newAlertNum.ToString() : "";
                }

                if (checkStatus.Equals(1))
                {
                    Session["AlertStatus"] = "1";
                }
            }
        }

        //For member index / profile page
        public List<NotificationItem> LoadNotification(int memberId)
        {
            if (memberId > 0)
            {
                Byte checkStatus = (Session == null || Session["AlertStatus"] == null) ? (Byte)1 : (Byte)0;
                var notifications = _commonService.GetNotification(memberId, checkStatus);
                if (checkStatus.Equals(1))
                {
                    Session["SysInfoStatus"] = "1";
                }
                if (notifications != null && notifications.Count > 0)
                {
                    return notifications;
                }
            }
            return null;
        }


        private Boolean IsMobile()
        {
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            if (b.IsMatch(u) || v.IsMatch(u.Substring(0, 4)))
            {
                return true;
            }
            return false;
        }


        private string GetAccessToken()
        {
            SocialCacheItem cachedToken;
            var accessToken = StaticCacheProvider.GetObject(Constants.CacheDicKeys.WeChatAccessToken);
            if (System.Configuration.ConfigurationManager.AppSettings["ENV"].Equals(ConfigConstants.EnvSetting.LiveEnvName))
            {
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
            return "";
        }


    }
}
