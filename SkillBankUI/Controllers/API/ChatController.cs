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
            public int ToId { get; set; }
            public String MessageText { get; set; }
            public String FromName { get; set; }
            public String ToMobile { get; set; }
        }
        //
        // GET: /Message/

        public ChatController(ICommonService commonService)
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
                //LoadNotificationAlert(memberId);
                var messages = _commonService.GetMessageList(memberId);
                var unReadMessageNum = _commonService.GetMessageUnReadNum(memberId);
                foreach (var item in messages)
                {
                    var contactId = item.From_Id.Equals(memberId) ? item.To_Id : item.From_Id;
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
        public List<Message> GetDetail(int id)
        {
            int contactId = id;
            int memberId = GetMemberId(true);
            
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
                Boolean sendNotify = System.Configuration.ConfigurationManager.AppSettings["ENV"].Equals(ConfigConstants.EnvSetting.LiveEnvName);
                if (sendNotify)
                {
                    if (result.Equals(2))
                    {
                        _commonService.SendNewMessageSMS(chatItem.ToMobile, chatItem.FromName, Constants.PageURL.MobileMessagePage, true);
                    }
                    else
                    {
                        //SendCloudEmail.SendMessageReceiveMail(mremail, chatItem.t, chatItem.FromName, Constants.PageURL.MessagePage);
                    }
                }
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
