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
        int AddStudentReview(int orderId, Byte scoreA, Byte scoreB, Byte scoreC, Byte feedBack, String comment, String privateComment);
        List<StudentReview> GetStudentReviews(Byte loadBy, int id, int minReviewId);
        //List<StudentReview> GetStudentReviewsByClass(Byte loadBy, int classId, int memberId, int minReviewId);
    }

    public class StudentReviewRepository : DbContext, IStudentReviewRepository
    {
        public StudentReviewRepository()
            : base("name=Entities")
        {
        }

        public int AddStudentReview(int orderId, Byte scoreA, Byte scoreB, Byte scoreC, Byte feedBack, String comment, String privateComment)
        {
            return StudentReview_Add_p(orderId, scoreA, scoreB, scoreC, feedBack, comment, privateComment);
        }
        
        //public List<StudentReview> GetStudentReviewsByClass(Byte loadBy, int classId, int memberId, int minReviewId)
        //{
        //    var result = StudentReview_LoadByClass_p(loadBy, classId, memberId, minReviewId);
        //    return CommentMapper.Map(result);
        //}
        
        public List<StudentReview> GetStudentReviews(Byte loadBy, int paraId, int minReviewId)
        {
            var result = StudentReview_Load_p(loadBy, paraId);
            return CommentMapper.Map(result);
        }



        private ObjectResult<StudentReview> StudentReview_Load_p(Byte loadType, int paraId)
        {
            var loadByParameter = new ObjectParameter("LoadBy", loadType);

            var idParameter = new ObjectParameter("ParaId", paraId);

            ObjectResult<StudentReview> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<StudentReview>("StudentReview_Load_p", loadByParameter, idParameter);
            return result;
        }

        private int StudentReview_Add_p(int orderId, Byte scoreA, Byte scoreB, Byte scoreC, Byte feedBack, String comment, String privateComment)
        {
            ObjectParameter paraId = new ObjectParameter("ParaId", 0);
            ObjectParameter orderIdParameter = new ObjectParameter("OrderId", orderId);
            ObjectParameter score1Parameter = new ObjectParameter("ScoreA", scoreA);
            ObjectParameter score2Parameter = new ObjectParameter("ScoreB", scoreB);
            ObjectParameter score3Parameter = new ObjectParameter("ScoreC", scoreC);
            ObjectParameter feedBackParameter = new ObjectParameter("FeedBack", feedBack);
            ObjectParameter commentParameter = new ObjectParameter("Comment", comment);
            ObjectParameter priComment = new ObjectParameter("priComment", privateComment);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("StudentReview_Save_p", orderIdParameter, score1Parameter, score2Parameter, score3Parameter, feedBackParameter, commentParameter, paraId, priComment);
            return (int)paraId.Value;
        }

        private ObjectResult<StudentReview> StudentReview_LoadByClass_p(Byte loadBy, int classId, int memberId, int maxReviewId)
        {
            ObjectParameter loadByParameter = new ObjectParameter("LoadBy", loadBy);
            ObjectParameter classIdParameter = new ObjectParameter("ClassId", classId);
            ObjectParameter memberIdParameter = new ObjectParameter("MemberId", memberId);
            ObjectParameter minIdParameter = new ObjectParameter("MaxId", maxReviewId);

            ObjectResult<StudentReview> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<StudentReview>("StudentReview_LoadByClass_p", loadByParameter, classIdParameter, memberIdParameter, minIdParameter);
            return result;
        }
    }
}

