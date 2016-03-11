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
    public class OrderHelperController : Controller
    {
        //
        // GET: /ClassHelper/

        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public OrderHelperController(IContentService contentService, ICommonService commonService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        /// <summary>
        /// Add new order  -->1
        /// </summary>
        /// <param name="toId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddOrder(int classid, DateTime bookdate, String remark, String mailaddr = "", String mailname = "", String title = "", String mobile = "", String name = "", String phone = "", String email = "")
        {
            int studentId = WebContext.Current.MemberId;
            String deviceToken;
            var result = _commonService.AddOrder(out deviceToken, studentId, classid, bookdate, remark, name, phone, email);

            OrderNotificationHelper.NotifyOrderStatusUpdate((Byte)Enums.OrderStatus.Booked, mobile, mailaddr, title, mailname);


            return Json("true", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Status 1-->4
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AcceptOrder(int orderid, int studentid, String mailaddr = "", String mailname = "", String title = "", String mobile = "", String name = "", String phone = "", String email = "")
        {
            int teacherId = WebContext.Current.MemberId;
            String deviceToken;
            var result = _commonService.AcceptOrder(out deviceToken, orderid, studentid, teacherId, name, phone, email);
            OrderNotificationHelper.NotifyOrderStatusUpdate((Byte)Enums.OrderStatus.Accepted, mobile, mailaddr, title, mailname);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConfirmOrder(int orderid, int teacherid, int classid, Byte feedback, String comment = "", String mailaddr = "", String mailname = "", String title = "", String mobile = "")
        {
            String deviceToken;
            String phone;
            Byte status = (Byte)Enums.OrderStatus.Confirmed;
            int studentId = WebContext.Current.MemberId;
            var result = _commonService.UpdateOrderStatusWithCoins(out deviceToken, orderid, status, studentId, teacherid);
            _commonService.AddStudentReview(orderid, classid, feedback, comment, out deviceToken, out phone);

            OrderNotificationHelper.NotifyOrderStatusUpdate(status, mobile, mailaddr, title, mailname);

            return Json("true", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Status 1->2 , 1->3, 4->5, 4->6, 6->8
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="status"></param>
        /// <param name="sid"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateOrderStatus(int orderid, Byte status, String mailaddr = "", String mailname = "", String title = "", String mobile = "", int student = 0)
        {
            int teacher = 0;
            String deviceToken;
            if (status.Equals((Byte)Enums.OrderStatus.Rejected) || status.Equals((Byte)Enums.OrderStatus.RefundReject))//teacher reject
            {   //teacher id instead
                teacher = WebContext.Current.MemberId;
            }
            else if (status.Equals(6))//student request
            {
                student = WebContext.Current.MemberId;
            }
            var result = _commonService.UpdateOrderStatus(out deviceToken, orderid, status, student, teacher);

            OrderNotificationHelper.NotifyOrderStatusUpdate(status, mobile, mailaddr, title, mailname);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //TO DO:6->7, 6-11
        [HttpPost]
        public JsonResult RefundProve(int orderid, int studentid, String mailaddr = "", String mailname = "", String title = "", String mobile = "")
        {
            int teacherId = WebContext.Current.MemberId;
            String deviceToken;
            var result = _commonService.UpdateOrderStatusWithCoins(out deviceToken, orderid, (Byte)Enums.OrderStatus.RefundProve, studentid, teacherId);

            OrderNotificationHelper.NotifyOrderStatusUpdate((Byte)Enums.OrderStatus.RefundProve, mobile, mailaddr, title, mailname);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}