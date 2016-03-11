using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;
using SkillBank.Site.Services.Net.Mail;
using SkillBank.Site.Services.Net.SMS;
using SkillBank.Site.Services.Utility;

namespace SkillBankWeb.Controllers.API
{
    public class OrderController : ApiController
    {
        public readonly ICommonService _commonService;

        public class OrderItem
        {
            public int MemberId { get; set; }
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
            public Byte Status { get; set; } 
            public int MemberId { get; set; } 
            public int MyId { get; set; }  
            public String Title { get; set; } 
            public String Name { get; set; }
            public String Phone { get; set; }//only for memeber without phone to fill phone number
            public String MyName { get; set; }//update teacher info when accept order
            public String MyPhone { get; set; }//update teacher info when accept order
            public String Email { get; set; }
            public Byte FeedBack { get; set; }
            public String Comment { get; set; }
            public DateTime BookDate { get; set; }
        }

        //
        // GET: /Message/

        public OrderController(ICommonService commonService)
        {
            _commonService = commonService;

        }

        public string Options()
        {
            return null; // HTTP 200 response with empty body 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns>-1 error  0 init  2 not enough coin(payment order)/status change(stauts order) 3 status change </returns>
        [HttpPut]
        public Byte Put(int id, OrderUpdateItem item)
        {
            Byte result = 0;
            Boolean isWeb = item.MyId.Equals(0);
            item.OrderId = id;

            try
            {
                if (item.Status.Equals((Byte)Enums.OrderStatus.ChangeBookDate))
                {
                    result = (item.BookDate < DateTime.Now.Date) ? (Byte)2 : _commonService.UpdateOrderDate(item.OrderId, item.BookDate);
                }
                else
                {
                    int memberId = isWeb ? APIHelper.GetMemberId(true) : item.MyId;
                    MemberInfo contactInfo = _commonService.GetMemberInfo(item.MemberId);
                    var isMobileVerified = (contactInfo.VerifyTag & 1).Equals(1);
                    int teacherId;
                    int studentId;
                    String deviceToken = "";
                    item.Comment = String.IsNullOrEmpty(item.Comment) ? "" : item.Comment;

                    switch (item.Status)
                    {
                        case (Byte)Enums.OrderStatus.Rejected://2 reject,T
                            teacherId = memberId;
                            studentId = item.MemberId;
                            result = _commonService.UpdateOrderStatus(out deviceToken, item.OrderId, item.Status, studentId, teacherId);
                            if (result.Equals(1))
                            {
                                OrderNotificationHelper.NotifyOrderStatusUpdate(item.Status, item.Phone, item.Email, item.Title, item.Name);
                                PushManager.PushNotification(deviceToken, (Byte)Enums.PushNotificationType.BookRejected);
                            }
                            break;
                        case (Byte)Enums.OrderStatus.Cancled://3 cancle,S
                            teacherId = item.MemberId;
                            studentId = memberId;
                            result = _commonService.UpdateOrderStatus(out deviceToken, item.OrderId, item.Status, studentId, teacherId);
                            break;
                        case (Byte)Enums.OrderStatus.Accepted://4 accpet order,T
                            teacherId = memberId;
                            studentId = item.MemberId;
                            //update own contact info when teacher accept order
                            result = _commonService.AcceptOrder(out deviceToken, item.OrderId, studentId, teacherId, item.MyName, item.MyPhone);
                            if (result.Equals(1))
                            {
                                OrderNotificationHelper.NotifyOrderStatusUpdate(item.Status, item.Phone, item.Email, item.Title, item.Name);
                                PushManager.PushNotification(deviceToken, (Byte)Enums.PushNotificationType.BookAccepted);
                            }
                            break;
                        case (Byte)Enums.OrderStatus.Refund://6 refund
                            teacherId = item.MemberId;
                            studentId = memberId;
                            result = _commonService.UpdateOrderStatus(out deviceToken, item.OrderId, item.Status, studentId, teacherId);
                            if (result.Equals(1))
                            {
                                OrderNotificationHelper.NotifyOrderStatusUpdate(item.Status, item.Phone, item.Email, item.Title, item.Name);
                                PushManager.PushNotification(deviceToken, (Byte)Enums.PushNotificationType.RefundRequest);
                            }
                            break;
                        case (Byte)Enums.OrderStatus.RefundProve:// 7 RefundProve
                            teacherId = memberId;
                            studentId = item.MemberId;
                            result = _commonService.UpdateOrderStatusWithCoins(out deviceToken, item.OrderId, item.Status, studentId, teacherId);
                            if (result.Equals(1))
                            {
                                OrderNotificationHelper.NotifyOrderStatusUpdate(item.Status, item.Phone, item.Email, item.Title, item.Name);
                                PushManager.PushNotification(deviceToken, (Byte)Enums.PushNotificationType.RefundProved);
                            }
                            break;
                        case (Byte)Enums.OrderStatus.RefundReject://reject,8
                            teacherId = memberId;
                            studentId = item.MemberId;
                            result = _commonService.UpdateOrderStatus(out deviceToken, item.OrderId, item.Status, studentId, teacherId);
                            if (result.Equals(1))
                            {
                                OrderNotificationHelper.NotifyOrderStatusUpdate(item.Status, item.Phone, item.Email, item.Title, item.Name);
                                PushManager.PushNotification(deviceToken, (Byte)Enums.PushNotificationType.RefundRejected);
                            }
                            break;
                        case (Byte)Enums.OrderStatus.Confirmed://9 confirm
                            teacherId = item.MemberId;
                            studentId = memberId;
                            String phone;
                            result = _commonService.UpdateOrderStatusWithCoins(out deviceToken, item.OrderId, item.Status, studentId, teacherId);
                            _commonService.AddStudentReview(item.OrderId, 0, item.FeedBack, item.Comment, out deviceToken, out phone);//leave classid as 0 and get id in sp
                            if (result.Equals(1))
                            {
                                OrderNotificationHelper.NotifyOrderStatusUpdate(item.Status, item.Phone, item.Email, item.Title, item.Name);
                                PushManager.PushNotification(deviceToken, (Byte)Enums.PushNotificationType.OrderPaid);
                            }
                            break;
                        case (Byte)Enums.OrderStatus.TeacherCancled://12 teacher cancled after accept
                            teacherId = memberId;
                            studentId = item.MemberId;
                            result = _commonService.UpdateOrderStatusWithCoins(out deviceToken, item.OrderId, item.Status, studentId, teacherId);
                            if (result.Equals(1))//update successfully
                            {
                                OrderNotificationHelper.NotifyOrderStatusUpdate(item.Status, item.Phone, item.Email, item.Title, item.Name);
                                PushManager.PushNotification(deviceToken, (Byte)Enums.PushNotificationType.BookCancled);
                            }
                            break;
                        default://5 finish 10 autoconfirm 11://autorefund
                            result = 0;
                            break;

                    }
                }
            }
            catch
            { }
            return result;
        }

        [HttpPost]
        public Byte Post(OrderItem item)
        {
            Byte result = 0;
            Boolean isWeb = item.MemberId.Equals(0);
            try
            {
                String deviceToken;
                int memberId = isWeb ? APIHelper.GetMemberId(true) : item.MemberId;
                item.Remark = String.IsNullOrEmpty(item.Remark) ? "" : item.Remark;
                
                result = _commonService.AddOrder(out deviceToken, memberId, item.ClassId, item.BookDate, item.Remark, item.Name, item.Phone);
                if (result.Equals(1))
                {
                    OrderNotificationHelper.NotifyOrderStatusUpdate((Byte)Enums.OrderStatus.Booked, item.TeacherPhone, item.TeacherMail, item.ClassName, item.TeacherName);
                    PushManager.PushNotification(deviceToken, (Byte)Enums.PushNotificationType.BookRequest);
                }
            }
            catch
            {          
            }
            return result;
        }

        
    }
}
