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
        List<TeacherReview> GetTeacherReviews(Byte loadBy, int id);
    }

    public class TeacherReviewRepository : DbContext, ITeacherReviewRepository
    {
        public TeacherReviewRepository()
            : base("name=Entities")
        {
        }

        public int AddTeacherReview(int orderId, Byte feedBack, String comment, String privateComment)
        {
            return TeacherReview_Add_p(orderId, feedBack, comment, privateComment);
        }

        public List<TeacherReview> GetTeacherReviews(Byte loadBy, int paraId)
        {
            var result = TeacherReview_Load_p(loadBy, paraId);
            return CommentMapper.Map(result);
        }


        private ObjectResult<TeacherReview> TeacherReview_Load_p(Byte loadType, int paraId)
        {
            var loadByParameter = new ObjectParameter("LoadBy", loadType);
            var idParameter = new ObjectParameter("ParaId", paraId);

            ObjectResult<TeacherReview> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<TeacherReview>("TeacherReview_Load_p", loadByParameter, idParameter);
            return result;
        }

        private int TeacherReview_Add_p(int orderId, Byte feedBack, String comment, String privateComment)
        {
            ObjectParameter paraId = new ObjectParameter("ParaId", 0);
            ObjectParameter orderIdParameter = new ObjectParameter("OrderId", orderId);
            ObjectParameter feedBackParameter = new ObjectParameter("FeedBack", feedBack);
            ObjectParameter commentParameter = new ObjectParameter("Comment", comment);
            ObjectParameter priComment = new ObjectParameter("priComment", privateComment);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TeacherReview_Save_p", orderIdParameter, feedBackParameter, commentParameter, paraId, priComment);
            return (int)paraId.Value;
        }
    }
}

