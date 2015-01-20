using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.DataSource.Mapper
{
    public class MessageMapper
    {
        public static List<Message> Map(ObjectResult<Message> objMessages)
        {
            if (objMessages != null)
            {
                var messages = objMessages.ToList<Message>();
                //var messages = objMessages.Select(item => new Message() { MessageId = item.MessageId, MessageText = item.MessageText, From_Id = item.From_Id, To_Id = item.To_Id, IsLatest = item.IsLatest, IsRead = item.IsRead, CreatedDate = item.CreatedDate, Show4From = item.Show4From, Show4To = item.Show4To }).ToList();
                return (messages.Count > 0) ? messages : null;
            }
            return null;
        }

        public static List<MessageListItem> Map(ObjectResult<MessageListItem> objMessages)
        {
            if (objMessages != null)
            {
                var messages = objMessages.Select(item => new MessageListItem() { MessageId = item.MessageId, Avatar = item.Avatar, CreatedDate = item.CreatedDate, From_Id = item.From_Id, MessageText = item.MessageText, Name = item.Name, To_Id = item.To_Id }).ToList();
                return (messages.Count > 0) ? messages : null;
            }
            return null;
        }

        public static List<MessageUnReadItem> Map(ObjectResult<MessageUnReadItem> objMessageUnReadItems)
        {
            var result = new Dictionary<int, int>();
            if (objMessageUnReadItems != null)
            {
                var items = objMessageUnReadItems.Select(item => new MessageUnReadItem() { From_Id = item.From_Id, Num = item.Num }).ToList();
                return (items.Count > 0) ? items : null;
            }
            return null;
        }

        //SQL
        //public static List<MessageListItem> Map(ObjectResult<Message_LoadByMemberId_p_Result> objMessages)
        //{
        //    if (objMessages != null)
        //    {
        //        var messages = objMessages.Select(item => new MessageListItem() { MessageId = item.MessageId, Avatar = item.Avatar, CreatedDate = item.CreatedDate, From_Id = item.From_Id, MessageText = item.MessageText, Name = item.Name, To_Id = item.To_Id }).ToList();
        //        return (messages.Count > 0) ? messages : null;
        //    }
        //    return null;
        //}




        //public static List<MessageUnReadItem> Map(ObjectResult<Message_LoadUnReadNo_p_Result> objMessageUnReadItems)
        //{
        //    var result = new Dictionary<int, int>();
        //    if (objMessageUnReadItems != null)
        //    {
        //        var items = objMessageUnReadItems.Select(item => new MessageUnReadItem() { From_Id = item.From_Id, Num = item.Num }).ToList();
        //        return (items.Count > 0) ? items : null;
        //    }
        //    return null;
        //}

    }
}
