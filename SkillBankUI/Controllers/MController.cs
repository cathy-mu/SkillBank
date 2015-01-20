using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

using SkillBank.Site.Common;
using SkillBank.Site.Services;
using SkillBank.Site.DataSource;
using SkillBank.Site.Web;
using SkillBank.Site.Web.ViewModel;
using SkillBank.Site.Web.Context;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.Controllers
{
    public class MController : Controller
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public MController(IContentService contentService, ICommonService commonService)
        {
            _contentService = contentService;
            _commonService = commonService;

            //int memberId = WebContext.Current.MemberId;
            //if (memberId > 0)
            //{
            //    var handleKey = OrderHandlerHelper.GetHandleMemberOrderKey(WebContext.Current.MemberId, WebContext.Current.OrderHandleDate);
            //    if (!String.IsNullOrEmpty(handleKey))
            //    {
            //        _commonService.HandleMemberOrder(memberId);
            //        WebContext.Current.OrderHandleDate = handleKey;
            //    }
            //}

        }

        // GET: /M/

        /// <summary>
        /// 默认达人课程 
        /// </summary>
        /// <param name="by">默认 1</param>
        /// <param name="type">默认 1</param>
        /// <returns></returns>
        public ActionResult Index(Byte by = 1, Byte type = 1, String key = "", String city = "")
        {
            //TO DO:Test data
            int cityId = 0;
            Decimal x = Convert.ToDecimal(121.4165);
            Decimal y = Convert.ToDecimal(31.2190);
            //city = "上海市";

            var metaTags = MetaTagHelper.GetMetaTags("classsearch");
            ViewBag.MetaTagTitle = metaTags[0];
            ViewBag.MetaTagKeyWords = metaTags[1];
            ViewBag.MetaTagDescription = metaTags[2];

            int memberId = WebContext.Current.MemberId;
            var memberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;
            ViewBag.MemberInfo = memberInfo;
            //LoadNotificationAlert(memberId);

            
            ClassListModel classListModel = new ClassListModel();

            var categories = _contentService.GetAllCategories();
            classListModel.CategoryLkp = LookupHelper.GetCatagory4Picker(categories);
            
            var cityDic = _contentService.GetCities("cn");
            if (by.Equals((Byte)Enums.DBAccess.ClassTabListLoadType.NearBy) && !String.IsNullOrEmpty(city))
            {
                city = city.Replace("市", "");
                var cityItems = cityDic.Where(c => c.Value.CityName.StartsWith(city));
                if (cityItems.Count() > 0)
                {
                    cityId = cityItems.First().Key;
                }
            }

            var result = _commonService.GetClassTabList(by, type, memberId, key, x, y, cityId);
            classListModel.ClassList = result;
            return View(classListModel);
        }

        public ActionResult Course(int id = 0)
        {
            String className = "";
            ClassDetailModel classDetailModel = new ClassDetailModel();

            int currMemberId = WebContext.Current.MemberId;
            ViewBag.MemberInfo = currMemberId > 0 ? _commonService.GetMemberInfo(currMemberId) : null;
            //LoadNotificationAlert(currMemberId);

            if (id > 0)
            {
                var classInfo = _commonService.GetClassEditInfo(id, currMemberId, false);
                if (classInfo != null)
                {
                    int maxId0 = 0, minId0 = 0, maxId1 = 0, minId1 = 0, maxId2 = 0, minId2 = 0, likeNum = 0;
                    likeNum = classInfo.ClassId;
                    classInfo.ClassId = id;
                    classDetailModel.ClassInfo = classInfo;
                    int memberId = classInfo.Member_Id;
                    ViewBag.ContactorId = memberId;
                    //class owner
                    className = String.IsNullOrEmpty(classInfo.Title) ? ResourceHelper.GetTransText(560) : classInfo.Title;

                    var memberInfo = _commonService.GetMemberInfo(memberId);
                    classDetailModel.MemberInfo = memberInfo;

                    var studentReview = _commonService.GetClassReviews((Byte)Enums.DBAccess.ReviewLoadType.ByClass, memberId, id, 0, 0);
                    if (studentReview != null && studentReview.Count > 0)
                    {
                        classDetailModel.ClassReview = studentReview.Where(r => r.TabId == 0).ToList();
                        if (classDetailModel.ClassReview != null && classDetailModel.ClassReview.Count() != 0)
                        {
                            maxId0 = classDetailModel.ClassReview.Max(i => i.ReviewId);
                            minId0 = classDetailModel.ClassReview.Min(i => i.ReviewId);
                        }
                        classDetailModel.OtherClassReview = studentReview.Where(r => r.TabId == 1).ToList();
                        if (classDetailModel.OtherClassReview != null && classDetailModel.OtherClassReview.Count() != 0)
                        {
                            maxId1 = classDetailModel.OtherClassReview.Max(i => i.ReviewId);
                            minId1 = classDetailModel.OtherClassReview.Min(i => i.ReviewId);
                        }

                        classDetailModel.OtherClassReview = studentReview.Where(r => r.TabId == 1).ToList();
                        if (classDetailModel.OtherClassReview != null && classDetailModel.OtherClassReview.Count() != 0)
                        {
                            maxId1 = classDetailModel.OtherClassReview.Max(i => i.ReviewId);
                            minId1 = classDetailModel.OtherClassReview.Min(i => i.ReviewId);
                        }

                        classDetailModel.ClassComment = studentReview.Where(r => r.TabId == 2).ToList();
                        if (classDetailModel.ClassComment != null && classDetailModel.ClassComment.Count() != 0)
                        {
                            maxId2 = classDetailModel.ClassComment.Max(i => i.ReviewId);
                            minId2 = classDetailModel.ClassComment.Min(i => i.ReviewId);
                        }
                    }


                    var classList = _commonService.GetClassInfoByTeacherId(memberId, (Byte)Enums.DBAccess.ClassLoadType.ByTeacherPublished);
                    if (classList != null && classList.Count > 0)
                    {
                        classDetailModel.ClassList = classList.Where(c => c.ClassId != id).ToList();
                    }
                    else
                    {
                        classDetailModel.ClassList = null;
                    }

                    classDetailModel.IsLogin = (currMemberId > 0);
                    classDetailModel.IsOwner = memberId.Equals(currMemberId);
                    if (!classDetailModel.IsOwner)
                    {
                        var myInfo = _commonService.GetMemberInfo(currMemberId);
                        classDetailModel.MyInfo = myInfo;
                    }

                    //init numbers on page
                    var numDic = _commonService.GetNumsByMemberClass(memberId, id);
                    int sum0 = numDic["r01"] + numDic["r02"] + numDic["r03"];
                    int sum1 = numDic["r11"] + numDic["r12"] + numDic["r13"];
                    numDic.Add("sum0", sum0);
                    numDic.Add("sum1", sum1);
                    numDic.Add("min0", minId0);
                    numDic.Add("max0", maxId0);
                    numDic.Add("min1", minId1);
                    numDic.Add("max1", maxId1);
                    numDic.Add("min2", minId2);
                    numDic.Add("max2", maxId2);
                    numDic.Add("like", likeNum);

                    classDetailModel.ClassNumDic = numDic;
                    //Dictionary<String,int> classNumList
                    //classDetailModel.ClassNums = _commonService.GetNumsByMemberClass(memberId, id);

                    //classDetailModel.ClassRank = Convert.ToInt32(classRank);
                    //classDetailModel.ClassCounter = new int[4] { classNum, studentNum, reviewNum, otherReviewNum };//Order, stdent, review
                }
                else
                {
                    classDetailModel.ClassInfo = null;
                    ViewBag.ErrorMessage = ResourceHelper.GetTransText(580);//No class info
                }
            }

            var metaTags = MetaTagHelper.GetMetaTags("classdetail");
            ViewBag.MetaTagTitle = metaTags[0].Replace("{0}", className);
            ViewBag.MetaTagKeyWords = metaTags[1];
            ViewBag.MetaTagDescription = metaTags[2];

            return View(classDetailModel);
        }


        public ActionResult Message()
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("message");

            var memberId = WebContext.Current.MemberId;
            ViewBag.MemberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;

            MessageListModel messageListModel = new MessageListModel();
            if (memberId > 0)
            {
                //LoadNotificationAlert(memberId);
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
                //LoadNotificationAlert(memberId);
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

        public ActionResult Personal(int id = 0)
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
                //LoadNotificationAlert(memberId);
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
                userName = memberInfo == null ? "" : memberInfo.Name;
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


    }
}
