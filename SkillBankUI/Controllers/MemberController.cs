using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Web;
using SkillBank.Site.Common;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;

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
        /// Dashboard
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
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
                dashboardModel.HasBasicInfo = (memberInfo.CityId>0);
                dashboardModel.HasClass = (dashboardModel.ClassEditList != null && dashboardModel.ClassEditList.Count > 0);
                
                var ShareText = ResourceHelper.GetTransText(606).Replace("{0}", ConfigConstants.ThirdPartySetting.SocialNetwork.SkillBankAccountName[memberInfo.SocialType]).Replace("{1}", ";").Split(';');
                ViewBag.ShareText1 = ShareText[0];
                ViewBag.ShareText2 = ShareText.Length > 1 ? ShareText[1] : "";
                ViewBag.IsSocialLogin = CheckLoginStatus(true, true) ? 1 : 0;

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

                var ShareText = ResourceHelper.GetTransText(606).Replace("{0}", ConfigConstants.ThirdPartySetting.SocialNetwork.SkillBankAccountName[memberInfo.SocialType]).Replace("{1}", ";").Split(';');
                ViewBag.ShareText1 = ShareText[0];
                ViewBag.ShareText2 = ShareText.Length > 1 ? ShareText[1] : "";
                ViewBag.IsSocialLogin = CheckLoginStatus(true, true) ? 1 : 0;
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

                ViewBag.IsMobileVerified = _commonService.CheckIsMobileVerified(memberId).Equals(1);
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


    }
}
