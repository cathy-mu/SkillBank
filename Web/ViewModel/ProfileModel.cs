using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.Web.ViewModel
{
    public class ProfilelModel
    {
        public MemberInfo MemberInfo;
        public List<ClassInfo> ClassList;
        public List<MemberReviewItem> StuentReview;
        public List<MemberReviewItem> TeacherReview;
        public Dictionary<Enum, int> ProfileNumDic;
        public Dictionary<Byte, String> MasterInfos;
    }
}
