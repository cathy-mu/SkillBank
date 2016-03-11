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
using SkillBank.Site.Services.Utility;


namespace SkillBankWeb.API
{
    public class OrderReviewController : ApiController
    {
        public readonly ICommonService _commonService;

        public class ReviewItem
        {
            public int MemberId { get; set; }
            public int OrderId { get; set; }
            public Byte Feedback { get; set; }
            public String Comment { get; set; }
            public Boolean IsStudent { get; set; }
        }

        /// <summary>
        /// For web/h5/app to add review
        /// </summary>
        /// <param name="commonService"></param>
        public OrderReviewController(ICommonService commonService)
        {
            _commonService = commonService;

        }

        public string Options()
        {
            return null; // HTTP 200 response with empty body 
        }

        /// <summary>
        /// Add student and teacher review
        /// </summary>
        /// <param name="item">student review should has classid</param>
        /// <returns>true : add successful</returns>
        [HttpPost]
        public Boolean Post(ReviewItem item)
        {
            int memberId = (item.MemberId > 0) ? item.MemberId : APIHelper.GetMemberId(true);
            Boolean result = false;
            String deviceToken = "";
            String phone = "";
            Boolean sendNotify = System.Configuration.ConfigurationManager.AppSettings["ENV"].Equals(ConfigConstants.EnvSetting.LiveEnvName);

            if (memberId > 0)
            {
                //student review
                if (item.IsStudent)
                {
                    result = _commonService.AddStudentReview(item.OrderId, 0, item.Feedback, item.Comment, out deviceToken, out phone);
                    if (sendNotify)
                    {
                        PushManager.PushNotification(deviceToken, (Byte)Enums.PushNotificationType.StudentReview);
                        if (!String.IsNullOrEmpty(phone))
                        {
                            _commonService.SendSMSWithLink((Byte)Enums.SmsType.StudentReview, phone, sendNotify);
                        }
                    }
                }
                //teacher review
                else
                {
                    result = _commonService.AddTeacherReview(item.OrderId, item.Feedback, item.Comment, out deviceToken, out phone);
                    if (sendNotify)
                    {
                        PushManager.PushNotification(deviceToken, (Byte)Enums.PushNotificationType.TeacherReview);
                        if (!String.IsNullOrEmpty(phone))
                        {
                            _commonService.SendSMSWithLink((Byte)Enums.SmsType.TeacherReview, phone, sendNotify);
                        }
                    }
                }
            }
            return result;
        }

    }
}