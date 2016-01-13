using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SkillBank.Site.Common;
using SkillBank.Site.Services;
using SkillBank.Site.DataSource;
using SkillBank.Site.Web;
using SkillBank.Site.Web.ViewModel;
using SkillBank.Site.Web.Context;

namespace SkillBankWeb.Controllers
{
    public class NotificationHelperController : Controller
    {
        //
        // GET: /FeedbackHelper

        public readonly ICommonService _commonService;

        public NotificationHelperController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpPost]
        public JsonResult SetNotificationAsRead()
        {
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetNotificationAsPoped()
        {
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// PC site remove all order notification
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetOrderNotificationAsReadByMemberId()
        {
            var memberId = WebContext.Current.MemberId;
            _commonService.UpdateNotification((Byte)Enums.DBAccess.NotificationTagUpdateType.SetOrderNotificationAsReadByMemberId, memberId);
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// PC site remove single order notification by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetOrderNotificationAsReadByNotificationId(int id)
        {
            _commonService.UpdateNotification((Byte)Enums.DBAccess.NotificationTagUpdateType.SetNotificationAsReadById, 0, id);
            return Json("true", JsonRequestBehavior.AllowGet);
        }
    }
}