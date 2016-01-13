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
    public class ChatListController : ApiController
    {
        public readonly ICommonService _commonService;

        //
        // GET: /Message/

        public ChatListController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        /// <summary>
        /// GET api/chat/1
        /// </summary>
        /// <param name="id">memberId</param>
        /// <returns></returns>
        public List<MessageListItem> Get()
        {
            int memberId = GetMemberId(true);
            if (memberId > 0)
            {
                var messages = _commonService.GetMessageList(memberId);
                var unReadMessageNum = _commonService.GetMessageUnReadNum(memberId);
                foreach (var item in messages)
                {
                    var contactId = item.From_Id.Equals(memberId) ? item.To_Id : item.From_Id;
                    item.UnReadNumber = (unReadMessageNum != null && unReadMessageNum.ContainsKey(contactId)) ? unReadMessageNum[contactId] : 0;
                }
                return messages;
            }
            return null;
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
