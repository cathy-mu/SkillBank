using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;

namespace SkillBank.Site.Services.Managers
{
    public interface IFeedBackManager
    {
        List<TeacherReviewItem> GetTeacherReviewsByMemberId(int memberId, int maxReviewId = 0);
        //List<StudentReviewItem> GetStudentReviewsByMemberId(int memberId, int maxReviewId = 0);
        Boolean AddStudentReview(int orderId, int classId, Byte feedBack, String comment, String privateComment);
        Boolean AddTeacherReview(int orderId, Byte feedBack, String comment, String privateComment);
        List<MemberReviewItem> GetClassReviews(Byte loadBy, int memberId, int classId, Byte feedback = 0, int maxReviewId = 0);
        List<MemberReviewItem> GetMemberReviews(Byte loadBy, int memberId, Byte feedback = 0, int maxReviewId = 0);
    }

    public class FeedBackManager : IFeedBackManager
    {
        private readonly IStudentReviewRepository _stdReviewRep;
        private readonly ITeacherReviewRepository _teaReviewRep;

        public FeedBackManager(IStudentReviewRepository stdReviewRep, ITeacherReviewRepository teaReviewRep)
        {
            _stdReviewRep = stdReviewRep;
            _teaReviewRep = teaReviewRep;
        }

        //public List<StudentReviewItem> GetStudentReviewsByMemberId(int memberId, int maxReviewId = 0)
        //{
        //    //TO DO:1.2 Temply load all , no paging and just by memeber id
        //    Byte loadBy = (Byte)Enums.DBAccess.StudentReviewLoadType.ByMemberId;
        //    int classId = 0;
        //    var result = _stdReviewRep.GetStudentReviews(loadBy, memberId, classId, maxReviewId);
        //    return result;
        //}

        public List<TeacherReviewItem> GetTeacherReviewsByMemberId(int memberId, int maxReviewId = 0)
        {
            //TO DO:1.2 Temply load all , no paging and just by memeber id
            Byte loadBy = (Byte)Enums.DBAccess.StudentReviewLoadType.ByMemberId;
            var result = _teaReviewRep.GetTeacherReviews(loadBy, memberId, maxReviewId);
            return result;
        }

        public Boolean AddStudentReview(int orderId, int classId, Byte feedBack, String comment, String privateComment)
        {
            var result = _stdReviewRep.AddStudentReview(orderId, classId, 0, 0, 0, feedBack, comment, privateComment);
            return (result > 0);
        }

        public Boolean AddTeacherReview(int orderId, Byte feedBack, String comment, String privateComment)
        {
            var result = _teaReviewRep.AddTeacherReview(orderId, feedBack, comment, privateComment);
            return (result > 0);
        }

        public List<MemberReviewItem> GetClassReviews(Byte loadBy, int memberId, int classId, Byte feedback = 0, int maxReviewId = 0)
        {
            var result = _stdReviewRep.GetClassReviews(loadBy, memberId, classId, feedback, maxReviewId);
            return result;
        }

        public List<MemberReviewItem> GetMemberReviews(Byte loadBy, int memberId, Byte feedback = 0, int maxReviewId = 0)
        {
            var result = _stdReviewRep.GetMemberReviews(loadBy, memberId, feedback, maxReviewId);
            return result;
        }

        
    }
}
