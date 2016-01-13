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

namespace SkillBankWeb.API
{
    public class FansController : ApiController
    {
        public readonly ICommonService _commonService;

        public class FansModel
        {
            public String Avatar;
            public List<FavoriteItem> Fans;
        }
        
        public FansController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        /// <summary>
        /// Get like/follow member list for Fans/Following page(Public) (v1.0 15-08-18)
        /// </summary>
        /// <param name="id">MemberId</param>
        /// <param name="relatedId">ViewerId</param>
        /// <param name="type">1:Show Following by Member And ViewerId 2:Show Fans by Member And ViewerId</param>
        /// <returns></returns>
        public FansModel Get(int id = 0, int relatedId = 0, Byte type = (Byte)Enums.DBAccess.FavoriteLoadType.ByFansMemberAViewerId)//
        {
            var memberInfo = id > 0 ? _commonService.GetMemberInfo(id) : null;
            var fans = _commonService.GetFavorites(type, id, relatedId);// or (Byte)Enums.DBAccess.FavoriteLoadType.ByFollwingMemberAViewerId;

            FansModel model = new FansModel();
            model.Avatar = memberInfo.Avatar;
            model.Fans = fans;
            return model;
        }

    }
}
