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
using SkillBank.Site.Services.Utility;

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
        [Obsolete]
        public JsonResult UpdateLikeClass(int classId, Boolean isLike)
        {
            int memberId = WebContext.Current.MemberId;
            String deviceToken = "";
            _commonService.UpdateClassLikeTag(memberId, classId, isLike, out deviceToken);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

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
        public JsonResult UpdateClassStatus(int classId, Byte infoValue, Byte saveType, Boolean isProved = true, String className = "", String name = "", String email = "", String mobile = "", String device = "")
        {
            Boolean result;
            //Active/Disactive course 
            if (saveType.Equals((Byte)Enums.DBAccess.ClassSaveType.SetActiveTag))
            {
                result = _commonService.UpdateClassEditInfo(saveType, classId, infoValue);
            }
            //prove course
            else
            {
                result = _commonService.UpdateClassEditInfo(saveType, classId, infoValue, isProved);
                Boolean sendNotify = System.Configuration.ConfigurationManager.AppSettings["ENV"].Equals(ConfigConstants.EnvSetting.LiveEnvName);

                if (sendNotify)
                {
                    if (isProved)//Send Class Prove notify
                    {
                        if(!String.IsNullOrEmpty(device))
                        {
                            PushManager.PushNotification(device, (Byte)Enums.PushNotificationType.ClassProved);
                        }

                        if (!String.IsNullOrEmpty(mobile))
                        {
                            _commonService.SendClassProveSMS(true, mobile, className, Constants.PageURL.MobileMyCoursePage);
                        }
                        else if (!String.IsNullOrEmpty(className))
                        {
                            SendCloudEmail.SendClassProvedMail(email, name, className);
                        }
                    }
                    else //Send reject notify
                    {
                        if (!String.IsNullOrEmpty(device))
                        {
                            PushManager.PushNotification(device, (Byte)Enums.PushNotificationType.ClassRejected);
                        } 
                        
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
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateClassTagCategory(int classId, Byte cateId, String tags)
        {
            var result = false;
            //Set category and tags
            Byte saveType = (Byte)Enums.DBAccess.ClassSaveType.UpdateCategoryATags;
            result = _commonService.UpdateClassEditInfo(saveType, classId, cateId, tags);
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
                    _commonService.UpdateMemberInfo(memberId, (Byte)Enums.DBAccess.MemberSaveType.UpdateCity, cityId.ToString(), cityNoClass ? "1" : "");
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

        [Obsolete]
        public JsonResult AddComment(int classId, String comment)
        {
            int memberId = WebContext.Current.MemberId;
            String deviceToken = "";
            _commonService.AddComment(memberId, classId, comment, out deviceToken);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}
