using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SkillBank.Site.Common;
using SkillBank.Site.Services;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Web.ViewModel;


namespace SkillBankUI.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Member/

        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public ProfileController(IContentService contentService, ICommonService commonService)
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

        public ActionResult Index(int id = 0)
        {
            String userName = "";
            int likeNum = 0;
            ProfilelModel profileModel = new ProfilelModel();
            var currMemberId = WebContext.Current.MemberId;
            var currMemberInfo = currMemberId > 0 ? _commonService.GetMemberInfo(currMemberId) : null;
            ViewBag.MemberInfo = currMemberInfo;

            int memberId = 0;
            //view other member's page
            if (id > 0)
            {
                LoadNotificationAlert(memberId);
                memberId = id;
                var memberInfo = _commonService.GetMemberInfo(memberId, currMemberId);
                if (memberInfo != null)
                {
                    likeNum = memberInfo.MemberId;
                    memberInfo.MemberId = memberId;
                    profileModel.MemberInfo = memberInfo;
                }
                else
                {
                    return View();
                }
                userName = memberInfo==null?"":memberInfo.Name;
            }
            else if (id == 0)
            {
                memberId = currMemberId;
                profileModel.MemberInfo = currMemberInfo;
                userName = currMemberInfo.Name;
            }
            profileModel.ClassList = _commonService.GetClassInfoByTeacherId(memberId, (Byte)Enums.DBAccess.ClassLoadType.ByTeacherPublished);
            if (memberId > 0)
            {
                int maxId0 = 0, minId0 = 0, maxId1 = 0, minId1 = 0;
                var reviews = _commonService.GetMemberReviews((Byte)Enums.DBAccess.ReviewLoadType.ByMember, memberId, 0, 0);
                if (reviews != null && reviews.Count > 0)
                {
                    profileModel.StuentReview = reviews.Where(r => r.TabId == 0).ToList();
                    if (profileModel.StuentReview != null && profileModel.StuentReview.Count() != 0)
                    {
                        maxId0 = profileModel.StuentReview.Max(i => i.ReviewId);
                        minId0 = profileModel.StuentReview.Min(i => i.ReviewId);
                    }
                    profileModel.TeacherReview = reviews.Where(r => r.TabId == 1).ToList();
                    if (profileModel.TeacherReview != null && profileModel.TeacherReview.Count() != 0)
                    {
                        maxId1 = profileModel.TeacherReview.Max(i => i.ReviewId);
                        minId1 = profileModel.TeacherReview.Min(i => i.ReviewId);
                    }
                }

                var numDic = _commonService.GetNumsByMember(memberId);
                int sum0 = numDic["r01"] + numDic["r02"] + numDic["r03"];
                int sum1 = numDic["r11"] + numDic["r12"] + numDic["r13"];
                numDic.Add("sum0", sum0);
                numDic.Add("sum1", sum1);
                numDic.Add("sum", sum0 + sum1);
                numDic.Add("max0", maxId0);
                numDic.Add("max1", maxId1);
                numDic.Add("min0", minId0);
                numDic.Add("min1", minId1);
                numDic.Add("like", likeNum);

                profileModel.ProfileNumDic = numDic;
            }

            var metaTags = MetaTagHelper.GetMetaTags("profile");
            ViewBag.MetaTagTitle = metaTags[0].Replace("{0}", userName);
            ViewBag.MetaTagKeyWords = metaTags[1];
            ViewBag.MetaTagDescription = metaTags[2];

            return View(profileModel);
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
