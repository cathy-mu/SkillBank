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

namespace SkillBankWeb.Controllers.API
{
    public class OrderController : ApiController
    {
        //public readonly ICommonService _commonService;
        public readonly ICommonService _commonService;

        public class OrderItem
        {
            //public int MemberId { get; set; }
            public int ClassId { get; set; }
            public DateTime BookDate { get; set; }
            public String Remark { get; set; }
            public String Name { get; set; }
            //public String Phone { get; set; }
        }
        //public String Mobile { get; set; }
        //
        // GET: /Message/

        public OrderController(ICommonService commonService)
        {
            //_contentService = contentService;
            _commonService = commonService;

        }

        public Boolean AddOrder(OrderItem item)//Boolean
        {
            Boolean result = false;
            try
            {
                int memberId = GetMemberId(true);
                item.Remark = String.IsNullOrEmpty(item.Remark) ? "" : item.Remark;
                item.Name = String.IsNullOrEmpty(item.Name) ? "" : item.Name;
                if (item.BookDate > DateTime.Now.Date)
                {
                    result = _commonService.AddOrder(memberId, item.ClassId, item.BookDate, item.Remark, item.Name);
                }
            }
            catch
            {
                return result;
            }
            return result;
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
