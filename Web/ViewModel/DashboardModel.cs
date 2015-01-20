using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.DataSource.Data;
//using SkillBank.Site.Services.Models;

namespace SkillBank.Site.Web.ViewModel
{
    public class DashboardModel
    {
        //public List<ClassInfo> ClassInfo;//Check should be classinfo or classitem,  public List<ClassItem> ClassList;
        public List<ClassInfo> ClassList;
        public List<ClassEditItem> ClassEditList;
        public List<MessageListItem> Messages;
        public Dictionary<int, int> UnReadMessageNumDic;
        public List<NotificationItem> ClassNotifications;
        public List<NotificationItem> OrderNotifications;
        public Boolean HasClass;
        public Boolean HasBasicInfo;
        public int UnfinishedClassId;
    }
}
