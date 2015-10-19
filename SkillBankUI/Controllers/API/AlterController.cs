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
    public class AlterController : ApiController
    {
        public readonly ICommonService _commonService;


        public AlterController(ICommonService commonService)
        {
            //_contentService = contentService;
            _commonService = commonService;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">1:Update Notification to altered(Use server code ,but not API for now) 2:Update messages to altered</param>
        /// <returns></returns>
        public Boolean UpdateAlter(Byte type)
        {
            int memberId = GetMemberId(true);
            if (memberId > 0)
            {
                //(Byte)Enums.DBAccess.NotificationTagUpdateType
                _commonService.UpdateNotification(type, memberId);
                return true;
            }
            return false;
        }


        private int GetMemberId(Boolean shouldAuthorize)
        {

            int memberId = WebContext.Current.MemberId;
            if (shouldAuthorize && memberId == 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.StatusCode = 401;
                HttpContext.Current.Response.End();
            }
            return memberId;
        }

    }
}
