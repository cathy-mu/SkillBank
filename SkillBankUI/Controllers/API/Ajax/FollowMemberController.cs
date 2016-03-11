using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;

using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Services.Utility;

namespace SkillBankWeb.API
{
    public class FollowMemberController : ApiController
    {
        public readonly ICommonService _commonService;

        public class FollowMemberItem
        {
            public int? MemberId { get; set; }
            public int FollowingId { get; set; }
            public Boolean IsFollow { get; set; }
        }
        //
        // GET: /Message/

        public FollowMemberController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        public string Options()
        {
            return null; // HTTP 200 response with empty body 
        }

        [HttpPost]
        public Boolean UpdateFollowMember(FollowMemberItem item)
        {
            int memberId = (item.MemberId.HasValue) ? item.MemberId.Value : WebContext.Current.MemberId;
            String deviceToken = "";

            if (memberId > 0)
            {
                _commonService.UpdateMemberLikeTag(memberId, item.FollowingId, item.IsFollow, out deviceToken);
                //push liked notification
                if (item.IsFollow && !String.IsNullOrEmpty(deviceToken))
                {
                    PushManager.PushNotification(deviceToken, (Byte)Enums.PushNotificationType.Followed);
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

