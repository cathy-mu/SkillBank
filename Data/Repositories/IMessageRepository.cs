using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Data.Entity;

using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using SkillBank.Site.DataSource.Mapper;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;

namespace SkillBank.Site.DataSource.Data
{
    public interface IMessageRepository
    {
        int UpdateMessage(Byte saveType, int id, int fromId, int toId, String messageText);

        List<MessageListItem> GetLastestMessagesByMemberId(int memberId);
        List<Message> GetMessagesByFromToId(int memberId1, int memberId2);
        List<MessageUnReadItem> GetMessageUnReadNum(int memberId);
    }

    public class MessageRepository : Entities, IMessageRepository
    {
        public MessageRepository()
        //: base("name=Entities")
        {
        }

        public int UpdateMessage(Byte saveType, int id, int fromId, int toId, String messageText)
        {
            return Message_Save_p(saveType, id, fromId, toId, messageText);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<MessageListItem> GetLastestMessagesByMemberId(int memberId)
        {
            var paraIdPara = new ObjectParameter("ParaId", memberId);

            ObjectResult<Message_LoadByMemberId_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Message_LoadByMemberId_p_Result>("Message_LoadByMemberId_p", MergeOption.NoTracking, paraIdPara);
            return MessageMapper.Map(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Message> GetMessagesByFromToId(int memberId1, int memberId2)
        {
            var memberId1Para = new ObjectParameter("MemberId1", memberId1);
            var memberId2Para = new ObjectParameter("MemberId2", memberId2);

            var Context = ((IObjectContextAdapter)this).ObjectContext;
            var result = Context.ExecuteFunction<Message_LoadByFromAndToId_p_Result>("Message_LoadByFromAndToId_p", MergeOption.NoTracking, memberId1Para, memberId2Para);

            return MessageMapper.Map(result);
        }

        public List<MessageUnReadItem> GetMessageUnReadNum(int memberId)
        {
            return Message_LoadUnReadNo_p(memberId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="id"></param>
        /// <param name="fromId"></param>
        /// <param name="toId"></param>
        /// <param name="messageText"></param>
        /// <returns></returns>
        private int Message_Save_p(Byte saveType, int id, int fromId, int toId, String messageText)
        {
            var saveTypePara = new ObjectParameter("SaveType", saveType);
            var idPara = new ObjectParameter("Id", id);
            var fromIdPara = new ObjectParameter("FromId", fromId);
            var toIdPara = new ObjectParameter("ToId", toId);
            var messageTextIdPara = new ObjectParameter("Message", messageText);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Message_Save_p", fromIdPara, toIdPara, messageTextIdPara, saveTypePara, idPara);
            return (int)idPara.Value;
        }

        /// <summary>
        /// Un read item count for Message List page
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        private List<MessageUnReadItem> Message_LoadUnReadNo_p(int memberId)
        {
            var memberIdPara = new ObjectParameter("MemberId", memberId);

            ObjectResult<Message_LoadUnReadNo_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Message_LoadUnReadNo_p_Result>("Message_LoadUnReadNo_p", MergeOption.NoTracking, memberIdPara);
            return MessageMapper.Map(result);
        }

    }
}


