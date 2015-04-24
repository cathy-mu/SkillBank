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
    public class ClassController : Controller
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public ClassController(IContentService contentService, ICommonService commonService)
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult BeTeacher()
        {
            var metaTags = MetaTagHelper.GetMetaTags("beteacher");
            ViewBag.MetaTagTitle = metaTags[0];
            ViewBag.MetaTagKeyWords = metaTags[1];
            ViewBag.MetaTagDescription = metaTags[2];

            int memberId = WebContext.Current.MemberId;
            LoadNotificationAlert(memberId);
            ViewBag.MemberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Publish()
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("classpublish");

            int memberId = WebContext.Current.MemberId;
            LoadNotificationAlert(memberId);
            ViewBag.MemberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;

            return View();
        }
            
        /// <summary>
        /// default page show class list for search
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int id = 1, String k = "", Byte tabid = 0)
        {
            var metaTags = MetaTagHelper.GetMetaTags("classsearch");
            ViewBag.MetaTagTitle = metaTags[0];
            ViewBag.MetaTagKeyWords = metaTags[1];
            ViewBag.MetaTagDescription = metaTags[2];

            int memberId = WebContext.Current.MemberId;
            var memberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;
            ViewBag.MemberInfo = memberInfo;
            LoadNotificationAlert(memberId);

            ClassListModel classListModel = new ClassListModel();

            int cityId = CookieManager.GetCookieIntValue(Constants.CookieKeys.ClassListCity, Request);
            int pageSize = 36;
            int pageId = id;
            int classNum = 0;//out parameter, get result number for paging

            var categories = _contentService.GetAllCategories();
            classListModel.CategoryLkp = LookupHelper.GetCatagory4Picker(categories);
            classListModel.CityLkp = _contentService.GetClassCities(WebContext.Current.MarketCode);
            Boolean isParentCate = false;
            Byte categoryId = CookieManager.GetCookieByteBoolValue(Constants.CookieKeys.ClassListCategory, Request, Constants.Setting.CacheKeySpliter, out isParentCate);//0 for all category
            classListModel.TabId = tabid;
            if (tabid.Equals(0))
            {
                Boolean isAsc = false;
                Byte orderBy = CookieManager.GetCookieByteBoolValue(Constants.CookieKeys.ClassListOrder, Request, Constants.Setting.CacheKeySpliter, out isAsc);
                Byte loadBy = 0;
                classListModel.OrderByKey = String.Format("{0}_{1}", orderBy, isAsc ? 1 : 0);//default is order by lastupdate date desc

                //int pageListSize = 17;
                //int pageListSizeBuffer = (pageListSize-1)/2;

                classListModel.SelCityId = cityId;
                classListModel.SelCategoryId = categoryId;


                if (memberInfo == null || memberInfo.CityId.Equals(0) || memberInfo.PosX.Equals(0))
                {
                    var result = _commonService.GetClassList(loadBy, orderBy, cityId, categoryId, isParentCate, pageSize, pageId, 0, out classNum, k, 0, 0);
                    if (result != null && result.Count > 0)
                    {
                        classListModel.ClassList = result;
                        classNum = classListModel.ClassList[0].ClassNum;
                    }
                }
                else
                {
                    var result = _commonService.GetClassList(loadBy, orderBy, cityId, categoryId, isParentCate, pageSize, pageId, memberInfo.MemberId, out classNum, k, memberInfo.PosX, memberInfo.PosY);
                    if (result != null && result.Count > 0)
                    {
                        classListModel.ClassList = result;
                        classNum = classListModel.ClassList[0].ClassNum;
                    }
                }

                classListModel.ClassNum = classNum;
                if (String.IsNullOrEmpty(k))
                {
                    classListModel.SearchResultTitle = ResourceHelper.GetTransText(608).Replace("{0}", classNum.ToString());
                }
                else
                {
                    classListModel.SearchResultTitle = ResourceHelper.GetTransText(221).Replace("{0}", k).Replace("{1}", classNum.ToString());
                }
            }
            else
            {
                //var result = _commonService.GetClassList(1, 0, cityId, categoryId, false, pageSize, pageId, 0, out classNum, k, 0, 0);
                var result = _commonService.GetRecommendationClassList(tabid, pageSize, pageId, memberId, out classNum);
                if (result != null && result.Count > 0)
                {
                    classListModel.ClassList = result;
                    classNum = classListModel.ClassList[0].ClassNum;
                }
            }

            classListModel.PageNum = (classNum % pageSize == 0) ? (classNum / pageSize) : (classNum / pageSize) + 1;
            classListModel.PageId = id;

            return View(classListModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id = 0)
        {
            String className = "";
            ClassDetailModel classDetailModel = new ClassDetailModel();

            int currMemberId = WebContext.Current.MemberId;
            ViewBag.MemberInfo = currMemberId > 0 ? _commonService.GetMemberInfo(currMemberId): null ;
            LoadNotificationAlert(currMemberId);

            if (id > 0)
            {
                var classInfo = _commonService.GetClassInfoItem((Byte)Enums.DBAccess.ClassLoadType.ByClassAndCurrMemberId, id, currMemberId);
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


                    var classList = _commonService.GetClassInfo((Byte)Enums.DBAccess.ClassLoadType.ByTeacherPublished, 0, memberId);
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
                    //if (!classDetailModel.IsOwner)
                    //{
                    //    var myInfo = _commonService.GetMemberInfo(currMemberId);
                    //    classDetailModel.MyInfo = myInfo;
                    //}

                    //init numbers on page
                    var numDic = _commonService.GetNumsByMemberClass(memberId, id);
                    int sum0 = numDic[Enums.NumberDictionaryKey.Result01] + numDic[Enums.NumberDictionaryKey.Result02] + numDic[Enums.NumberDictionaryKey.Result03];
                    int sum1 = numDic[Enums.NumberDictionaryKey.Result11] + numDic[Enums.NumberDictionaryKey.Result12] + numDic[Enums.NumberDictionaryKey.Result13];
                    numDic.Add(Enums.NumberDictionaryKey.Sum0, sum0);
                    numDic.Add(Enums.NumberDictionaryKey.Sum1, sum1);
                    numDic.Add(Enums.NumberDictionaryKey.Min0, minId0);
                    numDic.Add(Enums.NumberDictionaryKey.Max0, maxId0);
                    numDic.Add(Enums.NumberDictionaryKey.Min1, minId1);
                    numDic.Add(Enums.NumberDictionaryKey.Max1, maxId1);
                    numDic.Add(Enums.NumberDictionaryKey.Min2, minId2);
                    numDic.Add(Enums.NumberDictionaryKey.Max2, maxId2);
                    numDic.Add(Enums.NumberDictionaryKey.Like, likeNum);

                    classDetailModel.ClassNumDic = numDic;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="edit"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0, Byte edit = 1)
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("classadd");

            ClassEditModel classEditModel = new ClassEditModel();
            var memberId = WebContext.Current.MemberId;
            var memberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;
            ViewBag.MemberInfo = memberInfo;
            LoadNotificationAlert(memberId);
                
            var categoryLkp = _contentService.GetAllCategories();
            var categories = LookupHelper.GetCatagory4Picker(categoryLkp);
            if (categories != null)
            {
                classEditModel.CategoryLkp = categories;
            }
            
            classEditModel.isEdit = (edit == 1);//0 new class, 1 edit class

            //Load class info by class id or get lastest course if not course id
            var classInfo = _commonService.GetClassInfoItem((Byte)Enums.DBAccess.ClassLoadType.ByClassId, id, memberId);
            
            List<String> whiteListMem = ConfigurationManager.AppSettings["MemberWhiteList"].Split(',').ToList<String>();
            var IsAdmin = whiteListMem.Contains(memberId.ToString());
            //classPreviewModel.IsOwner = memberId.Equals(currMemberId);
            
            if (classInfo != null && (classInfo.Member_Id.Equals(memberId) || IsAdmin))
            {
                if (classInfo.IsProved.Equals(3) && !IsAdmin)
                {
                    Response.Redirect("/class/publish");
                }
                else if (memberInfo != null)
                {
                    classEditModel.MyInfo = memberInfo;
                    if (id.Equals(0))
                    {
                        id = classInfo.ClassId;
                    }
                    classEditModel.ClassInfo = classInfo;
                    int categoryId = classInfo.Category_Id;
                    if (categoryId > 0 && categories != null)
                    {
                        String subCateKey = String.Format(";{0},", categoryId);
                        var parentCategory = categories.Where(i => (i.SubCategories != null && i.SubCategories.Contains(subCateKey))).FirstOrDefault();
                        if (parentCategory != null)
                        {
                            classEditModel.ParentCategoryId = parentCategory.CateId;
                        }
                        else
                        {
                            classEditModel.ParentCategoryId = categoryId;
                        }
                        classEditModel.ParentCategoryName = categoryLkp.ContainsKey(classEditModel.ParentCategoryId) ? categoryLkp[classEditModel.ParentCategoryId].CategoryInfo.CategoryName : "";
                    }
                    ViewBag.ClassId = id;
                    
                }

                ViewBag.CharacterCountText = ResourceHelper.GetTransText(283).Replace("{0}", ";").Split(';');
            }
            else
            {
                ViewBag.ErrorMessage = ResourceHelper.GetTransText(580);
            }
            return View(classEditModel);

        }

        public ActionResult Add(int id = 0, Byte edit = 1)//Change default value to 1
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("classadd");
            ViewBag.ClassId = 0;
            var memberId = WebContext.Current.MemberId;
            var memberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;
            ViewBag.MemberInfo = memberInfo;
            LoadNotificationAlert(memberId);
                
            ClassAddModel classAddModel = new ClassAddModel();
            var classInfo = _commonService.GetClassInfoByClassId(id);
            var localeCode = WebContext.Current.MarketCode;

            if (classInfo != null && classInfo.Member_Id.Equals(memberId))
            {
                classAddModel.isEdit = (edit == 1);
                var categoryLkp = _contentService.GetAllCategories();
                var categories = LookupHelper.GetCatagory4Picker(categoryLkp);

                classAddModel.CategoryName = ClassHelper.GetCategoryNameById(categoryLkp, categories, classInfo.Category_Id);
                if (memberInfo != null)
                {
                    classAddModel.MyInfo = memberInfo;
                    if (!String.IsNullOrEmpty(memberInfo.SelfIntro) && !memberInfo.CityId.Equals(0) && !memberInfo.PosX.Equals(0))
                    {
                        classAddModel.hasAboutMeInfo = true;
                    }
                    else
                    {
                        classAddModel.hasAboutMeInfo = false;
                    }
                    classAddModel.CityForMap = _contentService.GetCityNameById(localeCode, memberInfo.CityId).Trim();

                    if (id > 0)
                    {
                        classAddModel.ClassInfo = classInfo;
                        ViewBag.ClassId = id;
                    }
                }
                ViewBag.CharacterCountText = ResourceHelper.GetTransText(283).Replace("{0}", ";").Split(';');
            }
            else
            {
                ViewBag.ErrorMessage = ResourceHelper.GetTransText(580);
            }
            return View(classAddModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Preview(int id = 0)
        {
            var currMemberId = WebContext.Current.MemberId;
            var currMemberInfo = currMemberId > 0 ? _commonService.GetMemberInfo(currMemberId) : null;
            ViewBag.MemberInfo = currMemberInfo;

            String className = "";
            ClassPreviewModel classPreviewModel = new ClassPreviewModel();
            LoadNotificationAlert(currMemberId);
            if (id > 0 && currMemberId > 0)
            {
                var classInfo = _commonService.GetClassInfoItem((Byte)Enums.DBAccess.ClassLoadType.ByClassId, id, 0);
                if (classInfo != null)
                {
                    className = classInfo.Title;
                    int memberId = classInfo.Member_Id;
                    List<String> whiteListMem = ConfigurationManager.AppSettings["MemberWhiteList"].Split(',').ToList<String>();
                    classPreviewModel.IsAdmin = whiteListMem.Contains(currMemberId.ToString());
                    classPreviewModel.IsOwner = memberId.Equals(currMemberId);

                    //administrator or class owner
                    if (classPreviewModel.IsOwner || classPreviewModel.IsAdmin)
                    {
                        classPreviewModel.ClassEditInfo = classInfo;
                        var memberInfo = memberId.Equals(currMemberId) ? currMemberInfo : _commonService.GetMemberInfo(memberId);//class owner info
                        classPreviewModel.MemberInfo = memberInfo;
                        var RecommendationList = _commonService.GetRecommendation(id);
                        if (RecommendationList != null)
                        {
                            Dictionary<Byte, RecommendationItem> recommendationDic = new Dictionary<byte, RecommendationItem>();
                            foreach (var item in RecommendationList)
                            {
                                recommendationDic.Add(item.GroupId, item);
                            }
                            classPreviewModel.RecommendationDic = recommendationDic;
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = ResourceHelper.GetTransText(582);//Not owner or administrator
                    }
                }
                else
                {
                    classPreviewModel.ClassInfo = null;
                    ViewBag.ErrorMessage = ResourceHelper.GetTransText(580);//No class has this id 
                }
            }

            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("classpreview").Replace("{0}", className);

            return View(classPreviewModel);
        }

        
        public ActionResult Skill()
        {
            ViewBag.MetaTagTitle = MetaTagHelper.GetMetaTitle("classskill");

            var memberId = WebContext.Current.MemberId;
            
            var memberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;
            if (memberId > 0 && memberInfo != null)
            {
                ViewBag.MemberInfo = memberInfo;
                LoadNotificationAlert(memberId);
                ClassSkillModel classSkillModel = new ClassSkillModel();
                classSkillModel.CategoryLkp = LookupHelper.GetCatagory4Picker(_contentService.GetAllCategories());
                //classSkillModel.CityLkp = LookupHelper.GetCity4Picker(_contentService.GetCities(WebContext.Current.MarketCode));
                classSkillModel.hasMemberCityInfo = (memberId > 0 && memberInfo != null && memberInfo.CityId > 0);

                return View(classSkillModel);
            }
            else
            {
                return RedirectToAction("BeTeacher", "Class");
            }
        }


        public ActionResult _HeaderTabsPartial()
        {
            return View();
        }

        public void LoadNotificationAlert(int memberId)
        {
            if (memberId > 0)
            {
                Byte checkStatus = (Byte)((Session == null || Session["AlertStatus"] == null) ? Enums.DBAccess.NotificationAlterLoadType.WebCheckStatus : Enums.DBAccess.NotificationAlterLoadType.Web);
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

        #region old class edit process

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Rejected()
        {
            ViewBag.MetaTagTitle = "Rejected Class";

            var memberId = WebContext.Current.MemberId;
            ViewBag.MemberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;

            ClassProveModel classProveModel = new ClassProveModel();
            List<String> whiteListMem = ConfigurationManager.AppSettings["MemberWhiteList"].Split(',').ToList<String>();
            if (whiteListMem.Contains(memberId.ToString()))
            {
                classProveModel.IsAdmin = true;
                var classList = _commonService.GetClassInfoForAdmin(true);
                if (classList == null)
                {
                    ViewBag.ErrorMessage = ResourceHelper.GetTransText(585);
                }
                else
                {
                    classProveModel.ClassList = classList;
                }

            }
            else
            {
                ViewBag.ErrorMessage = ResourceHelper.GetTransText(582);
                classProveModel.IsAdmin = false;
                classProveModel.ClassList = null;
            }
            return View(classProveModel);
        }

        public ActionResult Prove()
        {
            ViewBag.MetaTagTitle = "Prove Class";

            var memberId = WebContext.Current.MemberId;
            ViewBag.MemberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;

            ClassProveModel classProveModel = new ClassProveModel();
            List<String> whiteListMem = ConfigurationManager.AppSettings["MemberWhiteList"].Split(',').ToList<String>();
            if (whiteListMem.Contains(memberId.ToString()))
            {
                classProveModel.IsAdmin = true;
                var classList = _commonService.GetClassInfoForAdmin(false);
                if (classList == null)
                {
                    ViewBag.ErrorMessage = ResourceHelper.GetTransText(585);
                }
                else
                {
                    classProveModel.ClassList = classList;
                }

            }
            else
            {
                ViewBag.ErrorMessage = ResourceHelper.GetTransText(582);
                classProveModel.IsAdmin = false;
                classProveModel.ClassList = null;
            }
            return View(classProveModel);
        }
        
        
        #endregion

    }
}
