using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.Web.ViewModel
{
    public class MessageDetailModel
    {
        public List<Message> Messages;
        public MemberInfo Contact;
        public List<ClassInfo> ContactClass;
        public int MaxMessageId;
    }
}
