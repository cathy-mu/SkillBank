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
    public interface IStudentReviewRepository
    {
        int AddStudentReview(int orderId, int classId, Byte feedBack, String comment, out String deviceToken, out String phone);
        List<StudentReviewItem> GetStudentReviews(Byte loadBy, int paraid);
        List<MemberReviewItem> GetClassReviews(Byte loadBy, int memberId, int classId, Byte feedback, int maxReviewId);
        List<MemberReviewItem> GetMemberReviews(Byte loadBy, int memberId, Byte feedback, int maxReviewId);
    }

    public class StudentReviewRepository : Entities, IStudentReviewRepository
    {
        public StudentReviewRepository()
        // : base("name=Entities")
        {
        }

        public int AddStudentReview(int orderId, int classId, Byte feedBack, String comment, out String deviceToken, out String phone)
        {
            return StudentReview_Add_p(orderId, classId, feedBack, comment, out deviceToken, out phone);
        }

        public List<StudentReviewItem> GetStudentReviews(Byte loadBy, int paraid)
        {
            var result = StudentReview_Load_p(loadBy, paraid);
            return FeedBackMapper.Map(result);
        }

        private int StudentReview_Add_p(int orderId, int classId, Byte feedBack, String comment, out String deviceToken, out String phone)
        {
            String privateComment = "";
            deviceToken = "";
            phone = "";
            ObjectParameter paraId = new ObjectParameter("paraId", 0);
            ObjectParameter orderIdParameter = new ObjectParameter("orderId", orderId);
            ObjectParameter classIdParameter = new ObjectParameter("classId", classId);
            ObjectParameter feedBackParameter = new ObjectParameter("feedBack", feedBack);
            ObjectParameter commentParameter = new ObjectParameter("comment", comment);
            ObjectParameter priComment = new ObjectParameter("priComment", privateComment);
            ObjectParameter devicePara = new ObjectParameter("deviceToken", deviceToken);
            ObjectParameter phonePara = new ObjectParameter("phone", phone);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("StudentReview_Save_p", orderIdParameter, classIdParameter, feedBackParameter, commentParameter, paraId, priComment, devicePara, phonePara);
            deviceToken = (String)devicePara.Value;
            phone = (String)phonePara.Value;
            return (int)paraId.Value;
        }

        private ObjectResult<StudentReview_Load_p_Result> StudentReview_Load_p(Byte loadBy, int paraId)
        {
            ObjectParameter loadByParameter = new ObjectParameter("loadBy", loadBy);
            ObjectParameter paraIdParameter = new ObjectParameter("paraId", paraId);
            
            ObjectResult<StudentReview_Load_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<StudentReview_Load_p_Result>("StudentReview_Load_p", loadByParameter, paraIdParameter);
            return result;
        }

        public List<MemberReviewItem> GetMemberReviews(Byte loadBy, int memberId, Byte feedback, int maxReviewId)
        {
            var result = StudentReview_LoadByMember_p(loadBy, memberId, feedback, maxReviewId);
            return FeedBackMapper.Map(result);
        }

        public List<MemberReviewItem> GetClassReviews(Byte loadBy, int memberId, int classId, Byte feedback, int maxReviewId)
        {
            var result = StudentReview_LoadByClass_p(loadBy, memberId, classId, feedback, maxReviewId);
            return FeedBackMapper.Map(result);
        }


        private ObjectResult<StudentReview_LoadByClass_p_Result> StudentReview_LoadByClass_p(Byte loadBy, int memberId, int classId, Byte feedback, int maxReviewId)
        {
            ObjectParameter loadByParameter = new ObjectParameter("loadBy", loadBy);
            ObjectParameter classIdParameter = new ObjectParameter("classId", classId);
            ObjectParameter memberIdParameter = new ObjectParameter("memberId", memberId);
            ObjectParameter maxIdParameter = new ObjectParameter("maxId", maxReviewId);
            ObjectParameter feedbackParameter = new ObjectParameter("feedback", feedback);

            ObjectResult<StudentReview_LoadByClass_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<StudentReview_LoadByClass_p_Result>("StudentReview_LoadByClass_p", loadByParameter, classIdParameter, memberIdParameter, maxIdParameter, feedbackParameter);
            return result;
        }

        private ObjectResult<StudentReview_LoadByMember_p_Result> StudentReview_LoadByMember_p(Byte loadBy, int memberId, Byte feedback, int maxReviewId)
        {
            ObjectParameter loadByParameter = new ObjectParameter("loadBy", loadBy);
            ObjectParameter memberIdParameter = new ObjectParameter("memberId", memberId);
            ObjectParameter maxIdParameter = new ObjectParameter("maxId", maxReviewId);
            ObjectParameter feedbackParameter = new ObjectParameter("feedback", feedback);

            ObjectResult<StudentReview_LoadByMember_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<StudentReview_LoadByMember_p_Result>("StudentReview_LoadByMember_p", loadByParameter, memberIdParameter, maxIdParameter, feedbackParameter);
            return result;
        }

    }
}
