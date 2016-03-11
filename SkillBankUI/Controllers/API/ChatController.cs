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
using SkillBank.Site.Services.Utility;

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
            public Boolean HasRCToken { get; set; }
        }
        
        //
        // For mobile site to save message and send message to rong cloud
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
                String envName = System.Configuration.ConfigurationManager.AppSettings["ENV"];
if (!chatItem.HasRCToken)
                    {
                        SaveRongCloudToken(memberId);
                    }
                if (envName.Equals(ConfigConstants.EnvSetting.LiveEnvName))
                {
                    

                    //App user(already got rong cloud token)， push rong cloud message 
                    if (result.Equals(3))
                    {
                        RongCloudHelper.PushMessage(envName, memberId.ToString(), toId.ToString(), messageText);
                    }
                    //web user with phone number ,and first message between 2 users
                    else if (result.Equals(2))
                    {
                        _commonService.SendNewMessageSMS(chatItem.ToMobile, chatItem.FromName, Constants.PageURL.MobileMessagePage, true);
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Generate and save current user's RongCloud Token
        /// </summary>
        /// <param name="memberId"></param>
        private void SaveRongCloudToken(int memberId)
        {
            MemberInfo memberInfo = _commonService.GetMemberInfo(memberId);
            //Generate and save own RongCloud Token
            if (memberInfo != null && String.IsNullOrEmpty(memberInfo.RCToken))
            {
                String rcToken = RongCloudHelper.GetToken(System.Configuration.ConfigurationManager.AppSettings["ENV"], memberInfo.MemberId, memberInfo.Name, memberInfo.Avatar);
                if (!String.IsNullOrEmpty(rcToken))
                {
                    Byte type = (Byte)Enums.DBAccess.MemberSaveType.UpdateRCTokenADeviceToken;
                    MemberInfo updateInfo = new MemberInfo() { MemberId = memberInfo.MemberId, Avatar = rcToken };
                    var updateResult = _commonService.UpdateMemberProfile(updateInfo, type);
                }
            }
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
