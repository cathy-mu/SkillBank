using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;
using SkillBank.Site.Services.Net.Mail;
using SkillBank.Site.Services.Net.SMS;

namespace SkillBankWeb.Controllers.API
{
    public class OrderController : ApiController
    {
        public readonly ICommonService _commonService;

        public class OrderItem
        {
            public int ClassId { get; set; }
            public DateTime BookDate { get; set; }
            public String Remark { get; set; }
            public String Name { get; set; }
            public String Phone { get; set; }
            public String ClassName { get; set; }
            public String TeacherPhone { get; set; }
            public String TeacherMail { get; set; }
            public String TeacherName { get; set; }
        }

        public class OrderUpdateItem
        {
            public int OrderId { get; set; }
            public Byte Status { get; set; }//*
            public int MemberId { get; set; }//*
            public String Title { get; set; }//*
            public String Phone { get; set; }
            public String Name { get; set; }
            public String MyName { get; set; }
            public String MyPhone { get; set; }
            public String Email { get; set; }
            public Byte FeedBack { get; set; }
            public String Comment { get; set; }
        }

        //
        // GET: /Message/

        public OrderController(ICommonService commonService)
        {
            _commonService = commonService;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns>-1 error  0 init  2 not enough coin(payment order)/status change(stauts order) 3 status change </returns>
        public Byte Put(int id, OrderUpdateItem item)
        {
            Byte result = 0;
            try
            {
                int memberId = APIHelper.GetMemberId(true);
                MemberInfo contactInfo = _commonService.GetMemberInfo(item.MemberId);
                var isMobileVerified = (contactInfo.VerifyTag & 1).Equals(1);
                item.OrderId = id;
                int teacherId;
                int studentId;
                item.Comment = String.IsNullOrEmpty(item.Comment) ? "" : item.Comment;

                switch (item.Status)
                {
                    case (Byte)Enums.OrderStatus.Rejected://2 reject,T
                        teacherId = memberId;
                        studentId = item.MemberId;
                        result = _commonService.UpdateOrderStatus(item.OrderId, item.Status, studentId, teacherId);
                        NotifyOrderStatusUpdate(item.Status, item.Phone, item.Email, item.Title, item.Name, Constants.PageURL.MobileLearnPage);
                        break;
                    case (Byte)Enums.OrderStatus.Cancled://3 cancle,S
                        teacherId = item.MemberId;
                        studentId = memberId;
                        result = _commonService.UpdateOrderStatus(item.OrderId, item.Status, studentId, teacherId);
                        NotifyOrderStatusUpdate(item.Status, item.Phone, item.Email, item.Title, item.Name, Constants.PageURL.MobileTeachPage);
                        break;
                    case (Byte)Enums.OrderStatus.Accepted://4 accpet order,T
                        teacherId = memberId;
                        studentId = item.MemberId;
                        item.Name = String.IsNullOrEmpty(item.Name) ? "" : item.Name;
                        item.Phone = String.IsNullOrEmpty(item.Phone) ? "" : item.Phone;
                        result = _commonService.AcceptOrder(item.OrderId, studentId, teacherId, item.MyName, item.MyPhone);
                        NotifyOrderStatusUpdate(item.Status, item.Phone, item.Email, item.Title, item.Name, Constants.PageURL.MobileLearnPage);
                        break;
                    case (Byte)Enums.OrderStatus.Refund://6 refund
                        teacherId = item.MemberId;
                        studentId = memberId;
                        result = _commonService.UpdateOrderStatus(item.OrderId, item.Status, studentId, teacherId);
                        NotifyOrderStatusUpdate(item.Status, item.Phone, item.Email, item.Title, item.Name, Constants.PageURL.MobileTeachPage);
                        break;
                    case (Byte)Enums.OrderStatus.RefundProve:// 7 RefundProve
                        teacherId = memberId;
                        studentId = item.MemberId;
                        result = _commonService.UpdateOrderStatusWithCoins(item.OrderId, item.Status, studentId, teacherId);
                        NotifyOrderStatusUpdate(item.Status, item.Phone, item.Email, item.Title, item.Name, Constants.PageURL.MobileLearnPage);
                        break;
                    case (Byte)Enums.OrderStatus.RefundReject://reject,8
                        teacherId = memberId;
                        studentId = item.MemberId;
                        result = _commonService.UpdateOrderStatus(item.OrderId, item.Status, studentId, teacherId);
                        NotifyOrderStatusUpdate(item.Status, item.Phone, item.Email, item.Title, item.Name, Constants.PageURL.MobileLearnPage);
                        break;
                    case (Byte)Enums.OrderStatus.Confirmed://9 confirm
                        teacherId = item.MemberId;
                        studentId = memberId;
                        result = _commonService.UpdateOrderStatusWithCoins(item.OrderId, item.Status, studentId, teacherId);
                        _commonService.AddStudentReview(item.OrderId, 0, item.FeedBack, item.Comment, "");//leave classid as 0 and get id in sp
                        NotifyOrderStatusUpdate(item.Status, item.Phone, item.Email, item.Title, item.Name, Constants.PageURL.MobileTeachPage);
                        break;
                    default://5 finish 10 autoconfirm 11://autorefund
                        result = 0;
                        break;
                }

            }
            catch
            { }
            return result;
        }

        public Boolean Post(OrderItem item)
        {
            Boolean result = false;
            try
            {
                int memberId = APIHelper.GetMemberId(true);
                item.Remark = String.IsNullOrEmpty(item.Remark) ? "" : item.Remark;
                item.Name = String.IsNullOrEmpty(item.Name) ? "" : item.Name;
                item.Phone = String.IsNullOrEmpty(item.Phone) ? "" : item.Phone;

                result = _commonService.AddOrder(memberId, item.ClassId, item.BookDate, item.Remark, item.Name, item.Phone);
                if (result)
                {
                    NotifyOrderStatusUpdate((Byte)Enums.OrderStatus.Booked, item.TeacherPhone, item.TeacherMail, item.ClassName, item.TeacherName, Constants.PageURL.MobileTeachPage);
                }
            }
            catch
            {
                return result;
            }
            return result;
        }

        #region Non-Public Methods

        private void NotifyOrderStatusUpdate(Byte orderStatus, String mobile, String email, String className, String receiverName, String pageUrl)
        {
            //validate mobile later
            if (!String.IsNullOrEmpty(mobile))
            {
                _commonService.SendOrderUpdateSMS(orderStatus, mobile, className, pageUrl, true);
            }
            //Validate email later
            else if (!String.IsNullOrEmpty(email))
            {
                SendOrderStatusUpdateEmail(orderStatus, email, receiverName, className);
            }
        }

        private void SendOrderStatusUpdateEmail(Byte orderStatus, String email, String receiverName, String className)
        {
            if (!String.IsNullOrEmpty(email))
            {
                String mailText = default(String);
                String pageUrl = default(String);
                String linkText = default(String);

                switch (orderStatus)
                {
                    case (Byte)Enums.OrderStatus.Booked://1 book,T
                        mailText = TextContentHelper.ReplaeceBlurbParameterWithText(626, className);
                        linkText = ResourceHelper.GetTransText(478);
                        pageUrl = Constants.PageURL.MemberTeachPage;
                        break;
                    case (Byte)Enums.OrderStatus.Rejected://2 reject,T
                        mailText = TextContentHelper.ReplaeceBlurbParameterWithText(628, className);
                        linkText = ResourceHelper.GetTransText(480);
                        pageUrl = Constants.PageURL.MemberLearnPage;
                        break;
                    case (Byte)Enums.OrderStatus.Cancled://3 cancle,S
                        mailText = TextContentHelper.ReplaeceBlurbParameterWithText(627, className);
                        pageUrl = Constants.PageURL.MemberTeachPage;
                        linkText = ResourceHelper.GetTransText(480);
                        break;
                    case (Byte)Enums.OrderStatus.Accepted://4 accpet order,T
                        mailText = TextContentHelper.ReplaeceBlurbParameterWithText(629, className);
                        pageUrl = Constants.PageURL.MemberLearnPage;
                        linkText = ResourceHelper.GetTransText(479);
                        break;
                    case (Byte)Enums.OrderStatus.Refund://6 refund
                        mailText = TextContentHelper.ReplaeceBlurbParameterWithText(630, className);
                        pageUrl = Constants.PageURL.MemberTeachPage;
                        linkText = ResourceHelper.GetTransText(481);
                        break;
                    case (Byte)Enums.OrderStatus.RefundProve:// 7 RefundProve
                        mailText = TextContentHelper.ReplaeceBlurbParameterWithText(631, className);
                        pageUrl = Constants.PageURL.MemberLearnPage;
                        linkText = ResourceHelper.GetTransText(480);
                        break;
                    case (Byte)Enums.OrderStatus.RefundReject://reject,8
                        mailText = TextContentHelper.ReplaeceBlurbParameterWithText(633, className);
                        pageUrl = Constants.PageURL.MemberLearnPage;
                        linkText = ResourceHelper.GetTransText(480);
                        break;
                    case (Byte)Enums.OrderStatus.Confirmed://9 confirm
                        mailText = TextContentHelper.ReplaeceBlurbParameterWithText(632, className);
                        pageUrl = Constants.PageURL.MemberTeachPage;
                        linkText = ResourceHelper.GetTransText(480);
                        break;
                    default://5: finish //10 :autoconfirm //11:autorefund
                        break;
                }

                if (!String.IsNullOrEmpty(mailText))
                {
                   SendCloudEmail.SendOrderStatusUpdateMail(email, receiverName, mailText, pageUrl, linkText);
                }
            }

        }

        #endregion
    }
}
