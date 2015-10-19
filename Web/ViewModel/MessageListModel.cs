using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.Web.ViewModel
{
    public class MessageListModel
    {
        public List<MessageListItem> Messages;
        public Dictionary<int,int> UnReadMessageNumDic;
        public List<NotificationItem> Notifications;
    }
}
