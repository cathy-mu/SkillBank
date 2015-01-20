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
            public int MemberId { get; set; }
            public int ClassId { get; set; }
            public DateTime BookDate { get; set; }
            public String Remark { get; set; }
            public String Name { get; set; }
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
            item.Name = String.IsNullOrEmpty(item.Name) ? "" : item.Name;
            //item.Mobile = String.IsNullOrEmpty(item.Mobile) ? "" : item.Mobile;

            var result = _commonService.AddOrder(item.MemberId, item.ClassId, item.BookDate, item.Remark, item.Name);
            return result;
        }

    }
}
