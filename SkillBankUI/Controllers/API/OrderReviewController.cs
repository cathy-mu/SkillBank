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


namespace SkillBankWeb.API
{
    public class OrderReviewController : ApiController
    {
        public readonly ICommonService _commonService;

        public class ReviewItem
        {
            public int OrderId { get; set; }
            public Byte Feedback { get; set; }
            public String Comment { get; set; }
            public Boolean IsStudent { get; set; }
        }

        public OrderReviewController(ICommonService commonService)
        {
            _commonService = commonService;

        }

        /// <summary>
        /// Add student and teacher review
        /// </summary>
        /// <param name="item">student review should has classid</param>
        /// <returns>true : add successful</returns>
        public Boolean Post(ReviewItem item)
        {
            int memberId = APIHelper.GetMemberId(true);
            Boolean result = false;
            if (memberId > 0)
            {
                //student review
                if (item.IsStudent)
                {
                    result = _commonService.AddStudentReview(item.OrderId, 0, item.Feedback, item.Comment, "");
                }
                //teacher review
                else
                {
                    result = _commonService.AddTeacherReview(item.OrderId, item.Feedback, item.Comment, "");
                }
            }
            return result;
        }

    }
}