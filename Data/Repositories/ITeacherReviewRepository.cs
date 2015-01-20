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
    public interface ITeacherReviewRepository
    {
        int AddTeacherReview(int orderId, Byte feedBack, String comment, String privateComment);
        List<TeacherReviewItem> GetTeacherReviews(Byte loadBy, int id, int maxReviewId = 0);
    }

    public class TeacherReviewRepository : Entities, ITeacherReviewRepository
    {
        public TeacherReviewRepository()
            //: base("name=Entities")
        {
        }

        public int AddTeacherReview(int orderId, Byte feedBack, String comment, String privateComment)
        {
            return TeacherReview_Add_p(orderId, feedBack, comment, privateComment);
        }

        public List<TeacherReviewItem> GetTeacherReviews(Byte loadBy, int paraId, int maxReviewId = 0)
        {
            var result = TeacherReview_Load_p(loadBy, paraId, maxReviewId);
            return FeedBackMapper.Map(result);
        }


        private ObjectResult<TeacherReview_Load_p_Result> TeacherReview_Load_p(Byte loadType, int paraId, int maxReviewId)
        {
            var loadByParameter = new ObjectParameter("loadBy", loadType);
            var paraidParameter = new ObjectParameter("paraId", paraId);
            var maxidParameter = new ObjectParameter("maxId", maxReviewId);

            ObjectResult<TeacherReview_Load_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<TeacherReview_Load_p_Result>("TeacherReview_Load_p", loadByParameter, paraidParameter, maxidParameter);
            return result;
        }

        private int TeacherReview_Add_p(int orderId, Byte feedBack, String comment, String privateComment)
        {
            ObjectParameter paraId = new ObjectParameter("paraId", 0);
            ObjectParameter orderIdParameter = new ObjectParameter("orderId", orderId);
            ObjectParameter feedBackParameter = new ObjectParameter("feedBack", feedBack);
            ObjectParameter commentParameter = new ObjectParameter("comment", comment);
            ObjectParameter priComment = new ObjectParameter("priComment", privateComment);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TeacherReview_Save_p", orderIdParameter, feedBackParameter, commentParameter, paraId, priComment);
            return (int)paraId.Value;
        }
    }
}

