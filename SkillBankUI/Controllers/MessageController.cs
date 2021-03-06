﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions; 

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
            if (IsMobile())
            {
                Response.Status = "301 Moved Permanently";
                Response.AddHeader("Location", Constants.PageURL.MobileMessagePage);
                Response.End();
            } 
            
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
                var contact =  _commonService.GetMemberInfo(contactId);
                messageDetailModel.Contact = contact;
                messageDetailModel.ContactClass = _commonService.GetClassInfo((Byte)Enums.DBAccess.ClassLoadType.ByTeacherPublished, 0, contactId);
                messageDetailModel.MaxMessageId = (messages == null || messages.Count.Equals(0)) ? 0 : messageDetailModel.Messages.Max(m => m.MessageId);
                ViewBag.ContactMobile = (contact.NotifyTag & 1).Equals(1) ? contact.Phone : "";//for send SMS notify
                    
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
                Byte checkStatus = (Byte)((Session == null || Session["AlertStatus"] == null) ? Enums.DBAccess.NotificationAlterLoadType.WebCheckStatus: Enums.DBAccess.NotificationAlterLoadType.Web);
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
        
    }
}
