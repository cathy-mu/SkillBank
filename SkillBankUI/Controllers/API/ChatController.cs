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
    public class ChatController : ApiController
    {
        //public readonly ICommonService _commonService;
        public readonly ICommonService _commonService;
        public class ChatItem
        {
            //public int FromId { get; set; }
            public int ToId { get; set; }
            public String MessageText { get; set; }
        }
        //
        // GET: /Message/

        public ChatController(ICommonService commonService)
        {
            //_contentService = contentService;
            _commonService = commonService;

        }
        
        /// <summary>
        /// GET api/chat/1
        /// </summary>
        /// <param name="id">memberId</param>
        /// <returns></returns>
        public List<MessageListItem> GetItem(int id)
        {
            int memberid = id;
            if (memberid > 0)
            {
                //LoadNotificationAlert(memberId);
                var messages = _commonService.GetMessageList(memberid);
                var unReadMessageNum = _commonService.GetMessageUnReadNum(memberid);
                foreach (var item in messages)
                {
                    var contactId = item.From_Id.Equals(memberid) ? item.To_Id : item.From_Id;
                    item.UnReadNumber = (unReadMessageNum!=null&&unReadMessageNum.ContainsKey(contactId)) ? unReadMessageNum[contactId] : 0;
                }
                return messages;
            }
            return null;
        }

        /// <summary>
        /// GET api/chat/?from=1&to=2
        /// </summary>
        /// <param name="from">from member id</param>
        /// <param name="to">to member id</param>
        /// <returns></returns>
        public List<Message> GetDetail(int id, int contact)
        {
            int memberId = id;
            int contactId = contact;
            if (memberId > 0)
            {
                var messages = _commonService.GetMessageDetail(memberId, contactId);
                //TO DO:set message as read after test
                //_commonService.SetMessageAsRead(0, memberId, contactId);
                
                return messages;
            }
            return null;
        }

        public Boolean AddMessage(ChatItem chatItem)
        {
            int memberId = GetMemberId(true);
            if (memberId > 0)
            {
                int toId = chatItem.ToId;
                String messageText = chatItem.MessageText;
                var result = _commonService.AddMessage(memberId, toId, messageText);
                return true;
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
