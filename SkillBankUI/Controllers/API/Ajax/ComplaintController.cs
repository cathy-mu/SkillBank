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

namespace SkillBankWeb.API
{
    public class ComplaintController : ApiController
    {
        public readonly ICommonService _commonService;
        public class ComplaintItem
        {
            public int MemberId { get; set; }
            public int RelatedId { get; set; }
            public Byte Type { get; set; }
        }
        //
        // GET: /Message/

        public ComplaintController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        public string Options()
        {
            return null; // HTTP 200 response with empty body 
        }
        /// <summary>
        /// Add complaint
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="RelatedId"></param>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        [HttpPost]
        public Boolean Post(ComplaintItem item)
        {
            int memberId = item.MemberId.Equals(0) ? WebContext.Current.MemberId : item.MemberId;
            if (memberId > 0)
            {
                Byte saveType = (Byte)Enums.DBAccess.ComplaintSaveType.Add;
                var result = _commonService.UpdateComplaint(saveType, memberId, item.RelatedId, item.Type);
                if (result.Equals(1))
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

        /// <summary>
        /// Disactive complaint for admin
        /// </summary>
        /// <param name="RelatedId">Complaint indentity key id</param>
        /// <returns></returns>
        [HttpPut]
        public Boolean Put(int id)
        {
            int memberId = WebContext.Current.MemberId;
            List<String> whiteListMem = System.Configuration.ConfigurationManager.AppSettings["MemberWhiteList"].Split(',').ToList<String>();
            if (whiteListMem.Contains(memberId.ToString()))
            {
                Byte saveType = (Byte)Enums.DBAccess.ComplaintSaveType.DisActive;
                var result = _commonService.UpdateComplaint(saveType, 0, id, (Byte)0);
                if (result.Equals(1))
                {
                    return true;
                }
            }
            return false;
        }

        //[HttpGet]
        //public Boolean Get(int MemberId = 0)
        //{
        //    return true;
        //}
    }
}
