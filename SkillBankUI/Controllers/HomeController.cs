using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SkillBank.Site.Services;
using SkillBank.Site.DataSource;
using SkillBank.Site.Web;
using SkillBank.Site.Common;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.Controllers
{
    public class HomeController : Controller
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public HomeController(IContentService contentService, ICommonService commonService)
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

        public ActionResult Intro()
        {
            

            int memberId = WebContext.Current.MemberId;
            LoadNotificationAlert(memberId);
            var metaTags = MetaTagHelper.GetMetaTags("home");
            ViewBag.MetaTagTitle = metaTags[0];
            ViewBag.MetaTagKeyWords = metaTags[1];
            ViewBag.MetaTagDescription = metaTags[2];

            ViewBag.MemberId = memberId;
            ViewBag.MemberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;

            return View();
        }

        public ActionResult Index()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["ENV"].Equals(ConfigConstants.EnvSetting.Web1EnvName))
            {
                Response.Status = "301 Moved Permanently";
                Response.AddHeader("Location", "http://www.skillbank.cn");
                Response.End();
            }
            
            HomePageModel homePageModel = new HomePageModel();
            
            int memberId = WebContext.Current.MemberId;
            LoadNotificationAlert(memberId);
            var metaTags = MetaTagHelper.GetMetaTags("home");
            ViewBag.MetaTagTitle = metaTags[0];
            ViewBag.MetaTagKeyWords = metaTags[1];
            ViewBag.MetaTagDescription = metaTags[2];

            ViewBag.MemberId = memberId;
            ViewBag.MemberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;


            var result = _commonService.GetRecommendationClassPopList(memberId);
            if (result != null && result.Count > 0)
            {
                homePageModel.MasterClassList = result.Take(4).ToList();
                homePageModel.LatestClassList = (result.Count > 4) ? result.Skip(4).Take(4).ToList() : null;
                homePageModel.OddClassList = (result.Count > 8) ? result.Skip(8).Take(4).ToList() : null;
            }

            return View(homePageModel);
        }
        
        public void LoadNotificationAlert(int memberId)
        {
            if (memberId > 0)
            {
                Boolean checkStatus = (Session == null || Session["AlertStatus"] == null);
                var alerts = _commonService.GetPopNotification(memberId, checkStatus);
                if (alerts != null && alerts.Count() > 0)
                {
                    ViewBag.Notification = alerts;
                    var newAlertNum = alerts.Count(i => i.PopNum > 0);
                    ViewBag.NotificationNum = newAlertNum > 0 ? newAlertNum.ToString() : "";
                }
                
                if (checkStatus)
                {
                    Session["AlertStatus"] = "1";
                }
            }
        }

    }
}
