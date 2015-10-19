using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Text.RegularExpressions;

using SkillBank.Site.Common;
using SkillBank.Site.Services;
using SkillBank.Site.DataSource;
using SkillBank.Site.Web;
using SkillBank.Site.Web.ViewModel;
using SkillBank.Site.Web.Context;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services.Utility;
            

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
            Boolean isMobileRedirect = (IsMobile());
            String envCode = System.Configuration.ConfigurationManager.AppSettings["ENV"];
            if (isMobileRedirect)
            {
                Response.Redirect(ConfigConstants.EnvSetting.MobileHome[envCode]);
            }

            var metaTags = MetaTagHelper.GetMetaTags("classsearch");
            ViewBag.MetaTagTitle = metaTags[0];
            ViewBag.MetaTagKeyWords = metaTags[1];
            ViewBag.MetaTagDescription = metaTags[2];

            int memberId = WebContext.Current.MemberId;
            var memberInfo = memberId > 0 ? _commonService.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.ByMemberExtraInfo, memberId, 0) : null;
            ViewBag.MemberInfo = memberInfo;
            if (!tabid.Equals(0) && memberInfo != null)
            {
                ViewBag.FavoriteClassList = memberInfo.ExtraInfo;
            }
            else
            {
                ViewBag.FavoriteClassList = "";
            }
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
            String envCode = System.Configuration.ConfigurationManager.AppSettings["ENV"];
            if (IsMobile() && envCode.Equals(ConfigConstants.EnvSetting.LiveEnvName))
            {
                Response.Redirect(String.Concat(Constants.PageURL.MobileClassPage, id.ToString()));
            }

            String className = "";
            ClassDetailModel classDetailModel = new ClassDetailModel();

            int currMemberId = WebContext.Current.MemberId;
            //Update to cached class info content
            //ViewBag.MemberInfo = currMemberId > 0 ? _commonService.GetMemberInfo(currMemberId) : null;
            var currMemberInfo = currMemberId > 0 ? _commonService.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.ByWebClassDetail, currMemberId, 0) : null;
            ViewBag.MemberInfo = currMemberInfo;
            if (currMemberInfo != null && !String.IsNullOrEmpty(currMemberInfo.ExtraInfo))
            {
                //ViewBag.IsLike = (currMemberInfo.ExtraInfo.StartsWith(id.ToString() + ",") || currMemberInfo.ExtraInfo.Contains("," + id.ToString() + ","));
                ViewBag.IsLike = DataTagHelper.GetIsLike(currMemberInfo.ExtraInfo, id);
            }
            else
            {
                ViewBag.IsLike = false;
            }
                                 
            LoadNotificationAlert(currMemberId);

            if (id > 0)
            {
                //var classInfo = _commonService.GetClassInfoItem((Byte)Enums.DBAccess.ClassLoadType.ByClassAndCurrMemberId, id, currMemberId);
                var classInfo = _commonService.GetClassItem(id);
                if (classInfo != null)
                {
                    int maxId0 = 0, minId0 = 0, maxId1 = 0, minId1 = 0, maxId2 = 0, minId2 = 0, likeNum = 0;
                    likeNum = classInfo.LikeNum;
                    //hack to make cache like number like real
                    if (ViewBag.IsLike && likeNum.Equals(0))
                    {
                        classInfo.LikeNum = 1;
                    }
                    classInfo.ClassId = id;
                    classDetailModel.ClassInfo = classInfo;
                    int memberId = classInfo.Member_Id;
                    ViewBag.ContactorId = memberId;
                    //class owner
                    ViewBag.IsOwner = memberId.Equals(currMemberId);
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
                    ViewBag.ContactMobile = (memberInfo.NotifyTag & 1).Equals(1) ? memberInfo.Phone : "";//for send SMS notify

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
                    
                    classDetailModel.ClassNumDic = numDic;
                }
                else
                {
                    ViewBag.IsOwner = false;
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
            ViewBag.IsOwner = classInfo.Member_Id.Equals(memberId);

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
                List<String> whiteListMem = ConfigurationManager.AppSettings["MemberWhiteList"].Split(',').ToList<String>();
                classPreviewModel.IsAdmin = whiteListMem.Contains(currMemberId.ToString());
                Byte loadType = (Byte)(classPreviewModel.IsAdmin ? Enums.DBAccess.ClassLoadType.ByAdminPreview : Enums.DBAccess.ClassLoadType.ByClassId);
                var classInfo = _commonService.GetClassInfoItem(loadType, id, 0);
                if (classInfo != null)
                {
                    className = classInfo.Title;
                    int memberId = classInfo.Member_Id;
                    classPreviewModel.IsOwner = memberId.Equals(currMemberId);

                    //administrator or class owner
                    if (classPreviewModel.IsOwner || classPreviewModel.IsAdmin)
                    {
                        classPreviewModel.ClassEditInfo = classInfo;
                        var memberInfo = memberId.Equals(currMemberId) ? currMemberInfo : _commonService.GetMemberInfo(memberId);//class owner info
                        classPreviewModel.MemberInfo = memberInfo;
                        if (classPreviewModel.IsAdmin)
                        {
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

                            var categories = _contentService.GetCategories(0);
                            classPreviewModel.ClassCategoryLkp = categories;
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

        public Boolean IsMobile()
        {
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            if (b.IsMatch(u) || v.IsMatch(u.Substring(0, 4)))
            {
                return true;
            }
            return false;
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
