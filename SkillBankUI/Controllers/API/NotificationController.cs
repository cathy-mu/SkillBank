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
using SkillBank.Site.Web.ViewModel;

namespace SkillBankWeb.Controllers.API
{
    public class NotificationController : ApiController
    {
        public readonly ICommonService _commonService;
        public class NotificationItem
        {
            public int Id { get; set; }
            public Byte Type { get; set; }
        }
        
        //
        // GET: /Message/

        public NotificationController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        public void Post(NotificationItem item)
        {
            int memberId = WebContext.Current.MemberId;
            if(memberId>0)
            {
                _commonService.UpdateNotification(item.Type, memberId, 0);
            }
        }

        
    }
}
