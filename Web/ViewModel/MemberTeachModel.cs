using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.Web.ViewModel
{
    public class MemberTeachModel
    {
        public List<ClassInfo> ClassList;//TO DO:4 Class Edit 1.1, Remove after roll out
        public List<ClassEditItem> ClassEditList;
        public List<OrderItem> Orders;
        public Boolean IsMemberInfoCompleted;
    }
}
