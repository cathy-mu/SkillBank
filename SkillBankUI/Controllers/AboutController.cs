using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SkillBank.Site.Services;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Common;

namespace SkillBankWeb.Controllers
{
    public class AboutController : Controller
    {
        //
        // GET: /About/
        public readonly ICommonService _commonService;

        public AboutController(ICommonService commonService)
        {
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

        public ActionResult AboutUs()
        {
            var metaTags = MetaTagHelper.GetMetaTags("aboutus");
            ViewBag.MetaTagTitle = metaTags[0];
            ViewBag.MetaTagKeyWords = metaTags[1];
            ViewBag.MetaTagDescription = metaTags[2];

            int memberId = WebContext.Current.MemberId;
            LoadNotificationAlert(memberId);
            ViewBag.MemberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;

            return View();
        }

        public ActionResult QAndA(int id = 0)
        {

            var metaTags = MetaTagHelper.GetMetaTags("qanda");
            ViewBag.MetaTagTitle = metaTags[0];
            ViewBag.MetaTagKeyWords = metaTags[1];
            ViewBag.MetaTagDescription = metaTags[2];
            ViewBag.ActiveTabId = id;

            int memberId = WebContext.Current.MemberId;
            LoadNotificationAlert(memberId);
            ViewBag.MemberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;

            return View();
        }

        public ActionResult Terms()
        {
            var metaTitle = MetaTagHelper.GetMetaTitle("terms");
            ViewBag.MetaTagTitle = metaTitle;

            int memberId = WebContext.Current.MemberId;
            LoadNotificationAlert(memberId);
            ViewBag.MemberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;

            return View();
        }
        
        public ActionResult Recruitment()
        {
            ViewBag.MetaTagTitle = "校园技能先锋招募";

            int memberId = WebContext.Current.MemberId;
            LoadNotificationAlert(memberId);
            ViewBag.MemberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;

            return View();
        }

        public void LoadNotificationAlert(int memberId)
        {
            if (memberId > 0)
            {
                Byte checkStatus = (Byte)((Session == null || Session["AlertStatus"] == null) ? Enums.DBAccess.NotificationAlterLoadType.WebCheckStatus : Enums.DBAccess.NotificationAlterLoadType.Web);
                var alerts = _commonService.GetPopNotification(memberId, checkStatus);
                var newAlertNum = "";
                if (alerts != null && alerts.Count() > 0)
                {
                    ViewBag.Notification = alerts;
                    newAlertNum = alerts.Count(i => i.PopNum > 0).ToString();
                }

                ViewBag.NotificationNum = newAlertNum; 
                if (checkStatus.Equals(1))
                {
                    Session["AlertStatus"] = "1";
                }
            }
        }

    }
}
