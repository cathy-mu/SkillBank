using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;

namespace SkillBankWeb.API
{
    public class MemberInfoController : ApiController
    {
        public readonly ICommonService _commonService;

        public class MemberInfoItem
        {
            public int MemberId;
            public String Avatar;
            public String Name;
            
            //added for mobile API，not use yet
            public int Coins { get; set; }
            //public int CoinsLocked { get; set; }
            public int Credit { get; set; }
            public Boolean IsSignIn { get; set; }
        }

        //Not use yet ,leave it to check if should there
        [Obsolete]
        public MemberInfoController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        /// <summary>
        /// Profile edit page (public) v1.0
        /// </summary>
        /// <param name="id">page owner id</param>
        /// <param name="viewerId">current viewer member id</param>
        /// <returns></returns>
        public MemberInfoItem Get(int id = 0)
        {
            MemberInfo memberInfo = id > 0 ? _commonService.GetMemberInfo(id) : null;
            if (memberInfo != null)
            {
                var profileInfo = new MemberInfoItem() { MemberId = memberInfo.MemberId, Name = memberInfo.Name, Avatar = memberInfo.Avatar, Coins = memberInfo.Coins };
                return profileInfo;
            }
            //member not exists or disactive
            return null;
        }

        

    }
}
