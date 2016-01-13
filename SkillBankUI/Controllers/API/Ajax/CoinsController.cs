using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;

namespace SkillBankWeb.Controllers.API
{
    public class CoinsController : ApiController
    {
        public readonly ICommonService _commonService;

        public class CoinUpdateItem
        {
            public int MemberId { get; set; }
            public Byte UpdateType { get; set; }
        }

        public CoinsController(ICommonService commonService)
        {
            //_contentService = contentService;
            _commonService = commonService;
        }

        public string Options()
        {
            return null; // HTTP 200 response with empty body 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public Boolean UpdateCoin(CoinUpdateItem item)
        {
            int memberId = item.MemberId.Equals(0) ? GetMemberId(true) : item.MemberId;
            if (memberId > 0 && item.UpdateType.Equals(0))
            {
                var result = _commonService.AddMembersCoin(memberId, Constants.BizConfig.FreeCoinForShareClass, (Byte)Enums.DBAccess.CoinUpdateType.ClassShareOnSocial);
                return !result;
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
