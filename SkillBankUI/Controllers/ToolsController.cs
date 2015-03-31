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

namespace SkillBank.Controllers
{
    public class ToolsController : Controller
    {
        //
        // GET: /Tools/
        public readonly ICommonService _commonService;

        public ToolsController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        public ActionResult Links()
        {
            int memberId = WebContext.Current.MemberId;

            List<String> whiteListMem = ConfigurationManager.AppSettings["MemberWhiteList"].Split(',').ToList<String>();
            ViewBag.IsAdmin = whiteListMem.Contains(memberId.ToString());
            ViewBag.MemberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;

            return View();
        }

        public ActionResult Coins()
        {
            int memberId = WebContext.Current.MemberId;
            List<String> whiteListMem = ConfigurationManager.AppSettings["MemberWhiteList"].Split(',').ToList<String>();
            if (whiteListMem.Contains(memberId.ToString()))
            {
                ViewBag.MemberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
                var classList = _commonService.GetClassEditInfoForAdmin(false);
                if (classList == null)
                {
                    ViewBag.ErrorMessage = ResourceHelper.GetTransText(585);
                }
                else
                {
                    classProveModel.ClassEditList = classList;
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
                var classList = _commonService.GetClassEditInfoForAdmin(true);
                if (classList == null)
                {
                    ViewBag.ErrorMessage = ResourceHelper.GetTransText(585);
                }
                else
                {
                    classProveModel.ClassEditList = classList;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ClassView(int id = 0)
        {
            var currMemberId = WebContext.Current.MemberId;
            var currMemberInfo = currMemberId > 0 ? _commonService.GetMemberInfo(currMemberId) : null;
            ViewBag.MemberInfo = currMemberInfo;

            String className = "";
            ClassPreviewModel classPreviewModel = new ClassPreviewModel();
            
            if (id > 0 && currMemberId > 0)
            {
                var classInfo = _commonService.GetClassInfoByClassId(id);
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
                        classPreviewModel.ClassInfo = classInfo;
                        var memberInfo = memberId.Equals(currMemberId) ? currMemberInfo : _commonService.GetMemberInfo(memberId);//class owner info
                        classPreviewModel.MemberInfo = memberInfo;
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

        public ActionResult Report()
        {
            ReportNumModel repModel = new ReportNumModel();
            var memberId = WebContext.Current.MemberId;
            List<String> whiteListMem = ConfigurationManager.AppSettings["MemberWhiteList"].Split(',').ToList<String>();
            if (whiteListMem.Contains(memberId.ToString()))
            {
                ViewBag.IsAdmin = true;
                repModel.ReportNumList = _commonService.GetReportClassMemberNum();
                return View(repModel);
            }
            else
            {
                ViewBag.IsAdmin = false;
            }
            return View();
        }

        public ActionResult OrderReport(String b = "", String e = "", Byte t = 0)
        {
            DateTime beginDate = String.IsNullOrEmpty(b) ? DateTime.Now.AddMonths(-1) : Convert.ToDateTime(b);
            DateTime endDate = String.IsNullOrEmpty(e) ? DateTime.Now.Date : Convert.ToDateTime(e);

            ReportOrderStatusModel repModel = new ReportOrderStatusModel();
            var memberId = WebContext.Current.MemberId;
            List<String> whiteListMem = ConfigurationManager.AppSettings["MemberWhiteList"].Split(',').ToList<String>();
            if (whiteListMem.Contains(memberId.ToString()))
            {
                ViewBag.IsAdmin = true;
                var result = _commonService.GetReportClassMemberNum(t, beginDate, endDate);
                if (t.Equals(0))
                {
                    repModel.RejectOrderList = result.Where(a => a.GroupId == 1).ToList();
                    repModel.FinshOrderList = result.Where(a => a.GroupId == 2).ToList();
                    repModel.WaitingOrderList = result.Where(a => a.GroupId == 3).ToList();
                    repModel.CancelOrderList = result.Where(a => a.GroupId == 4).ToList();
                    repModel.InprogressOrderList = result.Where(a => a.GroupId == 5).ToList();
                }
                else if (t.Equals(1))
                {
                    repModel.RejectOrderList = result;
                }
                else if (t.Equals(2))
                {
                    repModel.FinshOrderList = result;
                }
                else if (t.Equals(3))
                {
                    repModel.WaitingOrderList = result;
                }
                else if (t.Equals(4))
                {
                    repModel.CancelOrderList = result;
                }
                else if (t.Equals(5))
                {
                    repModel.InprogressOrderList = result;
                }
                return View(repModel);
            }
            else
            {
                ViewBag.IsAdmin = false;
            }
            return View();
        }


        public ActionResult MemberProfile(int id = 0)
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
                memberId = id;
                var memberInfo = _commonService.GetMemberInfo(memberId, currMemberId);
                if (memberInfo != null)
                {
                    likeNum = memberInfo.MemberId;
                    memberInfo.MemberId = memberId;
                    profileModel.MemberInfo = memberInfo;

                    String masterInfo = memberInfo.MasterInfo;
                    if (!String.IsNullOrEmpty(masterInfo))
                    {
                        var masterInfoDic = new Dictionary<Byte, String>();
                        masterInfo = masterInfo.Substring(0, masterInfo.Length - 1);
                        String[] paras = masterInfo.Split(';');
                        foreach (String para in paras)
                        {
                            String[] items = para.Split(',');
                            masterInfoDic.Add(Convert.ToByte(items[0]), items[1]);
                        }
                        profileModel.MasterInfos = masterInfoDic;
                    }
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
            profileModel.ClassList = _commonService.GetClassInfo((Byte)Enums.DBAccess.ClassLoadType.ByTeacherPublished, 0, memberId);
            
            var metaTags = MetaTagHelper.GetMetaTags("profile");
            ViewBag.MetaTagTitle = metaTags[0].Replace("{0}", userName);
            ViewBag.MetaTagKeyWords = metaTags[1];
            ViewBag.MetaTagDescription = metaTags[2];

            return View(profileModel);
        }

        public ActionResult SearchMemeber(Byte t, string k)
        {
            ViewBag.Key = k;
            ViewBag.Type = t;

            ViewBag.MemberInfos = _commonService.GetMemberInfos(t, k);

            return View();
        }

        public ActionResult APITest()
        {
            //method 1
            string osPat = "mozilla|m3gate|winwap|openwave|Windows NT|Windows 3.1|95|Blackcomb|98|ME|X Window|Longhorn|ubuntu|AIX|Linux|AmigaOS|BEOS|HP-UX|OpenBSD|FreeBSD|NetBSD|OS/2|OSF1|SUN";
            string uAgent = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex reg = new Regex(osPat);
            if (reg.IsMatch(uAgent))
            {
                ViewBag.Divice1 = "电脑访问";
            }
            else
            {
                ViewBag.Divice1 = "手机访问";
            }

            //method 2
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            if (b.IsMatch(u) || v.IsMatch(u.Substring(0, 4)))
            {
                ViewBag.Divice2 = "手机访问";
            }
            else
            {
                ViewBag.Divice2 = "电脑访问";
            }

            // Tablet
            Regex regexTablet = new Regex("^.*iPad.*$|^.*tablet.*$|^.*Android\\s3.*$|^(?!.*Mobile.*).*Android.*$", RegexOptions.IgnoreCase);
            // Mobile
            Regex regexMobile = new Regex("^.*(iPhone|iPod|Android.*Mobile|Windows\\sPhone|IEMobile|BlackBerry|Mobile).*$", RegexOptions.IgnoreCase);


            ViewBag.Divice3 = regexTablet.IsMatch(u);
            ViewBag.Divice4 = regexMobile.IsMatch(u);
            ViewBag.Agent = u;

            return View();
        }

        #region ajax function

        [HttpPost]
        public JsonResult CoinUpdate(Byte type, int member, int id, int amount)
        {
            var result = _commonService.CoinUpdate(type, member, id, amount);
            if (result)
            {
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SetRecommendationClass(int classId, string paraStr)
        {
            if (!String.IsNullOrEmpty(paraStr))
            {
                paraStr = paraStr.Substring(0, paraStr.Length - 1);
            }
            _commonService.SaveRecommendationClass(classId, paraStr, ';');
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveMasterMember(int memberId, string paraStr)
        {
            paraStr = paraStr.Substring(0, paraStr.Length - 1);
            _commonService.SaveMasterMember(memberId, paraStr, ';');
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UnbindAccount(Byte type, String account)
        {
            int memberId = 0;
            String verifyAccount = "";
            Boolean isValid = false;
            if (type.Equals((Byte)Enums.DBAccess.MobileVerificationSaveType.UnbindSocialByMemberid))
            {
                memberId = Convert.ToInt32(account);
                isValid = true;
            }
            else if (type.Equals((Byte)Enums.DBAccess.MobileVerificationSaveType.UnbindMobile) || type.Equals((Byte)Enums.DBAccess.MobileVerificationSaveType.UnbindSocialBySocialid))
            {
                verifyAccount = account;
                isValid = true;
            }
            if (isValid)
            {
                _commonService.UpdateVerification(type, memberId, verifyAccount);
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        
        #endregion
    }
}
