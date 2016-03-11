using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;
using SkillBank.Site.Services.Utility;

namespace SkillBankWeb.API
{
    public class LikeClassController : ApiController
    {
        //public readonly ICommonService _commonService;
        public readonly ICommonService _commonService;
        public class LikeClassItem
        {
            public int? MemberId { get; set; }
            public int ClassId { get; set; }
            public Boolean IsLike { get; set; }
        }
        //
        // GET: /Message/

        public LikeClassController(ICommonService commonService)
        {
            //_contentService = contentService;
            _commonService = commonService;
        }

        public string Options()
        {
            return null; // HTTP 200 response with empty body 
        }

        [HttpPost]
        public Boolean UpdateLikeTag(LikeClassItem item)
        {
            int memberId = (item.MemberId.HasValue) ? item.MemberId.Value : WebContext.Current.MemberId;
            String deviceToken = "";
            if (memberId > 0)
            {
                _commonService.UpdateClassLikeTag(memberId, item.ClassId, item.IsLike, out deviceToken);
                //push liked notification
                if (item.IsLike && !String.IsNullOrEmpty(deviceToken))
                {
                    PushManager.PushNotification(deviceToken, (Byte)Enums.PushNotificationType.ClassLiked);
                }
                return true;
            }
            else
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.StatusCode = 401;
                HttpContext.Current.Response.End();
            }
            return false;
        }

        


    }
}
