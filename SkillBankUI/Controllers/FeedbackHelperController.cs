using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SkillBank.Site.Services;
using SkillBank.Site.DataSource;
using SkillBank.Site.Web;
using SkillBank.Site.Common;
using SkillBank.Site.Web.ViewModel;
using SkillBank.Site.Web.Context;

namespace SkillBankWeb.Controllers
{
    public class FeedbackHelperController : Controller
    {
        //
        // GET: /FeedbackHelper

        public readonly ICommonService _commonService;

        public FeedbackHelperController(ICommonService commonService)
        {
            _commonService = commonService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="feedback"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddTeacherReview(int orderid, byte feedback, String comment = "")
        {
            _commonService.AddTeacherReview(orderid, feedback, comment, "");
            return Json("true", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetClassReview(Byte tabid, int memberid, int classid, byte feedback, int maxid)
        {
            tabid = tabid.Equals(0) ? (Byte)Enums.DBAccess.ReviewLoadType.ByClassTab1 : (Byte)Enums.DBAccess.ReviewLoadType.ByClassTab2;
            var reviews = _commonService.GetClassReviews(tabid, memberid, classid, feedback, maxid);
            if (reviews != null && reviews.Count() > 0)
            {
                return Json(reviews, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMemberReview(Byte tabid, int memberid, Byte feedback, int maxid)
        {
            tabid = tabid.Equals(0) ? (Byte)Enums.DBAccess.ReviewLoadType.ByMemberTab1 : (Byte)Enums.DBAccess.ReviewLoadType.ByMemberTab2;
            var reviews = _commonService.GetMemberReviews(tabid, memberid, feedback, maxid);
            if (reviews != null && reviews.Count() > 0)
            {
                return Json(reviews, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

    }
}