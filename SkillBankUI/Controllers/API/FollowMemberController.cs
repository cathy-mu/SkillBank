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
    public class FollowMemberController : ApiController
    {
        //public readonly ICommonService _commonService;
        public readonly ICommonService _commonService;
        public class FollowMemberItem
        {
            public int MemberId { get; set; }
            public int FollowingId { get; set; }
            public Boolean IsFollow { get; set; }
        }
        //
        // GET: /Message/

        public FollowMemberController(ICommonService commonService)
        {
            //_contentService = contentService;
            _commonService = commonService;
        }
             
        public Boolean UpdateFollowMember(FollowMemberItem item)
        {
            _commonService.UpdateMemberLikeTag(item.MemberId, item.FollowingId, item.IsFollow);
            return true;
        }


    }
}

