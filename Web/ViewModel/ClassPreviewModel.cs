using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.Web.ViewModel
{
    public class ClassPreviewModel
    {
        public ClassInfo ClassInfo;//TO DO:4 Class Edit Process 1.1, Remove after roll out
        public ClassEditItem ClassEditInfo;
        public MemberInfo MemberInfo;

        public List<ClassItem> ClassList;
        public Boolean IsOwner;
        public Boolean IsAdmin;
        public int[] ClassCounter;//0Class:Finished Orders, 1Student:Finished Order Related Number, 2Review Review nums for current class

        //public List<RecommendationItem> RecommendationList;
        public Dictionary<Byte, RecommendationItem> RecommendationDic;
        public List<ClassCategory> ClassCategoryLkp;
    }
}
