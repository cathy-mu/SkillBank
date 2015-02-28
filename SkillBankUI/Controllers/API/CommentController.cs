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
    public class CommentController : ApiController
    {
        //public readonly ICommonService _commonService;
        public readonly ICommonService _commonService;
        public class CommentItem
        {
            //public int MemberId { get; set; }
            public int ClassId { get; set; }
            public String CommentText { get; set; }
        }
        //
        // GET: /Message/

        public CommentController(ICommonService commonService)
        {
            //_contentService = contentService;
            _commonService = commonService;

        }
        
        
        public Boolean AddComment(CommentItem item)
        {
            int memberId = WebContext.Current.MemberId;
            if (memberId > 0)
            {
                var result = _commonService.AddComment(memberId, item.ClassId, item.CommentText);
                if (result>0) 
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
