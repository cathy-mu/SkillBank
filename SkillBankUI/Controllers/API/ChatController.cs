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
        /// GET api/chat/?id=1
        /// </summary>
        /// <param name="id">contact member id</param>
        /// <returns></returns>
        public List<Message> Get(int id)
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

        public Boolean Post(ChatItem chatItem)
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
