﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;

namespace SkillBank.Site.Services.Managers
{
    public interface IMessageManager
    {
        int AddMessage(int fromId, int toId, String messageText);
        Boolean SetMessageAsRead(int maxId, int memberId, int contactorId);
        Boolean SetMessageAsDeleted(int maxId, int memberId, int contactorId);

        List<MessageListItem> GetLastestMessagesByMemberId(int memberId, int pageSize, Byte loadBy);
        List<Message> GetMessagesByFromToId(int memberId1, int memberId2, Byte loadBy);
        Dictionary<int, int> GetMessagesUnReadNum(int memberId);
        List<ComplaintItem> GetComplaintList(Byte loadType);
        Byte UpdateComplaint(Byte saveType, int memberId, int relatedId, Byte type);
    }

    public class MessageManager : IMessageManager
    {
        private readonly IMessageRepository _messageRep;

        public MessageManager(IMessageRepository messageRep)
        {
            _messageRep = messageRep;
        }

        public Byte UpdateComplaint(Byte saveType, int memberId, int relatedId, Byte type)
        {
            return _messageRep.UpdateComplaint(saveType, memberId, relatedId, type);
        }

        public List<ComplaintItem> GetComplaintList(Byte loadType)
        {
            return _messageRep.GetComplaintList(loadType);
        }

        public int AddMessage(int fromId, int toId, String messageText)
        {
            Byte saveType = (Byte)Enums.DBAccess.MessageSaveType.Add;
            int id = 0;
            var result = _messageRep.UpdateMessage(saveType, id, fromId, toId, messageText);
            return result;
        }

        /// <summary>
        /// Set Message from member with fromId to toId as Read
        /// </summary>
        /// <param name="maxId"></param>
        /// <param name="memberId">Action Sender, Action is send by toId</param>
        /// <param name="contactorId"></param>
        /// <returns></returns>
        public Boolean SetMessageAsRead(int maxId, int memberId, int contactorId)
        {
            Byte saveType = (Byte)Enums.DBAccess.MessageSaveType.SetAsRead;
            String messageText = "";
            var result = _messageRep.UpdateMessage(saveType, maxId, contactorId, memberId, messageText);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxId"></param>
        /// <param name="memberId">Action Sender, Action is send by fromId</param>
        /// <param name="contactorId"></param>
        /// <returns></returns>
        public Boolean SetMessageAsDeleted(int maxId, int memberId, int contactorId)
        {
            Byte saveType = (Byte)Enums.DBAccess.MessageSaveType.Delete;
            String messageText = "";
            var result = _messageRep.UpdateMessage(saveType, maxId, memberId, contactorId, messageText);
            return true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public List<MessageListItem> GetLastestMessagesByMemberId(int memberId, Byte loadBy)
        {
            var messages = _messageRep.GetLastestMessagesByMemberId(memberId, loadBy);
            if (messages != null && messages.Count > 0)
            {
                return messages.ToList();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<MessageListItem> GetLastestMessagesByMemberId(int memberId, int pageSize = 0, Byte loadBy = 1)
        {
            var messages = GetLastestMessagesByMemberId(memberId, loadBy);
            if (messages != null && pageSize != 0 && messages.Count > pageSize)
            {
                var count = messages.Count - pageSize + 1;
                messages.RemoveRange(pageSize - 1, count);
            }

            return messages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId1"></param>
        /// <param name="memberId2"></param>
        /// <returns></returns>
        public List<Message> GetMessagesByFromToId(int memberId1, int memberId2, Byte loadBy)
        {
            return _messageRep.GetMessagesByFromToId(memberId1, memberId2, loadBy);
        }
               
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public Dictionary<int, int> GetMessagesUnReadNum(int memberId)
        {
            var messageUnNum = _messageRep.GetMessageUnReadNum(memberId);
            if (messageUnNum != null && messageUnNum.Count > 0)
            {
                Dictionary<int, int> UnReadMessageDic = new Dictionary<int, int>();
                foreach (var item in messageUnNum)
                {
                    //UnReadMessageDic.Add(item.From_Id, item.Num);
                    UnReadMessageDic.Add(item.From_Id, item.Num.HasValue ? item.Num.Value : 0);
                }
                return UnReadMessageDic;
            }
            return null;
        }
        
    }

}
