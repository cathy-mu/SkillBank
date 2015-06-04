using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.ViewModel;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Services.Net.Mail;

namespace SkillBankWeb.Controllers
{
    public class ClassHelperController : Controller
    {
        //
        // GET: /ClassHelper/

        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public ClassHelperController(IContentService contentService, ICommonService commonService)
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
        /// Search for city dropdown content (4 skill share page)
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchCity(String searchKey, Boolean isMatch = false)
        {
            searchKey = searchKey.Replace("市", "").Replace(" ", "");
            if (!String.IsNullOrEmpty(searchKey))
            {
                var cityList = LookupHelper.GetCity4Picker(_contentService.GetCities(WebContext.Current.MarketCode));
                if (isMatch)
                {
                    //for Local Name
                    var tempKey = String.Format("{0}(", searchKey);
                    var result = cityList.Where(c => c.Text.ToLower().Contains(tempKey.ToLower())).ToList();
                    if (result == null || result.Count != 1)
                    {
                        //for English Key
                        tempKey = String.Format("({0})", searchKey);
                        result = cityList.Where(c => c.Text.ToLower().Contains(tempKey.ToLower())).ToList();
                        if (result == null || result.Count != 1)
                        {
                            return Json("", JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = cityList.Where(c => c.Text.ToLower().Contains(searchKey.ToLower())).ToList();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            //no search result
            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="classId"></param>
        /// <param name="isLike"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateLikeClass(int classId, Boolean isLike)
        {
            int memberId = WebContext.Current.MemberId;
            _commonService.UpdateClassLikeTag(memberId, classId, isLike);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #region old class edit process
        /// <summary>
        /// Ajax update class info (4 create class and class info edit page)
        /// </summary>
        /// <param name="updateType"></param>
        /// <param name="classId"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateClassTextInfo(Byte updateType, int classId, String infoValue, Byte completeStatus = 1)
        {
            var result = true;
            _commonService.UpdateClassInfo(updateType, classId, infoValue, completeStatus);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ajax update class info (4 create class and class info edit page)
        /// </summary>
        /// <param name="updateType"></param>
        /// <param name="classId"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateClassSByteInfo(Byte updateType, int classId, Byte infoValue, Byte completeStatus = 1)
        {
            var result = _commonService.UpdateClassInfo(updateType, classId, infoValue, completeStatus);
            //Send Class Prove
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateType"></param>
        /// <param name="classId"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateClassBoolInfo(Byte updateType, int classId, Boolean infoValue, Byte completeStatus = 1)
        {
            var result = _commonService.UpdateClassInfo(updateType, classId, infoValue);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
              
        #endregion

        /// <summary>
        /// Ajax update class info (4 create class and class info edit page)
        /// </summary>
        /// <param name="updateType"></param>
        /// <param name="classId"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateClassEditTextInfo(Byte updateType, int classId, String infoValue)
        {
            var result = true;
            _commonService.UpdateClassEditInfo(updateType, classId, infoValue);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ajax update class info (4 create class and class info edit page)
        /// </summary>
        /// <param name="updateType"></param>
        /// <param name="classId"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateClassEditByteInfo(Byte updateType, int classId, Byte infoValue)
        {
            var result = _commonService.UpdateClassEditInfo(updateType, classId, infoValue);
            //Send Class Prove
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateType"></param>
        /// <param name="classId"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateClassEditBoolInfo(Byte updateType, int classId, Boolean infoValue)
        {
            var result = _commonService.UpdateClassEditInfo(updateType, classId, infoValue);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="infoValue"></param>
        /// <param name="className"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateClassStatus(int classId, Byte infoValue, Boolean forActive = false, String className = "", String name = "", String email = "", String mobile = "")
        {
            Byte updateType = forActive ? (Byte)Enums.DBAccess.ClassSaveType.SetActiveTag : (Byte)Enums.DBAccess.ClassSaveType.UpdateProvedTag;

            var result = _commonService.UpdateClassEditInfo(updateType, classId, infoValue);
            Boolean sendNotify = System.Configuration.ConfigurationManager.AppSettings["ENV"].Equals(ConfigConstants.EnvSetting.LiveEnvName);

            if (!forActive && sendNotify)
            {
                if (infoValue.Equals(1))
                {
                    //Send Class Prove email 
                    if (!String.IsNullOrEmpty(mobile))
                    {
                        _commonService.SendClassProveSMS(true, mobile, className, Constants.PageURL.MobileMyCoursePage);
                    }
                    else
                    {
                        SendCloudEmail.SendClassProvedMail(email, name, className);
                    }
                }
                else if (infoValue.Equals(2))
                {
                    //Send reject email
                    if (!String.IsNullOrEmpty(mobile))
                    {
                        _commonService.SendClassProveSMS(false, mobile, className, Constants.PageURL.MobileMyCoursePage);
                    }
                    else
                    {
                        SendCloudEmail.SendClassRejectMail(email, name, className);
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// Create new class
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="teachLevel"></param>
        /// <param name="skillLevel"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddClass(int categoryId, Byte skillLevel, Byte teachLevel, int cityId = -1, Boolean cityNoClass = false)
        {
            //if new member will keep skill info in cookie and go to register page, then call this function after register with member id
            var result = new JsonResult();
            SByte actionType;
            Boolean isExist = false;// should load exist class info on class add/edit page
            int classId = 0;

            int memberId = WebContext.Current.MemberId;
            if (memberId > 0)
            {
                var memberInfo = _commonService.GetMemberInfo(memberId);
                if (memberInfo == null)
                {
                    actionType = 0;//Stay on skill page
                }
                else if (String.IsNullOrEmpty(memberInfo.SelfIntro) || memberInfo.PosX.Equals(0) || memberInfo.PosY.Equals(0))
                {
                    classId = _commonService.CreateClass(memberId, categoryId, skillLevel, teachLevel, out isExist);
                    //should edit member info, go to Add class page tab1 
                    if (isExist)
                    {
                        actionType = 2;
                    }
                    else
                    {
                        actionType = 1;
                    }
                }
                else
                {
                    classId = _commonService.CreateClass(memberId, categoryId, skillLevel, teachLevel, out isExist);
                    //go to Edit class page tab 2
                    if (isExist)
                    {
                        actionType = 4;
                    }
                    else
                    {
                        actionType = 3;
                    }
                }

                if (cityId > 0 && cityId != memberInfo.CityId)
                {
                    _commonService.UpdateMemberInfo(memberId, (Byte)Enums.DBAccess.MemberSaveType.UpdateCity, cityId.ToString(), cityNoClass?"1":"");
                }
                result.Data = new { classId = classId, type = actionType, memberId = memberId };
            }
            else
            {
                result.Data = "";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult BookClass(int classId, String bookDate, String comment, String name, String phone)
        {
            var result = false;
            //return result 1 success, 2 nocoins left
            int memberId = WebContext.Current.MemberId;

            //update memberinfo
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetClassInfo(int cityId, Byte categoryId, Boolean isParentCate, int pageId, Byte type = (Byte)ClientSetting.ClassListOrderType.ByDisctince, Boolean asc = true)
        {
            int memberId = WebContext.Current.MemberId;
            var memberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;
            ViewBag.MemberInfo = memberInfo;

            int pageSize = 12;
            int classNum = 0;
            int pageNum = 0;
            //List<ClassItem> result;
            if (memberInfo != null)
            {
                var result = _commonService.SearchClass(cityId, categoryId, isParentCate, pageSize, pageId, out classNum, out pageNum, type, asc, memberInfo.PosX, memberInfo.PosY);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //not a member
                var result = _commonService.SearchClass(cityId, categoryId, isParentCate, pageSize, pageId, out classNum, out pageNum, type, asc, 0, 0);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddComment(int classId, String comment)
        {
            int memberId = WebContext.Current.MemberId;
            _commonService.AddComment(memberId, classId, comment);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        
                 

       
    }
}
