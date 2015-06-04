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
using SkillBank.Site.Services.Net.Mail;


namespace SkillBankWeb.Controllers
{
    public class MessageHelperController : Controller
    {
        //
        // GET: /ClassHelper/

        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public MessageHelperController(IContentService contentService, ICommonService commonService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        /// <summary>
        /// Add new message to another member
        /// </summary>
        /// <param name="toId"></param>
        /// <param name="message"></param>
        /// <param name="classTitle">Provide Class title if it's from class page</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddMessage(int to, String message, String msname, String mremail, String mrname, String title = "", String mobile = "")
        {
            var fromId = WebContext.Current.MemberId;
            if (!String.IsNullOrEmpty(title))
            {
                message = String.Format("{0}      [{1}]", message, title);
            }
            var result = _commonService.AddMessage(fromId, to, message);
            if (result.Equals(2) && !String.IsNullOrEmpty(mobile))
            {
                _commonService.SendNewMessageSMS(mobile, mrname, Constants.PageURL.MobileMessagePage, true);
            }
            else if (!String.IsNullOrEmpty(mremail))
            {
                SendCloudEmail.SendMessageReceiveMail(mremail, mrname, msname, Constants.PageURL.MessagePage);
            }
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Set all message with another member as read
        /// </summary>
        /// <param name="maxId">Last message id when member load message page</param>
        /// <param name="toId">Another member id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetMessageAsRead(int maxId, int contactorId)
        {
            var memberId = WebContext.Current.MemberId;
            _commonService.SetMessageAsRead(maxId, memberId, contactorId);
            return Json("true", JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxId"></param>
        /// <param name="fromId"></param>
        /// <param name="toId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetMessageAsDeleted(int maxId, int contactorId)
        {
            var memberId = WebContext.Current.MemberId;
            _commonService.SetMessageAsDeleted(maxId, memberId, contactorId);
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetAlertAsClicked()
        {
            var memberId = WebContext.Current.MemberId;
            _commonService.UpdateNotification((Byte)Enums.DBAccess.NotificationTagUpdateType.SetPopTagAsClicked, memberId);
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetAlertAsRead()
        {
            var memberId = WebContext.Current.MemberId;
            _commonService.UpdateNotification((Byte)Enums.DBAccess.NotificationTagUpdateType.SetPopTagAsRead, memberId);
            return Json("true", JsonRequestBehavior.AllowGet);
        }


    }
}