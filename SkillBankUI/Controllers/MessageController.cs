using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;

namespace SkillBankWeb.Controllers
{
    public class MessageController : Controller
    {
        //
        // GET: /Message/
        
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public MessageController(IContentService contentService, ICommonService commonService)
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

        public ActionResult Index()
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("message");

            var memberId = WebContext.Current.MemberId;
            ViewBag.MemberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;

            MessageListModel messageListModel = new MessageListModel();
            if (memberId > 0)
            {
                LoadNotificationAlert(memberId);
                var messages = _commonService.GetMessageList(memberId);
                messageListModel.Messages = messages;

                var unReadMessageNum = _commonService.GetMessageUnReadNum(memberId);
                messageListModel.UnReadMessageNumDic = unReadMessageNum;
            }
            return View(messageListModel);
        }

        public ActionResult Chat(int id = 0)
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("chat");

            int contactId = id;
            var memberId = WebContext.Current.MemberId;
            ViewBag.MemberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;

            MessageDetailModel messageDetailModel = new MessageDetailModel();
            if (memberId > 0 && memberId != contactId && contactId > 0)
            {
                LoadNotificationAlert(memberId);
                var messages = _commonService.GetMessageDetail(memberId, contactId);
                messageDetailModel.Messages = messages;
                messageDetailModel.Contact = _commonService.GetMemberInfo(contactId);
                messageDetailModel.ContactClass = _commonService.GetClassInfoByTeacherId(contactId, (Byte)Enums.DBAccess.ClassLoadType.ByTeacherPublished);
                messageDetailModel.MaxMessageId = (messages == null || messages.Count.Equals(0)) ? 0 : messageDetailModel.Messages.Max(m => m.MessageId);

                //TO DO : Move to client side with set max message id
                _commonService.SetMessageAsRead(0, memberId, contactId);
                return View(messageDetailModel);
            }
            else
            {
                return View();
            }
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
