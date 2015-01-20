using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.DataSource.Data;
//using SkillBank.Site.Services.Models;

namespace SkillBank.Site.Web.ViewModel
{
    public class ClassDetailModel
    {
        public ClassEditItem ClassInfo;
        public MemberInfo MemberInfo;
        public MemberInfo MyInfo;
        public List<MemberReviewItem> OtherClassReview;
        public List<MemberReviewItem> ClassReview;
        public List<MemberReviewItem> ClassComment;

        public List<ClassInfo> ClassList;
        public Boolean IsOwner;
        public Boolean IsLogin;
        public int[] ClassNums;//0Class:Finished Orders, 1Student:Finished Order Related Number, 2 Rank, 3 Review Review nums for current class
        public Dictionary<String, int> ClassNumDic;
    }
}
