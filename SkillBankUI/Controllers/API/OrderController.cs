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
                item.OrderId = id;
                int teacherId;
                int studentId;
                String mailText;
                item.Comment = String.IsNullOrEmpty(item.Comment) ? "" : item.Comment;
                
                switch (item.Status)
                {
                    case (Byte)Enums.OrderStatus.Rejected://2 reject,T
                        teacherId = memberId;
                        studentId = item.MemberId;
                        result = _commonService.UpdateOrderStatus(item.OrderId, item.Status, studentId, teacherId);
                        mailText = TextContentHelper.ReplaeceBlurbParameterWithText(628, item.Title);
                        //SendCloudEmail.SendOrderStatusUpdateMail(item.Email, Receiver, mailText, Constants.PageURL.MemberLearnPage, ResourceHelper.GetTransText(480));
                        break;
                    case (Byte)Enums.OrderStatus.Cancled://3 cancle,S
                        teacherId = item.MemberId;
                        studentId = memberId;
                        result = _commonService.UpdateOrderStatus(item.OrderId, item.Status, studentId, teacherId);
                        mailText = TextContentHelper.ReplaeceBlurbParameterWithText(627, item.Title);
                        //SendCloudEmail.SendOrderStatusUpdateMail(item.Email, Receiver, mailText, Constants.PageURL.MemberLearnPage, ResourceHelper.GetTransText(480));
                        break;
                    case (Byte)Enums.OrderStatus.Accepted://4 accpet order,T
                        teacherId = memberId;
                        studentId = item.MemberId;
                        item.Name = String.IsNullOrEmpty(item.Name) ? "" : item.Name;
                        item.Phone = String.IsNullOrEmpty(item.Phone) ? "" : item.Phone;

                        result = _commonService.AcceptOrder(item.OrderId, studentId, teacherId, item.MyName, item.MyPhone);
                        mailText = TextContentHelper.ReplaeceBlurbParameterWithText(629, item.Title);
                        //SendCloudEmail.SendOrderStatusUpdateMail(item.Email, Receiver, mailText, Constants.PageURL.MemberLearnPage, ResourceHelper.GetTransText(479));
                        
                        break;
                    case (Byte)Enums.OrderStatus.Refund://6 refund
                        teacherId = item.MemberId;
                        studentId = memberId;
                        result = _commonService.UpdateOrderStatus(item.OrderId, item.Status, studentId, teacherId);
                        mailText = TextContentHelper.ReplaeceBlurbParameterWithText(630, item.Title);
                        //SendCloudEmail.SendOrderStatusUpdateMail(item.Email, Receiver, mailText, Constants.PageURL.MemberLearnPage, ResourceHelper.GetTransText(480));
                        break;
                    case (Byte)Enums.OrderStatus.RefundProve:// 7 RefundProve
                        teacherId = memberId;
                        studentId = item.MemberId;
                        result = _commonService.UpdateOrderStatusWithCoins(item.OrderId, item.Status, studentId, teacherId);
                        break;
                    case (Byte)Enums.OrderStatus.RefundReject://reject,8
                        teacherId = memberId;
                        studentId = item.MemberId;
                        result = _commonService.UpdateOrderStatus(item.OrderId, item.Status, studentId, teacherId);
                        mailText = TextContentHelper.ReplaeceBlurbParameterWithText(633, item.Title);
                        //SendCloudEmail.SendOrderStatusUpdateMail(item.Email, Receiver, mailText, Constants.PageURL.MemberLearnPage, ResourceHelper.GetTransText(480));
                        break;
                    case (Byte)Enums.OrderStatus.Confirmed://9 confirm
                        teacherId = item.MemberId;
                        studentId = memberId;
                        result = _commonService.UpdateOrderStatusWithCoins(item.OrderId, item.Status, studentId, teacherId);
                        _commonService.AddStudentReview(item.OrderId, 0, item.FeedBack, item.Comment, "");//leave classid as 0 and get id in sp
                        mailText = TextContentHelper.ReplaeceBlurbParameterWithText(632, item.Title);
                        //SendCloudEmail.SendOrderStatusUpdateMail(mailaddr, mailname, mailText, Constants.PageURL.MemberTeachPage, ResourceHelper.GetTransText(480));
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
            }
            catch
            {
                return result;
            }
            return result;
        }
        
    }
}
